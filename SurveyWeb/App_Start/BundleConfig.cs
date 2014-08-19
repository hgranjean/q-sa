using System.Web;
using System.Web.Optimization;

namespace SurveyWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Fix for bundles not rendered correctly in the release version
            bundles.IgnoreList.Clear();

            bundles.Add(new ScriptBundle("~/bundles/aqs").Include(
                        "~/Scripts/AQS/aqs.js",
                        "~/Scripts/jquery-1.11.0.js", // "~/Scripts/jquery-{version}.js"
                        "~/Scripts/jquery-ui-1.11.0.js" 
                        ));

            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryobtr").Include(
                        "~/Scripts/jquery.unobtrusive-ajax*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/component.css",
                      "~/Content/Demo.css",
                      "~/Content/Normalize.css",
                      "~/Content/jquery-ui.css"));            
                      
            bundles.Add(new ScriptBundle("~/bundles/rgraph").Include(
                    "~/Scripts/rgraph/libraries/RGraph.common.core.js",
                    "~/Scripts/rgraph/libraries/RGraph.common.dynamic.js",
                    "~/Scripts/rgraph/libraries/RGraph.common.tooltips.js",
                    "~/Scripts/rgraph/libraries/RGraph.drawing.marker1.js",
                    "~/Scripts/rgraph/libraries/RGraph.bar.js",
                    "~/Scripts/rgraph/libraries/RGraph.gantt.js"));

            bundles.Add(new StyleBundle("~/Content/dropdown").Include(
                      "~/Content/dropit.css"));

            bundles.Add(new ScriptBundle("~/bundles/dropdown").Include(
                      "~/Scripts/dropit.js"));
        }
    }
}
