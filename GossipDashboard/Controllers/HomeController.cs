using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GossipDashboard.Models;
using System.IO;
using System.Text;
using GossipDashboard.Helper;
using GossipDashboard.Repository;
using System.Timers;
using System.Text.RegularExpressions;
using GossipDashboard.ViewModel;

namespace GossipDashboard.Controllers
{
    public class HomeController : Controller
    {
        private HtmlNode result;
        private LogRepository repoLog = new Repository.LogRepository();
        private LogErrorRepository repoErrorLog = new Repository.LogErrorRepository();
        private PostRepository repo = new PostRepository();
        private ManagementPostRepository repoManagementPost = new ManagementPostRepository();
        public HomeController()
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "راه های ارتباطی";

            return View();
        }
    }
}
