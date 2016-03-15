namespace Ot_Sims_Givebox.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Ot_Sims_Givebox.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;

    internal sealed class Configuration : DbMigrationsConfiguration<Ot_Sims_Givebox.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Ot_Sims_Givebox.Models.ApplicationDbContext context)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var user = new ApplicationUser()
            {
                Id = "superUser",
                Email = "simon@rispal.info",
                EmailConfirmed = true,
                UserName = "simon@rispal.info"
            };

            manager.Create(user, "Azerty2302");

            var user2 = new ApplicationUser()
            {
                Id = "normalUser",
                Email = "robert@rispal.info",
                EmailConfirmed = true,
                UserName = "robert@rispal.info"
            };

            manager.Create(user2, "Azerty2302");
            
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            if (roleManager.Roles.Count() == 0)
            {
                roleManager.Create(new IdentityRole { Name = "SuperAdmin" });
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "User" });
            }


            var adminUser = manager.FindByName("simon@rispal.info");
            var normalUser = manager.FindByName("robert@rispal.info");
            manager.AddToRoles(adminUser.Id, new string[] { "SuperAdmin", "Admin" });
            manager.AddToRoles(normalUser.Id, new string[] { "User" });
        }
    }
}
