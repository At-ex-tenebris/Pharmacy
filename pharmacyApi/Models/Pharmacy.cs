namespace pharmacyApi.Models {
    public class Pharmacy {
		public int Id { get; set; }     //Поле идентифкатора
		public string Login { get; set; }       //Поле логина
		public string Password { get; set; }    //Поле пароля
		public string PharmacyName { get; set; }    //Поле наименования
		public double Latitude { get; set; }        //Поле широты(в градусах)
		public double Longitude { get; set; }       //Поле долготы(в градусах)
		public City City { get; set; }
		public int CityId { get; set; }
	}
}
