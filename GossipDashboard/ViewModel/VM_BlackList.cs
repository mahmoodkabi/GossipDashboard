using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipDashboard.ViewModel
{
    public class VM_BlackList
    {
        public int BlackListID { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> ModifyDateTime { get; set; }
    }
}