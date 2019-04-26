using GossipDashboard.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GossipDashboard
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        //public static void RegisterRoutes(RouteCollection routes)
        //{
        //    routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

        //    routes.Add("ProductDetails", new SeoFriendlyRoute("quiz/Post/{id}",
        //        new RouteValueDictionary(new { controller = "quiz", action = "Post" }),
        //        new MvcRouteHandler()));

        //    routes.MapRoute(
        //        "Default",
        //        "{controller}/{action}/{id}",
        //        new { controller = "Home", action = "Index", id = UrlParameter.Optional }
        //        );
        //}
    }
}
