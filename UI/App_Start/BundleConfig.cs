using System.Web;
using System.Web.Optimization;

namespace UI
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-min.js",
                        "~/Scripts/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/fontawesome").Include(
                      "~/Content/font-awesome.min.css"));

            bundles.Add(new StyleBundle("~/Content/ionicons").Include(
                      "~/Content/ionicons.min.css"));

            bundles.Add(new StyleBundle("~/Content/adminLTE").Include(
                      "~/Content/AdminLTE.min.css"));

            bundles.Add(new StyleBundle("~/Content/skins").Include(
                     "~/Content/skins/_all-skins.css"));

            bundles.Add(new StyleBundle("~/Content/datepicker").Include(
                     "~/Content/datepicker/bootstrap-datepicker.min.css",
                     "~/Content/datepicker/daterangepicker.css"));

            bundles.Add(new StyleBundle("~/Content/morris").Include(
                    "~/Content/morris.css"));

            bundles.Add(new StyleBundle("~/Content/jvectormap").Include(
                    "~/Content/jquery-jvectormap.css"));

            bundles.Add(new StyleBundle("~/Content/logincss").Include(
                    "~/Content/LoginStyle.css"));
        }
    }
}
