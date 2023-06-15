using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pharmacyApi.Data;
using pharmacyApi.Models;
using pharmacyApi.Utils;


namespace pharmacyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CorsAllowAny")]
    public class PharmacyController : ControllerBase
    {
        private ApplicationContext db;
        private IConfiguration config;
        const string AUTH_FAILED = "Данные авторизации введены неверно.";
        const string AUTH_ROLE = "pharmacy";
        const string ID_NOT_FOUND = "Запись с заданным индентификатором не найдена";
        const string AUTH_INVALID = "Логин или пароль не проходят авторизацию.";

        public PharmacyController(ApplicationContext db, IConfiguration config)
        {
            this.db = db;
            this.config = config;
        }

        [HttpPost("token")]
        public IActionResult Login([FromBody] AuthData authData)
        {
            var login = authData.Login;
            var password = PasswordHasher.Hash(authData.Password);
            var pharmacy = db.Pharmacies.FirstOrDefault(x => x.Login == login && x.Password == password);
            if (pharmacy == null)
            {
                return BadRequest(AUTH_INVALID);
            }
            var token = TokenHandler.BuildToken(new string[] { AUTH_ROLE }, config);
            return Ok(new { token = token, entry = pharmacy });
        }

        [HttpGet("list")]
        public IActionResult GetList([FromQuery] long cityId = 0, [FromQuery] string pharmacyName = "",
            [FromQuery] int pageNum = 1, [FromQuery] int pageSize = 2)
        {
            var entries = db.Pharmacies.Include(x => x.City).Where(x => true);
            if (cityId != 0) entries = entries.Where(x => x.CityId == cityId);
            if (pharmacyName != "") entries = entries.Where(x => x.PharmacyName.Contains(pharmacyName));
            var fullSize = entries.Count();
            var pagesAmount = fullSize / pageSize + (fullSize % pageSize != 0 ? 1 : 0);
            if (pageNum != 0) entries = entries.Skip(pageSize * (pageNum - 1)).Take(pageSize);
            return Ok(new { entries, pagesAmount });
        }

        [HttpGet("{id}")]
        public IActionResult GetConcrete([FromRoute] long id)
        {
            var entry = db.Pharmacies.Include(x => x.City).ThenInclude(x => x.Region).ThenInclude(x => x.Country).FirstOrDefault(x => x.Id == id);
            if (entry == null) return NotFound(ID_NOT_FOUND);
            return Ok(entry);
        }

        //[Authorize]
        [HttpPost]
        public IActionResult AddEntry([FromBody] PharmacyRequest request, [FromQuery] string authType)
        {
            var City = db.Cities.FirstOrDefault(x => x.Id == request.fullPharmacy.CityId);
            if (City == null) return NotFound(ID_NOT_FOUND);
            if (!AuthValidation.isValid(db, City, request.authData, authType)) return BadRequest(AUTH_INVALID);
            var model = Pharmacy.FromFull(request.fullPharmacy);
            db.Pharmacies.Add(model);
            db.SaveChanges();
            return Ok(model);
        }

        public class PharmacyRequest
        {
            public AuthData authData { get; set; }
            public FullPharmacy fullPharmacy { get; set; }
        }

        [HttpPut("{id}")]
        public IActionResult RedactEntry([FromBody] PharmacyRequest request, [FromRoute] long id, [FromQuery] string authType)
        {
            var City = db.Cities.FirstOrDefault(x => x.Id == request.fullPharmacy.CityId);
            if (City == null) return NotFound(ID_NOT_FOUND);
            var entry = db.Pharmacies.FirstOrDefault(x => x.Id == id);
            if (entry == null) return NotFound(ID_NOT_FOUND);
            var entryCity = db.Cities.FirstOrDefault(x => x.Id == entry.CityId);
            if (entryCity == null) return NotFound(ID_NOT_FOUND);
            bool selfRedaction = City == entryCity;
            if (!AuthValidation.isValid(db, City, request.authData, authType, selfRedaction) ||
                !AuthValidation.isValid(db, entryCity, request.authData, authType, selfRedaction)) return BadRequest(AUTH_INVALID);
            var model = request.fullPharmacy;
            entry.Login = model.Login;
            if (entry.Password != model.Password)
                entry.Password = PasswordHasher.Hash(model.Password);
            entry.PharmacyName = model.PharmacyName;
            entry.CityId = model.CityId;
            db.Pharmacies.Update(entry);
            db.SaveChanges();
            return Ok(entry);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEntry([FromBody] AuthData authData, [FromRoute] long id, [FromQuery] string authType)
        {
            var entry = db.Pharmacies.FirstOrDefault(x => x.Id == id);
            if (entry == null) return NotFound(ID_NOT_FOUND);
            var City = db.Cities.FirstOrDefault(x => x.Id == entry.CityId);
            if (City == null) return NotFound(ID_NOT_FOUND);
            if (!AuthValidation.isValid(db, City, authData, authType)) return BadRequest(AUTH_INVALID);
            db.Pharmacies.Remove(entry);
            db.SaveChanges();
            return Ok(entry);
        }

        [HttpPost("{id}")]
        public IActionResult GetEntry([FromBody] AuthData authData, [FromRoute] long id, [FromQuery] string authType)
        {
            var entry = db.Pharmacies.FirstOrDefault(x => x.Id == id);
            if (entry == null) return NotFound(ID_NOT_FOUND);
            var City = db.Cities.FirstOrDefault(x => x.Id == entry.CityId);
            if (City == null) return NotFound(ID_NOT_FOUND);
            if (!AuthValidation.isValid(db, City, authData, authType)) return BadRequest(AUTH_INVALID);
            var fullEntry = FullPharmacy.FromStd(entry);
            return Ok(fullEntry);
        }
    }

    

}
