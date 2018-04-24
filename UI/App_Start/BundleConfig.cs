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

            bundles.Add(new ScriptBundle("~/bundles/jquerySparkline").Include(
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

            bundles.Add(new ScriptBundle("~/bundles/jquerySlimscroll").Include(
                 "~/Scripts/jquery.slimscroll/jquery.slimscroll.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/fastclick").Include(
                 "~/Scripts/fastclick.js"));

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

            bundles.Add(new ScriptBundle("~/bundles/chartJS").Include(
                       "~/Scripts/Chart.js"));

            bundles.Add(new ScriptBundle("~/bundles/toastr").Include(
                      "~/Scripts/toastr.js"));

            bundles.Add(new ScriptBundle("~/bundles/dashboard2").Include(
                      "~/Scripts/dashboard2.js"));

            bundles.Add(new ScriptBundle("~/bundles/iCheck").Include(
                      "~/Scripts/icheck.min.js"));

            //***************Start of CSS Bundles*************************
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
                     "~/Content/skins/_all-skins.css",
                     "~/Content/skins/skin-black-light.min.css",
                     "~/Content/skins/skin-black.min.css",
                     "~/Content/skins/skin-blue-light.min.css",
                     "~/Content/skins/skin-blue.min.css",
                     "~/Content/skins/skin-green-light.min.css",
                     "~/Content/skins/skin-green.min.css",
                     "~/Content/skins/skin-purple-light.min.css",
                     "~/Content/skins/skin-purple.min.css",
                     "~/Content/skins/skin-red-light.min.css",
                     "~/Content/skins/skin-red.min.css",
                     "~/Content/skins/skin-yellow-light.min.css",
                     "~/Content/skins/skin-yellow.min.css"));

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

            bundles.Add(new StyleBundle("~/Content/placeholderIcons").Include(
                    "~/Content/placeholder-icons.css"));

            bundles.Add(new StyleBundle("~/Content/dropzone").Include(
                    "~/Content/dropzone.css"));

            bundles.Add(new StyleBundle("~/Content/pagedList").Include(
                    "~/Content/PagedList.css"));

            bundles.Add(new StyleBundle("~/Content/clearSearch").Include(
                    "~/Content/clearTextbox.css"));

            bundles.Add(new StyleBundle("~/Content/toastr").Include(
                    "~/Content/toastr.css"));

            bundles.Add(new StyleBundle("~/Content/KnowledgeBase").Include(
                  "~/Content/KnowledgeBaseStyle.css"));

            bundles.Add(new StyleBundle("~/Content/iCheck").Include(
                  "~/Content/iCheck/all.css",
                  "~/Content/iCheck/flat/_all.css"));
        }
    }
}
