using GossipDashboard.Helper;
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
    public class AnimalController : Controller
    {
        private PostRepository repo = new PostRepository();
        private HtmlNode result;
        private LogErrorRepository repoErrorLog = new Repository.LogErrorRepository();
        private string path;
        private string domain;
        private string ip;

        public AnimalController()
        {

        }
        public AnimalController(string path, string domain, string ip)
        {
            this.path = path;
            this.domain = domain;
            this.ip = ip;
        }

        public ActionResult Index()
        {
            return View();
        }


        public JsonResult CreateAnimalMenu()
        {
            PostManagement postManagement = new PostManagement(path,domain,ip);

            List<VM_Post> listAll = new List<VM_Post>();
            var docIndex = new HtmlDocument();

            try
            {
                docIndex.Load(path + "/Views/Animal/Index.cshtml", System.Text.Encoding.UTF8);
                var nodesIndex = docIndex.DocumentNode.SelectNodes("//div");

                //حذف محتويات ند بلاك-catlist
                postManagement.ClearContentNode(nodesIndex, "bloglist default");

                //ایجاد  محتواي bloglist default
                var posts = repo.SelectPostUser().Where(p => p.SourceSiteUrl.Contains("motamem") || p.SourceSiteUrl.Contains("yjc") || p.SourceSiteUrl.Contains("alef")).Take(150).ToList();
                listAll = Utilty.SortGroupsList(posts, PlaceInMainPage.BloglistDefault);
                foreach (var item in listAll)
                {
                    //ریزور در رندر کردن یو آر ال هایی که "ات ساین" دارند به مشکل بر می خورد
                    if (item.Image1_1 != null && item.Image1_1.Contains("@"))
                        continue;

                    item.JalaliModifyDate = item.ModifyDate.ToPersianDateTime();

                    //ايجاد محتوا براي -- bloglist default
                    var itSelfNode = postManagement.CreateBloglistDefault(item);
                    if (itSelfNode != null)
                    {
                        result = postManagement.AddHeadToContentDiv(nodesIndex, "bloglist default", itSelfNode);
                    }
                }


                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(result.OuterHtml);
                htmlDoc.Save(path + "/Views/Animal/Index.cshtml", Encoding.UTF8);

                return Json( true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string innerException = ex.InnerException != null ? ex.InnerException.ToString() : "";
                string innerExceptionMessage = innerException != "" ? ex.InnerException.Message : "";
                string innerException2 = innerException != "" ? ex.InnerException.InnerException.ToString() : "";
                string innerException2Message = innerException2 != "" ? ex.InnerException.InnerException.Message : "";
                var errorMessage = "innerException Message: " + innerExceptionMessage +
                 "   innerException2 Message: " + innerException2Message +
                 "   innerException: " + innerException +
                 "   innerException2: " + innerException2;

                repoErrorLog.Add(new VM_LogError()
                {
                    ErrorDescription = errorMessage,
                    IP = ip,
                    ModifyDateTime = DateTime.Now,
                    PostName = "Animal Page",
                    PostID = -100
                });

                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}