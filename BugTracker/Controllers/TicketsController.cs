using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Helpers;
using Microsoft.AspNet.Identity;
using BugTracker.Models;


namespace BugTracker.Controllers
{
    //[Authorize]
    public class TicketsController : Controller
    {
        
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectHelper projectHelper = new ProjectHelper();
        private RolesHelper rolesHelper = new RolesHelper();
        private TicketHelper ticketHelper = new TicketHelper();
        private HistoryHelper historyHelper = new HistoryHelper();
        
        // GET: Tickets
        
        public ActionResult Index()
        {
            //var userId = User.Identity.GetUserId();
            //var myRole = rolesHelper.ListUserRoles(userId).FirstOrDefault();
            //List<Ticket> model = new List<Ticket>();
            //switch (myRole)
            //{
            //    case "Admin":
            //        model = db.Tickets.ToList();
            //        break;
            //    case "Project Manager":
            //    case "Developer":
            //        model = projectHelper.ListUserProjects(userId).SelectMany(p => p.Tickets).ToList();
            //        break;
            //    case "Submitter":
            //        model = db.Tickets.Where(t => t.SubmitterId == userId).ToList();
            //        break;

            //    default:
            //        return RedirectToAction("Index", "Home");
            //}
            //return View(model);
            return View(db.Tickets.ToList());
        }
        
        public ActionResult GetProjectTickets()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var ticketList = new List<Ticket>();
            ticketList = user.Projects.SelectMany(p => p.Tickets).ToList();
            return View("Index", ticketList);
        }
        public ActionResult GetMyTickets()
        {
            var userId = User.Identity.GetUserId();
            var ticketList = new List<Ticket>();
            if(User.IsInRole("Developer"))
            {
               ticketList = db.Tickets.Where(t => t.DeveloperId == userId).ToList();
                return View("Index", ticketList);
            }
            if (User.IsInRole("Submitter"))
            {
                ticketList = db.Tickets.Where(t => t.SubmitterId == userId).ToList();
                return View("Index", ticketList);
            }
            else
            {
                return RedirectToAction("GetProjectTickets");
            }
        }

        // GET: Tickets/Details/5
        public ActionResult Dashboard(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Submitter")]
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            if (userId == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(projectHelper.ListUserProjects(userId), "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Submitter")]
        public ActionResult Create([Bind(Include = "Id,ProjectId,TicketTypeId,TicketPriorityId,Issue,IssueDescription")] Ticket ticket)
        {
            var userId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                //add back in :Created SubmitterId
                //Set: devId to null, isArchived and IsResolved will be false
                ticket.TicketStatusId = db.TicketStatuses.Where(ts => ts.Name == "Open").FirstOrDefault().Id;
                ticket.Created = DateTime.Now;
                ticket.SubmitterId = User.Identity.GetUserId();
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Details", "Projects", new { id = ticket.ProjectId });
            }

            ViewBag.ProjectId = new SelectList(projectHelper.ListUserProjects(userId), "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name");
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,SubmitterId,DeveloperId,Issue,IssueDescription,IsResolved,IsArchived")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                var newTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);
                historyHelper.ManageHistories(oldTicket, newTicket); 
                ticketHelper.ManageTicketNotifications(oldTicket, newTicket);
                return RedirectToAction("Index");
            }
            ViewBag.DeveloperId = new SelectList(db.Projects, "Id", "Name", ticket.DeveloperId);
            ViewBag.TicketPriorityId = new SelectList(db.Projects, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.Projects, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.Projects, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

       

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
