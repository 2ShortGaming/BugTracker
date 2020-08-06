using BugTracker.Helpers;
using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class AssignmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RolesHelper roleHelper = new RolesHelper();
        // GET: Assignments
        //[Authorize(Roles = "Admin")]
        public ActionResult ManageRoles()
        {
            //Use ViewBag tp hold a multi select list of Users
            //new multiselectlist(the data itself, "Id", "Email")
            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "Email");

            //use ViewBag to hold a select list of roles
            ViewBag.RoleName = new SelectList(db.Roles, "Name", "Name");

         return View(db.Users.ToList());
        }

        //POST: Assignments
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageRoles(List<string> userIds, string roleName)
        {
            //step 1 - if anyone was selected, remover them from all of their roles
            if (userIds == null)
                return RedirectToAction("RolesIndex");
            //if ppl were selected, go through them and strip them of their roles
            foreach (var userId in userIds)
            {
                foreach(var role in roleHelper.ListUserRoles(userId).ToList())
                {
                    roleHelper.RemoveUserFromRole(userId, role);
                }
                if(!string.IsNullOrEmpty(roleName))
                {
                    roleHelper.AddUserToRole(userId, roleName);
                }
            }
            //step 2 - if a role is chose, add each person to that role
            return RedirectToAction("ManageRoles");

        }



        public ActionResult ManageUserRoles()
        {
            return View();
        }
    }
}