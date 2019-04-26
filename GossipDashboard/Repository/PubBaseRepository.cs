using GossipDashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace GossipDashboard.Repository
{
    public class PubBaseRepository : IRepository<PubBase>
    {
        private GossipSiteEntities context = new GossipSiteEntities();

        public PubBase Add(PubBase entity)
        {
            throw new NotImplementedException();
        }

        public PubBase Delete(int id)
        {
            throw new NotImplementedException();
        }

        public PubBase Select(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<PubBase> SelectAll(string condition)
        {
            var res = context.PubBases.Where(p => p.NameEn == condition);
            return res;
        }

        public IQueryable<PubBase> SelectByParentName(string parentName)
        {
            if (parentName != "")
            {
                var parent = context.PubBases.FirstOrDefault(p => p.NameEn == parentName);
                if (parent != null)
                {
                    var res = context.PubBases.Where(p => p.ParentID == parent.PubBaseID);
                    return res;
                }
            }

            return null;
        }

        public PubBase Update(PubBase entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<PubBase> SelectAll()
        {
            var res = context.PubBases;
            return res;
        }
    }
}
