using GossipDashboard.Repository;
using GossipDashboard.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GossipDashboard.Controllers
{
    public class SubscriberController : Controller
    {
        SubscriberRepository repo = new SubscriberRepository();
        public JsonResult AddSubscriber(VM_Subscriber vm)
        {
            vm.IPAddress = Request.UserHostAddress;
            vm.ModifyDateTime = DateTime.Now;
            var res = repo.Add(vm);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}