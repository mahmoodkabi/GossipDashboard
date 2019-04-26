using GossipDashboard.Helper;
using GossipDashboard.Models;
using GossipDashboard.Repository;
using GossipDashboard.ViewModel;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Web;
using System.Web.Mvc;

namespace GossipDashboard.Controllers
{
    public class PostController : Controller
    {
        private HtmlNode result;
        private LogRepository repoLog = new Repository.LogRepository();
        private LogErrorRepository repoErrorLog = new Repository.LogErrorRepository();
        private PostRepository repo = new PostRepository();
        private ManagementPostRepository repoManagementPost = new ManagementPostRepository();
        private UserRepository repoUser = new UserRepository();
        private string path = "", domain = "http://redfun.ir", ip = "";


        public ActionResult Index()
        {
            return View(new VM_Post());
        }



        [HttpPost]
        public JsonResult ReadRelatedPost(string keyWords, string postID)
        {
            string path = ControllerContext.HttpContext.Server.MapPath("~");
            string domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + ":" + Request.Url.Port;
            PostManagement postManagement = new PostManagement(path, domain, ip);
            List<string> lstArticle = new List<string>();
            HtmlNode article = null;

            //به دست آوردن پست های مشابه
            var res = repo.ReadRelatedPost(keyWords, postID);

            //رندر کردن به فرمت اچ تی ام ال
            foreach (var item in res)
            {
                article = postManagement.CreateBloglist(item, "RelatedPost", 0);
                lstArticle.Add(article.OuterHtml);
            }

            return Json(lstArticle, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ایجاد پست های هر کتگوری به صورت اجکس
        /// </summary>
        /// <param name="postCategory">نام کتگوری</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreatePostAjax(string postCategory)
        {
            string path = ControllerContext.HttpContext.Server.MapPath("~");
            string domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + ":" + Request.Url.Port;
            PostManagement postManagement = new PostManagement(path, domain, ip);
            List<string> lstArticle = new List<string>();
            HtmlNode article = null;
           
            var res = repo.SelectPostByCategory(postCategory).OrderByDescending(p => p.PostID).Take(50).ToList();

            //رندر کردن به فرمت اچ تی ام ال
            foreach (var item in res)
            {
                article = postManagement.CreateBloglist(item, "CreateBlogListAjax", 0);
                lstArticle.Add(article.OuterHtml);
            }

            return Json(lstArticle, JsonRequestBehavior.AllowGet);
        }


        //public ActionResult Search()
        //{
       
        //}
        /// <summary>
        /// جستجو
        /// </summary>
        /// <param name="postCategory">عبارت مورد جستجو</param>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult SearchPostAjax(string search)
        {
            string path = ControllerContext.HttpContext.Server.MapPath("~");
            string domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + ":" + Request.Url.Port;

            PostManagement postManagement = new PostManagement(path, domain, ip);
            List<VM_SearchResult> lstArticle = new List<VM_SearchResult>();
            List<VM_Post> res = new List<VM_Post>();
            HtmlNode article = null;
            string str1 = "", str2 = "", str3 = "";

            if (search.Trim().Length == 0)
                return View("Search", lstArticle);

            //جدا کردن کلمات سرچ شده
            var arrSearch = search.Trim().Split(' ');

            if (arrSearch.Count() == 1)
            {
                str1 = arrSearch[0];
                res = repo.SelectPostUser().Where(p => p.keyWords.Contains(str1) || p.Subject1.Contains(str1)).OrderByDescending(p => p.PostID).Take(25).ToList();
            }
            else if (arrSearch.Count() == 2)
            {
                str1 = arrSearch[0];
                str2 = arrSearch[1];
                res = repo.SelectPostUser().Where(p => (p.keyWords.Contains(str1) && p.keyWords.Contains(str2)) ||
                                                      (p.Subject1.Contains(str1) && p.Subject1.Contains(str2)))
                                           .OrderByDescending(p => p.PostID).Take(25).ToList();
            }
            else if (arrSearch.Count() == 3)
            {
                str1 = arrSearch[0];
                str2 = arrSearch[1];
                str3 = arrSearch[2];
                res = repo.SelectPostUser().Where(p => (p.keyWords.Contains(str1) && p.keyWords.Contains(str2) && p.keyWords.Contains(str3)) ||
                                                       (p.Subject1.Contains(str1) && p.Subject1.Contains(str2) && p.Subject1.Contains(str3)))
                                           .OrderByDescending(p => p.PostID).Take(25).ToList();
            }

            ////اگر جستجو نتیجه ای  در بر نداشت
            ////کلمات کلیدی را برای متن های طولانی استخراج کن و درباره جستجو کن
            //if(res.Count == 0)
            //{
            //    ProcessWord Pwords = new ProcessWord();
            //    // فچ کردن لیست کلمات پرتکرار و غیر کلیدی در زبان فارسی
            //    var wordFrequency = context.FrequencyWords.Select(p => p.FrequencyWord1).ToList();
            //    //به دست آوردن کلمات کلیدی پست
            //    var keyWords = Pwords.FindKeywordsFromPost(search,wordFrequency);
            //}

            //رندر کردن به فرمت اچ تی ام ال
            foreach (var item in res)
            {
                article = postManagement.CreateBloglist(item, "SearchPostAjax", 0);
                lstArticle.Add(new VM_SearchResult() { Article = article.OuterHtml, SearchValue = search });
            }

            return View("Search", lstArticle);
        }

        public ActionResult ShowImage(int id)
        {
            var post = repoUser.Select(id);
            if (post == null || post.Image == null)
                return null;
            return File(post.Image, "image/jpg");
        }

        [Authorize]
        [HttpPost]
        public ActionResult SaveImage(HttpPostedFileBase UploadFile)
        {
            if (UploadFile != null)
            {

                byte[] fileContect = new byte[UploadFile.InputStream.Length];
                UploadFile.InputStream.Read(fileContect, 0, fileContect.Length);

                var user = new User
                {
                    FirstName = "ارسلان",
                    LastName = "آرماني",
                    AboutUser = "يکي از نويسنده هاي نامدار ايران",
                    Image = fileContect,
                    ModifyDate = DateTime.Now,
                    ModifyUserID = 1,
                    Password = "123",
                    Salt = "123",
                    UserName = "admin",
                };

                repoUser.Add(user);
                return Json("OK");
            }


            return Json("Fails");
        }
    }
}