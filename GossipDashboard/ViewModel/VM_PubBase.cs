using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipDashboard.ViewModel
{
    public class VM_PubBase
    {
        public int PubBaseID { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string NameFa { get; set; }
        public string NameEn { get; set; }
        public string GroupBase { get; set; }
        public string Description { get; set; }
        public string ClassName { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> ModifyUserID { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public int? PostID { get; set; }
        public string AbobeClassName { get;  set; }
    }
}