namespace pharmacyApi.Models
{
    public class FullCountry
    {

        public int Id { get; set; } //  Поле идентифкатора
        public string Login { get; set; } // Поле логина
        public string Password { get; set; } // Поле пароля
        public string CountryName { get; set; } // Название Государства

        public static FullCountry FromStd(Country obj)
        {
            return new FullCountry { Id = obj.Id, Login = obj.Login, CountryName = obj.CountryName, Password = obj.Password };
        }

    }
}
