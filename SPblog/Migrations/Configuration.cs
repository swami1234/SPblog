namespace SPblog.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using SPblog.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            var userManager = new UserManager<ApplicationUser>( new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "sarveshpatel123@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "sarveshpatel123@gmail.com",
                    Email = "sarveshpatel123@gmail.com",
                    FirstName = "Sarvesh",
                    LastName = "Patel",
                    DisplayName = "Sarvesh Patel"
                }, "Abc#123");
            }
            var userId = userManager.FindByEmail("sarveshpatel123@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");

             roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {
                roleManager.Create(new IdentityRole { Name = "Moderator" });
            }

             userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "sarveshpatel1234@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "sarveshpatel1234@gmail.com",
                    Email = "sarveshpatel1234@gmail.com",
                    FirstName = "Sarvesh4",
                    LastName = "Patel4",
                    DisplayName = "Sarvesh Patel4"
                }, "Abc#123");
            }
             userId = userManager.FindByEmail("sarveshpatel123@gmail.com").Id;
            userManager.AddToRole(userId, "Moderator");
        }
    }
}
