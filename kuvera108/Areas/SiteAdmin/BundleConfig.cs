using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace kuvera108.Areas
{
    internal static class BundleConfig
    {
        internal static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/leftMenu").Include(
                      "~/Areas/SiteAdmin/Scripts/leftMenu.js"));

            bundles.Add(new StyleBundle("~/Content/leftMenu").Include(
                      "~/Areas/SiteAdmin/Content/simple-sidebar.css"));

        }
    }
}