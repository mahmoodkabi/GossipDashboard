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
    
    public partial class PostComment
    {
        public int PostCommentID { get; set; }
        public int PostID_fk { get; set; }
        public string FullName { get; set; }
        public Nullable<int> UserID { get; set; }
        public string Comment { get; set; }
        public Nullable<int> LikeComment { get; set; }
        public Nullable<int> DislikeComment { get; set; }
        public string IPAddress { get; set; }
        public Nullable<System.DateTime> Datetime { get; set; }
    
        public virtual Post Post { get; set; }
    }
}
