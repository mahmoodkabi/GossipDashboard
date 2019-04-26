using GossipDashboard.Models;
using GossipDashboard.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipDashboard.Repository
{
    public class BlackListRepository : IRepository<VM_BlackList>
    {
        private GossipSiteEntities context = new GossipSiteEntities();
        public VM_BlackList Add(VM_BlackList vm)
        {
            throw new NotImplementedException();
        }

        public VM_BlackList Delete(int id)
        {
            throw new NotImplementedException();
        }

        public VM_BlackList Select(int id)
        {
            throw new NotImplementedException();
        }


        public IQueryable<VM_BlackList> SelectByParentName(string parentName)
        {
            if (parentName != "")
            {
                var parent = context.BlackLists.FirstOrDefault(p => p.Description == parentName);
                if (parent != null)
                {
                    var res = context.BlackLists.Where(p => p.ParentID == parent.BlackListID)
                        .Select(p => new VM_BlackList()
                        {
                            BlackListID = p.BlackListID,
                            Description = p.Description,
                            ModifyDateTime = p.ModifyDateTime,
                            Name = p.Name,
                            ParentID = p.ParentID
                        });

                    return res;
                }
            }

            return null;
        }

        public IQueryable<VM_BlackList> SelectAll(string condition)
        {
            return context.BlackLists.Select(p => new VM_BlackList()
            {
                BlackListID = p.BlackListID,
                Description = p.Description,
                ModifyDateTime = p.ModifyDateTime,
                Name = p.Name,
                ParentID = p.ParentID
            });
        }

        public VM_BlackList Update(VM_BlackList vm)
        {
            throw new NotImplementedException();
        }
    }
}