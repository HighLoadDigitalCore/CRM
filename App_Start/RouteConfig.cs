using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Trading
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
                name: "DefaultOrder",
                url: "{controller}/{action}/ShopID/{ShopID}",
                defaults: new { controller = "Home", action = "Index", ShopID = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "DefaultOrderSearch",
                url: "{controller}/{action}/{Page}/ShopID/{ShopID}",
                defaults: new { controller = "Common", action = "OrderSearch", Page = "Common", ShopID = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "DefaultProductSearch",
                url: "{controller}/{action}/{Page}/ShopID/{ShopID}/StoreID/{StoreID}",
                defaults: new { controller = "Common", action = "ProductSearch", Page = "Common", ShopID = UrlParameter.Optional, StoreID = UrlParameter.Optional }
            );
        }
    }
}