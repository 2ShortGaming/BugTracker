using System;
using System.Collections.Generic;
using System.Linq;
using BugTracker.Models;
using System.Web;
using Microsoft.AspNet.Identity;

namespace BugTracker.Helpers
{
    public class NotificationHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RolesHelper roleHelper = new RolesHelper();
        private ProjectHelper projectHelper = new ProjectHelper();
        private UserHelper userHelper = new UserHelper();
        private TicketHelper ticketHelper = new TicketHelper();
        

        #region general Notification methods
        //public void NotificationRead(int notificationId)
        //{
        //    var notification = GetNotification(notificationId);
        //    notification.IsRead = true;
        //    db.SaveChanges();
        //}
        #endregion

        #region Ticket Notification methods
        public void TicketChangeNotification(Ticket oldTicket, Ticket newTicket)
        {
            TicketDeveloperChange(oldTicket, newTicket);
            TicketClosedNotification(newTicket);
            db.SaveChanges();

        }
        public void TicketClosedNotification(Ticket ticket)
        {
            if (ticket.IsResolved == true)
            {
                foreach (var user in ticketHelper.ListTicketUsers(ticket.Id))
                {
                    var newNotification = new TicketNotification()
                    {
                        TicketId = ticket.Id,
                        UserId = user.Id,
                        Created = DateTime.Now,
                        Subject = $"Ticket Id: {ticket.Id} Has been Closed",
                        Message = $"Hello, {user.FullName} you have been assigned to Ticket: {ticket.Issue} on Project {ticket.Project.Name}",
                        NotificationType = "Ticket"
                    };
                    db.TicketNotifications.Add(newNotification);
                }
                db.SaveChanges();
            }
        }
        private void TicketDeveloperChange(Ticket oldTicket, Ticket newTicket)
        {

            if (oldTicket.DeveloperId != newTicket.DeveloperId && newTicket.DeveloperId != null)
            {
                var newNotification = new TicketNotification()
                {
                    TicketId = newTicket.Id,
                    UserId = newTicket.DeveloperId,
                    Created = DateTime.Now,
                    Subject = $"New Assignment to Ticket Id: {newTicket.Id}",
                    Message = $"Hello, {newTicket.Developer.FullName} you have been assigned to Ticket: {newTicket.Issue} on Project {newTicket.Project.Name}",
                    NotificationType = "Ticket"
                };
                db.TicketNotifications.Add(newNotification);
            }

            if (oldTicket.DeveloperId != null)
            {
                var newNotification = new TicketNotification()
                {
                    TicketId = newTicket.Id,
                    UserId = oldTicket.DeveloperId,
                    Created = DateTime.Now,
                    Subject = $"Removed from Id: {newTicket.Id}",
                    Message = $"Hello, {oldTicket.Developer.FullName} you have been Removed from Ticket: {newTicket.Issue} on Project {newTicket.Project.Name}",
                    NotificationType = "Ticket"
                };
                db.TicketNotifications.Add(newNotification);
            }



        }
        public void TicketNewCommentAdded(TicketComment newComment)
        {
            var CurrUser = db.Users.Find(HttpContext.Current.User.Identity.GetUserId());
            foreach (var user in ticketHelper.ListTicketUsers(newComment.Ticket.Id))
            {
                var newNotification = new TicketNotification()
                {
                    TicketId = newComment.TicketId,
                    UserId = user.Id,
                    Created = DateTime.Now,
                    Subject = $"New Comment Added to  {newComment.TicketId}",
                    Message = $"Hello {user.FullName}, A new Comment has been added to the Ticket: {newComment.TicketId}, by {CurrUser.FullName}",
                    NotificationType = "Ticket"
                };
                db.TicketNotifications.Add(newNotification);
            }
            db.SaveChanges();

        }
        public void TicketNewAttachmentAdded(TicketAttachment newAttachment)
        {
            var CurrUser = db.Users.Find(HttpContext.Current.User.Identity.GetUserId());

            foreach (var user in ticketHelper.ListTicketUsers(newAttachment.Ticket.Id))
            {
                var newNotification = new TicketNotification()
                {
                    TicketId = newAttachment.TicketId,
                    UserId = user.Id,
                    Created = DateTime.Now,
                    Subject = $"New Comment Added to  {newAttachment.TicketId}",
                    Message = $"Hello {user.FullName}, A new Comment has been added to the Ticket: {newAttachment.TicketId}, by {CurrUser.FullName}",
                    NotificationType = "Ticket"
                };
                db.TicketNotifications.Add(newNotification);
            }
            db.SaveChanges();

        }
        public void NewTicketNotification(Ticket newTicket)
        {
            var projectManager = projectHelper.ListUserOnProjectInRole(newTicket.ProjectId, "ProjectManager").FirstOrDefault();
            var newNotification = new TicketNotification()
            {
                TicketId = newTicket.Id,
                UserId = projectManager.Id,
                Created = DateTime.Now,
                Subject = $"New Ticket has been Created Id: {newTicket.Id}",
                Message = $"Hello {projectManager.FullName}, A New Ticket new Ticket has been Created {newTicket.Issue} on Project {newTicket.Project.Name}"
            };
            db.TicketNotifications.Add(newNotification);
            db.SaveChanges();
        }
        #endregion
        //public Notification GetNotification(int notificationId)
        //{
        //    return db.Notifications.Find(notificationId);
        //}
        //#region Project Notification Methods
        //public void ProjectChangedNotification(Project oldProject, Project newProject)
        //{
        //    ProjectUserChange(oldProject, newProject);
        //    db.SaveChanges();
        //}
        //public void ProjectUserChange(Project oldProject, Project newProject)
        //{
        //    var addedUsers = newProject.Users.Except(oldProject.Users);
        //    var removedUsers = oldProject.Users.Except(newProject.Users);
        //    foreach (var user in addedUsers)
        //    {
        //        var newNotification = new ProjectNotification()
        //        {
        //            ProjectId = newProject.Id,
        //            UserId = user.Id,
        //            Created = DateTime.Now,
        //            Subject = $"Added to Project: {newProject.Name}",
        //            Message = $"Hello, {user.FullName} you have been Added to the project: {newProject.Name}",
        //            NotificationType = "Project"
        //        };
        //        db.ProjectNotifications.Add(newNotification);
        //    }
        //    foreach (var user in removedUsers)
        //    {
        //        var newNotification = new ProjectNotification()
        //        {
        //            ProjectId = newProject.Id,
        //            UserId = user.Id,
        //            Created = DateTime.Now,
        //            Subject = $"Removed from Project: {newProject.Name}",
        //            Message = $"Hello, {user.FullName} you have been removed from the project: {newProject.Name}",
        //            NotificationType = "Project"
        //        };
        //        db.ProjectNotifications.Add(newNotification);
        //    }



        //}
        //public void NewProjectCreated(Project project)
        //{
        //    var userId = HttpContext.Current.User.Identity.GetUserId();
        //    foreach (var user in project.Users.Where(u => u.Id != userId))
        //    {
        //        var newNotification = new ProjectNotification()
        //        {
        //            ProjectId = project.Id,
        //            UserId = user.Id,
        //            Created = DateTime.Now,
        //            Subject = $"Added to Project Id: {project.Id}",
        //            Message = $"Hello, {user.FullName} you have been Added to the project: {project.Name}",
        //            NotificationType = "Project"
        //        };
        //        db.ProjectNotifications.Add(newNotification);

        //    }
        //    db.SaveChanges();
        //}
        //#endregion
    }
}