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
    
    public partial class PostAnswer
    {
        public int PostAnswerID { get; set; }
        public int PostQuestionID_fk { get; set; }
        public Nullable<bool> Answer { get; set; }
    
        public virtual PostQuestion PostQuestion { get; set; }
    }
}
