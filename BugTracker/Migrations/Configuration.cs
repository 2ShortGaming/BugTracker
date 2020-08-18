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
                }, "Abc&123!");

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

                var userId = userManager.FindByEmail("arussell@coderfoundry.com").Id;
                userManager.AddToRole(userId, "Project Manager");
            }

            if (!context.Users.Any(u => u.Email == "dev@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "dev@mailinator.com",
                    UserName = "dev@mailinator.com",
                    FirstName = "Aydin",
                    LastName = "Swaney",
                    AvatarPath = "/Avatars/default_user.png",
                }, "Abc&123!");

                var userId = userManager.FindByEmail("dev@mailinator.com").Id;
                userManager.AddToRole(userId, "Developer");
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

                var userId = userManager.FindByEmail("sub@mailinator.com").Id;
                userManager.AddToRole(userId, "Submitter");
            }

            if (!context.Users.Any(u => u.Email == "dg@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "dg@mailinator.com",
                    UserName = "dg@mailinator.com",
                    FirstName = "Dick",
                    LastName = "TGrayson",
                    AvatarPath = "/Avatars/default_user.png",
                }, "Abc&123!");

                var userId = userManager.FindByEmail("dg@mailinator.com").Id;
                userManager.AddToRole(userId, "Submitter");
            }

            if (!context.Users.Any(u => u.Email == "bw@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "bw@mailinator.com",
                    UserName = "bw@mailinator.com",
                    FirstName = "Bruce",
                    LastName = "Wayne",
                    AvatarPath = "/Avatars/default_user.png",
                }, "Abc&123!");

                var userId = userManager.FindByEmail("bw@mailinator.com").Id;
                userManager.AddToRole(userId, "Submitter");
            }

            if (!context.Users.Any(u => u.Email == "jt@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "jt@mailinator.com",
                    UserName = "jt@mailinator.com",
                    FirstName = "Jason",
                    LastName = "Todd",
                    AvatarPath = "/Avatars/default_user.png",
                }, "Abc&123!");

                var userId = userManager.FindByEmail("jt@mailinator.com").Id;
                userManager.AddToRole(userId, "Submitter");
            }

            if (!context.Users.Any(u => u.Email == "mj@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "mj@mailinator.com",
                    UserName = "mj@mailinator.com",
                    FirstName = "Mr.",
                    LastName = "J",
                    AvatarPath = "/Avatars/default_user.png",
                }, "Abc&123!");

                var userId = userManager.FindByEmail("mj@mailinator.com").Id;
                userManager.AddToRole(userId, "Developer");
            }

            if (!context.Users.Any(u => u.Email == "sk@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "sk@mailinator.com",
                    UserName = "sk@mailinator.com",
                    FirstName = "Selena",
                    LastName = "Kyle",
                    AvatarPath = "/Avatars/default_user.png",
                }, "Abc&123!");

                var userId = userManager.FindByEmail("sk@mailinator.com").Id;
                userManager.AddToRole(userId, "Developer");
            }

            if (!context.Users.Any(u => u.Email == "td@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "td@mailinator.com",
                    UserName = "td@mailinator.com",
                    FirstName = "Tim",
                    LastName = "Drake",
                    AvatarPath = "/Avatars/default_user.png",
                }, "Abc&123!");

                var userId = userManager.FindByEmail("td@mailinator.com").Id;
                userManager.AddToRole(userId, "Developer");
            }

            if (!context.Users.Any(u => u.Email == "bg@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "bg@mailinator.com",
                    UserName = "bg@mailinator.com",
                    FirstName = "Barbara",
                    LastName = "Gordon",
                    AvatarPath = "/Avatars/default_user.png",
                }, "Abc&123!");

                var userId = userManager.FindByEmail("bg@mailinator.com").Id;
                userManager.AddToRole(userId, "Developer");
            }
            context.SaveChanges();
            #region TicketType Seed
            context.TicketTypes.AddOrUpdate(
                tt => tt.Name,
                new TicketType() { Name = "Software" },
                new TicketType() { Name = "Hardware" },
                new TicketType() { Name = "UI" },
                new TicketType() { Name = "Defect" },
                new TicketType() { Name = "Other" },
                new TicketType() { Name = "Feature Request" }
                );
            #endregion

            #region TicketPriority Seed
            context.TicketPriorities.AddOrUpdate(
               tp => tp.Name,
               new TicketPriority() { Name = "Low" },
               new TicketPriority() { Name = "Medium" },
               new TicketPriority() { Name = "High" },
               new TicketPriority() { Name = "On Hold" }

               );
            #endregion

            #region TicketStatus Seed
            context.TicketStatuses.AddOrUpdate(
               ts => ts.Name,
               new TicketStatus() { Name = "Open" },
               new TicketStatus() { Name = "Assigned" },
               new TicketStatus() { Name = "Resolved" },
               new TicketStatus() { Name = "Reopened" },
               new TicketStatus() { Name = "Archived" }

               );
            #endregion
            #region Project Seed
            context.Projects.AddOrUpdate(
                p => p.Name,
                new Project() { Name = "Seed 1", Created = DateTime.Now.AddDays(-60), IsArchived = true },
                new Project() { Name = "Seed 2", Created = DateTime.Now.AddDays(-45) },
                new Project() { Name = "Seed 3", Created = DateTime.Now.AddDays(-30) },
                new Project() { Name = "Seed 4", Created = DateTime.Now.AddDays(-15) },
                new Project() { Name = "Seed 5", Created = DateTime.Now.AddDays(-7) }

                );
            #endregion
            context.SaveChanges();

            //#region Ticket Seed
            //context.Tickets.AddOrUpdate(
            //    t => t.Name,
            //    new Ticket() { Name = "Tick Seed 1", Created = DateTime.Now.AddDays(-30) },
            //    new Ticket() { Name = "Tick Seed 2", Created = DateTime.Now.AddDays(-30) },
            //    new Ticket() { Name = "Tick Seed 3", Created = DateTime.Now.AddDays(-20) },
            //    new Ticket() { Name = "Tick Seed 4", Created = DateTime.Now.AddDays(-18) },
            //    new Ticket() { Name = "Tick Seed 5", Created = DateTime.Now.AddDays(-17) },
            //    new Ticket() { Name = "Tick Seed 6", Created = DateTime.Now.AddDays(-15) },
            //    new Ticket() { Name = "Tick Seed 7", Created = DateTime.Now.AddDays(-10) },
            //    new Ticket() { Name = "Tick Seed 8", Created = DateTime.Now.AddDays(-5) }


            //    );


            //#endregion

            //context.SaveChanges();
        }
    }
}
