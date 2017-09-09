using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using kuvera108.Areas.SiteAdmin.Models;
using kuvera108.Data;
using System.Data.Entity;

namespace kuvera108
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            //AppDbInitializer dba = new AppDbInitializer();
            //Database.SetInitializer<ApplicationDbContext>(dba);
            //dba.InitializeDatabase(new ApplicationDbContext());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
