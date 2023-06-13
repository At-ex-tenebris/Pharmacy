using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using pharmacyApi.Data;
using pharmacyApi.Models;
using pharmacyApi.Utils;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

namespace pharmacyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CorsAllowAny")]
    public class RegionController: ControllerBase
    {
        const string AUTH_ROLE = "region";
        const string ID_NOT_FOUND = "Запись с заданным идентификатором не найдена.";
        const string AUTH_INVALID = "Логин или пароль не проходят авторизацию.";

        private ApplicationContext db;
        private IConfiguration config;

        public RegionController(ApplicationContext context, IConfiguration config)
        {
            this.db = context;
            this.config = config;
        }


        [HttpPost("token")]
        public IActionResult Login([FromBody] AuthData authData)
        {
            var login = authData.Login;
            var password = PasswordHasher.Hash(authData.Password);
           
            var Region = db.Regions.FirstOrDefault(x => x.Login == login && x.Password == password);
            var token = TokenHandler.BuildToken(new string[] { AUTH_ROLE }, config);
            return Ok(new { token = token, entry = Region });
        }


        [HttpGet("list")]
        public IActionResult GetList([FromQuery] long countryId = 0, [FromQuery] string regionName = "",
            [FromQuery] int pageNum = 1, [FromQuery] int pageSize = 2)
        {
            var entries = db.Regions.Include(x => x.Country).Where(x => true);
            if (countryId != 0) entries = entries.Where(x => x.CountryId == countryId);
            if (regionName != "") entries = entries.Where(x => x.Name.Contains(regionName));
            var fullSize = entries.Count();
            var pagesAmount = fullSize / pageSize + (fullSize % pageSize != 0 ? 1 : 0);
            if (pageNum != 0) entries = entries.Skip(pageSize * (pageNum - 1)).Take(pageSize);
            return Ok(new { entries, pagesAmount });
        }

        [HttpGet("{id}")]
        public IActionResult GetConcrete([FromRoute] long id)
        {
            var entry = db.Regions.Include(x => x.Country).FirstOrDefault(x => x.Id == id);
            if (entry == null) return NotFound(ID_NOT_FOUND);
            return Ok(entry);
        }

        //[Authorize]
        [HttpPost]
        public IActionResult AddEntry([FromBody] RegionRequest request, [FromQuery] string authType)
        {
            var Country = db.Countries.FirstOrDefault(x => x.Id == request.fullRegion.CountryId);
            if (Country == null) return NotFound(ID_NOT_FOUND);
            if (!AuthValidation.isValid(db, Country, request.authData, authType)) return BadRequest(AUTH_INVALID);
            var model = Region.FromFull(request.fullRegion);
            db.Regions.Add(model);
            db.SaveChanges();
            return Ok(model);
        }

        public class RegionRequest
        {
                public AuthData authData {  get; set; } 
                public FullRegion fullRegion { get; set; }
        }

        //[Authorize]
        [HttpPut("{id}")]
        public IActionResult RedactEntry([FromBody] RegionRequest request, [FromRoute] long id, [FromQuery] string authType)
        {
            var Country = db.Countries.FirstOrDefault(x => x.Id == request.fullRegion.CountryId);
            if (Country == null) return NotFound(ID_NOT_FOUND);
            var entry = db.Regions.FirstOrDefault(x => x.Id == id);
            if (entry == null) return NotFound(ID_NOT_FOUND);
            var entryCountry = db.Countries.FirstOrDefault(x => x.Id == entry.CountryId);
            if (entryCountry == null) return NotFound(ID_NOT_FOUND);
            bool selfRedaction = Country == entryCountry;
            if (!AuthValidation.isValid(db, Country, request.authData, authType, selfRedaction)) return BadRequest(AUTH_INVALID);
            var model = request.fullRegion;
            entry.Login = model.Login;
            if(entry.Password != model.Password)
                entry.Password = PasswordHasher.Hash(model.Password);
            entry.Name = model.Name;
            entry.CountryId = model.CountryId;
            db.Regions.Update(entry);
            db.SaveChanges();
            return Ok(entry);
        }

        //[Authorize(Roles = AUTH_ROLE)]
        [HttpDelete("{id}")]
        public IActionResult DeleteEntry([FromBody] AuthData authData, [FromRoute] long id, [FromQuery] string authType)
        {
            var entry = db.Regions.FirstOrDefault(x => x.Id == id);
            if (entry == null) return NotFound(ID_NOT_FOUND);
            var Country = db.Countries.FirstOrDefault(x => x.Id == entry.CountryId);
            if (Country == null) return NotFound(ID_NOT_FOUND);
            if (!AuthValidation.isValid(db, Country, authData, authType)) return BadRequest(AUTH_INVALID);
            db.Regions.Remove(entry);
            db.SaveChanges();
            return Ok(entry);
        }

    }


}
