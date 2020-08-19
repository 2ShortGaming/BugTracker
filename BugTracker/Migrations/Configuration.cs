namespace BugTracker.Migrations
{
    using BugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using BugTracker.Helpers;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Configuration;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        private ProjectHelper projectHelper = new ProjectHelper(); 
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

            if (!context.Roles.Any(r => r.Name == "DemoAdmin"))
            {
                roleManager.Create(new IdentityRole { Name = "DemoAdmin" });
            }
            if (!context.Roles.Any(r => r.Name == "DemoPM"))
            {
                roleManager.Create(new IdentityRole { Name = "DemoPM" });
            }
            if (!context.Roles.Any(r => r.Name == "DemoDev"))
            {
                roleManager.Create(new IdentityRole { Name = "DemoDev" });
            }
            if (!context.Roles.Any(r => r.Name == "DemoSub"))
            {
                roleManager.Create(new IdentityRole { Name = "DemoSub" });
            }

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            var demoAdminPassword = WebConfigurationManager.AppSettings["DemoAdminPassword"];
            var demoPMPassword = WebConfigurationManager.AppSettings["DemoPMPassword"];
            var demoDevPassword = WebConfigurationManager.AppSettings["DemoDevPassword"];
            var demoSubPassword = WebConfigurationManager.AppSettings["DemoSubPassword"];


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

            if (!context.Users.Any(u => u.Email == "DemoAdmin@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "DemoAdmin@mailinator.com",
                    UserName = "DemoAdmin@mailinator.com",
                    FirstName = "Demo",
                    LastName = "Admin",
                    AvatarPath = "/Avatars/default_user.png",
                }, "DemoPassword");

                //get the id that just created by adding the above user
                var userId = userManager.FindByEmail("DemoAdmin@mailinator.com").Id;

                userManager.AddToRole(userId, "DemoAdmin");
            }

            if (!context.Users.Any(u => u.Email == "DemoPM@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "DemoPM@mailinator.com",
                    UserName = "DemoPM@mailinator.com",
                    FirstName = "Demo",
                    LastName = "PM",
                    AvatarPath = "/Avatars/default_user.png",
                }, "DemoPassword");

                //get the id that just created by adding the above user
                var userId = userManager.FindByEmail("DemoPM@mailinator.com").Id;

                userManager.AddToRole(userId, "DemoPM");
            }

            if (!context.Users.Any(u => u.Email == "DemoDev@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "DemoDev@mailinator.com",
                    UserName = "DemoDev@mailinator.com",
                    FirstName = "Demo",
                    LastName = "Dev",
                    AvatarPath = "/Avatars/default_user.png",
                }, "DemoPassword");

                //get the id that just created by adding the above user
                var userId = userManager.FindByEmail("DemoDev@mailinator.com").Id;

                userManager.AddToRole(userId, "DemoDev");
            }

            if (!context.Users.Any(u => u.Email == "DemoSub@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "DemoSub@mailinator.com",
                    UserName = "DemoSub@mailinator.com",
                    FirstName = "Demo",
                    LastName = "Sub",
                    AvatarPath = "/Avatars/default_user.png",
                }, "DemoPassword");

                //get the id that just created by adding the above user
                var userId = userManager.FindByEmail("DemoSub@mailinator.com").Id;

                userManager.AddToRole(userId, "DemoSub");
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

            var userList = context.Users.ToList();
            var projectList = context.Projects.ToList();
            foreach(var project in projectList)
            {
                    foreach( var user in userList)
                {
                        projectHelper.AddUserToProject(user.Id, project.Id);
                  
                }
            }

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
