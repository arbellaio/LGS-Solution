using System.Web;
using System.Web.Optimization;

namespace LGS
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/admin-lte/js").Include(
                                  "~/admin-lte/js/app.js",
                                  "~/admin-lte/js/adminlte.js",
                                  "~/admin-lte/ckeditor/ckeditor.js",
                                  "~/admin-lte/ckeditor/styles.js",
                                  "~/admin-lte/ckeditor/config.js",
                                  "~/admin-lte/js/chart.js",
                                  "~/admin-lte/js/jquery.min.js",
                                  "~/admin-lte/js/fastclick.js",
                                  "~/Scripts/jquery-ui-1.10.1.custom.min.js",
                                  "~/Scripts/respond.js",
                                 "~/admin-lte/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/cardview.css",
                      "~/Content/iFrameLayout.css",
                      "~/admin-lte/css/AdminLTE.css",
                      "~/admin-lte/css/font-awesome.css",
                      "~/admin-lte/css/ionicons.css",
                      "~/admin-lte/ckeditor/skins/moono-lisa/editor.css",
                      "~/admin-lte/ckeditor/skins/moono-lisa/dialog.css",
                      "~/admin-lte/css/skins/skin-purple-light.css",
                      "~/admin-lte/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.min.css"
                    ));
        }
    }
}
