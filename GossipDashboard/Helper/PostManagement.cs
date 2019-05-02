using GossipDashboard.Models;
using GossipDashboard.ViewModel;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipDashboard.Helper
{
    public class PostManagement
    {
        //private LogRepository repoLog = new Repository.LogRepository();
        private string path;
        private string ip;

        public PostManagement()
        {
        }

        public PostManagement(string path, string domain, string ip)
        {
            this.path = path;
            Domain = domain;
            this.ip = ip;
        }

        /// <summary>
        /// ساخت آرتیکل های وسط صفحه  
        /// </summary>
        /// <param name="post">شی پست</param>
        /// <param name="templateCategory">تمپلیتی که قصد داریم از روی آن آرتیکل را بسازیم</param>
        /// <returns></returns>
        internal HtmlNode CreateBloglist(VM_Post post, string fromWhere, int rowID)
        {
            string postClassArticle = "", postClassCategory = "", postCol = "col-md-4";

            //پست های مشابه به صورت ایجکس فراخوانی می شود و با سی اس اس نیز فرمت دهی میشود
            //احتیاج ندارد col-md-4  برای همین به 
            //کاشی
            if ((fromWhere == "RelatedPost"))
                postCol = "";

            string categoryAboveClass = "", categoryAboveName = "";
            var docTemplates = new HtmlDocument();
            HtmlNodeCollection nodes = new HtmlNodeCollection(HtmlNode.CreateNode("div"));
            //string formatPostName = "standard";

            foreach (var item in post.PostFormat)
            {
                postClassArticle += " " + item.ClassName + " ";
            }

            foreach (var item in post.PostCategory)
            {
                postClassCategory += " " + item.ClassName + " ";
                PostCategory = post.PostCategory.ToList().First().NameEn;
                PostID = post.PostID.ToString();
                Subject = post.Subject1;
                categoryAboveClass += " " + item.AbobeClassName + " ";
                categoryAboveName += " " + item.NameFa + " ";
            }

            foreach (var item in post.PostCol)
            {
                //مربوط به صفحه CreateIndexPage
                //در صورتی که در سه پست اولیه تعداد ستون چهارتا نبود به چهار تنظیم شود
                if ((fromWhere == "CreateIndexPage" && rowID <= 2)  || (fromWhere == "CreatePostAjax") || (fromWhere == "MusicPage") || (fromWhere == "SearchPostAjax"))
                {
                    int val = 0;
                    var col = item.NameEn;
                    if (col.Split('-').LastOrDefault() != null)
                    {
                        if (int.TryParse(col.Split('-').Last(), out val))
                        {
                            if (val != 4)
                            {
                                postCol = "col-md-4 col-lg-4";
                            }
                        }
                    }
                }
                //پست های مشابه به صورت ایجکس فراخوانی می شود و با سی اس اس نیز فرمت دهی میشود
                //احتیاج ندارد col-md-4  برای همین به 
                //کاشی
                 else if ((fromWhere == "RelatedPost"))
                    postCol = "";
                else
                    postCol = " " + item.ClassName + " ";
            }

            //هر پست یک فرمت پست دارد
            var postFormat = post.PostFormat.ToList().FirstOrDefault();
            if (postFormat != null)
                formatPostName = postFormat.NameEn;

            switch (formatPostName)
            {
                case "standard":
                    docTemplates.Load(path + "/Templates/format-standard.html", System.Text.Encoding.UTF8);
                    nodes = CreateHeadStandard(post, categoryAboveClass, categoryAboveName, urlCategory, docTemplates, postUrl);
                    break;

                case "audio":
                    docTemplates.Load(path + "/Templates/format-audio.html", System.Text.Encoding.UTF8);
                    nodes = CreateHeadAudio(post, categoryAboveClass, categoryAboveName, urlCategory, docTemplates, postUrl);
                    break;

                case "video":
                    docTemplates.Load(path + "/Templates/format-video.html", System.Text.Encoding.UTF8);
                    nodes = CreateHeadVideo(post, categoryAboveClass, categoryAboveName, urlCategory, docTemplates, postUrl);
                    break;

                case "gallery":
                    docTemplates.Load(path + "/Templates/format-gallery.html", System.Text.Encoding.UTF8);
                    nodes = CreateHeadGallery(post, categoryAboveClass, categoryAboveName, urlCategory, docTemplates, postUrl);
                    break;

                case "link":
                    docTemplates.Load(path + "/Templates/format-link.html", System.Text.Encoding.UTF8);
                    nodes = CreateHeadLink(post, categoryAboveClass, categoryAboveName, urlCategory, docTemplates, postUrl);
                    break;

                case "quote":
                    docTemplates.Load(path + "/Templates/format-quote.html", System.Text.Encoding.UTF8);
                    nodes = CreateHeadQuote(post, categoryAboveClass, categoryAboveName, urlCategory, docTemplates, postUrl);
                    break;

                case "image ":
                    docTemplates.Load(path + "/Templates/format-image.html", System.Text.Encoding.UTF8);
                    nodes = CreateHeadimage(post, categoryAboveClass, categoryAboveName, urlCategory, docTemplates, postUrl);
                    break;

                case "status":
                    docTemplates.Load(path + "/Templates/format-status.html", System.Text.Encoding.UTF8);
                    nodes = CreateHeadStatus(post, categoryAboveClass, categoryAboveName, urlCategory, docTemplates, postUrl);
                    break;

                case "aparat":
                    docTemplates.Load(path + "/Templates/format-aparat.html", System.Text.Encoding.UTF8);
                    nodes = CreateHeadAparat(post, categoryAboveClass, categoryAboveName, urlCategory, docTemplates, postUrl);
                    break;

                default:
                    docTemplates.Load(path + "/Templates/format-standard.html", System.Text.Encoding.UTF8);
                    nodes = CreateHeadStandard(post, categoryAboveClass, categoryAboveName, urlCategory, docTemplates, postUrl);
                    break;
            }

            //article ايجاد تگ
            HtmlNode articleNode = HtmlNode.CreateNode("<article class='" + postCol + " hentry " + postClassArticle + postClassCategory + "'></article>");
            articleNode.AppendChild(nodes.FirstOrDefault());

            //repoLog.Add(new VM_Log()
            //{
            //    IP = "",
            //    ModifyDateTime = DateTime.Now,
            //    PostName = "CreateBloglist",
            //    PostID = -100,
            //    LogTypeID_fk = 59,
            //});

            return articleNode;
        }

        /// <summary>
        /// ساخت آرتیکل های استاندارد وسط صفحه
        /// </summary>
        /// <param name="post">شی پست</param>
        /// <param name="templateCategory">تمپلیتی که قصد داریم از روی آن آرتیکل را بسازیم</param>
        /// <returns></returns>
        //private HtmlNode CreateHeadStandard(VM_Post post, string templateCategory)
        private HtmlNodeCollection CreateHeadStandard(VM_Post post, string categoryAboveClass, string categoryAboveName, string urlCategory, HtmlDocument docTemplates, string postUrl)
        {
            var nodes = docTemplates.DocumentNode.SelectNodes("//div");
            foreach (var itemNode in nodes)
            {
                var attrs = itemNode.Attributes;
                foreach (var itemAttr in attrs)
                {
                    //entry-cover پيدا كردن تگ ديو با كلاس
                    if (itemAttr.Value == "entry-cover")
                    {
                        //ايجاد entry-cover
                        //برادر اول
                        HtmlNode oldChild = itemNode.SelectSingleNode("//a");
                        HtmlNode newChild = HtmlNode.CreateNode("<a href='" + postUrl + "' name='" + post.PostID + "'>  <img  class='lazy'  width='290' height='170' src='" + post.Image1_1 + "'  alt='" + post.Subject1 + "' />  </a>");
                        itemNode.ReplaceChild(newChild, oldChild);

                        //برادر بعدي
                        oldChild = itemNode.SelectSingleNode("//div/div/div");
                        newChild = HtmlNode.CreateNode(@"<div class='post-category'><a href='" + urlCategory + "' class='" + categoryAboveClass + "'>" + categoryAboveName + "</a></div>");
                        itemNode.ReplaceChild(newChild, oldChild);

                        // برادر بعدي
                        oldChild = itemNode.SelectSingleNode("/article[1]/div[1]/div[1]/a[2]");
                        newChild = HtmlNode.CreateNode(@"<a href='" + postUrl + "' class='special-rm-arrow'><i class='fa fa-arrow-right'></i></a>");
                        itemNode.ReplaceChild(newChild, oldChild);
                    }

                    //entry-content پيدا كردن تگ ديو با كلاس
                    if (itemAttr.Value == "entry-content")
                    {
                        //ايجاد entry-content
                        HtmlNode oldChild = itemNode.SelectSingleNode("/article[1]/div[1]/div[2]/h3");
                        HtmlNode newChild = HtmlNode.CreateNode(@"<h3 class=""entry-title""> <a href='" + postUrl + "'>" + post.Subject1 + "</a></h3>");
                        itemNode.ReplaceChild(newChild, oldChild);
                    }

                    //entry-footer پيدا كردن تگ ديو با كلاس
                    if (itemAttr.Value == "entry-footer")
                    {
                        HtmlNode oldChild = itemNode.SelectSingleNode("/article[1]/div[1]/div[3]/div[1]");
                        HtmlNode newChild = HtmlNode.CreateNode(@"<div class=' row'>
                                                                        <div class='col-md-12'>
                                                                            <ul class='common-meta' style='width:auto'>
                                                                                <li>
                                                                                    <i class='fa fa-user'></i>
                                                                                    <a href='#' title=ایجادکننده'" + post.Fullname + "' rel='author'>" + post.Fullname + "</a>" +
                                                                                "</li>" +
                                                                                "<li>" +
                                                                                    "<i class='fa fa-comment'> " + post.CommentCount + "</i>" +
                                                                                    "<a href='#' > " +
                                                                                "</li>" +
                                                                                "<li class='post-like'>" +
                                                                                    "<a href='#'>" +
                                                                                        "<p class='fa fa-eye'></p> " + post.Views +
                                                                                   "</a>" +
                                                                                "</li>" +
                                                                            "</ul>" +
                                                                        "</div>" +
                                                                    "</div>");
                        itemNode.ReplaceChild(newChild, oldChild);
                    }
                }
            }

            return nodes;
        }

        private HtmlNodeCollection CreateHeadAudio(VM_Post post, string categoryAboveClass, string categoryAboveName, string urlCategory, HtmlDocument docTemplates, string postUrl)
        {
            var nodes = docTemplates.DocumentNode.SelectNodes("//div");
            foreach (var itemNode in nodes)
            {
                var attrs = itemNode.Attributes;
                foreach (var itemAttr in attrs)
                {
                    //defaultForAllPost پيدا كردن تگ ديو با كلاس
                    if (itemAttr.Value.Contains("defaultForAllPost"))
                    {
                        HtmlNode oldChild = itemNode.SelectSingleNode("/article[1]/div[1]/div[1]");
                        HtmlNode newChild = HtmlNode.CreateNode(@"<div class='post-box'>" +
                                                                        "<div class='entry-cover'>" +
                                                                            "<div class='entry-cover'>" +
                                                                                "<a href='" + postUrl + "'>" +
                                                                                    "<img   width='290' height='170' src='" + post.Image1_1 + "'" +
                                                                                         "class='lazy'" +
                                                                                         "alt='" + post.Subject1 + "' />" +
                                                                                "</a>" +
                                                                            "</div>" +
                                                                            "<div class='audio-container'>" +
                                                                                "<!--[if lt IE 9]><script>document.createElement('audio');</script><![endif]-->" +
                                                                                "<audio class='wp-audio-shortcode' id='" + post.PostID + "' preload='none' style='width: 100%;' controls='controls'>" +
                                                                                    "<source type='audio/mpeg' src='" + post.UrlMP3 + "'?_=1' />" +
                                                                                    "<a href='" + post.UrlMP3 + "'>" + post.UrlMP3 + "</a>" +
                                                                                "</audio>" +
                                                                            "</div>" +
                                                                            "<div class='post-category'>" +
                                                                                "<a href='" + urlCategory + "' class='" + categoryAboveClass + "'>" + categoryAboveName + " </a>" +
                                                                            "</div>" +
                                                                            "<a href='" + postUrl + "' class='special-rm-arrow'>" +
                                                                                "<i class='fa fa-arrow-right'></i>" +
                                                                            "</a>" +

                                                                        //"<a href='post/post-21.html' class='special-rm-arrow'>" +
                                                                        //    "<i class='fa fa-arrow-right'></i>" +
                                                                        //"</a>" +
                                                                        "</div>" +
                                                                        "<div class='entry-content'>" +
                                                                            "<h3 class='entry-title'>" +
                                                                                "<a href='" + postUrl + "'>" + post.Subject1 + "</a>" +
                                                                            "</h3>" +
                                                                        "</div>" +
                                                                        "<div class='entry-footer'>" +
                                                                            "<div class=' row'>" +
                                                                                "<div class='col-md-12'>" +
                                                                                    "<ul class='common-meta'>" +
                                                                                        "<li>" +
                                                                                            "<i class='fa fa-user'></i>" +
                                                                                            "<a href='#' title=ایجاد شده توسط'" + post.Fullname + "' rel='author'>" + post.Fullname + "</a>" +
                                                                                       " </li>" +

                                                                                        "<li>" +
                                                                                            "<i class='fa fa-comment'></i>" +
                                                                                            "<a href='#'>" + post.CommentCount + "</a> " +
                                                                                        "</li> " +
                                                                                        "<li class='post-like'>" +
                                                                                            "<a href='#'> " +
                                                                                                "<i class='fa fa-eye'></i>" + post.Views + "" +
                                                                                            "</a>" +
                                                                                        "</li>" +
                                                                                    "</ul>" +
                                                                                "</div>" +
                                                                           " </div>" +
                                                                        "</div>" +
                                                                   "</div>");
                        itemNode.ReplaceChild(newChild, oldChild);
                    }
                }
            }

            return nodes;
        }

        private HtmlNodeCollection CreateHeadGallery(VM_Post post, string categoryAboveClass, string categoryAboveName, string urlCategory, HtmlDocument docTemplates, string postUrl)
        {
            var nodes = docTemplates.DocumentNode.SelectNodes("//div");
            foreach (var itemNode in nodes)
            {
                var attrs = itemNode.Attributes;
                foreach (var itemAttr in attrs)
                {
                    //defaultForAllPost پيدا كردن تگ ديو با كلاس
                    if (itemAttr.Value.Contains("defaultForAllPost"))
                    {
                        HtmlNode oldChild = itemNode.SelectSingleNode("/article[1]/div[1]/div[1]");
                        HtmlNode newChild = HtmlNode.CreateNode("<div class='post-box'>" +
                                                                    "<div class='entry-cover'>" +
                                                                        "<div id='galpost" + post.PostID + "' class='carousel slide gallery_post' data-ride='carousel'>" +
                                                                            "<div class='carousel-inner' role='listbox'>" +
                                                                                "<div class='item'>" +
                                                                                    "<img   src='" + post.Image1_1 + "' class='blog-post-img lazy'" +
                                                                                            "alt='" + post.Subject1 + "' width='665' height='315' />" +
                                                                                "</div>" +
                                                                                "<div class='item'>" +
                                                                                    "<img   src='" + post.Image1_2 + "' class='blog-post-img lazy'" +
                                                                                            "alt='" + post.Subject1 + "' width='665' height='315' />" +
                                                                                "</div>" +
                                                                                "<div class='item'>" +
                                                                                    "<img   src='" + post.Image1_3 + "' class='blog-post-img lazy'" +
                                                                                            "alt='" + post.Subject1 + "' width='665' height='315' />" +
                                                                                "</div>" +
                                                                            "</div>" +
                                                                            "<a class='left carousel-control' href='#galpost" + post.PostID + "' role='button' data-slide='prev'>" +
                                                                                "<span class='glyphicon glyphicon-chevron-left' aria-hidden='true'></span>" +
                                                                                "<span class='sr-only'>قبلی</span>" +
                                                                            "</a>" +
                                                                            "<a class='right carousel-control' href='#galpost" + post.PostID + "' role='button' data-slide='next'>" +
                                                                                "<span class='glyphicon glyphicon-chevron-right' aria-hidden='true'></span>" +
                                                                                "<span class='sr-only'>بعدی</span>" +
                                                                            "</a>" +
                                                                        "</div>" +
                                                                        "<script type='text/javascript'> " +
                                                                            "jQuery('document').ready(function () {" +
                                                                                "jQuery('.gallery_post .carousel-inner div.item').first().addClass('active'); " +
                                                                                "jQuery('.carousel').carousel({" +
                                                                                    "interval: 3000" +
                                                                                "}); " +
                                                                            "})" +
                                                                        "</script>" +
                                                                        "<div class='post-category'>" +
                                                                            "<a href='" + urlCategory + "' class='" + categoryAboveClass + "'>" + categoryAboveName + "</a>" +
                                                                        "</div>" +
                                                                        "<a href='" + postUrl + "' class='special-rm-arrow'>" +
                                                                            "<i class='fa fa-arrow-right'></i>" +
                                                                        "</a>" +
                                                                    "</div>" +
                                                                    "<div class='entry-content'>" +
                                                                        "<h3 class='entry-title'>" +
                                                                            "<a href='" + postUrl + "'>" + post.Subject1 + "</a>" +
                                                                        "</h3>" +
                                                                    "</div>" +
                                                                    "<div class='entry-footer'>" +
                                                                        "<div class=' row'>" +
                                                                            "<div class='col-md-12'>" +
                                                                                "<ul class='common-meta'>" +
                                                                                    "<li>" +
                                                                                        "<i class='fa fa-user'></i>" +
                                                                                        "<a href='#' title='ایجاد شده توسط' rel='author'>" + post.Fullname + "</a>" +
                                                                                    "</li>" +
                                                                                    "<li>" +
                                                                                        "<i class='fa fa-comment'></i>" +
                                                                                        "<a href='#'>" + post.CommentCount + "</a> " +
                                                                                    "</li> " +
                                                                                    "<li class='post-like'>" +
                                                                                        "<a href='#'> " +
                                                                                            "<i class='fa fa-eye'></i>" + post.Views + "" +
                                                                                        "</a>" +
                                                                                    "</li>" +
                                                                                "</ul>" +
                                                                            "</div>" +
                                                                        "</div>" +
                                                                    "</div>" +
                                                                 "</div>");
                        itemNode.ReplaceChild(newChild, oldChild);
                    }
                }
            }

            return nodes;
        }

        private HtmlNodeCollection CreateHeadimage(VM_Post post, string categoryAboveClass, string categoryAboveName, string urlCategory, HtmlDocument docTemplates, string postUrl)
        {
            var nodes = docTemplates.DocumentNode.SelectNodes("//div");
            foreach (var itemNode in nodes)
            {
                var attrs = itemNode.Attributes;
                foreach (var itemAttr in attrs)
                {
                    //defaultForAllPost پيدا كردن تگ ديو با كلاس
                    if (itemAttr.Value.Contains("defaultForAllPost"))
                    {
                        HtmlNode oldChild = itemNode.SelectSingleNode("/article[1]/div[1]/div[1]");
                        HtmlNode newChild = HtmlNode.CreateNode(" <div class='post-box' style='background-repeat:no-repeat; background-size:cover; background-image:url(" + post.Image1_1 + ")'>" +
                                                                    "<div class='bg-overlay' style='background-color:rgba(0,0,0, 0.8)'></div>" +
                                                                    "<div class='entry-cover'>" +
                                                                        "<div class='post-category'>" +
                                                                            "<a href = '" + urlCategory + "' class='" + categoryAboveClass + "'>" + categoryAboveName + "</a>" +
                                                                        "</div>" +
                                                                    "</div>" +
                                                                    "<div class='entry-content'>" +
                                                                        "<h3 class='entry-title'>" +
                                                                            "<a href = '" + postUrl + "' >" + post.Subject1 + "</a>" +
                                                                        "</h3>" +
                                                                    "</div>" +
                                                                    "<div class='entry-footer'>" +
                                                                        "<div class=' row'>" +
                                                                            "<div class='col-md-12'>" +
                                                                                "<ul class='common-meta'>" +
                                                                                    "<li>" +
                                                                                        "<i class='fa fa-user'></i>" +
                                                                                        "<a href = '#' title=ایجاد شده توسط'" + post.Fullname + "' rel='author'>" + post.Fullname + "</a>" +
                                                                                    "</li>" +
                                                                                    "<li>" +
                                                                                        "<i class='fa fa-comment'></i>" +
                                                                                        "<a href = '#'> 0 </ a > " +
                                                                                    "</li > " +
                                                                                    "<li class='post-like'>" +
                                                                                        "<a href = '#' > " +
                                                                                            "<i class='fa fa-eye'></i>" + post.Views +
                                                                                        "</a>" +
                                                                                    "</li>" +
                                                                                "</ul>" +
                                                                            "</div>" +
                                                                        "</div>" +
                                                                    "</div>" +
                                                                "</div>");
                        itemNode.ReplaceChild(newChild, oldChild);
                    }
                }
            }

            return nodes;
        }

        private HtmlNodeCollection CreateHeadLink(VM_Post post, string categoryAboveClass, string categoryAboveName, string urlCategory, HtmlDocument docTemplates, string postUrl)
        {
            var nodes = docTemplates.DocumentNode.SelectNodes("//div");
            foreach (var itemNode in nodes)
            {
                var attrs = itemNode.Attributes;
                foreach (var itemAttr in attrs)
                {
                    //defaultForAllPost پيدا كردن تگ ديو با كلاس
                    if (itemAttr.Value.Contains("defaultForAllPost"))
                    {
                        HtmlNode oldChild = itemNode.SelectSingleNode("/article[1]/div[1]/div[1]");
                        HtmlNode newChild = HtmlNode.CreateNode("<div class='post-box'>" +
                                                                    "<div class='entry-content'>" +
                                                                        "<div class='bg-overlay' style='background-color:" + post.BackgroundColor + "'></div>" +
                                                                        "<h3>" +
                                                                           post.Subject1 +
                                                                        "</h3>" +
                                                                       " <a href = '" + post.SourceSiteUrl + "' target='_blank'> " +
                                                                            "<i class='fa fa-link'></i> لینک خبر" +
                                                                        "</a>" +
                                                                    "</div>" +
                                                                "</div>");
                        itemNode.ReplaceChild(newChild, oldChild);
                    }
                }
            }

            return nodes;
        }

        private HtmlNodeCollection CreateHeadQuote(VM_Post post, string categoryAboveClass, string categoryAboveName, string urlCategory, HtmlDocument docTemplates, string postUrl)
        {
            var nodes = docTemplates.DocumentNode.SelectNodes("//div");
            foreach (var itemNode in nodes)
            {
                var attrs = itemNode.Attributes;
                foreach (var itemAttr in attrs)
                {
                    //defaultForAllPost پيدا كردن تگ ديو با كلاس
                    if (itemAttr.Value.Contains("defaultForAllPost"))
                    {
                        HtmlNode oldChild = itemNode.SelectSingleNode("/article[1]/div[1]/div[1]");
                        HtmlNode newChild = HtmlNode.CreateNode("<div class='post-box imgwrapper'>" +
                                                                    "<div class='entry-content img-responsive' style='background-image:url(" + post.Image1_1 + ")'>" +
                                                                        "<div class='bg-overlay' style='background-color:" + post.BackgroundColor + "'></div>" +
                                                                        "<blockquote>" +
                                                                            "<h4>" +
                                                                                "<a href = '" + postUrl + "' >" + post.Subject1 + "</a>" +
                                                                            "</h4>" +
                                                                            "<cite>" + post.QuotedFrom + "</cite>" +
                                                                        "</blockquote>" +
                                                                    "</div>" +
                                                                "</div>");
                        itemNode.ReplaceChild(newChild, oldChild);
                    }
                }
            }

            return nodes;
        }

        private HtmlNodeCollection CreateHeadStatus(VM_Post post, string categoryAboveClass, string categoryAboveName, string urlCategory, HtmlDocument docTemplates, string postUrl)
        {
            var nodes = docTemplates.DocumentNode.SelectNodes("//div");
            foreach (var itemNode in nodes)
            {
                var attrs = itemNode.Attributes;
                foreach (var itemAttr in attrs)
                {
                    //defaultForAllPost پيدا كردن تگ ديو با كلاس
                    if (itemAttr.Value.Contains("defaultForAllPost"))
                    {
                        HtmlNode oldChild = itemNode.SelectSingleNode("/article[1]/div[1]/div[1]");
                        HtmlNode newChild = HtmlNode.CreateNode("<div class='post-box'>" +
                                                                    "<div class='entry-content'>" +
                                                                        "<h3 class='status lead'>" + post.Subject1 + "</h3>" +
                                                                    "</div>" +
                                                                "</div>");
                        itemNode.ReplaceChild(newChild, oldChild);
                    }
                }
            }

            return nodes;
        }

        private HtmlNodeCollection CreateHeadAparat(VM_Post post, string categoryAboveClass, string categoryAboveName, string urlCategory, HtmlDocument docTemplates, string postUrl)
        {
            var nodes = docTemplates.DocumentNode.SelectNodes("//div");
            foreach (var itemNode in nodes)
            {
                var attrs = itemNode.Attributes;
                foreach (var itemAttr in attrs)
                {
                    //defaultForAllPost پيدا كردن تگ ديو با كلاس
                    if (itemAttr.Value.Contains("defaultForAllPost"))
                    {
                        HtmlNode oldChild = itemNode.SelectSingleNode("/article[1]/div[1]/div[1]");
                        HtmlNode newChild = HtmlNode.CreateNode("<div class='post-box'>" +
                                                                    "<div class='entry-cover'>" +
                                                                      post.ScriptAparat +
                                                                        //"<div id = '15437560721097752' > " +
                                                                        //    "<script type='text/JavaScript' src='https://www.aparat.com/embed/0Z2eg?data[rnddiv]=15437560721097752&data[responsive]=yes'></script>" +
                                                                        //"</div>" +
                                                                        "<div class='post-category'>" +
                                                                            "<a href = '" + urlCategory + "' class='" + categoryAboveClass + "'> " + categoryAboveName + "</a>" +
                                                                        "</div>" +
                                                                    //"<a href = '@Url.Action('Post','bizarre', new {postID = 2249})' class='special-rm-arrow'><i class='fa fa-arrow-right'></i></a>"+
                                                                    "</div>" +
                                                                    "<div class='entry-content'>" +
                                                                        "<h3 class='entry-title'> <a href = '" + postUrl + "'>" + post.Subject1 + "</a></h3>" +
                                                                    "</div>" +
                                                                    "<div class='entry-footer'>" +
                                                                        "<div class=' row'>" +
                                                                            "<div class='col-md-12'>" +
                                                                                "<ul class='common-meta' style='width:auto'>" +
                                                                                    "<li>" +
                                                                                        "<i class='fa fa-user'></i>" +
                                                                                        "<a href = '#' title=ایجاد شده توسط'" + post.Fullname + "' rel='author'>" + post.Fullname + "</a>" +
                                                                                    "</li>" +
                                                                                   "<li>" +
                                                                                        "<i class='fa fa-comment'></i>" +
                                                                                        "<a href = '#'> 0 </ a > " +
                                                                                    "</li > " +
                                                                                    "<li class='post-like'>" +
                                                                                        "<a href = '#' > " +
                                                                                            "<i class='fa fa-eye'></i>" + post.Views +
                                                                                        "</a>" +
                                                                                    "</li>" +
                                                                                "</ul>" +
                                                                            "</div>" +
                                                                        "</div>" +
                                                                    "</div>" +
                                                                "</div>");
                        itemNode.ReplaceChild(newChild, oldChild);
                    }
                }
            }

            return nodes;
        }

        internal HtmlNode CreateStickerTop(VM_Post post)
        {
            string categoryAboveClass = "", categoryAboveName = "";


            foreach (var item in post.PostCategory)
            {

                PostCategory = post.PostCategory.ToList().First().NameEn;
                PostID = post.PostID.ToString();
                Subject = post.Subject1;

                var postFormat = post.PostFormat.ToList().FirstOrDefault();
                if (postFormat != null)
                    formatPostName = postFormat.NameEn;



                categoryAboveClass += " " + item.AbobeClassName + " ";
                categoryAboveName += " " + item.NameFa + " ";
            }

            HtmlNode newChild = HtmlNode.CreateNode("<a href='"+ postUrl + "'>"+ post.Subject1 + "</a>");
            return newChild;
        }

        private HtmlNodeCollection CreateHeadVideo(VM_Post post, string categoryAboveClass, string categoryAboveName, string urlCategory, HtmlDocument docTemplates, string postUrl)
        {
            var nodes = docTemplates.DocumentNode.SelectNodes("//div");
            foreach (var itemNode in nodes)
            {
                var attrs = itemNode.Attributes;
                foreach (var itemAttr in attrs)
                {
                    //defaultForAllPost پيدا كردن تگ ديو با كلاس
                    if (itemAttr.Value.Contains("defaultForAllPost"))
                    {
                        HtmlNode oldChild = itemNode.SelectSingleNode("/article[1]/div[1]/div[1]");
                        HtmlNode newChild = HtmlNode.CreateNode("<div class='post-box'>" +
                                                                    "<div class='entry-cover'>" +
                                                                        "<div class='flex-video'>" +
                                                                            "<div style = 'width: 640px;' class='wp-video'>" +
                                                                                "<!--[if lt IE 9]><script>document.createElement('video');</script><![endif]-->" +
                                                                                "<video class='wp-video-shortcode' id='video-" + post.PostID + "' width='640' height='360' preload='metadata' controls='controls'>" +
                                                                                    "<source type = 'video/mp4' src='" + post.UrlVideo + "?_=1'/>" +
                                                                                    "<a href = '" + post.UrlVideo + "' >" + post.UrlVideo + " </a> " +
                                                                                "</video> " +
                                                                            "</div> " +
                                                                        "</div> " +
                                                                        "<div class='post-category'>" +
                                                                             "<a href='" + urlCategory + "' class='" + categoryAboveClass + "'>" + categoryAboveName + "</a>" +
                                                                        "</div>" +
                                                                        "<a href = '" + postUrl + "' class='special-rm-arrow'>" +
                                                                            "<i class='fa fa-arrow-right'></i>" +
                                                                        "</a>" +
                                                                    "</div>" +
                                                                    "<div class='entry-content'>" +
                                                                        "<h3 class='entry-title'>" +
                                                                            "<a href = '" + postUrl + "' >" + post.Subject1 + "</a>" +
                                                                        "</h3>" +
                                                                   " </div>" +
                                                                    "<div class='entry-footer'>" +
                                                                        "<div class=' row'>" +
                                                                            "<div class='col-md-12'>" +
                                                                                "<ul class='common-meta'>" +
                                                                                    "<li>" +
                                                                                        "<i class='fa fa-user'></i>" +
                                                                                        "<a href = '#' title='" + post.Fullname + "' rel='author'>" + post.Fullname + "</a>" +
                                                                                    "</li>" +
                                                                                    "<li>" +
                                                                                        "<i class='fa fa-comment'></i>" +
                                                                                        "<a href = '#'" + post.CommentCount + "> 0 </a> " +
                                                                                    "</li> " +
                                                                                    "<li class='post-like'>" +
                                                                                        "<a href = '#' > " +
                                                                                            "<i class='fa fa-eye'></i>" + post.Views + "" +
                                                                                        "</a>" +
                                                                                    "</li>" +
                                                                                "</ul>" +
                                                                            "</div>" +
                                                                        "</div>" +
                                                                    "</div>" +
                                                                "</div>");
                        itemNode.ReplaceChild(newChild, oldChild);
                    }
                }
            }

            return nodes;
        }

        /// <summary>
        /// اضافه كردن محتوا به وسط صفحه
        /// </summary>
        /// <param name="nodesIndex">ند محتوا</param>
        /// <param name="attrValue">نام کلاسی که مشخص می کند محتوای این قسمت را می خواهیم</param>
        /// <param name="itSelfNode">ندی که می خواهید به محتوا اضافه کنید</param>
        /// <returns></returns>
        internal HtmlNode AddHeadToContentDiv(HtmlNodeCollection nodesIndex, string attrValue, HtmlNode itSelfNode)
        {
            foreach (var itemNode in nodesIndex)
            {
                var attrs = itemNode.Attributes;
                foreach (var itemAttr in attrs)
                {
                    if (itemAttr.Value == attrValue)
                    {
                        itemNode.AppendChild(itSelfNode);
                        return nodesIndex.FirstOrDefault();
                    }
                }
            }

            return null;
        }


        //
        internal HtmlNode AddHeadToContent(HtmlNodeCollection nodesIndex, string attrValue, HtmlNode itSelfNode)
        {
            var itemNode = nodesIndex.FirstOrDefault(x => x.Attributes.FirstOrDefault().Value == attrValue);
            if (itemNode != null)
            {
                itemNode.AppendChild(itSelfNode);
                return itemNode;
            }

            return null;
        }

        internal HtmlNode GetContentNode(HtmlNodeCollection nodesIndex, string attrValue)
        {
            var itemNode = nodesIndex.FirstOrDefault(x => x.Attributes.FirstOrDefault().Value == attrValue);
            return itemNode;
        }

        /// <summary>
        /// پاک کردن محتوایات ند
        /// </summary>
        /// <param name="nodesIndex">
        /// ندی که قصد دارید تمامی محتوا یا قسمتی از محتوای آن را حذف کنید
        /// </param>
        /// <param name="attrValue">نام کلاسی که مشخص می کند محتوا از این قسمت پاک گردد</param>
        /// <returns>bool</returns>
        internal bool ClearContentNode(HtmlNodeCollection nodesIndex, string attrValue)
        {
            if (nodesIndex == null || attrValue == "")
                return false;

            foreach (var itemNode in nodesIndex)
            {
                var attrs = itemNode.Attributes;
                foreach (var itemAttr in attrs)
                {
                    if (itemAttr.Value == attrValue)
                    {
                        itemNode.RemoveAllChildren();
                        return true;
                    }
                }
            }

            return false;
        }


        /// <summary>
        /// پاک کردن محتوایات ند
        /// </summary>
        /// <param name="nodesIndex">
        /// ندی که قصد دارید تمامی محتوا یا قسمتی از محتوای آن را حذف کنید
        /// </param>
        /// <param name="attrValue">نام کلاسی که مشخص می کند محتوا از این قسمت پاک گردد</param>
        /// <returns>bool</returns>
        internal bool ClearContentNodeByContain(HtmlNodeCollection nodesIndex, string attrValue)
        {
            if (nodesIndex == null || attrValue == "")
                return false;

            foreach (var itemNode in nodesIndex)
            {
                var attrs = itemNode.Attributes;
                foreach (var itemAttr in attrs)
                {
                    if (itemAttr.Value.Contains(attrValue))
                    {
                        itemNode.RemoveAllChildren();
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// حذف کردن ند
        /// </summary>
        /// <param name="nodesIndex">
        /// ندی که قصد دارید تمامی محتوا یا قسمتی از محتوای آن را حذف کنید
        /// </param>
        /// <param name="attrValue">نام کلاسی که مشخص می کند محتوا از این قسمت پاک گردد</param>
        /// <returns>bool</returns>
        public bool DeleteNodes(HtmlNodeCollection nodesIndex)
        {
            if (nodesIndex != null)
            {
                foreach (var itemNode in nodesIndex)
                {
                    itemNode.RemoveAll();
                }
            }

            return true;
        }

        //catlist-heading
        public HtmlNode CreateCatListHeading(List<PubBase> category)
        {
            var docTemplates = new HtmlDocument();
            docTemplates.Load(path + "/Templates/cat-list.html", System.Text.Encoding.UTF8);
            var nodes = docTemplates.DocumentNode.SelectNodes("//div");
            foreach (var itemNode in nodes)
            {
                var attrs = itemNode.Attributes;
                foreach (var itemAttr in attrs)
                {
                    //catlist-heading پيدا كردن تگ ديو با كلاس
                    if (itemAttr.Value.Contains("catlist-heading"))
                    {
                        var ul = "ul class='nav nav-tabs";
                        int i = 0;
                        foreach (var item in category)
                        {
                            if (i == 0)
                                ul += "<li>" + "<a href = '#' data-filter='." + item.NameEn + "' class='active'>" + item.NameFa + "</a>" + "</li>";
                            else
                                ul += "<li>" + "<a href = '#' data-filter='." + item.NameEn + "' class=''>" + item.NameFa + "</a>" + "</li>";

                            i++;
                        }
                        ul += "</ul>";

                        HtmlNode oldChild = itemNode.SelectSingleNode("/div[1]/div[1]/div[1]");
                        HtmlNode newChild = HtmlNode.CreateNode("<div><h4>" +
                                                                    "<span class='catlist-title'>رویدادهای دیگر</span>" +
                                                                "</h4>"
                                                                + ul + "</div>"
                                                                );

                        return itemNode.ReplaceChild(newChild, oldChild);
                    }
                }
            }

            return null;
        }

        public HtmlNode CreateCatListContent(VM_Post post)
        {
            string catListClass = "";

            foreach (var item in post.PostCategory)
            {


                //urlCategory = "@Url.Action(\"Index\",\"" + post.PostCategory.ToList().First().NameEn + "\")";
                //postUrl = "@Url.Action(\"Post\",\"" + post.PostCategory.ToList().First().NameEn + "\", new {postID = \"" + post.PostID + "&" + Utilty.GenerateSlug(post.Subject1) + "\"})";


                PostCategory = post.PostCategory.ToList().First().NameEn;
                PostID = post.PostID.ToString();
                Subject = post.Subject1;

                var postFormat = post.PostFormat.ToList().FirstOrDefault();
                if (postFormat != null)
                    formatPostName = postFormat.NameEn;

                catListClass += " " + item.NameEn + " ";
            }

            var docTemplates = new HtmlDocument();
            docTemplates.Load(path + "/Templates/cat-list.html", System.Text.Encoding.UTF8);
            var nodes = docTemplates.DocumentNode.SelectNodes("//div");
            foreach (var itemNode in nodes)
            {
                var attrs = itemNode.Attributes;
                foreach (var itemAttr in attrs)
                {
                    //tab-content پيدا كردن تگ ديو با كلاس
                    if (itemAttr.Value.Contains("tab-content"))
                    {
                        HtmlNode oldChild = itemNode.SelectSingleNode("/div[1]/div[2]/div[1]");
                        HtmlNode newChild = HtmlNode.CreateNode("<div class='col-md-6 catlist-posts small-posts " + catListClass + "  ' style='position: absolute; display: none; right:0px;'>" +
                                                                    "<a href = '" + postUrl + "' > " +
                                                                        "<img   width = '70' height = '70' src = '" + post.Image1_1 + "' class='lazy' alt='' srcset='' sizes='(max-width: 70px) 100vw, 70px'>" +
                                                                    "</a>" +
                                                                    "<div class='catitem-sm-content'>" +
                                                                        "<h4 class='catitem-title'>" +
                                                                            "<a href = '" + postUrl + "'> " + post.Subject1 + "</a>" +
                                                                        "</h4>" +
                                                                        "<div class='catitem-meta'>" +
                                                                            "<span class='catitem-date'>" +
                                                                                "<i class='fa fa-calendar'></i>" + post.JalaliModifyDate + "" +
                                                                            "</span>" +
                                                                            "<span class='catitem-author'>" +
                                                                                "<i class='fa fa-user'></i> " + post.Fullname + "" +
                                                                            "</span>" +
                                                                        "</div>" +
                                                                    "</div>" +
                                                                "</div>");

                        return itemNode.ReplaceChild(newChild, oldChild);
                    }
                }
            }

            return null;
        }


        internal HtmlNode CreateBloglistContent(VM_Post post)
        {

            string categoryAboveClass = "", categoryAboveName = "";


            foreach (var item in post.PostCategory)
            {
                //urlCategory = "@Url.Action(\"Index\",\"" + post.PostCategory.ToList().First().NameEn + "\")";
                //postUrl = "@Url.Action(\"Post\",\"" + post.PostCategory.ToList().First().NameEn + "\", new {postID = \"" + post.PostID + "&" + Utilty.GenerateSlug(post.Subject1) + "\"})";
                ////postUrl = "@Url.Action(\"Post\",\"" + post.PostCategory.ToList().First().NameEn + "\", new {postID = " + post.PostID + "})";


                PostCategory = post.PostCategory.ToList().First().NameEn;
                PostID = post.PostID.ToString();
                Subject = post.Subject1;

                var postFormat = post.PostFormat.ToList().FirstOrDefault();
                if (postFormat != null)
                    formatPostName = postFormat.NameEn;

                categoryAboveClass += " " + item.AbobeClassName + " ";
                categoryAboveName += " " + item.NameFa + " ";
            }

            var docTemplates = new HtmlDocument();
            docTemplates.Load(path + "/Templates/bloglist-content.html", System.Text.Encoding.UTF8);
            var nodes = docTemplates.DocumentNode.SelectNodes("//div");

            var itemNode = nodes.FirstOrDefault(x => x.Attributes.FirstOrDefault().Value == "row bloglist-content");
            if (itemNode != null)
            {
                HtmlNode oldChild = itemNode.SelectSingleNode("/div[1]/article[1]");
                HtmlNode newChild = HtmlNode.CreateNode("<article class='col-md-4'>" +
                                                               "<div class='post-object' style=' background-image:url(" + post.Image1_1 + "); background-size:cover; background-repeat:no-repeat'>" +
                                                                   "<div class='overlay'></div>" +
                                                                   "<div class='post-content'>" +
                                                                       "<a href = '" + urlCategory + "' class='post-cat " + categoryAboveClass + "'>" + categoryAboveName + "</a>" +
                                                                       "<h3 class='title'>" +
                                                                           "<a href = '" + postUrl + "' > " + post.Subject1 + "</a>" +
                                                                       "</h3>" +
                                                                       "<a href = '" + urlCategory + "' class='readmore'>بيشتر</a>" +
                                                                   "</div>" +
                                                               "</div>" +
                                                           "</article>");

                return itemNode.ReplaceChild(newChild, oldChild);
            }

            return null;
        }


        internal HtmlNode CreateBloglistDefault(VM_Post post)
        {
            try
            {
                string categoryAboveClass = "", categoryAboveName = "";
                string headerResult = "", headerText1 = "", headerText2 = "";


                foreach (var item in post.PostCategory)
                {
                    //urlCategory = "@Url.Action(\"Index\",\"" + post.PostCategory.ToList().First().NameEn + "\")";
                    //postUrl = "@Url.Action(\"Post\",\"" + post.PostCategory.ToList().First().NameEn + "\", new {postID = \"" + post.PostID + "&" + Utilty.GenerateSlug(post.Subject1) + "\"})";
                    ////postUrl = "@Url.Action(\"Post\",\"" + post.PostCategory.ToList().First().NameEn + "\", new {postID = " + post.PostID + "})";


                    PostCategory = post.PostCategory.ToList().First().NameEn;
                    PostID = post.PostID.ToString();
                    Subject = post.Subject1;

                    var postFormat = post.PostFormat.ToList().FirstOrDefault();
                    if (postFormat != null)
                        formatPostName = postFormat.NameEn;


                    categoryAboveClass += " " + item.AbobeClassName + " ";
                    categoryAboveName += " " + item.NameFa + " ";
                }

                var docTemplates = new HtmlDocument();
                docTemplates.Load(path + "/Templates/bloglist-default.html", System.Text.Encoding.UTF8);
                var nodes = docTemplates.DocumentNode.SelectNodes("//div");

                var itemNode = nodes.FirstOrDefault(x => x.Attributes.FirstOrDefault().Value == "bloglist default");
                if (itemNode != null)
                {
                    headerText1 = post.SubSubject1_1.Substring(0, (post.SubSubject1_1.Length > 200 ? 200 : post.SubSubject1_1.Length));
                    headerText2 = post.ContentPost1_1.Substring(0, (post.ContentPost1_1.Length > 200 ? 200 : post.ContentPost1_1.Length));
                    headerResult = headerText1.Length > 0 ? headerText1 : headerText2;

                    HtmlNode oldChild = itemNode.SelectSingleNode("/div[1]/div[1]");
                    HtmlNode newChild = HtmlNode.CreateNode("<div class='bloglist-posts'>" +
                                                                "<article class='row'>" +
                                                                    "<div class='blogitem-media col-md-4'>" +
                                                                        "<a href = '" + postUrl + "' > " +
                                                                            "<img   width='290' height='170' src='" + post.Image1_1 + "' class='lazy' alt=''>" +
                                                                        "</a>" +
                                                                        "<a href = '" + urlCategory + "' class='post-cat " + categoryAboveClass + "'>" + categoryAboveName + "</a>" +
                                                                    "</div>" +
                                                                    "<div class='col-md-8'>" +
                                                                        "<div class='blogitem-content'>" +
                                                                            "<h4 class='blogitem-title'>" +
                                                                                "<a href = '" + postUrl + "' > " + post.Subject11 + "</a>" +
                                                                            "</h4>" +
                                                                            "<div class='blogitem-excerpt'>" + headerResult + "</div>" +
                                                                            "<div class='blogitem-meta'>" +
                                                                                "<span class='blogitem-author'>" +
                                                                                    "<i class='fa fa-user'></i> " + post.Fullname +
                                                                                "</span>" +
                                                                                "<span class='blogitem-comment'>" +
                                                                                    "<i class='fa fa-comment'></i> " + post.CommentCount +
                                                                                "</span>" +
                                                                                "<span class='blogitem-view'>" +
                                                                                    "<i class='fa fa-eye'></i>" + post.Views +
                                                                                "</span>" +
                                                                                "<span class='blogitem-date'>" +
                                                                                    "<i class='fa fa-calendar'></i>ا" + post.JalaliModifyDate +
                                                                                "</span>" +
                                                                            "</div>" +
                                                                        "</div>" +
                                                                    "</div>" +
                                                                "</article>" +
                                                                "<a href = '" + postUrl + "' class='readmore'>" +
                                                                    "<span>بیشتر بخوانيد</span>" +
                                                                "</a>" +
                                                            "</div>");

                    return itemNode.ReplaceChild(newChild, oldChild);
                }
            }
            catch (Exception e)
            {

            }

            return null;
        }



        internal HtmlNode CreateSliderImageBottom(VM_Post post)
        {
            string categoryAboveClass = "", categoryAboveName = "";


            foreach (var item in post.PostCategory)
            {
                //urlCategory = "@Url.Action(\"Index\",\"" + post.PostCategory.ToList().First().NameEn + "\")";
                //postUrl = "@Url.Action(\"Post\",\"" + post.PostCategory.ToList().First().NameEn + "\", new {postID = \"" + post.PostID + "&" + Utilty.GenerateSlug(post.Subject1) + "\"})";
                ////postUrl = "@Url.Action(\"Post\",\"" + post.PostCategory.ToList().First().NameEn + "\", new {postID = " + post.PostID + "})";


                PostCategory = post.PostCategory.ToList().First().NameEn;
                PostID = post.PostID.ToString();
                Subject = post.Subject1;

                categoryAboveClass += " " + item.AbobeClassName + " ";
                categoryAboveName += " " + item.NameFa + " ";
            }

            var docTemplates = new HtmlDocument();
            docTemplates.Load(path + "/Templates/postslider-container-slider-image-bottom.html", System.Text.Encoding.UTF8);
            var nodes = docTemplates.DocumentNode.SelectNodes("//div");

            var itemNode = nodes.FirstOrDefault(x => x.Attributes.FirstOrDefault().Value == "sp-slides sp-slider-image");
            if (itemNode != null)
            {
                HtmlNode oldChild = itemNode.SelectSingleNode("/div[1]/div[1]/div[1]/div[1]");
                HtmlNode newChild = HtmlNode.CreateNode("<div class='sp-slide'>" +
                                                            "<a href = '" + postUrl + "' > " +
                                                                "<img   width = '640' height = '426' src = '" + post.Image1_1 + "' class='sp-image lazy' alt='' srcset='' sizes='(max-width: 640px) 100vw, 640px'>" +
                                                            "</a>" +
                                                            "<a href = '" + urlCategory + "' class='post-cat " + categoryAboveClass + "'>" + categoryAboveName + "</a>" +
                                                            "<div class='sp-layer sp-black sp-padding' data-position='bottomLeft' data-vertical='0' data-width='100%' data-show-transition='up'>" +
                                                                "<h4>" +
                                                                    "<a href = '" + postUrl + "' >" + post.Subject1 + "</a>" +
                                                                "</h4>" +
                                                                "<a href = '" + postUrl + "' class='special-rm-arrow pull-right'>" +
                                                                    "<i class='fa fa-arrow-right'></i>" +
                                                                "</a>" +
                                                            "</div>" +
                                                        "</div>");

                return itemNode.ReplaceChild(newChild, oldChild);
            }

            return null;
        }



        internal HtmlNode CreateSliderImageBottom_ImageBottom(VM_Post post)
        {
            string categoryAboveClass = "", categoryAboveName = "";


            foreach (var item in post.PostCategory)
            {
                //urlCategory = "@Url.Action(\"Index\",\"" + post.PostCategory.ToList().First().NameEn + "\")";
                //postUrl = "@Url.Action(\"Post\",\"" + post.PostCategory.ToList().First().NameEn + "\", new {postID = \"" + post.PostID + "&" + Utilty.GenerateSlug(post.Subject1) + "\"})";


                PostCategory = post.PostCategory.ToList().First().NameEn;
                PostID = post.PostID.ToString();
                Subject = post.Subject1;

                categoryAboveClass += " " + item.AbobeClassName + " ";
                categoryAboveName += " " + item.NameFa + " ";
            }

            var docTemplates = new HtmlDocument();
            docTemplates.Load(path + "/Templates/postslider-container-slider-image-bottom.html", System.Text.Encoding.UTF8);
            var nodes = docTemplates.DocumentNode.SelectNodes("//div");

            var itemNode = nodes.FirstOrDefault(x => x.Attributes.FirstOrDefault().Value == "sp-thumbnails sp-slider-image");
            if (itemNode != null)
            {
                HtmlNode oldChild = itemNode.SelectSingleNode("/div[1]/div[1]/div[2]/img[1]");
                HtmlNode newChild = HtmlNode.CreateNode("<img   class='sp-thumbnail lazy' src='" + post.Image1_1 + "' width='70' height='70' alt=''>");

                return itemNode.ReplaceChild(newChild, oldChild);
            }

            return null;
        }

        internal HtmlNode CreatePostMostViewed(VM_Post post, int rowID)
        {
            string categoryAboveClass = "", categoryAboveName = "";


            foreach (var item in post.PostCategory)
            {
                //urlCategory = "@Url.Action(\"Index\",\"" + post.PostCategory.ToList().First().NameEn + "\")";
                //postUrl = "@Url.Action(\"Post\",\"" + post.PostCategory.ToList().First().NameEn + "\", new {postID = \"" + post.PostID + "&" + Utilty.GenerateSlug(post.Subject1) + "\"})";
                ////postUrl = "@Url.Action(\"Post\",\"" + post.PostCategory.ToList().First().NameEn + "\", new {postID = " + post.PostID + "})";


                PostCategory = post.PostCategory.ToList().First().NameEn;
                PostID = post.PostID.ToString();
                Subject = post.Subject1;

                var postFormat = post.PostFormat.ToList().FirstOrDefault();
                if (postFormat != null)
                    formatPostName = postFormat.NameEn;


                categoryAboveClass += " " + item.AbobeClassName + " ";
                categoryAboveName += " " + item.NameFa + " ";
            }

            var docTemplates = new HtmlDocument();
            docTemplates.Load(path + "/Templates/sidebar-widget.html", System.Text.Encoding.UTF8);
            var nodes = docTemplates.DocumentNode.SelectNodes("//ul");

            var itemNode = nodes.FirstOrDefault(x => x.Attributes.FirstOrDefault().Value == "recent_posts_wid right-slider1");
            if (itemNode != null)
            {
                HtmlNode oldChild = itemNode.SelectSingleNode("/aside[1]/div[1]/div[1]/div[1]/ul[1]/li[1]");
                HtmlNode newChild = HtmlNode.CreateNode("<li class='post-item'>" +
                                                            "<div class='media'>" +
                                                                "<a class='item-img media-left' href='" + postUrl + "'>" +
                                                                    rowID.ToString() +
                                                               "</a>" +
                                                                "<div class='media-body'>" +
                                                                    "<h4 class='media-heading item-title'>" +
                                                                        "<a href = '" + postUrl + "' > " + post.Subject1 + "</a>" +
                                                                    "</h4>" +
                                                                    "<ul class='item-meta'>" +
                                                                        "<li class='item-date'>" +
                                                                              "<i class='fa fa-calendar'></i>" + post.JalaliModifyDate + "" +
                                                                        "</li>" +
                                                                        "<li class='item-count'>" +
                                                                            "<i class='fa fa-eye'></i>" + post.Views +
                                                                       "</li>" +
                                                                    "</ul>" +
                                                                "</div>" +
                                                           "</div>" +
                                                       "</li>");

                return itemNode.ReplaceChild(newChild, oldChild);
            }

            return null;
        }

        internal HtmlNode CreatePostPopular(VM_Post post, int rowID)
        {
            string categoryAboveClass = "", categoryAboveName = "";


            foreach (var item in post.PostCategory)
            {
                //urlCategory = "@Url.Action(\"Index\",\"" + post.PostCategory.ToList().First().NameEn + "\")";
                //postUrl = "@Url.Action(\"Post\",\"" + post.PostCategory.ToList().First().NameEn + "\", new {postID = \"" + post.PostID + "&" + Utilty.GenerateSlug(post.Subject1) + "\"})";
                ////postUrl = "@Url.Action(\"Post\",\"" + post.PostCategory.ToList().First().NameEn + "\", new {postID = " + post.PostID + "})";


                PostCategory = post.PostCategory.ToList().First().NameEn;
                PostID = post.PostID.ToString();
                Subject = post.Subject1;

                var postFormat = post.PostFormat.ToList().FirstOrDefault();
                if (postFormat != null)
                    formatPostName = postFormat.NameEn;

                categoryAboveClass += " " + item.AbobeClassName + " ";
                categoryAboveName += " " + item.NameFa + " ";
            }

            var docTemplates = new HtmlDocument();
            docTemplates.Load(path + "/Templates/sidebar-widget.html", System.Text.Encoding.UTF8);
            var nodes = docTemplates.DocumentNode.SelectNodes("//ul");

            var itemNode = nodes.FirstOrDefault(x => x.Attributes.FirstOrDefault().Value == "recent_posts_wid right-slider2");
            if (itemNode != null)
            {
                HtmlNode oldChild = itemNode.SelectSingleNode("/aside[1]/div[1]/div[1]/div[2]/ul[1]/li[1]");
                HtmlNode newChild = HtmlNode.CreateNode("<li class='post-item'>" +
                                                            "<div class='media'>" +
                                                                "<a class='item-img media-left' href='" + postUrl + "'>" +
                                                                    rowID.ToString() +
                                                                "</a>" +
                                                                "<div class='media-body'>" +
                                                                    "<h4 class='media-heading item-title'>" +
                                                                        "<a href = '" + postUrl + "' >" + post.Subject1 + "</a>" +
                                                                    "</h4>" +
                                                                    "<ul class='item-meta'>" +
                                                                        "<li class='item-date'>" +
                                                                           "<i class='fa fa-calendar'></i>" + post.JalaliModifyDate + "" +
                                                                        "</li>" +
                                                                        "<li>" +
                                                                            "<a class='item-comment-count' href='" + postUrl + "'>" + post.CommentCount + "</a>" +
                                                                       "</li>" +
                                                                    "</ul>" +
                                                                "</div>" +
                                                            "</div>" +
                                                        "</li>");

                return itemNode.ReplaceChild(newChild, oldChild);
            }

            return null;
        }



        internal HtmlNode CreatePostSuperiorr(VM_Post post, int rowID)
        {
            string categoryAboveClass = "", categoryAboveName = "";


            foreach (var item in post.PostCategory)
            {
                //urlCategory = "@Url.Action(\"Index\",\"" + post.PostCategory.ToList().First().NameEn + "\")";
                ////postUrl = "@Url.Action(\"Post\",\"" + post.PostCategory.ToList().First().NameEn + "\", new {postID = " + post.PostID + "})";
                //postUrl = "@Url.Action(\"Post\",\"" + post.PostCategory.ToList().First().NameEn + "\", new {postID = \"" + post.PostID + "&" + Utilty.GenerateSlug(post.Subject1) + "\"})";


                PostCategory = post.PostCategory.ToList().First().NameEn;
                PostID = post.PostID.ToString();
                Subject = post.Subject1;

                var postFormat = post.PostFormat.ToList().FirstOrDefault();
                if (postFormat != null)
                    formatPostName = postFormat.NameEn;



                categoryAboveClass += " " + item.AbobeClassName + " ";
                categoryAboveName += " " + item.NameFa + " ";
            }

            var docTemplates = new HtmlDocument();
            docTemplates.Load(path + "/Templates/superior-posts.html", System.Text.Encoding.UTF8);
            var nodes = docTemplates.DocumentNode.SelectNodes("//ul");

            var itemNode = nodes.FirstOrDefault(x => x.Attributes.FirstOrDefault().Value == "superior-posts recent_posts_wid");
            if (itemNode != null)
            {
                HtmlNode oldChild = itemNode.SelectSingleNode("/aside[1]/ul[1]/li[1]");
                HtmlNode newChild = HtmlNode.CreateNode("<li class='post-item'>" +
                                                            "<div class='media'>" +
                                                                "<a class='item-img media-left' href='" + postUrl + "'>" +
                                                                    "<img   width = '70' height='70' src='" + post.Image1_1 + "' class='lazy' alt='' srcset='' sizes='(max-width: 70px) 100vw, 70px'>" +
                                                                "</a>" +
                                                                "<div class='media-body'>" +
                                                                    "<h4 class='media-heading item-title'>" +
                                                                        "<a href = '" + postUrl + "' > " + post.Subject1 + "</a>" +
                                                                    "</h4>" +
                                                                    "<ul class='item-meta'>" +
                                                                        "<li class='item-date'>" +
                                                                            "<i class='fa fa-calendar'></i>" + post.JalaliModifyDate + "" +
                                                                        "</li>" +
                                                                    "</ul>" +
                                                                "</div>" +
                                                            "</div>" +
                                                        "</li>");

                return itemNode.ReplaceChild(newChild, oldChild);
            }

            return null;
        }



        internal HtmlNode CreateSliderTop(VM_Post post)
        {
            string categoryAboveClass = "", categoryAboveName = "";


            foreach (var item in post.PostCategory)
            {
                //urlCategory = "@Url.Action(\"Index\",\"" + post.PostCategory.ToList().First().NameEn + "\")";
                ////postUrl = "@Url.Action(\"Post\",\"" + post.PostCategory.ToList().First().NameEn + "\", new {postID = " + post.PostID + "})";
                //postUrl = "@Url.Action(\"Post\",\"" + post.PostCategory.ToList().First().NameEn + "\", new {postID = \"" + post.PostID + "&" + Utilty.GenerateSlug(post.Subject1) + "\"})";


                PostCategory = post.PostCategory.ToList().First().NameEn;
                PostID = post.PostID.ToString();
                Subject = post.Subject1;

                urlCategory = "@Url.Action(\"Index\",\"" + post.PostCategory.ToList().First().NameEn + "\")";
                categoryAboveClass += " " + item.AbobeClassName + " ";
                categoryAboveName += " " + item.NameFa + " ";
            }

            var docTemplates = new HtmlDocument();
            docTemplates.Load(path + "/Templates/postslider-container-slider-top.html", System.Text.Encoding.UTF8);
            var nodes = docTemplates.DocumentNode.SelectNodes("//div");

            var itemNode = nodes.FirstOrDefault(x => x.Attributes.FirstOrDefault().Value == "sp-slides sp-slider-image-top");
            if (itemNode != null)
            {
                HtmlNode oldChild = itemNode.SelectSingleNode("/div[1]/div[1]/div[1]/div[1]");
                HtmlNode newChild = HtmlNode.CreateNode("<div class='sp-slide'>" +
                                                            "<a href = '" + postUrl + "' > " +
                                                                "<img   width = '640' height = '315' src = '" + post.Image1_1 + "' class='sp-image lazy' alt='" + post.Subject1 + "'>" +
                                                            "</a>" +
                                                            "<a href = '" + urlCategory + "' class='post-cat " + categoryAboveClass + "'>" + categoryAboveName + "</a>" +
                                                            "<div class='sp-layer sp-black sp-padding' data-position='bottomLeft' data-vertical='0' data-width='100%' data-show-transition='up'>" +
                                                                "<h4>" +
                                                                    "<a href = '" + postUrl + "' > " + post.Subject1 + "</a>" +
                                                                "</h4>" +
                                                                "<a href = '" + postUrl + "' class='special-rm-arrow pull-right'>" +
                                                                    "<i class='fa fa-arrow-right'></i>" +
                                                                "</a>" +
                                                            "</div>" +
                                                        "</div>");

                return itemNode.ReplaceChild(newChild, oldChild);
            }

            return null;
        }



        public HtmlNode CreateSliderTopThumbnails(VM_Post post)
        {
            string categoryAboveClass = "", categoryAboveName = "";


            foreach (var item in post.PostCategory)
            {
                //urlCategory = "@Url.Action(\"Index\",\"" + post.PostCategory.ToList().First().NameEn + "\")";
                ////postUrl = "@Url.Action(\"Post\",\"" + post.PostCategory.ToList().First().NameEn + "\", new {postID = " + post.PostID + "})";
                //postUrl = "@Url.Action(\"Post\",\"" + post.PostCategory.ToList().First().NameEn + "\", new {postID = \"" + post.PostID + "&" + Utilty.GenerateSlug(post.Subject1) + "\"})";


                PostCategory = post.PostCategory.ToList().First().NameEn;
                PostID = post.PostID.ToString();
                Subject = post.Subject1;

                categoryAboveClass += " " + item.AbobeClassName + " ";
                categoryAboveName += " " + item.NameFa + " ";
            }

            var docTemplates = new HtmlDocument();
            docTemplates.Load(path + "/Templates/postslider-container-slider-top.html", System.Text.Encoding.UTF8);
            var nodes = docTemplates.DocumentNode.SelectNodes("//div");

            var itemNode = nodes.FirstOrDefault(x => x.Attributes.FirstOrDefault().Value == "sp-thumbnails sp-slider-image-top");
            if (itemNode != null)
            {
                HtmlNode oldChild = itemNode.SelectSingleNode("/div[1]/div[1]/div[2]/div[1]");
                HtmlNode newChild = HtmlNode.CreateNode("<div class='sp-thumbnail'>" +
                                                            "<div class='sp-thumbnail-text'>" +
                                                                "<div class='sp-thumbnail-title'>" + post.Subject1 + "</div>" +
                                                            "</div>" +
                                                            "<div class='sp-thumbnail-image-container'>" +
                                                                "<img   width = '70' height='70' src='" + post.Image1_1 + "' class='sp-thumbnail-image lazy' alt='" + post.Subject1 + "' srcset='' sizes='(max-width: 70px) 100vw, 70px'>" +
                                                            "</div>" +
                                                        "</div>");

                return itemNode.ReplaceChild(newChild, oldChild);
            }

            return null;
        }

        public void RemoveRedundantTag(VM_Post res)
        {
            try
            {
                if (res == null || res.SourceSiteName == null || res.SourceSiteName == "")
                    return;

                PostManagement helperPostManagement = new PostManagement();
                var docTemplates = new HtmlDocument();
                HtmlNodeCollection nodes = null;

                docTemplates.LoadHtml(res.ContentHTML);

                //حذف همه لینک های داخلی سایت های دیگر
                var nodesAlla = docTemplates.DocumentNode.SelectNodes("//a");
                nodes = docTemplates.DocumentNode.SelectNodes("/");
                foreach (var item in nodesAlla)
                {
                    try
                    {
                        //لینک های دانلود از سایت آپارات حذف نشود
                        if (!item.InnerHtml.Contains("آپارات"))
                        {
                            HtmlNode newChild = HtmlNode.CreateNode("" + item.InnerHtml + "");
                            item.ParentNode.ReplaceChild(newChild, item);
                            //item.Remove();
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                //حذف تگ های نامناسب درهنگام نمایش پست
                switch (res.SourceSiteName.ToUpper())
                {
                    case "YJC":
                        res.ContentHTML = res.ContentHTML.Replace("انتهای پیام/", "");

                        docTemplates.LoadHtml(res.ContentHTML);
                        nodes = docTemplates.DocumentNode.SelectNodes("//div");

                        helperPostManagement.ClearContentNode(nodes, "row flowplayer-main-container");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                        break;
                    case "GADGETNEWS":
                        docTemplates.LoadHtml(res.ContentHTML);
                        nodes = docTemplates.DocumentNode.SelectNodes("/");

                        helperPostManagement.ClearContentNode(nodes, "su-button-center");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                        helperPostManagement.ClearContentNode(nodes, "su-button-center");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                        break;

                    case "ZOOMG":
                        docTemplates.LoadHtml(res.ContentHTML);

                        nodes = docTemplates.DocumentNode.SelectNodes("//div");

                        helperPostManagement.ClearContentNode(nodes, "relatedapepnt");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                        helperPostManagement.ClearContentNode(nodes, "article-source-row");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                        helperPostManagement.ClearContentNode(nodes, "article-tag-row");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                        helperPostManagement.ClearContentNode(nodes, "article-header item-list-text");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                        helperPostManagement.ClearContentNode(nodes, "banner");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                        helperPostManagement.ClearContentNode(nodes, "larticle");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                        break;

                    case "KARNAVAL":
                        docTemplates.LoadHtml(res.ContentHTML);
                        nodes = docTemplates.DocumentNode.SelectNodes("//div");

                        helperPostManagement.ClearContentNode(nodes, "post-info-container");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                        helperPostManagement.ClearContentNode(nodes, "alert alert-info");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                        break;

                    case "EURONEWS":
                        res.ContentHTML = res.ContentHTML.Replace("اندازه متن", "");

                        docTemplates.LoadHtml(res.ContentHTML);
                        nodes = docTemplates.DocumentNode.SelectNodes("//div");

                        helperPostManagement.ClearContentNode(nodes, "c-font-size-switcher medium-order-4 js-font-size-switch u-float-end u-padding-top-0 t-font-size-switcher--blue");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                        break;

                    case "ILIADMAG":

                        docTemplates.LoadHtml(res.ContentHTML);

                        //------------------------------------------------------------
                        nodes = docTemplates.DocumentNode.SelectNodes("//h1");

                        helperPostManagement.ClearContentNode(nodes, "magazine_article_title all_magazine_theme_color4");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;


                        //------------------------------------------------------------------------
                        nodes = docTemplates.DocumentNode.SelectNodes("//div");

                        helperPostManagement.ClearContentNode(nodes, "magazine_article_info");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                        helperPostManagement.ClearContentNode(nodes, "magazine_article_info2");
                        if (nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                        helperPostManagement.ClearContentNode(nodes, "magazine_article_title all_magazine_theme_color4");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                        helperPostManagement.ClearContentNode(nodes, "mag_instagram_msg");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                        helperPostManagement.ClearContentNode(nodes, "mag_telegram_msg");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                        break;

                    case "TASNIMNEWS":
                        res.ContentHTML = res.ContentHTML.Replace("انتهای پیام/*", "");
                        res.ContentHTML = res.ContentHTML.Replace("انتهای پیام/", "");

                        docTemplates.LoadHtml(res.ContentHTML);
                        var node = docTemplates.DocumentNode.SelectNodes("//blockquote").FirstOrDefault();
                        if (node != null)
                        {
                            var nodesAll = docTemplates.DocumentNode.SelectNodes("/");

                            var resNodesAll = RemoveSpecificNodeInAllChild(nodesAll, node);
                            if (resNodesAll != null && resNodesAll.FirstOrDefault() != null)
                                res.ContentHTML = resNodesAll.FirstOrDefault().OuterHtml;
                        }

                        break;

                    case "SETARE":
                        res.ContentHTML = res.ContentHTML.Replace("و همراهان ستاره", "");
                        res.ContentHTML = res.ContentHTML.Replace("بیشتر بخوانید:", "");
                        res.ContentHTML = res.ContentHTML.Replace("بیشتر بدانید", "");


                        docTemplates.LoadHtml(res.ContentHTML);


                        nodes = docTemplates.DocumentNode.SelectNodes("//h1");
                        helperPostManagement.ClearContentNode(nodes, "title htags");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;


                        //nodesAlla = docTemplates.DocumentNode.SelectNodes("//a[contains(@class, 'news_links')]");
                        //foreach (var item in nodesAlla)
                        //{
                        //    item.Remove();
                        //}

                        nodes = docTemplates.DocumentNode.SelectNodes("//div");

                        helperPostManagement.ClearContentNode(nodes, "news_toolbar row");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                        helperPostManagement.ClearContentNode(nodes, "title htags");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                        helperPostManagement.ClearContentNode(nodes, "social_meida_icons");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                        break;

                    case "AJIBTARIN":
             
                        nodesAlla = docTemplates.DocumentNode.SelectNodes("//img");
                        nodes = docTemplates.DocumentNode.SelectNodes("//div");
                        foreach (var item in nodesAlla)
                        {
                            var attrLazySrc = item.Attributes.FirstOrDefault(p => p.Name == "data-lazy-src");
                            if (attrLazySrc != null)
                            {
                                var attrSrc = item.Attributes.FirstOrDefault(p => p.Name == "src");
                                if (attrSrc != null)
                                    item.Attributes.FirstOrDefault(p => p.Name == "src").Value = attrLazySrc.Value;
                            }
                        }
                        res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                        helperPostManagement.ClearContentNode(nodes, "entry-header");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                        helperPostManagement.ClearContentNode(nodes, "gridlove-ad");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                        break;

                    case "CINEMASCHOOLS":
                        res.ContentHTML = res.ContentHTML.Replace("انتهای پیام/", "");

                        docTemplates.LoadHtml(res.ContentHTML);
                        nodes = docTemplates.DocumentNode.SelectNodes("//div");

                        helperPostManagement.ClearContentNode(nodes, "alert alert-info msg-help col-xs-12");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                        helperPostManagement.ClearContentNode(nodes, "col-xs-12 p-tags");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                        break;

                    case "IRNA":
                        res.ContentHTML = res.ContentHTML.Replace("انتهای پیام /*", "");

                        docTemplates.LoadHtml(res.ContentHTML);
                        nodes = docTemplates.DocumentNode.SelectNodes("//div");

                        helperPostManagement.ClearContentNode(nodes, "container-fluid SocialIconNews");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                        helperPostManagement.ClearContentNode(nodes, "Tags");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;                        

                        break;

                    case "TARIKHEMA":
                        docTemplates.LoadHtml(res.ContentHTML);
                        nodes = docTemplates.DocumentNode.SelectNodes("//div");

                        helperPostManagement.ClearContentNode(nodes, "wp-video");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                        break;

                    case "ROOZIATO":
                        docTemplates.LoadHtml(res.ContentHTML);
                        nodes = docTemplates.DocumentNode.SelectNodes("//div");

                        helperPostManagement.ClearContentNode(nodes, "mobile-ads visible-xs");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                        helperPostManagement.ClearContentNode(nodes, "insert-code");
                        if (nodes != null && nodes.FirstOrDefault() != null)
                            res.ContentHTML = nodes.FirstOrDefault().OuterHtml;

                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

            }

        }

        /// <summary>
        /// پارامتر نود را در پارامتر نودآل و تمام فرزندان نودآل جستجو می کند و سپس حذف می کند
        /// </summary>
        /// <param name="nodesAll"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public HtmlNodeCollection RemoveSpecificNodeInAllChild(HtmlNodeCollection nodesAll, HtmlNode node)
        {
            if (nodesAll == null || node == null)
                return null;

            foreach (var item in nodesAll)
            {
                if (item == node)
                {
                    nodesAll.Remove(node);
                    break;
                }

                RemoveSpecificNodeInAllChild(item.ChildNodes, node);
            }

            return nodesAll;
        }

        private string _Domain = "http://redfun.ir";
        public string Domain
        {
            get
            {
                return _Domain;
            }
            set
            {
                if (value.Contains("localhost"))
                    _Domain = "http://redfun.ir";
                else
                    _Domain = value;
            }
        }


        private string _urlCategory = "";
        public string urlCategory
        {
            get
            {
                return Domain + @"/" + PostCategory; ;
            }
            set
            {
                _urlCategory = value;
            }
        }

        private string _postUrl = "";
        public string postUrl
        {
            get
            {
                if (formatPostName == "audio")
                    return Domain + @"/" + PostCategory + "/AudioPost/?postid=" + PostID + "&" + Utilty.GenerateSlug(Subject);
                else if (formatPostName == "video")
                    return Domain + @"/" + PostCategory + "/VideoPost/?postid=" + PostID + "&" + Utilty.GenerateSlug(Subject);

                return Domain + @"/" + PostCategory + "/post/?postid=" + PostID + "&" + Utilty.GenerateSlug(Subject);
            }
            set
            {
                _postUrl = value;
            }
        }

        private string _PostCategory = "";
        public string PostCategory
        {
            get
            {
                return _PostCategory;
            }
            set
            {
                _PostCategory = value;
            }
        }

        private string _PostID = "";
        public string PostID
        {
            get
            {
                return _PostID;
            }
            set
            {
                _PostID = value;
            }
        }

        private string _Subject = "";
        public string Subject
        {
            get
            {
                return _Subject;
            }
            set
            {
                _Subject = value;
            }
        }

        public string formatPostName { get; private set; }
    }
}
