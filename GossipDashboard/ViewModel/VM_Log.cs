using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipDashboard.ViewModel
{
    public class VM_Log
    {
        public int LogID { get; set; }
        public int LogTypeID_fk { get; set; }
        public string IP { get; set; }
        public string PostName { get; set; }
        public Nullable<int> PostID { get; set; }
        public DateTime? ModifyDateTime { get; set; }
        public string JalaliModifyDateTime { get; set; }


    }
}