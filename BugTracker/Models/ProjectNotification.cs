using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class ProjectNotification : Notification
    {
        #region Parents/Children
        public int ProjectId { get; set; }

        public virtual ApplicationUser User { get; set; }
        #endregion

    }
}