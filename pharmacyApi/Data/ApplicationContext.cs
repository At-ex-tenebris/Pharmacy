using Microsoft.EntityFrameworkCore;
using pharmacyApi.Models;

namespace pharmacyApi.Data {
    public class ApplicationContext : DbContext {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { 
            //Database.EnsureCreated();
        }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Country>       Countries { get; set; }
        public DbSet<Region>        Regions { get; set; }
        public DbSet<City>          Cities { get; set; }
        public DbSet<Pharmacy>      Pharmacies { get; set; }
        public DbSet<Medicament>    Medicaments { get; set; }
    }
}
