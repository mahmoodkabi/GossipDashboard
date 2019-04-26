using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipDashboard.ViewModel
{
    public class VM_User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Salt { get; set; }
        public string Password { get; set; }
        public Nullable<int> ModifyUserID { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public byte[] Image { get; set; }
        public string AboutUser { get; set; }
    }
}