using Newtonsoft.Json;

namespace pharmacyApi.Models {
    public class Pharmacy {
		public int Id { get; set; }     //Поле идентифкатора
		[JsonIgnore]
		public string Login { get; set; }       //Поле логина
		[JsonIgnore]
		public string Password { get; set; }    //Поле пароля
		public string PharmacyName { get; set; }    //Поле наименования
		public double Latitude { get; set; }        //Поле широты(в градусах)
		public double Longitude { get; set; }       //Поле долготы(в градусах)
		public City? City { get; set; }
		public int CityId { get; set; }
        public static Pharmacy FromFull(FullPharmacy obj)
        {
            return new Pharmacy { 
				Id = obj.Id, 
				Login = obj.Login,
				Password = obj.Password,
				PharmacyName = obj.PharmacyName, 
				Latitude = obj.Latitude, 
				Longitude = obj.Longitude,
				City = obj.City,
				CityId = obj.CityId};
        }
    }
}
