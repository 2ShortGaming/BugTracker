using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{

    public abstract class Notification
    {
        public int Id { get; set; }
        #region Parents/Children
        public string UserId { get; set; }

        #endregion
        #region Actual Property
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
        public bool IsRead { get; set; }

        public string NotificationType { get; set; }

        #endregion
    }
}