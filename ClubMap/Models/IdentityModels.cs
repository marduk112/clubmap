using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using ClubMap.DbModels;
using ClubMap.DbModels.Sqlite;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ClubMap.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string DeviceId { get; set; }
        [ForeignKey("IconId")]
        public Image Icon { get; set; }
        public int? IconId { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            //Configuration.LazyLoadingEnabled = false;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Log> Logs { get; set; }
        public DbSet<Check> Checks { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Message> Messages { get; set; } 
    }

    public class SqLiteDbContext : DbContext
    {
        public SqLiteDbContext()
            : base("Sqlite"/*"DefaultConnection2"*/)
        {
            //comment this line if you set DefaultConnection2 as name of connection string
            Database.SetInitializer<SqLiteDbContext>(null);
        }
        public DbSet<Cafe> Cafes { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Pub> Pubs { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<MusicKind> MusicKinds { get; set; }
        public DbSet<AgeKind> AgeKinds { get; set; }
        public DbSet<EnterPriceDay> EnterPriceDays { get; set; }
        public DbSet<FoodType> FoodTypes { get; set; }
        public DbSet<CafeType> CafeTypes { get; set; } 
        public DbSet<Price> Prices { get; set; } 
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<VariableQueueData> VariableQueueDatas { get; set; } 
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Conventions
        //        .Remove<PluralizingTableNameConvention>();
        //}
    }
}