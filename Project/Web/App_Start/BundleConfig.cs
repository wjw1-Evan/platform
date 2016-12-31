using System.Web.Optimization;
namespace Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //BundleTable.EnableOptimizations = true;

            // App Styles


            bundles.Add(new StyleBundle("~/bundles/site").Include(
                    "~/Content/*.css",
                       "~/Content/app/css/app.css"
         ));


            bundles.Add(new ScriptBundle("~/bundles/script").Include(
                      "~/Scripts/jquery*",
                        "~/Scripts/bootstrap*",
                          "~/Scripts/bootstrap-rating/bootstrap*",
                        "~/Scripts/Chart.js"


          ));

            bundles.Add(new ScriptBundle("~/bundles/Angle").Include(
             // App init
             "~/Scripts/app/app.init.js",
             // Modules
             "~/Scripts/app/modules/bootstrap-start.js",
             "~/Scripts/app/modules/classyloader.js",
             "~/Scripts/app/modules/clear-storage.js",
             "~/Scripts/app/modules/constants.js",
             "~/Scripts/app/modules/fullscreen.js",//全屏
             "~/Scripts/app/modules/load-css.js",
             //"~/Scripts/app/modules/localize.js",//多语言
             "~/Scripts/app/modules/navbar-search.js",
             "~/Scripts/app/modules/notify.js",//弹出提示消息
            "~/Scripts/app/modules/panel-tools.js",
            "~/Scripts/app/modules/play-animation.js",
            "~/Scripts/app/modules/porlets.js",
            "~/Scripts/app/modules/sidebar.js",
            "~/Scripts/app/modules/skycons.js",
            "~/Scripts/app/modules/slimscroll.js",//漂亮的虚拟滚动条
             "~/Scripts/app/modules/sparkline.js",
             "~/Scripts/app/modules/table-checkall.js",
             "~/Scripts/app/modules/toggle-state.js",//ok
            "~/Scripts/app/modules/utils.js",
            "~/Scripts/app/modules/chart.js",
            "~/Scripts/app/modules/morris.js",//图形报表
            "~/Scripts/app/modules/rickshaw.js",
            "~/Scripts/app/modules/chartist.js",
            "~/Scripts/app/modules/tour.js",//用户向导
            "~/Scripts/app/modules/sweetalert.js",//替代alert 消息警告框插件
            "~/Scripts/app/modules/color-picker.js",
            "~/Scripts/app/modules/imagecrop.js",//图片裁剪
            "~/Scripts/app/modules/chart-knob.js",
            "~/Scripts/app/modules/chart-easypie.js"
));
        }
    }
}
