//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GossipDashboard.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Log
    {
        public int LogID { get; set; }
        public Nullable<int> LogTypeID_fk { get; set; }
        public string IP { get; set; }
        public string PostName { get; set; }
        public Nullable<int> PostID { get; set; }
        public Nullable<System.DateTime> ModifyDateTime { get; set; }
    }
}