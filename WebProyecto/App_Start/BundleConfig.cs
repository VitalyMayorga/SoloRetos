using System.Web;
using System.Web.Optimization;

namespace WebProyecto
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
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/respond.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include("~/Scripts/jquery-ui-1.11.4.js"));

            bundles.Add(new StyleBundle("~/Content/jqueryui").Include("~/Content/themes/base/all.css"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/site.css",
                      "~/Content/social-buttons.css",
                      "~/Content/main.css",
                      "~/Content/styles.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
           "~/content/site.css",
            "~/content/themes/base/accordion.css",
            "~/content/themes/base/all.css",
            "~/content/themes/base/autocomplete.css",
            "~/content/themes/base/base.css",
            "~/content/themes/base/button.css",
            "~/content/themes/base/core.css",
            "~/content/themes/base/datepicker.css",
            "~/content/themes/base/dialog.css",
            "~/content/themes/base/draggable.css",
            "~/content/themes/base/menu.css",
            "~/content/themes/base/progressbar.css",
            "~/content/themes/base/resizable.css",
            "~/content/themes/base/selectmenu.css",
            "~/content/themes/base/slider.css",
            "~/content/themes/base/sortable.css",
            "~/content/themes/base/spinner.css",
            "~/content/themes/base/tabs.css",
            "~/content/themes/base/theme.css",
            "~/content/themes/base/tooltip.css"));


        }
    }
}
