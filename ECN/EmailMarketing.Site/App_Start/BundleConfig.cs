using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace EmailMarketing.Site
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // uncomment this to test with minification enabled
            //BundleTable.EnableOptimizations = true;

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Content/js/jquery/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include("~/Content/jquery-ui/jquery-ui.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Content/js/jquery/jquery.validate.js",
                                                                        "~/Content/js/jquery/jquery.validate.unobtrusive.js",
                                                                        "~/Content/js/jquery/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/js/jquery/bootstrap.js",
                      "~/Content/js/jquery/respond.js"));

            bundles.Add(new ScriptBundle("~/scripts/main").Include(
                "~/Content/js/jquery/jquery.ui.potato.menu.js",
                "~/Content/js/highslide/highslide-full.js",
                "~/Content/js/ddmenu.js",
                "~/Content/js/toastr.js",
                "~/Content/js/chardinjs.js",
                "~/Content/js/wz_dragdrop.js",
                "~/Content/js/show-instructions.js",
                "~/Content/js/mm-functions.js",
                "~/Content/js/base-channel-layout.js",
                "~/Content/js/mainmenu.js"));

            bundles.Add(new StyleBundle("~/styles").Include(
                "~/Content/css/Site.css",
                "~/Content/css/ECN_Controls.css",
                "~/Content/css/ddmenu.css",
                "~/Content/css/images-stylesheet.css",
                "~/App_Themes/stylesheet.css",
                "~/App_Themes/1/ECN.css",
                "~/Content/jquery-ui/jquery.ui.potato.menu.css",
                "~/Content/css/jquery-ui/jquery-ui.css",
                "~/Content/js/highslide/highslide.css",
                "~/Content/css/toastr.css",
                "~/Content/css/chardinjs.css",
                "~/Content/css/KMPlatform.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}