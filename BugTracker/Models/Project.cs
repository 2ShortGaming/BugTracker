using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class Project
    {
        public int Id { get; set; }

        #region Parent/Children
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        #endregion

        #region Actual Properties
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public bool IsArchived { get; set; }
        public string SubmitterId { get; set; }
        public string DeveloperId { get; set; }
        #endregion

        #region Contructor
        public Project()
        {
            Tickets = new HashSet<Ticket>();
            Users = new HashSet<ApplicationUser>();
        }
        #endregion
    }
}