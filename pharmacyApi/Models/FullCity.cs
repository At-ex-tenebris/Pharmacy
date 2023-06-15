namespace pharmacyApi.Models
{
    public class FullCity
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string CityName { get; set; }
        public Region Region { get; set; }
        public int RegionId { get; set; }

        public static FullCity fromStd(City city) 
        { 
            return new FullCity{ Id = city.Id, Login = city.Login, Password = city.Password, CityName = city.CityName, Region = city.Region, RegionId = city.RegionId };
        }
    }
}
