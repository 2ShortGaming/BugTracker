using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketHistory
    {
        public int Id { get; set; }

        #region Parent/Children
        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        #endregion

        #region Actual Properties
        //the property of the ticket that was changed (Status, Type, Attachment added etc.)
        public string Property { get; set; }
        //what the property was originally set too
        public string OldValue { get; set; }
        //What the property is now set to
        public string NewValue { get; set; }
        public DateTime ChangedOn { get; set; }
        #endregion
    }
}