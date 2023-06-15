using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using pharmacyApi.Data;
using pharmacyApi.Utils;
using pharmacyApi.Models;

namespace pharmacyApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CorsAllowAny")]

    public class AdministratorsController : ControllerBase
    {
        private ApplicationContext db;
        private IConfiguration config;
        const string AUTH_FAILED = "Данные авторизации введены неверно.";
        const string AUTH_ROLE = "admin";

        public AdministratorsController(ApplicationContext context, IConfiguration config)
        {
            db = context;
            this.config = config;
        }

        [HttpPost("token")]
        public IActionResult Login([FromBody] AuthData authData)
        {
            if (!AuthValidation.isValidAdmin(db, authData, "admin"))
                return BadRequest(AUTH_FAILED);

            var token = TokenHandler.BuildToken(new string[] { AUTH_ROLE }, config);
            return Ok(new { token = token });
        }
    }


}
