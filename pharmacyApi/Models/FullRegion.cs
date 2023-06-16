namespace pharmacyApi.Models
{
    public class FullRegion
    {
        public int Id { get; set; } // идентификатор 
        public string Login { get; set; }  // логин
        public string Password { get; set; } // пароль 
        public string Name { get; set; } // наименование
        public Country? Country { get; set; } // связь с государством
        public int CountryId { get; set; } // Id гоусдарства

        public static FullRegion FromStd(Region obj)
        {
            return new FullRegion { Id = obj.Id, Login = obj.Login, Country = obj.Country, CountryId = obj.CountryId, Name = obj.Name, Password = obj.Password };
        }
    }
}
