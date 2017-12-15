using System.Web;
using System.Web.Optimization;

namespace UI
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery/dist/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                        "~/Scripts/jquery-ui/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/morris").Include(
                       "~/Scripts/morris.js/morris.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/raphael").Include(
                       "~/Scripts/raphael/raphael.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery.sparkline").Include(
                      "~/Scripts/jquery-sparkline/dist/jquery.sparkline.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jvectormap").Include(
                     "~/Scripts/jvectormap/jquery-jvectormap-1.2.2.min.js",
                     "~/Scripts/jvectormap/jquery-jvectormap-world-mill-en.js"));

            bundles.Add(new ScriptBundle("~/bundles/knob").Include(
                     "~/Scripts/jquery-knob/dist/jquery.knob.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                    "~/Scripts/moment/min/moment.min.js",
                    "~/Scripts/daterangepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/datepicker").Include(
                   "~/Scripts/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery.slimscroll").Include(
                 "~/Scripts/jquery.slimscroll/jquery.slimscroll.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/fastclick").Include(
                 "~/Scripts/fastclick.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-slimscroll").Include(
                "~/Scripts/jquery-slimscroll/jquery.slimscroll.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/wysihtml5").Include(
                 "~/Scripts/bootstrap3-wysihtml5.all.js",
                 "~/Scripts/bootstrap3-wysihtml5.all.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/adminLTE").Include(
                     "~/Scripts/adminlte.min.js",
                     "~/Scripts/dashboard.js",
                     "~/Scripts/demo.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                 "~/Scripts/jquery.dataTables.min.js",
                 "~/Scripts/dataTables.bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/dropdowns.less",
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

            bundles.Add(new StyleBundle("~/Content/wysihtml5").Include(
                    "~/Content/bootstrap-wysihtml5/bootstrap3-wysihtml5.min.css"));

            bundles.Add(new StyleBundle("~/Content/logincss").Include(
                    "~/Content/LoginStyle.css"));

            bundles.Add(new StyleBundle("~/Content/datatables.net-bs").Include(
                    "~/Content/datatables.net-bs/dataTables.bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/placeholder-icons").Include(
                    "~/Content/placeholder-icons.css"));
        }
    }
}
