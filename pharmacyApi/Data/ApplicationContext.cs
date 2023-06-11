using Microsoft.EntityFrameworkCore;
using pharmacyApi.Models;

namespace pharmacyApi.Data {
    public class ApplicationContext : DbContext {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { 
            //Database.EnsureCreated();
        }
    }
}
