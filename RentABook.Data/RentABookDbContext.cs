namespace RentABook.Data
{
    using System.Data.Entity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using RentABook.Data.Migrations;
    using RentABook.Models.Poco;
    using RentABook.Models.Mapping;

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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AddressMap());
            modelBuilder.Configurations.Add(new BookMap());
            modelBuilder.Configurations.Add(new BookHistoryMap());
            modelBuilder.Configurations.Add(new BookRentMap());
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new FavouriteMap());
            modelBuilder.Configurations.Add(new FeedbackMap());
            modelBuilder.Configurations.Add(new RentRequestMap());
            modelBuilder.Configurations.Add(new TownMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
