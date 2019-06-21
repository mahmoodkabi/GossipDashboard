using GossipDashboard.Helper;
using GossipDashboard.Repository;
using GossipDashboard.ViewModel;
using HtmlAgilityPack;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace GossipDashboard.Controllers
{
    public class DashboardController : Controller
    {
        private HtmlNode result;
        private LogRepository repoLog = new Repository.LogRepository();
        private LogErrorRepository repoErrorLog = new Repository.LogErrorRepository();
        private PostRepository repo = new PostRepository();
        private ManagementPostRepository repoManagementPost = new ManagementPostRepository();
        private UserRepository repoUser = new UserRepository();
        private string path = "", domain = "http://redfun.ir", ip = "";

        //[Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateIndex()
        {
            path = ControllerContext.HttpContext.Server.MapPath("~");
            domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + ":" + Request.Url.Port;
            ip = Request.UserHostAddress;

            PostManagement postManagement = new PostManagement(path, domain, ip);

            //ایجاد پست ها
            repo.CreatePost();

            //ایجاد صفحه اصلی
            CreateIndexPage(path, postManagement);

            //ایجاد صفحه کتگوری ها
            CreateAllCategory(path, domain, ip);

            //ایجاد صفحه حيوانات 
            AnimalController animaltCtr = new AnimalController(path, domain, ip);
            animaltCtr.CreateAnimalMenu();

            return RedirectToAction("Index", "Dashboard");
            //return View("Dashboard/Index");
        }

        //ایجاد صفحه اصلی
        private void CreateIndexPage(string path, PostManagement postManagement)
        {
            List<VM_Post> listAll = new List<VM_Post>();
            List<string> blackList = new List<string>();
            var docIndex = new HtmlDocument();

            /////////////////////////////create bloglist/////////////////////////////
            //وسط صفحه 
            try
            {
                docIndex.Load(path + "/Views/Home/Index.cshtml", System.Text.Encoding.UTF8);
                var nodesIndex = docIndex.DocumentNode.SelectNodes("//div");

                //حذف محتويات ند بلاك-author-grid
                postManagement.ClearContentNode(nodesIndex, "author-grid");

                ///// در هر گروه بر اساس نام سایت، بالاترین مقادیر را یکی یکی خارج می کند و لیست جدید می سازد
                //پست نوع استاتوس  و لینک را در داخل متد سورت گروپ ليست مقدار دهي مي کنيم
                var posts = repo.SelectPostUser().Where(p => p.PostFormat.FirstOrDefault().NameEn != "status"
                        && p.PostFormat.FirstOrDefault().NameEn != "link" && p.PostFormat.FirstOrDefault().NameEn != "video"
                        && p.PostFormat.FirstOrDefault().NameEn != "audio").OrderByDescending(p => p.PostID).Take(200).ToList();


                listAll = Utilty.SortGroupsList(posts, PlaceInMainPage.MiddleIndex);

                //ایجاد  تگ آرتیکل به ازای هر پست
                int i = 0; List<string> duplicateImage = new List<string>();
                foreach (var item in listAll)
                {
                    //ریزور در رندر کردن یو آر ال هایی که "ات ساین" دارند به مشکل بر می خورد
                    if (item.Image1_1 != null && item.Image1_1.Contains("@"))
                        continue;

                    // برای قسمت اصلی داشتن  تصویر مهم است یا پست آپارات باشد یا استاتوس باشد يا ويديو یا صوتی 
                    // 30 پست ایجاد گردد --------  i < 30
                    if ((item.Image1_1 != null && i < 30 && !item.Image1_1.Contains("@")) || (item.ScriptAparat != null && i < 30) || (item.Status != null && i < 30)
                        || (item.UrlVideo != null && i < 30) || (item.UrlMP3 != null && i < 30))
                    {
                        //در صفحه اصلی عکس تکراری نداشته باشیم
                        if (duplicateImage.FirstOrDefault(x => x == item.Image1_1) == null)
                        {
                            item.JalaliModifyDate = item.ModifyDate.ToPersianDateTime();

                            //ايجاد محتوا براي وسط صفحه-- author-grid
                            var itSelfNode = postManagement.CreateBloglist(item, "CreateIndexPage", i);
                            if (itSelfNode != null)
                            {
                                result = postManagement.AddHeadToContentDiv(nodesIndex, "author-grid", itSelfNode);

                                //این قسمت مرحله ای است که پابلیش هر پست نهایی می شود
                                // یک عدد به فیلد پابلیش کانت آنها اضافه کنیم
                                repoManagementPost.UpdatePublishCount(item.PostID);
                            }

                            i += 1;
                            duplicateImage.Add(item.Image1_1);
                        }
                    }
                }

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(result.OuterHtml);
                htmlDoc.Save(path + "/Views/Home/Index.cshtml", Encoding.UTF8);
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
                    PostName = "Create Main Page - PlaceInMainPage.MiddleIndex",
                    PostID = -100
                });

            }

            //////////////////////Create catlist///////////////////////////////////////
            //رویدادهای دیگر
            try
            {
                var startDaysAgo = DateTime.Now.AddDays(-10);
                var endDaysAgo = DateTime.Now.AddDays(-5);
                docIndex.Load(path + "/Views/Home/Index.cshtml", System.Text.Encoding.UTF8);
                var nodesIndex = docIndex.DocumentNode.SelectNodes("//div");

                //حذف محتويات ند بلاك-catlist
                postManagement.ClearContentNode(nodesIndex, "tab-content");

                //ایجاد  محتواي تب هاي کت ليست
                var posts = repo.SelectPostUser().Where(p => p.ModifyDate >= startDaysAgo && p.ModifyDate <= endDaysAgo && p.Status == null).OrderByDescending(p => p.PostID).Take(50).ToList();
                listAll = Utilty.SortGroupsList(posts, PlaceInMainPage.LastEvent);
                foreach (var item in listAll)
                {
                    //ریزور در رندر کردن یو آر ال هایی که "ات ساین" دارند به مشکل بر می خورد
                    if (item.Image1_1 != null && item.Image1_1.Contains("@"))
                        continue;


                    item.JalaliModifyDate = item.ModifyDate.ToPersianDateTime();

                    //ايجاد محتوا براي قسمت طبقه بندی-- catlist
                    var itSelfNode = postManagement.CreateCatListContent(item);
                    if (itSelfNode != null)
                    {
                        result = postManagement.AddHeadToContentDiv(nodesIndex, "tab-content", itSelfNode);
                    }
                }


                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(result.OuterHtml);
                htmlDoc.Save(path + "/Views/Home/Index.cshtml", Encoding.UTF8);
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
                    PostName = "Create Main Page - PlaceInMainPage.OtherEvent",
                    PostID = -100
                });
            }

            //////////////////////Create bloglist-content///////////////////////////////////////
            //مطالب جالب
            //قسمت کتگوری لیست که ترانسپرنت است
            try
            {
                var startDaysAgo = DateTime.Now.AddDays(-15);
                var endDaysAgo = DateTime.Now.AddDays(-10);
                docIndex.Load(path + "/Views/Home/Index.cshtml", System.Text.Encoding.UTF8);
                var nodesIndex = docIndex.DocumentNode.SelectNodes("//div");

                //حذف محتويات ند بلاك-catlist
                postManagement.ClearContentNode(nodesIndex, "row bloglist-content");

                //ایجاد  محتواي تب هاي کت ليست
                var posts = repo.SelectPostUser().Where(p => p.ModifyDate >= startDaysAgo && p.ModifyDate <= endDaysAgo && p.Status == null).OrderByDescending(x => x.CommentCount).Take(12).ToList();
                listAll = Utilty.SortGroupsList(posts, PlaceInMainPage.InterestingStuff);
                foreach (var item in listAll)
                {
                    //ریزور در رندر کردن یو آر ال هایی که "ات ساین" دارند به مشکل بر می خورد
                    if (item.Image1_1 != null && item.Image1_1.Contains("@"))
                        continue;


                    item.JalaliModifyDate = item.ModifyDate.ToPersianDateTime();

                    //ايجاد محتوا براي قسمت طبقه بندی-- catlist
                    var itSelfNode = postManagement.CreateBloglistContent(item);
                    if (itSelfNode != null)
                    {
                        result = postManagement.AddHeadToContentDiv(nodesIndex, "row bloglist-content", itSelfNode);
                    }
                }


                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(result.OuterHtml);
                htmlDoc.Save(path + "/Views/Home/Index.cshtml", Encoding.UTF8);
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
                    PostName = "Create Main Page - PlaceInMainPage.InterestingStuff",
                    PostID = -100
                });
            }

            //////////////////////Create bloglist-default///////////////////////////////////////
            // قسمتی که عکس در سمت راست و توضیجات در سمت راست دارد-- زیر مطالب جالب
            try
            {
                var startDaysAgo = DateTime.Now.AddDays(-12);
                var endDaysAgo = DateTime.Now.AddDays(-8);
                docIndex.Load(path + "/Views/Home/Index.cshtml", System.Text.Encoding.UTF8);
                var nodesIndex = docIndex.DocumentNode.SelectNodes("//div");

                //حذف محتويات ند بلاك-catlist
                postManagement.ClearContentNode(nodesIndex, "bloglist default");

                //ایجاد  محتواي bloglist default
                var posts = repo.SelectPostUser().Where(p => p.ModifyDate >= startDaysAgo && p.ModifyDate <= endDaysAgo && p.Status == null && (p.SubSubject1_1 != null || p.ContentPost1_1 != null)).OrderByDescending(p => p.Views).Take(5).ToList();
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
                htmlDoc.Save(path + "/Views/Home/Index.cshtml", Encoding.UTF8);
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
                    PostName = "Create Main Page - PlaceInMainPage.BloglistDefault",
                    PostID = -100
                });
            }

            //اسلایدر پایینی
            //////////////////////Create postslider-container slider-image-bottom///////////////////////////////////////
            var startDateSliderButtom = DateTime.Now.AddDays(-4);
            var endDateSliderButtom = DateTime.Now.AddDays(-2);
            var postSliderButton = repo.SelectPostUser().Where(p => p.ModifyDate >= startDateSliderButtom && p.ModifyDate <= endDateSliderButtom && p.Image1_1 != null && p.Status == null).OrderByDescending(p => p.LikePost).Take(6).ToList();
            listAll = Utilty.SortGroupsList(postSliderButton, PlaceInMainPage.SliderBottom);
            try
            {
                var someDaysAgo = DateTime.Now.AddDays(-5);
                docIndex.Load(path + "/Views/Home/Index.cshtml", System.Text.Encoding.UTF8);
                var nodesIndex = docIndex.DocumentNode.SelectNodes("//div");

                //حذف محتويات ند بلاك-slider-image-bottom
                postManagement.ClearContentNode(nodesIndex, "sp-slides sp-slider-image");

                //ایجاد  محتواي slider-image-bottom
                foreach (var item in listAll)
                {
                    //ریزور در رندر کردن یو آر ال هایی که "ات ساین" دارند به مشکل بر می خورد
                    if (item.Image1_1 != null && item.Image1_1.Contains("@"))
                        continue;


                    //اسلایدر بالا حتما عکس داشته باشد
                    if (item.Image1_1.Trim() == "")
                        continue;

                    item.JalaliModifyDate = item.ModifyDate.ToPersianDateTime();

                    //ايجاد محتوا براي -- bloglist default
                    var itSelfNode = postManagement.CreateSliderImageBottom(item);
                    if (itSelfNode != null)
                    {
                        result = postManagement.AddHeadToContentDiv(nodesIndex, "sp-slides sp-slider-image", itSelfNode);
                    }
                }


                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(result.OuterHtml);
                htmlDoc.Save(path + "/Views/Home/Index.cshtml", Encoding.UTF8);
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
                    PostName = "Create Main Page - PlaceInMainPage.SliderBottom",
                    PostID = -100
                });
            }

            //عکس های زیر اسلایدر پایینی 
            //////////////////////Create postslider-container slider-image-bottom sp-thumbnails sp-slider-image///////////////////////////////////////
            try
            {
                docIndex.Load(path + "/Views/Home/Index.cshtml", System.Text.Encoding.UTF8);
                var nodesIndex = docIndex.DocumentNode.SelectNodes("//div");

                //حذف محتويات ند بلاك-slider-image-bottom
                postManagement.ClearContentNode(nodesIndex, "sp-thumbnails sp-slider-image");

                //ایجاد  محتواي slider-image-bottom
                foreach (var item in listAll)
                {
                    //ریزور در رندر کردن یو آر ال هایی که "ات ساین" دارند به مشکل بر می خورد
                    if (item.Image1_1 != null && item.Image1_1.Contains("@"))
                        continue;

                    //اسلایدر بالا حتما عکس داشته باشد
                    if (item.Image1_1.Trim() == "")
                        continue;

                    item.JalaliModifyDate = item.ModifyDate.ToPersianDateTime();

                    //ايجاد محتوا براي -- bloglist default
                    var itSelfNode = postManagement.CreateSliderImageBottom_ImageBottom(item);
                    if (itSelfNode != null)
                    {
                        result = postManagement.AddHeadToContentDiv(nodesIndex, "sp-thumbnails sp-slider-image", itSelfNode);
                    }
                }


                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(result.OuterHtml);
                htmlDoc.Save(path + "/Views/Home/Index.cshtml", Encoding.UTF8);
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
                    PostName = "Create Main Page - PlaceInMainPage.SliderBottom - Picture",
                    PostID = -100
                });
            }


            //اسلایدر بالایی
            ////////////////////////sp-slides sp-slider-image-top///////////////////////////////////////
            var someDaysAgoSliderTop = DateTime.Now.AddDays(-5);
            var postSliderTop = repo.SelectPostUser().Where(p => p.ModifyDate >= someDaysAgoSliderTop && p.ModifyDate < DateTime.Now && p.Image1_1 != null && p.Status == null).OrderByDescending(p => p.Views).Take(6).ToList();
            listAll = Utilty.SortGroupsList(postSliderTop, PlaceInMainPage.SliderTop);
            try
            {
                docIndex.Load(path + "/Views/Home/Index.cshtml", System.Text.Encoding.UTF8);
                var nodesIndex = docIndex.DocumentNode.SelectNodes("//div");

                //حذف محتويات ند
                postManagement.ClearContentNode(nodesIndex, "sp-slides sp-slider-image-top");

                //ایجاد  محتوا
                foreach (var item in listAll)
                {
                    //ریزور در رندر کردن یو آر ال هایی که "ات ساین" دارند به مشکل بر می خورد
                    if (item.Image1_1 != null && item.Image1_1.Contains("@"))
                        continue;

                    //اسلایدر بالا حتما عکس داشته باشد
                    if (item.Image1_1.Trim() == "")
                        continue;

                    item.JalaliModifyDate = item.ModifyDate.ToPersianDateTime();

                    //ايجاد محتوا براي
                    var itSelfNode = postManagement.CreateSliderTop(item);
                    if (itSelfNode != null)
                    {
                        postManagement.AddHeadToContent(nodesIndex, "sp-slides sp-slider-image-top", itSelfNode);
                    }
                }


                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(docIndex.DocumentNode.OuterHtml);
                htmlDoc.Save(path + "/Views/Home/Index.cshtml", Encoding.UTF8);
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
                    PostName = "Create Main Page - PlaceInMainPage.SliderTop",
                    PostID = -100
                });
            }

            //عکس های سمت چپ اسلایدر بالایی
            ////////////////////////sp-thumbnails sp-slider-image-top///////////////////////////////////////
            try
            {
                docIndex.Load(path + "/Views/Home/Index.cshtml", System.Text.Encoding.UTF8);
                var nodesIndex = docIndex.DocumentNode.SelectNodes("//div");

                //حذف محتويات ند
                postManagement.ClearContentNode(nodesIndex, "sp-thumbnails sp-slider-image-top");

                //ایجاد  محتوا
                foreach (var item in listAll)
                {
                    //ریزور در رندر کردن یو آر ال هایی که "ات ساین" دارند به مشکل بر می خورد
                    if (item.Image1_1 != null && item.Image1_1.Contains("@"))
                        continue;

                    //اسلایدر بالا حتما عکس داشته باشد
                    if (item.Image1_1.Trim() == "")
                        continue;

                    item.JalaliModifyDate = item.ModifyDate.ToPersianDateTime();

                    //ايجاد محتوا براي
                    var itSelfNode = postManagement.CreateSliderTopThumbnails(item);
                    if (itSelfNode != null)
                    {
                        postManagement.AddHeadToContent(nodesIndex, "sp-thumbnails sp-slider-image-top", itSelfNode);
                    }
                }


                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(docIndex.DocumentNode.OuterHtml);
                htmlDoc.Save(path + "/Views/Home/Index.cshtml", Encoding.UTF8);
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
                    PostName = "Create Main Page - PlaceInMainPage.SliderTop - Picture",
                    PostID = -100
                });
            }


            ////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////// sidebar-widget mostviewed///////////////////////////////////////
            //بیشترین بازدید
            try
            {
                var someDaysAgo = DateTime.Now.AddDays(-5);
                docIndex.Load(path + "/Views/Shared/_Layout.cshtml", System.Text.Encoding.UTF8);
                var nodesIndex = docIndex.DocumentNode.SelectNodes("//ul");

                ////حذف محتويات ند بلاك-slider-image-bottom
                postManagement.ClearContentNode(nodesIndex, "recent_posts_wid right-slider1");

                ////ایجاد  محتوا
                int rowID = 1;
                var posts = repo.SelectPostUser().Where(p => p.ModifyDate > someDaysAgo && p.Status == null).OrderByDescending(p => p.Views).Take(5).ToList();
                listAll = Utilty.SortGroupsList(posts, PlaceInMainPage.Mostviewed);
                foreach (var item in listAll)
                {
                    //ریزور در رندر کردن یو آر ال هایی که "ات ساین" دارند به مشکل بر می خورد
                    if (item.Image1_1 != null && item.Image1_1.Contains("@"))
                        continue;

                    item.JalaliModifyDate = item.ModifyDate.ToPersianDateTime();

                    //ايجاد محتوا براي
                    var itSelfNode = postManagement.CreatePostMostViewed(item, rowID);
                    if (itSelfNode != null)
                    {
                        postManagement.AddHeadToContent(nodesIndex, "recent_posts_wid right-slider1", itSelfNode);
                    }
                    rowID++;
                }


                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(docIndex.DocumentNode.OuterHtml);
                htmlDoc.Save(path + "/Views/Shared/_Layout.cshtml", Encoding.UTF8);
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
                    PostName = "Create Main Page - PlaceInMainPage.Mostviewed",
                    PostID = -100
                });
            }

            //////////////////////// sidebar-widget popular///////////////////////////////////////
            //محبوب
            try
            {
                var someDaysAgo = DateTime.Now.AddDays(-8);
                docIndex.Load(path + "/Views/Shared/_Layout.cshtml", System.Text.Encoding.UTF8);
                var nodesIndex = docIndex.DocumentNode.SelectNodes("//ul");

                //حذف محتويات ند بلاك-slider-image-bottom
                postManagement.ClearContentNode(nodesIndex, "recent_posts_wid right-slider2");

                //ایجاد  محتوا
                int rowID = 1;
                var posts = repo.SelectPostUser().Where(p => p.ModifyDate > someDaysAgo && p.Status == null).OrderByDescending(x => x.LikePost).Take(5).ToList();
                listAll = Utilty.SortGroupsList(posts, PlaceInMainPage.Popular);
                foreach (var item in listAll)
                {
                    //ریزور در رندر کردن یو آر ال هایی که "ات ساین" دارند به مشکل بر می خورد
                    if (item.Image1_1 != null && item.Image1_1.Contains("@"))
                        continue;

                    item.JalaliModifyDate = item.ModifyDate.ToPersianDateTime();

                    //ايجاد محتوا براي
                    var itSelfNode = postManagement.CreatePostPopular(item, rowID);
                    if (itSelfNode != null)
                    {
                        postManagement.AddHeadToContent(nodesIndex, "recent_posts_wid right-slider2", itSelfNode);
                    }
                    rowID++;
                }


                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(docIndex.DocumentNode.OuterHtml);
                htmlDoc.Save(path + "/Views/Shared/_Layout.cshtml", Encoding.UTF8);
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
                    PostName = "Create Main Page - PlaceInMainPage.Popular",
                    PostID = -100
                });
            }

            ////////////////////////// آخرين خبرها///////////////////////////////////////
            try
            {
                docIndex.Load(path + "/Views/Shared/_Layout.cshtml", System.Text.Encoding.UTF8);
                var nodesIndex = docIndex.DocumentNode.SelectNodes("//ul");

                //حذف محتويات ند
                postManagement.ClearContentNode(nodesIndex, "superior-posts recent_posts_wid");

                //ایجاد  محتوا
                int rowID = 1;
                var posts = repo.SelectPostUser().Where(p => p.Status == null).OrderByDescending(x => x.PostID).Take(6).ToList();
                listAll = Utilty.SortGroupsList(posts, PlaceInMainPage.LaseNews);
                foreach (var item in listAll)
                {
                    //ریزور در رندر کردن یو آر ال هایی که "ات ساین" دارند به مشکل بر می خورد
                    if (item.Image1_1 != null && item.Image1_1.Contains("@"))
                        continue;

                    item.JalaliModifyDate = item.ModifyDate.ToPersianDateTime();

                    //ايجاد محتوا براي
                    var itSelfNode = postManagement.CreatePostSuperiorr(item, rowID);
                    if (itSelfNode != null)
                    {
                        postManagement.AddHeadToContent(nodesIndex, "superior-posts recent_posts_wid", itSelfNode);
                    }
                    rowID++;
                }


                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(docIndex.DocumentNode.OuterHtml);
                htmlDoc.Save(path + "/Views/Shared/_Layout.cshtml", Encoding.UTF8);
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
                    PostName = "Create Main Page - PlaceInMainPage.LaseNews",
                    PostID = -100
                });
            }

            //جالب
            ////////////////////////topbar-sidebar-left widget_alc_newsticker news///////////////////////////////////////
            try
            {
                docIndex.Load(path + "/Views/Shared/_Layout.cshtml", System.Text.Encoding.UTF8);
                var nodesIndex = docIndex.DocumentNode.SelectNodes("//div");

                //حذف محتويات ند
                postManagement.ClearContentNode(nodesIndex, "marquee newsticker_909 left");

                Random rand = new Random();
                int toSkip = rand.Next(0, 100);
                var someDaysAgo = DateTime.Now.AddDays(-30);
                var postTopSticker = repo.SelectPostUser().Where(p => p.ModifyDate >= someDaysAgo && p.ModifyDate < DateTime.Now && p.Status == null).OrderBy(p => p.PostID).Skip(toSkip).Take(4).ToList();
                listAll = Utilty.SortGroupsList(postTopSticker, PlaceInMainPage.StickerTop);
                //ایجاد  محتوا
                foreach (var item in listAll)
                {
                    item.JalaliModifyDate = item.ModifyDate.ToPersianDateTime();

                    //ايجاد محتوا براي
                    var itSelfNode = postManagement.CreateStickerTop(item);
                    if (itSelfNode != null)
                    {
                        postManagement.AddHeadToContentDiv(nodesIndex, "marquee newsticker_909 left", itSelfNode);
                    }
                }


                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(docIndex.DocumentNode.OuterHtml);
                htmlDoc.Save(path + "/Views/Shared/_Layout.cshtml", Encoding.UTF8);
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
                    PostName = "Create Main Page - PlaceInMainPage.SliderTop - Picture",
                    PostID = -100
                });
            }


            //ایجاد لاگ به ازای هر بار انجام فرایندهای مربوط به ایجاد صفحه اصلی
            repoLog.Add(new VM_Log()
            {
                IP = ip,
                ModifyDateTime = DateTime.Now,
                PostName = "Create Main Page",
                PostID = -100,
                LogTypeID_fk = 59,
            });
        }

        public ActionResult CreateAllCategory(string path, string domain, string ip)
        {
            //string path = ControllerContext.HttpContext.Server.MapPath("~");
            BizarreController bizarre = new BizarreController(path, domain, ip);
            bizarre.CreateContentCategory();

            AmazingController amazing = new AmazingController(path, domain, ip);
            amazing.CreateContentCategory();

            CuteController cute = new CuteController(path, domain, ip);
            cute.CreateContentCategory();

            EntertainmentController entertainment = new EntertainmentController(path, domain, ip);
            entertainment.CreateContentCategory();

            FilmsController films = new FilmsController(path, domain, ip);
            films.CreateContentCategory();

            PlacesController places = new PlacesController(path, domain, ip);
            places.CreateContentCategory();

            QuizController quiz = new QuizController(path, domain, ip);
            quiz.CreateContentCategory();

            ExcitingController exciting = new ExcitingController(path, domain, ip);
            exciting.CreateContentCategory();

            MusicController music = new MusicController(path, domain, ip);
            music.CreateMusicMenu();

            return View("/Home/Index");
        }

        public ActionResult FindKeyWords()
        {
            try
            {
                Models.GossipSiteEntities context = new Models.GossipSiteEntities();
                ProcessWord Pwords = new ProcessWord();

                // فچ کردن لیست کلمات پرتکرار و غیر کلیدی در زبان فارسی
                var wordFrequency = context.FrequencyWords.Select(p => p.FrequencyWord1).ToList();

                var posts = context.Posts.ToList();
                var tempPosts = context.PostTemperories.Where(p => p.ContentHTML != null || p.ContentHTMLText != null).ToList();

                foreach (var item in tempPosts)
                {
                    var post = context.Posts.FirstOrDefault(p => p.Subject1 == item.Subject1);
                    if (post != null)
                    {
                        //به دست آوردن کلمات کلیدی پست
                        var keyWords = Pwords.FindKeywordsFromPost((item.ContentHTMLText != null && item.ContentHTMLText != "") ?
                                                               item.ContentHTMLText :
                                                               item.SubSubject1_1 + item.Subject1,
                                         wordFrequency);

                        post.KeyWords = keyWords;
                        context.SaveChanges();
                    }

                }
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
                    PostName = "FindKeyWords All Post",
                    PostID = -100
                });
            }

          

            return Json(true, JsonRequestBehavior.AllowGet);

        }



        public ActionResult Edit()
        {
            return View();
        }


        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Pages()
        {
            return View();
        }


        public ActionResult Posts()
        {
            var repo = new Repository.PubBaseRepository();
            ViewData["PostCategory"] = repo.SelectByParentName("PostCategory");
            ViewData["PostFormat"] = repo.SelectByParentName("PostFormat");
            ViewData["PostCol"] = repo.SelectByParentName("PostCol");

            return View();
        }

        public ActionResult Users()
        {
            return View();
        }

        public ActionResult ReadPost([DataSourceRequest]DataSourceRequest request)
        {
            var res = repoManagementPost.ReadPost().ToList();
            return Json(res.ToDataSourceResult(request));
        }

        public JsonResult CreatePost([DataSourceRequest] DataSourceRequest request, VM_PostManage vm)
        {
            vm.ModifyUserID = 1;
            vm.ModifyDate = DateTime.Now;
            var res = repoManagementPost.Add(vm);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdatePost([DataSourceRequest] DataSourceRequest request, VM_PostManage vm)
        {
            var res = repoManagementPost.Update(vm);
            vm.ModifyDate = DateTime.Now;
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DestroyPost([DataSourceRequest] DataSourceRequest request, VM_PostManage vm)
        {
            var res = repoManagementPost.Delete(vm.PostID);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}