using pharmacyApi.Models;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;
using pharmacyApi.Models;
using pharmacyApi.Utils;

namespace pharmacyApi.Data {
    public class DbInitializer {

        public static Administrator AddAdminstrator(ApplicationContext db, string login, string password) {
            var administrator = new Administrator() { Id = 0, Login = login, Password = PasswordHasher.Hash(password) };
            db.Administrators.Add(administrator);
            return administrator;
        }

        public static Country AddCountry(ApplicationContext db, string name, string login, string password) {
            var country = new Country(){
                Id = 0,
                CountryName = name,
                Login = login,
                Password = PasswordHasher.Hash(password)
            };
            db.Countries.Add(country);
            return country;
        }
        
        public static Region AddRegion(ApplicationContext db, string name, Country country, string login, string password){
            var region = new Region(){
                Id = 0,
                Name = name,
                Country = country,
                Login = login,
                Password = PasswordHasher.Hash(password)
            };
            db.Regions.Add(region);
            return region;
        }
        public static City AddCity(ApplicationContext db, string name, Region region, string login, string password){
            var city = new City(){
                Id = 0,
                CityName = name,
                Region = region,
                Login = login,
                Password = PasswordHasher.Hash(password)
            };
            db.Cities.Add(city);
            return city;
        }
        public static Pharmacy AddPharmacy(ApplicationContext db, string name, City city, 
            double latitude, double longitude, string login, string password){
            var pharmacy = new Pharmacy(){
                Id = 0,
                PharmacyName = name,
                City = city,
                Latitude = latitude, 
                Longitude = longitude,
                Login = login,
                Password = PasswordHasher.Hash(password)
            };
            db.Pharmacies.Add(pharmacy);
            return pharmacy;
        }

        public static void Initialize(ApplicationContext context) {
            if(!context.Administrators.Any()){
                AddAdminstrator(context, "admin", "admin");
                var country = AddCountry(context, "Россия", "rLogin", "rPassword");
                var region = AddRegion(context, "Белгородская область", country, "belLogin", "belPassword");
                var city = AddCity(context, "Белгород", region, "belgorod", "belgorod");
                var pharmacy = AddPharmacy(context, "Е-аптека, Конева 53", city, 0.0, 0.0, "e-apteka-koneva", "e-apteka-koneva");
                context.SaveChanges();
            }
        }
    }
}
