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
    public class MedicamentsController : ControllerBase
    {
        private ApplicationContext db;

        const string AUTH_INVALID = "Текущий медикамент не прошёл валидацию.";
        const string ID_NOT_FOUND = "Запись с заданным идентификатором не найдена.";
        const string NOT_FOUND_PHARMACY = "Аптека для данного медикамента не найдена.";

        public MedicamentsController(ApplicationContext context)
        {
            db = context;
        }

        /// <summary>
        /// Метод получения списка позиций по параметрам поиска.
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public IActionResult GetList([FromQuery] int pharmacyId = 0, [FromQuery] string medicamentName = "",
            [FromQuery] int cityId = 0, [FromQuery] int pageNum = 1, [FromQuery] int pageSize = 2)
        {
            // Получение записей таблицы Medicament
            var entries = db.Medicaments.Include(x => x.Pharmacy).Where(x => true);

            // Проверка на отбор по Id
            if (pharmacyId != 0)
                entries = entries.Where(x => x.PharmacyId == pharmacyId);
            // Проверка на отбор по Id города
            if (cityId != 0)
                entries = entries.Where(x => x.Pharmacy.CityId == cityId);
            // Проверка на отбор по названию
            if (medicamentName != "")
                entries = entries.Where(x => x.Name.Contains(medicamentName));

            var fullSize = entries.Count();
            var pagesAmount = fullSize / pageSize + (fullSize % pageSize != 0 ? 1 : 0);

            if (pageNum != 0)
                entries = entries.Skip(pageSize * (pageNum - 1)).Take(pageSize);

            return Ok(new { entries, pagesAmount });
        }

        /// <summary>
        /// Метод получения конкретной позиции по идентификатору.
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult GetConcrete([FromRoute] int id)
        {
            var entry = db.Medicaments.Include(x => x.Pharmacy).ThenInclude(x => x.City).
                ThenInclude(x => x.Region).ThenInclude(x => x.Country).FirstOrDefault(x => x.Id == id);
            // Проверка на наличие записи с таким Id
            if (entry == null)
                return NotFound(ID_NOT_FOUND);

            return Ok(entry);
        }

        /// <summary>
        /// Метод добавления записи позиции.
        /// </summary>
        [HttpPost]
        public IActionResult AddEntry([FromBody] RequestMedicament request, [FromQuery] string authType)
        {
            var pharmacy = db.Pharmacies.Include(x => x.City).ThenInclude(x => x.Region).
                ThenInclude(x => x.Country).FirstOrDefault(x => x.Id == request.Model.PharmacyId);
            if (pharmacy == null)
                return BadRequest(NOT_FOUND_PHARMACY);

            // Проверка валидации медикамента
            if (!AuthValidation.isValid(db, pharmacy, request.AuthData, authType))
                return BadRequest(AUTH_INVALID);

            db.Medicaments.Add(request.Model);
            db.SaveChanges();

            return Ok(request.Model);
        }

        /// <summary>
        /// Метод редактирования записи позиции.
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult RedactEntry([FromRoute] int id, [FromBody] RequestMedicament request, [FromQuery] string authType)
        {
            var entry = db.Medicaments.Include(x => x.Pharmacy).ThenInclude(x => x.City).
                ThenInclude(x => x.Region).ThenInclude(x => x.Country).FirstOrDefault(x => x.Id == id);
            if (entry == null)
                return NotFound(ID_NOT_FOUND);

            var pharmacy = db.Pharmacies.FirstOrDefault(x => x.Id == entry.PharmacyId);
            if (pharmacy == null)
                return BadRequest(NOT_FOUND_PHARMACY);

            var model = request.Model;
            var requestPharmacy = db.Pharmacies.FirstOrDefault(x => x.Id == model.PharmacyId);
            if (requestPharmacy == null)
                return BadRequest(NOT_FOUND_PHARMACY);

            bool selfRedaction = pharmacy == requestPharmacy;

            // Проверка валидации медикамента
            if (!AuthValidation.isValid(db, pharmacy, request.AuthData, authType, selfRedaction) || 
                !AuthValidation.isValid(db, requestPharmacy, request.AuthData, authType, selfRedaction))
                return BadRequest(AUTH_INVALID);

            // Изменение значений полей экземпляре класса Medicament
            entry.Name = model.Name;
            entry.Description = model.Description;
            entry.Count = model.Count;
            entry.PharmacyId = model.PharmacyId;

            db.Medicaments.Update(entry);
            db.SaveChanges();

            return Ok(entry);
        }

        /// <summary>
        /// Метод удаления записи позиции.
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult DeleteEntry([FromRoute] int id, [FromBody] AuthData AuthData, [FromQuery] string authType)
        {
            var entry = db.Medicaments.Include(x => x.Pharmacy).ThenInclude(x => x.City).
                ThenInclude(x => x.Region).ThenInclude(x => x.Country).FirstOrDefault(x => x.Id == id);
            if (entry == null)
                return NotFound(ID_NOT_FOUND);

            var pharmacy = db.Pharmacies.FirstOrDefault(x => x.Id == entry.PharmacyId);
            if (pharmacy == null)
                return BadRequest(NOT_FOUND_PHARMACY);

            // Проверка валидации пользователя для возможности изменения записей в таблице Medicament
            if (!AuthValidation.isValid(db, pharmacy, AuthData, authType))
                return BadRequest(AUTH_INVALID);

            db.Medicaments.Remove(entry);
            db.SaveChanges();

            return Ok(entry);
        }

        public class RequestMedicament
        {
            public AuthData AuthData { get; set; }
            public Medicament Model { get; set; }
        }
    }
}
