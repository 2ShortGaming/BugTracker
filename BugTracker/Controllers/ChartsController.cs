using BugTracker.Models;
using BugTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class ChartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public JsonResult GetAllTicketPriorityData()
        {
            var tickets = db.Tickets.ToList();
            var data = new List<ChartsVM>();
            foreach(var priority in db.TicketPriorities.ToList())
            {

                data.Add(new ChartsVM()
                {
                    label = priority.Name,
                    value = tickets.Where(t => t.TicketPriorityId == priority.Id).Count()
                });
            }
            return Json(data);
        }

        public JsonResult GetAllTicketStatusData()
        {
            var tickets = db.Tickets.ToList();
            var data = new List<ChartsVM>();
            foreach (var status in db.TicketStatuses.ToList())
            {

                data.Add(new ChartsVM()
                {
                    label = status.Name,
                    value = tickets.Where(t => t.TicketStatusId == status.Id).Count()
                });
            }
            return Json(data);
        }

        public JsonResult GetAllTicketTypeData()
        {
            var tickets = db.Tickets.ToList();
            var data = new List<ChartsVM>();
            foreach (var type in db.TicketTypes.ToList())
            {

                data.Add(new ChartsVM()
                {
                    label = type.Name,
                    value = tickets.Where(t => t.TicketTypeId == type.Id).Count()
                });
            }
            return Json(data);
        }
    }
}