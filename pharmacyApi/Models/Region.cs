namespace pharmacyApi.Models {
    public class Region {
        public int Id { get; set; } // идентификатор 
        public string Login { get; set; }  // логин
        public string Password { get; set; } // пароль 
        public string Name { get; set; } // наименование
        public Country Country { get; set; } // связь с государством
        public int CountryId { get; set; } // Id гоусдарства
    }
}
