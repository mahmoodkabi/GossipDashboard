using GossipDashboard.Repository;
using GossipDashboard.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GossipDashboard.Controllers
{
    public class CommentController : Controller
    {
        CommentRepository repo = new CommentRepository();
        public JsonResult AddComment(VM_Comment vm)
        {
            vm.IPAddress = Request.UserHostAddress;
            vm.Datetime = DateTime.Now;
            var res = repo.Add(vm);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReadComment(int postID,int skip, int take)
        {
            var res = repo.SelectAll("").Where(p => p.PostID_fk == postID).OrderByDescending(p => p.PostCommentID).Skip(skip).Take(take).ToList();
            foreach (var item in res)
            {
                item.JalaliDatetime = item.Datetime.ToPersianDateTime();
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}