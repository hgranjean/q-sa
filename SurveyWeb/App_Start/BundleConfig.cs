using System.Web;
using System.Web.Optimization;

namespace SurveyWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Fix for bundles not rendered correctly in the release version (V= bug)
            bundles.IgnoreList.Clear();
            BundleTable.EnableOptimizations = false;

            // AQS scripts and styles

            bundles.Add(new ScriptBundle("~/bundles/aqs").Include(
                        "~/Scripts/AQS/aqs.js",
                        "~/Content/themes/arjuna/js/jquery-1.11.1.min.js",
                        // "~/Scripts/jquery-1.11.0.js", // "~/Scripts/jquery-{version}.js"
                        "~/Scripts/jquery-ui-1.11.0.js" 
                        ));

            // AQS theme enhancements

            bundles.Add(new StyleBundle("~/Content/css").Include(                      
                      "~/Content/site.css",
                      "~/Content/component.css",
                      "~/Content/Demo.css",
                      "~/Content/Normalize.css",
                      "~/Content/jquery-ui.css"));            

            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            // JQuery support for validation and unobtrusive ajax calls

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryobtr").Include(
                        "~/Scripts/jquery.unobtrusive-ajax*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            // Bootstrap

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            // Base theme

            bundles.Add(new StyleBundle("~/Content/basetheme").Include(
                      "~/Content/bootstrap.css"));            

            // Arjuna theme

            bundles.Add(new StyleBundle("~/Content/arjunatheme").Include(
                      "~/Content/themes/arjuna/css/bootstrap.css", // bootstrap v3.2.0
                      "~/Content/themes/arjuna/css/font-awesome.min.css", // font awesome v4.1.0
                      "~/Content/themes/arjuna/css/bootstrap-reset.css", // bootstrap reset
                      "~/Content/themes/arjuna/css/arjuna.css", // arjuna
                      "~/Content/themes/arjuna/plugins/owl.carousel/owl-carousel/owl.carousel.css", // plugins for carousel, dropdowns
                      "~/Content/themes/arjuna/plugins/owl.carousel/owl-carousel/owl.theme.css",
                      "~/Content/themes/arjuna/plugins/owl.carousel/owl-carousel/owl.transitions.css"
                      ));            

            // Arjuna scripts

            bundles.Add(new ScriptBundle("~/bundles/arjuna").Include(
                      "~/Content/themes/arjuna/js/arjuna.js" // arjuna
                      ));
                        
            // RGraph component
                                          
            bundles.Add(new ScriptBundle("~/bundles/rgraph").Include(
                    "~/Scripts/rgraph/libraries/RGraph.common.core.js",
                    "~/Scripts/rgraph/libraries/RGraph.common.dynamic.js",
                    "~/Scripts/rgraph/libraries/RGraph.common.tooltips.js",
                    "~/Scripts/rgraph/libraries/RGraph.drawing.marker1.js",
                    "~/Scripts/rgraph/libraries/RGraph.bar.js",
                    "~/Scripts/rgraph/libraries/RGraph.gantt.js"));

            // DropIt component for drop-down action menus

            bundles.Add(new StyleBundle("~/Content/dropdown").Include(
                      "~/Content/dropit.css"));

            bundles.Add(new ScriptBundle("~/bundles/dropdown").Include(
                      "~/Scripts/dropit.js"));
        }
    }
}
