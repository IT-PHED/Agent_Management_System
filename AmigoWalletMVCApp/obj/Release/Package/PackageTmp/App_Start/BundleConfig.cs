// -----------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="CardioReady">
// Bundle Configuration for Scripts,Styles
// </copyright>
// <summary>This is the BundleConfig class</summary>
// -----------------------------------------------------------------------

namespace Fedco.PHED.Agent.management
{
    using System.Configuration;
    using System.Web.Optimization;    

    /// <summary>
    /// Bundle Configuration
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// Registers the bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.10.2*"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui*", "~/Scripts/jquery.bt.min.js", "~/Scripts/excanvas.js"));

            
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

           
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/theme.css", "~/Content/Site.css", "~/Content/landing.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
            bundles.Add(new StyleBundle("~/Content/themes1/css").Include(
                       "~/Content/themes1/default/easyui.css",
                       "~/Content/themes1/icon.css"));
           
            bundles.Add(new ScriptBundle("~/bundles/sitejs").Include("~/Scripts/site.js"));

            bundles.Add(new ScriptBundle("~/bundles/dataTables").Include("~/Scripts/DataTables/jquery.dataTables.js", "~/Scripts/DataTables/jquery.dataTables.columnFilter.js"));
            bundles.Add(new ScriptBundle("~/bundles/dataTableButtons").Include("~/Scripts/DataTables/buttons.flash.min.js", "~/Scripts/DataTables/dataTables.buttons.min.js"));
            bundles.Add(new StyleBundle("~/Content/datatablesCss").Include("~/Content/DataTables/css/jquery.dataTables.css"));
          

            bundles.Add(new ScriptBundle("~/bundles/tableTools").Include("~/Scripts/TableTools/TableTools.js", "~/Scripts/TableTools/ZeroClipboard.js"));
           
            bundles.Add(new StyleBundle("~/Content/tableToolsCss").Include("~/Content/TableTools/TableTools.css"));
            BundleTable.EnableOptimizations = false;
        }
    }
}