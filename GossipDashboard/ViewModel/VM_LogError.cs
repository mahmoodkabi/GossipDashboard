using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipDashboard.ViewModel
{
    public class VM_LogError
    {
        public int LogErorrID { get; set; }
        public string IP { get; set; }
        public string ErrorDescription { get; set; }
        public int? ErrorID { get; set; }
        public string PostName { get; set; }
        public int? PostID { get; set; }
        public DateTime? ModifyDateTime { get; set; }

        public string JalaliModifyDateTime { get; set; }
    }
}