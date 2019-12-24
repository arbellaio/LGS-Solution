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
//            bundles.UseCdn = true;

//            bundles.Add(new ScriptBundle("~/admin-lte/js", "https://cdn.jsdelivr.net/npm/admin-lte@2.4.18/dist/js/adminlte.js"));
//            bundles.Add(new ScriptBundle("~/admin-lte/js", "https://cdn.jsdelivr.net/npm/admin-lte@2.4.18/bower_components/chart.js/Chart.js"));
//            bundles.Add(new ScriptBundle("~/admin-lte/js", "https://cdn.jsdelivr.net/npm/admin-lte@2.4.18/bower_components/jquery/dist/jquery.min.js"));
//            bundles.Add(new ScriptBundle("~/admin-lte/js", "https://cdn.jsdelivr.net/npm/admin-lte@2.4.18/plugins/iCheck/icheck.min.js"));

//            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
//                "~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js"));
            bundles.Add(new ScriptBundle("~/admin-lte/js").Include(
                "~/admin-lte/js/fastclick.min.js",
                "~/Scripts/respond.min.js",
                "~/Scripts/moment.min.js",
                "~/Scripts/datetimeFormat.min.js"
            ));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.min.css",
                "~/Content/cardview.min.css",
                "~/admin-lte/css/AdminLTE.min.css",
                "~/admin-lte/css/font-awesome.min.css",
                "~/admin-lte/css/ionicons.min.css",
                "~/admin-lte/css/skins/skin-purple-light.min.css",
                "~/admin-lte/plugins/iCheck/square/blue.min.css"
            ));



            //            bundles.Add(new ScriptBundle("~/cdnjs/", path + "adminlte.js" + token));
            //            bundles.Add(new ScriptBundle("~/cdnjs/", path + "ckeditor.js" + token));
            //            bundles.Add(new ScriptBundle("~/cdnjs/", path + "styles.js" + token));
            //            bundles.Add(new ScriptBundle("~/cdnjs/", path + "config.js" + token));
            //            bundles.Add(new ScriptBundle("~/cdnjs/", path + "icheck.min.js" + token));
            //            bundles.Add(new ScriptBundle("~/cdnjs/", path + "chart.js" + token));
            //            bundles.Add(new ScriptBundle("~/cdnjs/", path + "jquery.min.js" + token));
            //            bundles.Add(new ScriptBundle("~/cdnjs/", path + "fastclick.js" + token));
            //            bundles.Add(new ScriptBundle("~/cdnjs/", path + "jquery-ui-1.10.1.custom.min.js" + token));
            //            bundles.Add(new ScriptBundle("~/cdnjs/", path + "respond.js" + token));
            //            bundles.Add(new ScriptBundle("~/cdnjs/", path + "moment.min.js" + token));
            //            bundles.Add(new ScriptBundle("~/cdnjs/", path + "datetimeFormat.js" + token));
            //            bundles.Add(new ScriptBundle("~/cdnjs/", path + "bootstrap3-wysihtml5.all.min.js" + token));
            //            bundles.Add(new ScriptBundle("~/cdnjs/", path + "bootstrap.js" + token));








            //            bundles.Add(new StyleBundle("~/cdncss/", path + "bootstrap.css"+token));
            //            bundles.Add(new StyleBundle("~/cdncss/", path + "cardview.css"+token));
            //            bundles.Add(new StyleBundle("~/cdncss/", path + "AdminLTE.css"+token));
            //            bundles.Add(new StyleBundle("~/cdncss/", path + "Loader.css"+token));
            //            bundles.Add(new StyleBundle("~/cdncss/", path + "font-awesome.css"+token));
            //            bundles.Add(new StyleBundle("~/cdncss/", path + "ionicons.css"+token));
            //            bundles.Add(new StyleBundle("~/cdncss/", path + "editor.css"+token));
            //            bundles.Add(new StyleBundle("~/cdncss/", path + "dialog.css"+token));
            //            bundles.Add(new StyleBundle("~/cdncss/", path + "skin-purple-light.css" + token));
            //            bundles.Add(new StyleBundle("~/cdncss/", path + "bootstrap3-wysihtml5.min.css" + token));
            //            bundles.Add(new StyleBundle("~/cdncss/", path + "blue.css"+token));
            //            bundles.Add(new StyleBundle("~/cdncss/", path + "iFrameLayout.css"+token));






            //Needs to be false on Live Server in Release Mode
            BundleTable.EnableOptimizations = false;
        }
    }
}