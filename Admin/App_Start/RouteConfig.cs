using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Admin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Admin", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Category",
                url: "{controller}/{action}/{categoryId}/{currentPageIndex}",
                defaults: new { controller = "Category", action = "Index", categoryId = UrlParameter.Optional, currentPageIndex = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Product",
                url: "{controller}/{action}/{ProductId}",
                defaults: new { controller = "Product", action = "Index", productId = UrlParameter.Optional}
            );

            routes.MapRoute(
                name: "CategoryParameter",
                url: "{controller}/{action}/{categoryId}",
                defaults: new { controller = "CategoryParameter", action = "Index", categorytId = UrlParameter.Optional }
            );

        }
    }
}
