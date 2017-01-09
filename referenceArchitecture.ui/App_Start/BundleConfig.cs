using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Optimization;

namespace referenceArchitecture.ui
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Javascript ===========================================================
            // Js Vendors
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include( // JqueryValidate
                "~/Scripts/Vendor/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include( // Bootstrap and respond
                "~/Scripts/Vendor/respond.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/Vendor/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/timer").Include( // Bootstrap and respond
                "~/Scripts/Vendor/timer.jquery.min.js"));
            
                bundles.Add(new ScriptBundle("~/bundles/mask").Include( // Bootstrap and respond
                "~/Scripts/Vendor/jquery.maskedinput.min.js"));
             
            
            string jsMorrisTempPath = "~/Scripts/scriptTemplate/"; // js morris template in dasboard
            var jsMorrisTemp = new ScriptBundle("~/bundles/jsTemplateMorris").Include(
                jsMorrisTempPath + "morris.min.js",
                jsMorrisTempPath + "morris-data.js"
                );
            jsMorrisTemp.Orderer = new PassthruBundleOrderer();
            bundles.Add(jsMorrisTemp);

            string jsTempFlotPath = "~/Scripts/scriptTemplate/flot/"; // Js Flot template
            var jsTempFlot = new ScriptBundle("~/bundles/jsTemplate/flot").Include(
                //jsTempFlotPath + "excanvas.min.js",
                jsTempFlotPath + "jquery.flot.js",
                //jsTempFlotPath + "jquery.flot.pie.js",
                //jsTempFlotPath + "jquery.flot.resize.js",
                //jsTempFlotPath + "jquery.flot.time.js",
                jsTempFlotPath + "jquery.flot.tooltip.min.js"
                //jsTempFlotPath + "flot-data.js"
                );
            jsTempFlot.Orderer = new PassthruBundleOrderer();
            bundles.Add(jsTempFlot);

            string jsDataTableTemp = "~/Scripts/scriptTemplate/dataTable/"; // Js Datatable template
            var jsDataTable = new ScriptBundle("~/bundles/jsTemplate/dataTable").Include(
                jsDataTableTemp + "jquery.dataTables.min.js",
                jsDataTableTemp + "dataTables.bootstrap.min.js",
                jsDataTableTemp + "dataTables.responsive.js"
                );
            jsDataTable.Orderer = new PassthruBundleOrderer();
            bundles.Add(jsDataTable);
            
            //JsTemplate
            //todo ARCHITECTURE : Include the vendor bundle and the stringpath.
            string jsTemplatePath = "~/Scripts/scriptTemplate/";
            var jsTemplate = new ScriptBundle("~/bundles/jsTemplate").Include(
                jsTemplatePath + "jquery.min.js",
                jsTemplatePath + "bootstrap.min.js",
                jsTemplatePath + "metisMenu.min.js",
                jsTemplatePath + "raphael.min.js",
                jsTemplatePath + "sb-admin-2.js"
                );
            jsTemplate.Orderer = new PassthruBundleOrderer();
            bundles.Add(jsTemplate);

            // js Components
            string jsComponentsPath = "~/Scripts/Components/";
            var jsComponents = new ScriptBundle("~/bundles/Components").Include(
                    jsComponentsPath + "plot.js"
                );
            jsComponents.Orderer = new PassthruBundleOrderer();
            bundles.Add(jsComponents);

            // js Views
            var jsViewsPath = "~/Scripts/Views/";
            bundles.Add(new ScriptBundle("~/jsViews/common").Include( 
                 jsViewsPath + "common.js"));
            bundles.Add(new ScriptBundle("~/jsViews/Configuration").Include(
                 jsViewsPath + "Configuration.js"));
            bundles.Add(new ScriptBundle("~/jsViews/DashBoard").Include(
                 jsViewsPath + "DashBoard.js"));
            bundles.Add(new ScriptBundle("~/jsViews/Estimations").Include(
                 jsViewsPath + "Estimations.js"));
            bundles.Add(new ScriptBundle("~/jsViews/Projects").Include(
                 jsViewsPath + "Projects.js"));
            bundles.Add(new ScriptBundle("~/jsViews/Requirements").Include(
                 jsViewsPath + "Requirements.js"));
            bundles.Add(new ScriptBundle("~/jsViews/Tasks").Include(
                 jsViewsPath + "Tasks.js"));
            bundles.Add(new ScriptBundle("~/jsViews/Statistic").Include(
                jsViewsPath + "Statistic.js"));

            // CSS Style ===========================================================
            // css Template
            string csstemplatePath = "~/Content/cssTemplate/";
            var csstemplate = new StyleBundle("~/Content/cssTemplate").Include(
                csstemplatePath + "bootstrap.min.css",
                csstemplatePath + "metisMenu.min.css",
                csstemplatePath + "sb-admin-2.css",
                csstemplatePath + "morris.css",
                csstemplatePath + "font-awesome.min.css"
            );
            csstemplate.Orderer = new PassthruBundleOrderer();
            bundles.Add(csstemplate);

            // css datatable template
            string cssTempDataTablePath = "~/Content/cssTemplate/dataTable/";
            bundles.Add(new ScriptBundle("~/Content/cssTemplate/dataTable").IncludeDirectory(
                cssTempDataTablePath, "*.css", false));

            // css Views
            var cssViewsPath = "~/Content/Views/";
            bundles.Add(new ScriptBundle("~/Content/cssViews").IncludeDirectory(
                cssViewsPath, "*.css", false));
        }
    }

    /// <summary>
    /// Class to order the js and css files.
    /// </summary>
    public class PassthruBundleOrderer : IBundleOrderer
    {
        /// <summary>
        /// Order the css and js files.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="files">Files.</param>
        /// <returns>An collection of bundlefile.</returns>
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
}
