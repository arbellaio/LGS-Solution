using System.Collections.Generic;
using System.Web;
using System.Web.Optimization;

namespace LGS
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            string token = "?alt=media&token=2d00aa06-a757-4ba5-88f5-7c69909dc4b0";
            string path = "https://firebasestorage.googleapis.com/v0/b/testcdn-fd624.appspot.com/o/cdn%2F";
//            bundles.UseCdn = true;
//            BundleTable.EnableOptimizations = true;
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/cdnjs/", path + "adminlte.js" + token));
            bundles.Add(new ScriptBundle("~/cdnjs/", path + "ckeditor.js" + token));
            bundles.Add(new ScriptBundle("~/cdnjs/", path + "styles.js" + token));
            bundles.Add(new ScriptBundle("~/cdnjs/", path + "config.js" + token));
            bundles.Add(new ScriptBundle("~/cdnjs/", path + "icheck.min.js" + token));
            bundles.Add(new ScriptBundle("~/cdnjs/", path + "chart.js" + token));
            bundles.Add(new ScriptBundle("~/cdnjs/", path + "jquery.min.js" + token));
            bundles.Add(new ScriptBundle("~/cdnjs/", path + "fastclick.js" + token));
            bundles.Add(new ScriptBundle("~/cdnjs/", path + "jquery-ui-1.10.1.custom.min.js" + token));
            bundles.Add(new ScriptBundle("~/cdnjs/", path + "respond.js" + token));
            bundles.Add(new ScriptBundle("~/cdnjs/", path + "moment.min.js" + token));
            bundles.Add(new ScriptBundle("~/cdnjs/", path + "datetimeFormat.js" + token));
            bundles.Add(new ScriptBundle("~/cdnjs/", path + "bootstrap3-wysihtml5.all.min.js" + token));
            bundles.Add(new ScriptBundle("~/cdnjs/", path + "bootstrap.js" + token));


            bundles.Add(new ScriptBundle("~/admin-lte/js").Include(
                                  "~/admin-lte/js/adminlte.js",
                                  "~/admin-lte/ckeditor/ckeditor.js",
                                  "~/admin-lte/ckeditor/styles.js",
                                  "~/admin-lte/ckeditor/config.js",
                                  "~/admin-lte/plugins/iCheck/icheck.min.js",
                                  "~/admin-lte/js/chart.js",
                                  "~/admin-lte/js/jquery.min.js",
                                  "~/admin-lte/js/fastclick.js",
                                  "~/Scripts/jquery-ui-1.10.1.custom.min.js",
                                  "~/Scripts/respond.js",
                                  "~/Scripts/moment.min.js",
                                  "~/Scripts/datetimeFormat.js",
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

            bundles.Add(new StyleBundle("~/cdncss/", path + "bootstrap.css"+token));
            bundles.Add(new StyleBundle("~/cdncss/", path + "cardview.css"+token));
            bundles.Add(new StyleBundle("~/cdncss/", path + "AdminLTE.css"+token));
            bundles.Add(new StyleBundle("~/cdncss/", path + "Loader.css"+token));
            bundles.Add(new StyleBundle("~/cdncss/", path + "font-awesome.css"+token));
            bundles.Add(new StyleBundle("~/cdncss/", path + "ionicons.css"+token));
            bundles.Add(new StyleBundle("~/cdncss/", path + "editor.css"+token));
            bundles.Add(new StyleBundle("~/cdncss/", path + "dialog.css"+token));
            bundles.Add(new StyleBundle("~/cdncss/", path + "skin-purple-light.css" + token));
            bundles.Add(new StyleBundle("~/cdncss/", path + "bootstrap3-wysihtml5.min.css" + token));
            bundles.Add(new StyleBundle("~/cdncss/", path + "blue.css"+token));
            bundles.Add(new StyleBundle("~/cdncss/", path + "iFrameLayout.css"+token));
           

           


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/cardview.css",
                      "~/Content/Loader.css",
                      "~/Content/iFrameLayout.css",
                      "~/admin-lte/css/AdminLTE.css",
                      "~/admin-lte/css/font-awesome.css",
                      "~/admin-lte/css/ionicons.css",
                      "~/admin-lte/ckeditor/skins/moono-lisa/editor.css",
                      "~/admin-lte/ckeditor/skins/moono-lisa/dialog.css",
                      "~/admin-lte/css/skins/skin-purple-light.css",
                      "~/admin-lte/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.min.css",
                      "~/admin-lte/plugins/iCheck/square/blue.css"
                    ));
        }
    }
}