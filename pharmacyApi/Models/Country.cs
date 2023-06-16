using Newtonsoft.Json;

namespace pharmacyApi.Models {
    public class Country {

        public int Id { get; set; } //  Поле идентифкатора
        [JsonIgnore]
        public string Login { get; set; } // Поле логина
        [JsonIgnore]
        public string Password { get; set; } // Поле пароля
        public string CountryName { get; set; } // Название Государства

        public static Country FromFull(FullCountry obj)
        {
            return new Country { Id = obj.Id, Login = obj.Login, CountryName = obj.CountryName, Password = obj.Password };
        }
    }
}
