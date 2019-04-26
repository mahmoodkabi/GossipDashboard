using GossipDashboard.Helper;
using GossipDashboard.Models;
using GossipDashboard.Repository;
using GossipDashboard.ViewModel;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace GossipDashboard.Controllers
{
    public class QuizController : Controller
    {
        private LogErrorRepository repoErrorLog = new Repository.LogErrorRepository();
        PostRepository repo = new PostRepository();
        PostManagement helperPost = new PostManagement();
        private HtmlNode result;
        private string path;
        private string domain;
        private string ip;

        public QuizController()
        {

        }

        public QuizController(string path, string domain, string ip)
        {
            this.path = path;
            this.domain = domain;
            this.ip = ip;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Post(string postID)
        {
            int id = 0;
            string title = "";
            try
            {
                id = Convert.ToInt32(postID.Substring(0, postID.IndexOf('&')));
                title = postID.Substring(postID.IndexOf('&') + 1, postID.Length - postID.IndexOf('&') - 1);
            }
            catch (Exception ex)
            {
                int.TryParse(postID, out int resPostID);
                id = resPostID;
            }

            repo.UpdatePostViews(id);

            var res = repo.SelectPostUser().Where(p => p.PostID == id).FirstOrDefault();
            if (res == null)
            {
                repoErrorLog.Add(new VM_LogError()
                {
                    ErrorDescription = "Post Not Exists. Post NO: " + postID.ToString(),
                    IP = ip,
                    ModifyDateTime = DateTime.Now,
                    PostName = "Post Not Exists. Post NO: " + postID.ToString(),
                    PostID = id
                });
                return View("~/Views/ErrorHandler/NotFound.cshtml", res);
            }

            //حذف تگ های نامناسب درهنگام نمایش پست
            helperPost.RemoveRedundantTag(res);

            res.JalaliModifyDate = res.ModifyDate.ToPersianDateTime();

            //بعضی پست ها ی ویدیویی یا صوتی به اشتباه پست معمولی نمایش داده می شودند
            //برای جلوگیری از این اشتباه این شرط اضافه گردید
            if (res.UrlVideo != null)
                return View("~/Views/Post/VideoIndex.cshtml", res);
            else if (res.UrlMP3 != null)
                return View("~/Views/Post/AudioIndex.cshtml", res);

            return View("~/Views/Post/Index.cshtml", res);
        }

        public ActionResult VideoPost(string postID)
        {
            int id = 0;
            string title = "";


            try
            {
                id = Convert.ToInt32(postID.Substring(0, postID.IndexOf('&')));
                title = postID.Substring(postID.IndexOf('&') + 1, postID.Length - postID.IndexOf('&') - 1);

            }
            catch (Exception ex)
            {
                int.TryParse(postID, out int resPostID);
                id = resPostID;
            }

            repo.UpdatePostViews(id);

            var res = repo.SelectPostUser().Where(p => p.PostID == id).FirstOrDefault();
            if (res != null)
            {
                res.JalaliModifyDate = res.ModifyDate.ToPersianDateTime();

                //حذف تگ های نامناسب درهنگام نمایش پست
                helperPost.RemoveRedundantTag(res);
            }
            return View("~/Views/Post/VideoIndex.cshtml", res);
        }

        public ActionResult AudioPost(string postID)
        {
            int id = 0;
            string title = "";
            try
            {
                id = Convert.ToInt32(postID.Substring(0, postID.IndexOf('&')));
                title = postID.Substring(postID.IndexOf('&') + 1, postID.Length - postID.IndexOf('&') - 1);

            }
            catch (Exception ex)
            {
                int.TryParse(postID, out int resPostID);
                id = resPostID;
            }

            repo.UpdatePostViews(id);

            var res = repo.SelectPostUser().Where(p => p.PostID == id).FirstOrDefault();
            if (res != null)
            {
                //حذف تگ های نامناسب درهنگام نمایش پست
                helperPost.RemoveRedundantTag(res);

                res.JalaliModifyDate = res.ModifyDate.ToPersianDateTime();
            }
            return View("~/Views/Post/AudioIndex.cshtml", res);
        }

 
        public ActionResult CreateContentCategory()
        {
            //path = ControllerContext.HttpContext.Server.MapPath("~");
            PostManagement postManagement = new PostManagement(path, domain,ip);

            var docIndex = new HtmlDocument();
            docIndex.Load(path + "/Views/Quiz/Index.cshtml", Encoding.UTF8);
            var nodesIndex = docIndex.DocumentNode.SelectNodes("//div");

            //حذف محتويات ند بلاك-author-grid
            postManagement.ClearContentNode(nodesIndex, "author-grid");

            //ایجاد  تگ آرتیکل به ازای هر پست
            var repo = new PostRepository();
            var postQuiz = repo.SelectPostByCategory("quiz").OrderByDescending(p => p.PostID).Take(200).ToList();
            foreach (var item in postQuiz)
            {
                //ايجاد محتوا براي وسط صفحه-- author-grid 
                var itSelfNode = postManagement.CreateBloglist(item,"",0);
                if (itSelfNode != null)
                {
                    result = postManagement.AddHeadToContentDiv(nodesIndex, "author-grid", itSelfNode);
                }
            }


            try
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(result.OuterHtml);
                htmlDoc.Save(path + "/Views/Quiz/Index.cshtml", Encoding.UTF8);
            }
            catch (Exception)
            {

            }


            return View("Index");
        }
    }
}