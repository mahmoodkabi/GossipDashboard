using GossipDashboard.Models;
using GossipDashboard.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipDashboard.Repository
{
    public class LogErrorRepository
    {
        private GossipSiteEntities context = new GossipSiteEntities();
        public void Add(VM_LogError vm)
        {
            try
            {
                context.LogErrors.Add(new LogError()
                {
                    IP = vm.IP,
                    ModifyDateTime = vm.ModifyDateTime,
                    PostID = vm.PostID,
                    PostName = vm.PostName,
                    ErrorDescription = vm.ErrorDescription,
                    ErrorID = vm.ErrorID
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

        public IQueryable<LogError> SelectAll(string condition)
        {
            var res = context.LogErrors;
            return res;
        }

        public VM_Log Update(VM_Log vm)
        {
            throw new NotImplementedException();
        }
    }
}