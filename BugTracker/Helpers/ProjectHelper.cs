using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class ProjectHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RolesHelper roleHelper = new RolesHelper();
        public bool IsUserOnProject(string userId, int projectId)
        {
            Project project = db.Projects.Find(projectId);
            return project.Users.Any(u => u.Id == userId);
        }
        public ICollection<Project> ListUserProjects(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);
            var projects = user.Projects.ToList();
            //resultList.AddRange(user.Projects);
            return (projects);
        }
        public void AddUserToProject(string userId, int projectId)
        {
            if (!IsUserOnProject(userId, projectId))
            {
                var project = db.Projects.Find(projectId);
                var user = db.Users.Find(userId);
                project.Users.Add(user);
                db.SaveChanges();
            }
        }
        public bool RemoveUserFromProject(string userId, int projectId)
        {
            var project = db.Projects.Find(projectId);
            var user = db.Users.Find(userId);
            var result = project.Users.Remove(user);
            db.SaveChanges();
            return result;
        }
        public ICollection<ApplicationUser> ListUsersOnProject(int projectId)
        {
            return db.Projects.Find(projectId).Users;
        }
        public List<ApplicationUser> ListUsersNotOnProject(int projectId)
        {
            var resultList = new List<ApplicationUser>();
            foreach (var user in db.Users.ToList())
            {
                if (!IsUserOnProject(user.Id, projectId))
                {
                    resultList.Add(user);
                }
            }
            return resultList;
        }

        public List<ApplicationUser> ListUserOnProjectInRole(int projectId, string roleName)
        {
            var userList = ListUsersOnProject(projectId);
            var resultList = new List<ApplicationUser>();
            foreach (var user in userList)
            {
                if (roleHelper.IsUserInRole(user.Id, roleName))
                {
                    resultList.Add(user);
                }
            }
            return resultList;
        }

    }
}