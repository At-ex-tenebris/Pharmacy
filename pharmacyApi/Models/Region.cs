using Newtonsoft.Json;

namespace pharmacyApi.Models {
    public class Region {
        public int Id { get; set; } // идентификатор 
        [JsonIgnore]
        public string Login { get; set; }  // логин
        [JsonIgnore]
        public string Password { get; set; } // пароль 
        public string Name { get; set; } // наименование
        public Country? Country { get; set; } // связь с государством
        public int CountryId { get; set; } // Id гоусдарства
        public static Region FromFull(FullRegion obj)
        {
            return new Region { Id = obj.Id, Login = obj.Login, Country = obj.Country, CountryId = obj.CountryId, Name = obj.Name, Password = obj.Password };
        }
    }
}
