using GossipDashboard.Models;
using GossipDashboard.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipDashboard.Repository
{
    public class LogRepository 
    {
        private GossipSiteEntities context = new GossipSiteEntities();
        public void Add(VM_Log vm)
        {
            try
            {
                context.Logs.Add(new Log()
                {
                    IP = vm.IP,
                    LogTypeID_fk = 59,
                    ModifyDateTime = vm.ModifyDateTime,
                    PostID = vm.PostID,
                    PostName = vm.PostName,
                });
                context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }

        public VM_Log Delete(int id)
        {
            throw new NotImplementedException();
        }

        public VM_Log Select(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Log> SelectAll(string condition)
        {
            var res = context.Logs;
            return res;
        }

        public VM_Log Update(VM_Log vm)
        {
            throw new NotImplementedException();
        }
    }
}