using System.Web.Mvc;
using System.Web.Optimization;

namespace kuvera108.Areas.SiteAdmin
{
    public class SiteAdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SiteAdmin";
            }
        }
        public override void RegisterArea(AreaRegistrationContext context) 
        {
            RegisterRoutes(context);
            RegisterBundles();
        }
        private void RegisterRoutes(AreaRegistrationContext context)
        {
            RouteConfig.RegisterRoutes(context);
        }
        private void RegisterBundles()
        {
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}