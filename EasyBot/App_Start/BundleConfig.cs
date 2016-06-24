using System.Web;
using System.Web.Optimization;

namespace EasyBot
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

            // jquery-ui
            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                      "~/Scripts/jquery-ui.min.js"));

            // knockout
            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                      "~/Scripts/knockout-3.4.0.js"));

            // common
            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                      "~/Scripts/main.js"));

            // SignalR
            bundles.Add(new ScriptBundle("~/bundles/signalr").Include(
                      "~/Scripts/jquery.signalR-2.2.0.min.js",
                      "~/Scripts/streaming/signalR.js"));

            // zingchart
            bundles.Add(new ScriptBundle("~/bundles/zingchart").Include(
                      "~/Scripts/vendors/zingchart/zingchart.min.js",
                      "~/Scripts/chart/chart.js"));


            // jqgrid
            bundles.Add(new ScriptBundle("~/bundles/jqgrid").Include(
                      "~/Scripts/jquery.jqGrid.min.js",
                      "~/Scripts/locale/grid.locale-en.js"));

            // css
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                      "~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/custom").Include(
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/chart").Include(
                      "~/Content/chart.css"));

            bundles.Add(new StyleBundle("~/Content/jquery-ui").Include(
                      "~/Content/jquery-ui.css"));

            bundles.Add(new StyleBundle("~/Content/jqgrid").Include(
                      "~/Content/jquery-ui.css",
                      "~/Content/ui.jqgrid.css"));
        }
    }
}
