namespace BugTracker.Migrations
{
    using BugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
               new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }
            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "brandon.o.swaney@gmail.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "brandon.o.swaney@gmail.com",
                    UserName = "brandon.o.swaney@gmail.com",
                    FirstName = "Brandon",
                    LastName = "Swaney",
                    AvatarPath = "/Avatars/default_user.png",
                }, "Biggin112!");

                //get the id that just created by adding the above user
                var userId = userManager.FindByEmail("brandon.o.swaney@gmail.com").Id;

                userManager.AddToRole(userId, "Admin");
            }

            if (!context.Users.Any(u => u.Email == "arussell@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "arussell@coderfoundry.com",
                    UserName = "arussell@coderfoundry.com",
                    FirstName = "Andrew",
                    LastName = "Russell",
                    AvatarPath = "/Avatars/default_user.png",
                }, "Abc&123!");

                var modId = userManager.FindByEmail("arussell@coderfoundry.com").Id;
                userManager.AddToRole(modId, "Project Manager");
            }

            if (!context.Users.Any(u => u.Email == "dev@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "dev@mailinator.com",
                    UserName = "dev@mailinator.com",
                    FirstName = "Brandon",
                    LastName = "Swaney",
                    AvatarPath = "/Avatars/default_user.png",
                }, "Abc&123!");

                var modId = userManager.FindByEmail("dev@mailinator.com").Id;
                userManager.AddToRole(modId, "Developer");
            }

            if (!context.Users.Any(u => u.Email == "sub@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "sub@mailinator.com",
                    UserName = "sub@mailinator.com",
                    FirstName = "Jason",
                    LastName = "Twichell",
                    AvatarPath = "/Avatars/default_user.png",
                }, "Abc&123!");

                var modId = userManager.FindByEmail("sub@mailinator.com").Id;
                userManager.AddToRole(modId, "Submitter");
            }
        }
    }
}
