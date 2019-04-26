using GossipDashboard.Models;
using GossipDashboard.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipDashboard.Repository
{
    public class SubscriberRepository : IRepository<VM_Subscriber>
    {
        private GossipSiteEntities context = new GossipSiteEntities();
        public VM_Subscriber Add(VM_Subscriber vm)
        {
            var entity = new Subscriber()
            {
                Email = vm.Email,
                FullName = vm.FullName,
                IPAddress = vm.IPAddress,
                Message = vm.Message,
                WebSite = vm.WebSite,
                ModifyDateTime = vm.ModifyDateTime
            };

            context.Subscribers.Add(entity);
            context.SaveChanges();

            vm.SubscriberID = entity.SubscriberID;
            return vm;
        }

        public VM_Subscriber Delete(int id)
        {
            throw new NotImplementedException();
        }

        public VM_Subscriber Select(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<VM_Subscriber> SelectAll(string condition)
        {
            throw new NotImplementedException();
        }

        public VM_Subscriber Update(VM_Subscriber vm)
        {
            throw new NotImplementedException();
        }
    }
}