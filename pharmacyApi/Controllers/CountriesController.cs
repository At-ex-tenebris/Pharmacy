using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using pharmacyApi.Data;
using pharmacyApi.Utils;
using pharmacyApi.Models;
using Microsoft.EntityFrameworkCore;

namespace pharmacyApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CorsAllowAny")]

    public class CountriesController : ControllerBase
    {
        private ApplicationContext db;
        private IConfiguration config;
        const string AUTH_INVALID= "Данные авторизации введены неверно.";
        const string AUTH_ROLE = "country";
        const string ID_NOT_FOUND = "ID не найден";


        public CountriesController(ApplicationContext context, IConfiguration config)
        {
            db = context;
            this.config = config;
        }

        [HttpPost("token")]
        public IActionResult Login([FromBody] AuthData authData)
        {
            var login = authData.Login;
            var password = PasswordHasher.Hash(authData.Password);
            var country = db.Countries.FirstOrDefault(x => x.Login == login && x.Password == password);
            if (country == null) return BadRequest(AUTH_INVALID);
            var token = TokenHandler.BuildToken(new string[] { AUTH_ROLE }, config);
            return Ok(new { token = token, entry = FullCountry.FromStd(country) });
        }
        [HttpGet("list")]
        public IActionResult GetList( [FromQuery] string countryName = "",
            [FromQuery] int pageNum = 1, [FromQuery] int pageSize = 2)
        {
            var entries = db.Countries.Where(x => true);
            if (countryName != "") entries = entries.Where(x => x.CountryName.Contains(countryName));
            var fullSize = entries.Count();
            var pagesAmount = fullSize / pageSize + (fullSize % pageSize != 0 ? 1 : 0);
            if (pageNum != 0) entries = entries.Skip(pageSize * (pageNum - 1)).Take(pageSize);
            return Ok(new { entries, pagesAmount });
        }

        [HttpGet("{id}")]
        public IActionResult GetConcrete([FromRoute] long id)
        {
            var entry = db.Countries.FirstOrDefault(x => x.Id == id);
            if (entry == null) return NotFound(ID_NOT_FOUND);
            return Ok(entry);
        }

        //[Authorize]
        [HttpPost]
        public IActionResult AddEntry([FromBody] CountryRequest request, [FromQuery] string authType)
        {
            
            if (!AuthValidation.isValidAdmin(db,  request.authData, authType)) return BadRequest(AUTH_INVALID);
            var model = Country.FromFull(request.fullCountry);
            model.Password = PasswordHasher.Hash(model.Password);
            db.Countries.Add(model);
            db.SaveChanges();
            return Ok(model);
        }


        //[Authorize]
        [HttpPut("{id}")]
        public IActionResult RedactEntry([FromBody] CountryRequest request, [FromRoute] long id, [FromQuery] string authType)
        {
            
            var entry = db.Countries.FirstOrDefault(x => x.Id == id);
            if (entry == null) return NotFound(ID_NOT_FOUND);
            if (!AuthValidation.isValidAdmin(db, request.authData, authType)) return BadRequest(AUTH_INVALID);
            var model = request.fullCountry; 
            entry.Login = model.Login;
            if (entry.Password != model.Password)
                entry.Password = PasswordHasher.Hash(model.Password);
            entry.CountryName = model.CountryName;
            db.Countries.Update(entry);
            db.SaveChanges();
            return Ok(entry);
        }

        //[Authorize(Roles = AUTH_ROLE)]
        [HttpDelete("{id}")]
        public IActionResult DeleteEntry([FromBody] AuthData authData, [FromRoute] long id, [FromQuery] string authType)
        {
            var entry = db.Countries.FirstOrDefault(x => x.Id == id);
            if (entry == null) return NotFound(ID_NOT_FOUND);
            if (!AuthValidation.isValidAdmin(db, authData, authType)) return BadRequest(AUTH_INVALID);
            db.Countries.Remove(entry);
            db.SaveChanges();
            return Ok(entry);
        }


        //[Authorize(Roles = AUTH_ROLE)]
        [HttpPost("{id}")]
        public IActionResult GetEntry([FromBody] AuthData authData, [FromRoute] long id, [FromQuery] string authType)
        {
            var entry = db.Countries.FirstOrDefault(x => x.Id == id);
            if (entry == null) return NotFound(ID_NOT_FOUND);
            if (!AuthValidation.isValidAdmin(db, authData, authType)) return BadRequest(AUTH_INVALID);
            return Ok(FullCountry.FromStd(entry));
        }
        public class CountryRequest
        {
            public AuthData authData { get; set; }
            public FullCountry fullCountry { get; set; }
        }
    }
}
