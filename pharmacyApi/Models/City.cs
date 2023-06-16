using Newtonsoft.Json;

namespace pharmacyApi.Models
{
    public class City
    {
        public int Id { get; set; }
        [JsonIgnore]
        public string Login { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public string CityName { get; set; }
        public Region? Region { get; set; }
        public int RegionId { get; set; }

        public static City StdFrom(FullCity fullcity)
        {
            return new City { Id = fullcity.Id, Login = fullcity.Login, Password = fullcity.Password, CityName = fullcity.CityName, Region = fullcity.Region, RegionId = fullcity.RegionId };
        }
    }
    
}