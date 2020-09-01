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
        private HistoryHelper historyHelper = new HistoryHelper();
        private ProjectHelper projectHelper = new ProjectHelper();
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

        public void EditedTicket(Ticket oldTicket, Ticket newTicket)
        {
            historyHelper.ManageHistories(oldTicket, newTicket);
            ManageTicketNotifications(oldTicket, newTicket);
        }

        public List<Ticket> GetMyTickets()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            switch (myRole)
            {
                case "Admin":
                    return db.Tickets.ToList();
                case "Project Manager":
                    var ticketList = new List<Ticket>();
                    foreach (var project in projectHelper.ListUserProjects(userId).ToList())
                    {
                        ticketList.AddRange(project.Tickets.ToList());
                    }
                    return ticketList;
                case "Developer":
                    return db.Tickets.Where(t => t.DeveloperId == userId).ToList();
                case "Submitter":
                    return db.Tickets.Where(t => t.SubmitterId == userId).ToList();
                default:
                    return null;
            }
        }

        public bool MyTicket(int id)
        {
            var allowed = false;
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var ticket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == id);
            var userRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            switch (userRole)
            {
                case "Admin":
                    allowed = true;
                    break;
                case"Developer":
                        allowed = ticket.DeveloperId == userId ? true : false;
                    break;

                case "Submitter":
                    allowed = ticket.SubmitterId == userId ? true : false;
                    break;
                case "Project Manager":
                    var user = db.Users.Find(userId);
                   allowed = user.Projects.SelectMany(p => p.Tickets).Select(t => t.Id).Contains(id);
                    break;
            }
            return allowed;
        }
        public List<ApplicationUser> ListTicketUsers(int TicketId)
        {
            var UserList = new List<ApplicationUser>();
            var ticket = db.Tickets.Find(TicketId);
            UserList.Add(ticket.Developer);
            UserList.Add(ticket.Submitter);
            UserList.Add(projectHelper.ListUserOnProjectInRole(ticket.ProjectId, "ProjectManager").FirstOrDefault());
            return UserList;
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
                    Message = $"Heads up {newTicket.Developer.FullName}, you have been assigned to Ticket Id: {newTicket.Id} on Project {newTicket.Project.Name} "
                    //Body = $"Heads up {newTicket.Developer.FullName}, you have been assigned to Ticket Id: {newTicket.Id} titled '{newTicket.Title}' on Project {newTicket.Project.Name} "
                };
                db.TicketNotifications.Add(newNotification);
                db.SaveChanges();
            }
        }


        public List<Ticket> GetAllProjectTicketsForUser(string userId)
        {
            var user = db.Users.Find(userId);
            var ticketList = new List<Ticket>();
            return user.Projects.SelectMany(p => p.Tickets).ToList();
        }

        public List<Ticket> GetProjectTickets()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var ticketList = new List<Ticket>();
            ticketList = user.Projects.SelectMany(p => p.Tickets).ToList();
            return ticketList;
        }

    }
}