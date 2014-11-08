namespace RentABook.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using RentABook.Models.Poco;
    
    internal sealed class Configuration : DbMigrationsConfiguration<RentABookDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(RentABookDbContext context)
        {
            var UserManager = new UserManager<AppUser>(new UserStore<AppUser>(context));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            string name = "SiteAdmin@test.com";
            string password = "123";
            string adminRole = "Admin";
            string superAdminRole = "SuperAdmin";

            RoleManager.Create(new IdentityRole(superAdminRole));

            if (!RoleManager.RoleExists(adminRole))
            {
                var roleresult = RoleManager.Create(new IdentityRole(adminRole));
            }

            var user = new AppUser();
            user.UserName = name;
            var adminresult = UserManager.Create(user, password);

            if (adminresult.Succeeded)
            {
                var result = UserManager.AddToRole(user.Id, superAdminRole);
                result = UserManager.AddToRole(user.Id, adminRole);
            }

            base.Seed(context);
        }
    }
}
