﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class GossipSiteEntities : DbContext
    {
        public GossipSiteEntities()
            : base("name=GossipSiteEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BlackList> BlackLists { get; set; }
        public virtual DbSet<FrequencyWord> FrequencyWords { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<LogError> LogErrors { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PostAnswer> PostAnswers { get; set; }
        public virtual DbSet<PostAttribute> PostAttributes { get; set; }
        public virtual DbSet<PostComment> PostComments { get; set; }
        public virtual DbSet<PostQuestion> PostQuestions { get; set; }
        public virtual DbSet<PostTemperory> PostTemperories { get; set; }
        public virtual DbSet<PubBase> PubBases { get; set; }
        public virtual DbSet<Subscriber> Subscribers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserPost> UserPosts { get; set; }
    
        public virtual ObjectResult<CountGossip_Result> CountGossip()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CountGossip_Result>("CountGossip");
        }
    
        public virtual int sp_DeleteDuplicatePost(Nullable<bool> isDelete)
        {
            var isDeleteParameter = isDelete.HasValue ?
                new ObjectParameter("isDelete", isDelete) :
                new ObjectParameter("isDelete", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_DeleteDuplicatePost", isDeleteParameter);
        }
    }
}
