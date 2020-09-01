using System;
using System.Collections.Generic;
using System.Linq;
using BugTracker.Models;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;

namespace BugTracker.Helpers
{
    public class NotificationHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RolesHelper roleHelper = new RolesHelper();
        private ProjectHelper projectHelper = new ProjectHelper();
        private UserHelper userHelper = new UserHelper();
        private TicketHelper ticketHelper = new TicketHelper();
        EmailService svc = new EmailService();
        string from = "SwaneyBugTracker";

        public void MarkAsRead(int id)
        {
            var notification = db.TicketNotifications.Find(id);
            notification.IsRead = true;
            db.SaveChanges();
        }
        public List<TicketNotification> ListUsersNotifications(string userId)
        {
            var currentUserId = HttpContext.Current.User.Identity.GetUserId();
            return db.TicketNotifications.Where(n => n.UserId == currentUserId && !n.IsRead).OrderByDescending(n => n.Created).ToList();
        }
        public async Task ManageNotifications(Ticket oldTicket, Ticket newTicket)
        {
            var ticketAssigned = oldTicket.DeveloperId.IsNullOrWhiteSpace() && !newTicket.DeveloperId.IsNullOrWhiteSpace();
            var ticketUnassigned = !oldTicket.DeveloperId.IsNullOrWhiteSpace() && newTicket.DeveloperId.IsNullOrWhiteSpace();
            var ticketReassigned = !oldTicket.DeveloperId.IsNullOrWhiteSpace() && !newTicket.DeveloperId.IsNullOrWhiteSpace() && oldTicket.DeveloperId != newTicket.DeveloperId;
            if (ticketAssigned)
            {
                await AddAssignmentNotification(newTicket);
            }
            else if(ticketUnassigned)
            {
                await AddUnassignmentNotification(oldTicket);
            }
            else if(ticketReassigned)
            {
                await AddAssignmentNotification(newTicket);
                await AddUnassignmentNotification(oldTicket);
            }
        }
        private async Task AddAssignmentNotification(Ticket newTicket)
        {
            var notification = new TicketNotification
            {
                TicketId = newTicket.Id,
                IsRead = false,
                UserId = newTicket.DeveloperId,
                Created = DateTime.Now,
                Message = $"Hello {newTicket.Developer.FullName}, you have been assigned to Ticket Id {newTicket.Id} titled '{newTicket.Issue}' on Project '{newTicket.Project.Name}'"
            };
            db.TicketNotifications.Add(notification);
            db.SaveChanges();
            var userId = notification.UserId;
            var userEmail = db.Users.Find(userId).Email;
            try
            {
                var email = new MailMessage(from, userEmail)
                {
                    Subject = "Ticket Assignment",
                    Body = notification.Message,
                    IsBodyHtml = true
                };
                await svc.SendAsync(email);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                await Task.FromResult(0);
            };

        }
        private async Task AddUnassignmentNotification(Ticket oldTicket)
        {
            var notification = new TicketNotification
            {
                TicketId = oldTicket.Id,
                IsRead = false,
                UserId = oldTicket.DeveloperId,
                Created = DateTime.Now,
                Message = $"Hello {oldTicket.Developer.FullName}, you have been unassigned from Ticket Id {oldTicket.Id} titled '{oldTicket.Issue}' on Project '{oldTicket.Project.Name}'"
            };
            db.TicketNotifications.Add(notification);
            db.SaveChanges();
            var userId = notification.UserId;
            var userEmail = db.Users.Find(userId).Email;
            try
            {
                var email = new MailMessage(from, userEmail)
                {
                    Subject = "Ticket Unassignment",
                    Body = notification.Message,
                    IsBodyHtml = true
                };
                await svc.SendAsync(email);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                await Task.FromResult(0);
            };

        }



        #region one way
        //public async Task ManageTicketNotifications(Ticket oldTicket, Ticket newTicket)
        //{
        //    if (oldTicket.DeveloperId != newTicket.DeveloperId)
        //    {
        //        if (oldTicket.DeveloperId != null && newTicket.DeveloperId != null)
        //        {
        //            var newAssignmentNotification = new TicketNotification()
        //            {
        //                TicketId = newTicket.Id,
        //                UserId = newTicket.DeveloperId,
        //                Created = DateTime.Now,
        //                Subject = $"Assigned to Ticket Id: {newTicket.Id}",
        //                Message = $"Hello {newTicket.Developer.FullName}, you have been assigned to Ticket Id {newTicket.Id} titled '{newTicket.Issue}' on Project '{newTicket.Project.Name}'"
        //            };
        //            db.TicketNotifications.Add(newAssignmentNotification);
        //            try
        //            {
        //                var email = new MailMessage(from, newAssignmentNotification.User.Email)
        //                {
        //                    Subject = "Ticket Assignment",
        //                    Body = newAssignmentNotification.Message,
        //                    IsBodyHtml = true
        //                };
        //                await svc.SendAsync(email);
        //            }
        //            catch (Exception e)
        //            {
        //                Console.WriteLine(e.Message);
        //                await Task.FromResult(0);
        //            };

        //            var newUnassignmentNotification = new TicketNotification()
        //            {
        //                TicketId = oldTicket.Id,
        //                UserId = oldTicket.DeveloperId,
        //                Created = DateTime.Now,
        //                Subject = $"Unassigned from Ticket Id: {newTicket.Id}",
        //                Message = $"Hello {oldTicket.Developer.FullName}, you have been unassigned from Ticket Id {oldTicket.Id} titled '{oldTicket.Issue}' on Project '{oldTicket.Project.Name}'"
        //            };
        //            db.TicketNotifications.Add(newUnassignmentNotification);
        //            db.SaveChanges();
        //            try
        //            {
        //                var email = new MailMessage(from, newUnassignmentNotification.User.Email)
        //                {
        //                    Subject = "Ticket UnAssignment",
        //                    Body = newUnassignmentNotification.Message,
        //                    IsBodyHtml = true
        //                };
        //                await svc.SendAsync(email);
        //            }
        //            catch (Exception e)
        //            {
        //                Console.WriteLine(e.Message);
        //                await Task.FromResult(0);
        //            };



        //        }
        //        else
        //        {
        //            if (newTicket.DeveloperId != null)
        //            {
        //                var newAssignmentNotification = new TicketNotification()
        //                {
        //                    TicketId = newTicket.Id,
        //                    UserId = newTicket.DeveloperId,
        //                    Created = DateTime.Now,
        //                    Subject = $"Assigned to Ticket Id: {newTicket.Id}",
        //                    Message = $"Hello {newTicket.Developer.FullName}, you have been assigned to Ticket Id {newTicket.Id} titled '{newTicket.Issue}' on Project '{newTicket.Project.Name}'"
        //                };
        //                db.TicketNotifications.Add(newAssignmentNotification);
        //                db.SaveChanges();
        //                try
        //                {
        //                    var email = new MailMessage(from, newAssignmentNotification.User.Email)
        //                    {
        //                        Subject = "Ticket Assignment",
        //                        Body = newAssignmentNotification.Message,
        //                        IsBodyHtml = true
        //                    };
        //                    await svc.SendAsync(email);
        //                }
        //                catch (Exception e)
        //                {
        //                    Console.WriteLine(e.Message);
        //                    await Task.FromResult(0);
        //                };
        //            }
        //            if (oldTicket.DeveloperId != null)
        //            {
        //                var newUnassignmentNotification = new TicketNotification()
        //                {
        //                    TicketId = oldTicket.Id,
        //                    UserId = oldTicket.DeveloperId,
        //                    Created = DateTime.Now,
        //                    Subject = $"Unassigned from Ticket Id: {newTicket.Id}",
        //                    Message = $"Hello {oldTicket.Developer.FullName}, you have been unassigned from Ticket Id {oldTicket.Id} titled '{oldTicket.Issue}' on Project '{oldTicket.Project.Name}'"
        //                };
        //                db.TicketNotifications.Add(newUnassignmentNotification);
        //                db.SaveChanges();
        //                try
        //                {
        //                    var email = new MailMessage(from, newUnassignmentNotification.User.Email)
        //                    {
        //                        Subject = "Ticket UnAssignment",
        //                        Body = newUnassignmentNotification.Message,
        //                        IsBodyHtml = true
        //                    };
        //                    await svc.SendAsync(email);
        //                }
        //                catch (Exception e)
        //                {
        //                    Console.WriteLine(e.Message);
        //                    await Task.FromResult(0);
        //                };
        //            }
        //        }

        //    }
        //    else
        //    {
        //        if (oldTicket != newTicket && newTicket.DeveloperId != null)
        //        {
        //            var newTicketChangeNotification = new TicketNotification()
        //            {
        //                TicketId = newTicket.Id,
        //                UserId = newTicket.DeveloperId,
        //                Created = DateTime.Now,
        //                Subject = "Assigned tickets has been updated",
        //                Message = $"Hello {newTicket.Developer.FullName}, a ticket on Project '{newTicket.Project.Name}' with Ticket Id {newTicket.Id} titled '{newTicket.Issue}' has been changed."
        //            };
        //            db.TicketNotifications.Add(newTicketChangeNotification);
        //            db.SaveChanges();
        //            try
        //            {
        //                var email = new MailMessage(from, newTicketChangeNotification.User.Email)
        //                {
        //                    Subject = "Ticket Change",
        //                    Body = newTicketChangeNotification.Message,
        //                    IsBodyHtml = true
        //                };
        //                await svc.SendAsync(email);
        //            }
        //            catch (Exception e)
        //            {
        //                Console.WriteLine(e.Message);
        //                await Task.FromResult(0);
        //            };
        //        }
        //    }

        //}
        #endregion
    }
}
