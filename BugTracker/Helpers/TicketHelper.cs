using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using System.Web;
using BugTracker.Models;

namespace BugTracker.Helpers
{
    public class TicketHelper
    {
        private RolesHelper roleHelper = new RolesHelper();
        private ApplicationDbContext db = new ApplicationDbContext();
        public bool CanMakeComment(int ticketId)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            var ticket = db.Tickets.Find(ticketId);
            switch (myRole)
            {
                case "Admin":
                    return true;
                case "Project Manager":
                    var user = db.Users.Find(userId);
                    return (user.Projects.SelectMany(p => p.Tickets).Any(t => t.Id == ticketId));
                case "Developer":
                case "Submitter":
                    if(ticket.DeveloperId == userId || ticket.SubmitterId == userId)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                default:
                    return false;
            }
        }

        public bool CanEditComment(int ticketId)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            var ticket = db.Tickets.Find(ticketId);
            switch (myRole)
            {
                case "Admin":
                    return true;
                case "Project Manager":
                    var user = db.Users.Find(userId);
                    return (user.Projects.SelectMany(p => p.Tickets).Any(t => t.Id == ticketId));
                case "Developer":
                case "Submitter":
                    if (ticket.DeveloperId == userId || ticket.SubmitterId == userId)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                default:
                    return false;
            }
        }

        public bool CanEditTicket(int ticketId)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            var ticket = db.Tickets.Find(ticketId);
            switch (myRole)
            {
                case "Admin":
                    return true;
                case "Project Manager":
                    var user = db.Users.Find(userId);
                    return (user.Projects.SelectMany(p => p.Tickets).Any(t => t.Id == ticketId));
                case "Developer":
                case "Submitter":
                    if (ticket.DeveloperId == userId || ticket.SubmitterId == userId)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                default:
                    return false;
            }
        }

        public void ManageTicketNotifications(Ticket oldTicket, Ticket newTicket)
        {
            if (oldTicket.DeveloperId != newTicket.DeveloperId && newTicket.DeveloperId != null)
            {
                var newNotification = new TicketNotification()
                {
                    UserId = newTicket.DeveloperId,
                    Created = DateTime.Now,
                    TicketId = newTicket.Id,
                    Message = /*"You have been assigned Ticket Id: {newTicket.Id}",*/ $"Heads up {newTicket.Developer.FullName}, you have been assigned to Ticket Id: {newTicket.Id} on Project {newTicket.Project.Name} "
                    //Body = $"Heads up {newTicket.Developer.FullName}, you have been assigned to Ticket Id: {newTicket.Id} titled '{newTicket.Title}' on Project {newTicket.Project.Name} "
                };
                db.TicketNotifications.Add(newNotification);
                db.SaveChanges();

            }
        }

    }
}