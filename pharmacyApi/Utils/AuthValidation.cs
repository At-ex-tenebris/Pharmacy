using pharmacyApi.Data;
using pharmacyApi.Models;

namespace pharmacyApi.Utils{
    public class AuthValidation{
        const string ADMIN_AUTH_TYPE    = "admin";
        const string COUNTRY_AUTH_TYPE  = "country";
        const string REGION_AUTH_TYPE   = "region";
        const string CITY_AUTH_TYPE     = "city";
        const string PHARMACY_AUTH_TYPE = "pharmacy";

        public static bool isValidAdmin(ApplicationContext db, AuthData authData, string authType){
            var login = authData.Login; 
            var password = PasswordHasher.Hash(authData.Password);
            switch (authType) {
                case ADMIN_AUTH_TYPE:
                    return db.Administrators.Any(x => x.Login == login && x.Password == password);
                default:
                    return false;
            }
        }

        public static bool isValid(ApplicationContext db, Country country, 
            AuthData authData, string authType, bool selfAuth = true){
            var login = authData.Login; 
            var password = PasswordHasher.Hash(authData.Password);
            switch (authType) {
                case ADMIN_AUTH_TYPE:
                    return db.Administrators.Any(x => x.Login == login && x.Password == password);
                case COUNTRY_AUTH_TYPE:
                    return country.Login == login && country.Password == password && selfAuth;
                default:
                    return false;
            }
        }
        
        public static bool isValid(ApplicationContext db, Region region, 
            AuthData authData, string authType, bool selfAuth = true){
            var login = authData.Login; 
            var password = PasswordHasher.Hash(authData.Password);
            switch (authType) {
                case ADMIN_AUTH_TYPE:
                    return db.Administrators.Any(x => x.Login == login && x.Password == password);
                case COUNTRY_AUTH_TYPE:
                    return region.Country.Login == login && region.Country.Password == password;
                case REGION_AUTH_TYPE:
                    return region.Login == login && region.Password == password && selfAuth;
                default:
                    return false;
            }
        }

        public static bool isValid(ApplicationContext db, City city, 
            AuthData authData, string authType, bool selfAuth = true){
            var login = authData.Login; 
            var password = PasswordHasher.Hash(authData.Password);
            switch (authType) {
                case ADMIN_AUTH_TYPE:
                    return db.Administrators.Any(x => x.Login == login && x.Password == password);
                case COUNTRY_AUTH_TYPE:
                    return city.Region.Country.Login == login && city.Region.Country.Password == password;
                case REGION_AUTH_TYPE:
                    return city.Region.Login == login && city.Region.Password == password;
                case CITY_AUTH_TYPE:
                    return city.Login == login && city.Password == password && selfAuth;
                default:
                    return false;
            }
        }

        public static bool isValid(ApplicationContext db, Pharmacy pharmacy, 
            AuthData authData, string authType, bool selfAuth = true){
            var login = authData.Login; 
            var password = PasswordHasher.Hash(authData.Password);
            switch (authType) {
                case ADMIN_AUTH_TYPE:
                    return db.Administrators.Any(x => x.Login == login && x.Password == password);
                case COUNTRY_AUTH_TYPE:
                    return pharmacy.City.Region.Country.Login == login && pharmacy.City.Region.Country.Password == password;
                case REGION_AUTH_TYPE:
                    return pharmacy.City.Region.Login == login && pharmacy.City.Region.Password == password;
                case CITY_AUTH_TYPE:
                    return pharmacy.City.Login == login && pharmacy.City.Password == password;
                case PHARMACY_AUTH_TYPE:
                    return pharmacy.Login == login && pharmacy.Password == password && selfAuth;
                default:
                    return false;
            }
        }

        public static bool isValid(ApplicationContext db, Medicament medicament, 
            AuthData authData, string authType){
            return isValid(db, medicament.Pharmacy, authData, authType);
        }
    }
}
