using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProjektZespolowy.Models;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using ProjektZespolowy.Models.AirPort;
using ProjektZespolowy.Models.Passengers;

namespace IdentitySample.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("Entity", throwIfV1Schema: false)
        {
            
        }

        static ApplicationDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<AirLine> AirLines { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<AirRoute> AirRoutes { get; set; }

        public System.Data.Entity.DbSet<ProjektZespolowy.Models.Flight> Flights { get; set; }

        public System.Data.Entity.DbSet<ProjektZespolowy.Models.Ticket> Tickets { get; set; }
    }
}