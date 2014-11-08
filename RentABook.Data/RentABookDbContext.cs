namespace RentABook.Data
{
    using System.Data.Entity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using RentABook.Data.Migrations;
    using RentABook.Models.Poco;

    public class RentABookDbContext : IdentityDbContext<AppUser>
    {
        public RentABookDbContext()
            : base("RentABookConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<RentABookDbContext, Configuration>());
        }

        public static RentABookDbContext Create()
        {
            return new RentABookDbContext();
        }

        public IDbSet<Book> RentABook { get; set; }
    }
}
