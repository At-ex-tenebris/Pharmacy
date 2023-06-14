using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pharmacyApi.Models;
using pharmacyApi.Utils;
using pharmacyApi.Data;

namespace pharmacyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CorsAllowAny")]

    public class CitiesCotroller : ControllerBase
    {
        private ApplicationContext db;
        private IConfiguration config;
        const string AUTH_FAILED = "Данные авторизации введены неверно.";
        const string AUTH_ROLE = "city";
        const string ID_NOT_FOUND = "Запись по введенному идентификатору не найдена.";

        public CitiesCotroller(ApplicationContext context, IConfiguration config)
        {
            this.db = context;
            this.config = config;
        }

        [HttpPost("token")]
        public IActionResult Login([FromBody] AuthData authData)
        {
            var login = authData.Login;
            var password = PasswordHasher.Hash(authData.Password);
            var city = db.Cities.FirstOrDefault(x => x.Login == login && x.Password == password);
            if (city == null)
                return BadRequest(AUTH_FAILED);
            var token = TokenHandler.BuildToken(new string[] { AUTH_ROLE }, config);
            return Ok(new { token = token, entry = city });
        }

        [HttpGet("list")]
        public IActionResult GetList([FromQuery] int countryId = 0, [FromQuery] int regionId = 0, [FromQuery] string cityName = "",
            [FromQuery] int pageNum = 1, [FromQuery] int pageSize = 2)
        {
            var entries = db.Cities.Include(x => x.Region).Where(x => true);
            if (cityName != "") entries = entries.Where(x => x.CityName.Contains(cityName));
            if (regionId != 0) entries = entries.Where(x => x.RegionId == regionId);
            if (countryId != 0) entries = entries.Where(x => x.Region.CountryId == countryId);
            var fullSize = entries.Count();
            var pagesAmount = fullSize / pageSize + (fullSize % pageSize != 0 ? 1 : 0);
            if (pageNum != 0) entries = entries.Skip(pageSize * (pageNum - 1)).Take(pageSize);
            return Ok(new { entries, pagesAmount });
        }

        [HttpGet("{id}")]
        public IActionResult GetConcrete([FromRoute] long id)
        {
            var entry = db.Cities.Include(x => x.Region).ThenInclude(x => x.Country).FirstOrDefault(x => x.Id == id);
            if (entry == null) return NotFound(ID_NOT_FOUND);
            return Ok(entry);
        }

        [HttpPost]
        public IActionResult AddEntry([FromBody] CityRequest request, [FromQuery] string authType)
        {
            var region = db.Regions.Include(x => x.Country).FirstOrDefault(x => x.Id == request.fullCity.RegionId);
            if (region == null) return NotFound(ID_NOT_FOUND);
            if (!AuthValidation.isValid(db, region, request.authData, authType)) return BadRequest(AUTH_FAILED);
            var model = City.stdFrom(request.fullCity);
            db.Cities.Add(model);
            db.SaveChanges();
            return Ok(model);
        }

        //[Authorize]
        [HttpPut("{id}")]
        public IActionResult RedactEntry([FromBody] CityRequest request, [FromRoute] long id, [FromQuery] string authType)
        {
            var Region = db.Regions.Include(x => x.Country).FirstOrDefault(x => x.Id == request.fullCity.RegionId);
            if (Region == null) return NotFound(ID_NOT_FOUND);
            var entry = db.Cities.FirstOrDefault(x => x.Id == id);
            if (entry == null) return NotFound(ID_NOT_FOUND);
            var entryRegion = db.Regions.FirstOrDefault(x => x.Id == entry.RegionId);
            if (entryRegion == null) return NotFound(ID_NOT_FOUND);
            bool selfRedaction = Region == entryRegion;
            if (!AuthValidation.isValid(db, Region, request.authData, authType, selfRedaction) || 
                !AuthValidation.isValid(db, entryRegion, request.authData, authType, selfRedaction)) return BadRequest(AUTH_FAILED);
            var model = request.fullCity;
            entry.Login = model.Login;
            if (entry.Password != model.Password)
                entry.Password = PasswordHasher.Hash(model.Password);
            entry.CityName = model.CityName;
            entry.RegionId = model.RegionId;
            db.Cities.Update(entry);
            db.SaveChanges();
            return Ok(entry);
        }

        //[Authorize(Roles = AUTH_ROLE)]
        [HttpDelete("{id}")]
        public IActionResult DeleteEntry([FromBody] AuthData authData, [FromRoute] long id, [FromQuery] string authType)
        {
            var entry = db.Cities.Include(x => x.Region).ThenInclude(x => x.Country).FirstOrDefault(x => x.Id == id);
            if (entry == null) return NotFound(ID_NOT_FOUND);
            var Region = db.Regions.FirstOrDefault(x => x.Id == entry.RegionId);
            if (Region == null) return NotFound(ID_NOT_FOUND);
            if (!AuthValidation.isValid(db, Region, authData, authType)) return BadRequest(AUTH_FAILED);
            db.Cities.Remove(entry);
            db.SaveChanges();
            return Ok(entry);
        }

        [HttpPost("{id}")]
        public IActionResult GetInfo([FromBody] AuthData authData, [FromRoute] long id, [FromQuery] string authType)
        {
            var entry = db.Cities.Include(x => x.Region).ThenInclude(x => x.Country).FirstOrDefault(x => x.Id == id);
            if (entry == null) return NotFound(ID_NOT_FOUND);
            var entryRegion = db.Cities.FirstOrDefault(x => x.Id == entry.RegionId);
            if (entryRegion == null) return NotFound(ID_NOT_FOUND);
            if (!AuthValidation.isValid(db, entryRegion, authData, authType)) return BadRequest(AUTH_FAILED);
            return Ok(entry);
        }

        public class CityRequest
        {
            public AuthData authData { get; set; }
            public FullCity fullCity { get; set; }
        }
    }
} 