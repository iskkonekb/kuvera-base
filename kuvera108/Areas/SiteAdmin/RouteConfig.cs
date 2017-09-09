using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace kuvera108.Areas.SiteAdmin
{
    internal static class RouteConfig
    {
        internal static void RegisterRoutes(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SiteAdmin_default",
                "SiteAdmin/{controller}/{action}/{id}",
                new { controller = "Def", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "kuvera108.Areas.SiteAdmin.Controllers" }
            );
        }
    }
}