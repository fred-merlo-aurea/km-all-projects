using System.Web;
using System.Web.Optimization;

namespace UAD.DataCompare.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
               "~/Content/kendo/2015.1.429/kendo.common.min.css",
               "~/Content/kendo/2015.1.429/kendo.default.min.css",
               "~/fonts/frontello/css/kmform.css",
               "~/Content/kendo/2015.1.429/kendo.km.min.css",
                     "~/Content/bootstrap.css",
                     "~/Content/site.css",
                     "~/Content/kendo/2015.1.429/kendo.common.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/Kendo").Include(
                     "~/Scripts/kendo/2015.1.429/jquery.min.js",
                     "~/Scripts/kendo/2015.1.429/kendo.all.min.js",
                     "~/Scripts/kendo/2015.1.429/kendo.aspnetmvc.min.js",
                     "~/Scripts/kendo/2015.1.429/angular.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/JQueryOther").Include(
                     "~/Scripts/jquery.unobtrusive-ajax.min.js",
                     "~/Scripts/clipboard.min.js",
                     "~/Scripts/extensions.js",
                     "~/Scripts/main.js",
                     "~/Scripts/editor.js",
                     "~/Scripts/jquery.blockUI.js",
                     "~/Infrastructure/ActionMenu.js"));
           
        }
    }
}
