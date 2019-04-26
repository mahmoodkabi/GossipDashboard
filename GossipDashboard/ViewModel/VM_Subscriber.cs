using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipDashboard.ViewModel
{
    public class VM_Subscriber
    {
        public int SubscriberID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string IPAddress { get; set; }
        public string WebSite { get; set; }
        public string Message { get; set; }
        public Nullable<System.DateTime> ModifyDateTime { get; set; }
    }
}