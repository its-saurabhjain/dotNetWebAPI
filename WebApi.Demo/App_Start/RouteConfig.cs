﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApi.Demo
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

            routes.MapRoute(
                name: "Private",
                url: "private",
                defaults: new { controller = "Home", action = "Private", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "LogOut",
                url: "logout",
                defaults: new { controller = "Home", action = "LogOut", id = UrlParameter.Optional }
            );
        }
    }
}
