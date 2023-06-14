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
    }
} 