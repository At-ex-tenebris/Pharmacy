using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pharmacyApi.Models;
using pharmacyApi.Utils;
using pharmacyApi.Data;

namespace pharmacyApi.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CorsAllowAny")]
    public class TemplateController : ControllerBase {
        private ApplicationContext db;
        private IConfiguration config;

        //const string AUTH_PASSWORD = "auth_example";
        //const string AUTH_ROLE = "example_role";

        //const string INVALID_PASSWORD_MSG = "Пароль некорректен.";
        //const string ID_NOT_FOUND = "Запись с заданным идентификатором не найдена.";

        public TemplateController(ApplicationContext context, IConfiguration config) {
            this.db = context;
            this.config = config;
        }

        //[HttpPost("token")]
        //public IActionResult Login([FromBody] string password) {
        //    password = PasswordHasher.Hash(password);
        //    var authPassword = PasswordHasher.Hash(AUTH_PASSWORD);
        //    if (password != authPassword) return BadRequest(INVALID_PASSWORD_MSG);
        //    var token = TokenHandler.BuildToken(new string[] { AUTH_ROLE }, config);
        //    return Ok(new { token = token });
        //}

        //[HttpGet("list")]
        //public IActionResult GetList([FromQuery] long otmId = 0, [FromQuery] string strField = "",
        //    [FromQuery] int pageNum = 1, [FromQuery] int pageSize = 2) {
        //    var entries = db.TableTemplate.Include(x => x.OTMField).Where(x => true);
        //    if (otmId != 0) entries = entries.Where(x => x.OTMFieldId == otmId);
        //    if (strField != "") entries = entries.Where(x => x.StrField.Contains(strField));
        //    var fullSize = entries.Count();
        //    var pagesAmount = fullSize / pageSize + (fullSize % pageSize != 0 ? 1 : 0);
        //    if (pageNum != 0) entries = entries.Skip(pageSize * (pageNum - 1)).Take(pageSize);
        //    return Ok(new { entries, pagesAmount });
        //}

        ////// Аннотация авторизации, при включении требует
        ////// http-заголовок с выданным ранее jwt-токеном 
        ////// Формат: Authorization : Bearer TOKEN
        ////[Authorize] 
        //[HttpGet("{id}")]
        //public IActionResult GetConcrete([FromRoute] long id) {
        //    var entry = db.TableTemplate.Include(x => x.OTMField).FirstOrDefault(x => x.Id == id);
        //    if (entry == null) return NotFound(ID_NOT_FOUND);
        //    return Ok(entry);
        //}

        ////[Authorize]
        //[HttpPost]
        //public IActionResult AddEntry([FromBody] RequestTemplate request) {
        //    var password = PasswordHasher.Hash(request.Password);
        //    var authPassword = PasswordHasher.Hash(AUTH_PASSWORD);
        //    if (password != authPassword) return BadRequest(INVALID_PASSWORD_MSG);
        //    var model = request.Model;
        //    db.TableTemplate.Add(model);
        //    db.SaveChanges();
        //    return Ok(model);
        //}

        ////[Authorize]
        //[HttpPut("{id}")]
        //public IActionResult RedactEntry([FromBody] RequestTemplate request, [FromRoute] long id) {
        //    var password = PasswordHasher.Hash(request.Password);
        //    var authPassword = PasswordHasher.Hash(AUTH_PASSWORD);
        //    if (password != authPassword) return BadRequest(INVALID_PASSWORD_MSG);
        //    var entry = db.TableTemplate.FirstOrDefault(x => x.Id == id);
        //    if (entry == null) return NotFound(ID_NOT_FOUND);
        //    var model = request.Model;
        //    entry.StrField = model.StrField;
        //    entry.NullableStrField = model.NullableStrField;
        //    entry.DateField = model.DateField;
        //    entry.EnumField = model.EnumField;
        //    entry.OTMFieldId = model.OTMFieldId;
        //    db.TableTemplate.Update(entry);
        //    db.SaveChanges();
        //    return Ok(entry);
        //}

        ////[Authorize(Roles = AUTH_ROLE)]
        //[HttpDelete("{id}")]
        //public IActionResult DeleteEntry([FromBody] string password, [FromRoute] long id) {
        //    password = PasswordHasher.Hash(password);
        //    var authPassword = PasswordHasher.Hash(AUTH_PASSWORD);
        //    if (password != authPassword) return BadRequest(INVALID_PASSWORD_MSG);
        //    var entry = db.TableTemplate.FirstOrDefault(x => x.Id == id);
        //    if (entry == null) return NotFound(ID_NOT_FOUND);
        //    db.TableTemplate.Remove(entry);
        //    db.SaveChanges();
        //    return Ok(entry);
        //}

        //public class RequestTemplate {
        //    public string        Password { get; set; }
        //    public ModelTemplate Model { get; set; }
        //}

    }
}
