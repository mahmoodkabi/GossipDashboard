using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GossipDashboard.Models;

namespace GossipDashboard.ViewModel
{
    public class VM_Comment
    {
        public int PostCommentID { get; set; }
        public int PostID_fk { get; set; }
        public string Comment { get; set; }
        public Nullable<int> LikeComment { get; set; }
        public Nullable<int> DislikeComment { get; set; }
        public string FullName { get; set; }
        public Nullable<int> UserID { get; set; }
        public string IPAddress { get; set; }
        public Nullable<System.DateTime> Datetime { get; set; }
        public int TotalComment { get;  set; }
        public string JalaliDatetime { get;  set; }
    }
}