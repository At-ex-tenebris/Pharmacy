namespace pharmacyApi.Models {
    public class City
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string CityName { get; set; }
        public Region Region { get; set; }
        public int RegionId { get; set; }
    }
}
