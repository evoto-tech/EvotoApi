﻿using System.Web.Optimization;

namespace EvotoApi
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/moment.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/bootstrap-datetimepicker.js",
                "~/Scripts/sweetalert-dev.js",
                "~/Scripts/respond.js",
                "~/Scripts/icheck.js"));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css"));

            // Management CSS Bundle
            bundles.Add(new StyleBundle("~/bundles/css/management").Include(
                    "~/Content/bootstrap.css",
                    "~/Content/bootstrap-datetimepicker.css",
                    "~/Content/AdminLTE.css",
                    "~/Content/skins/skin-green-light.css",
                    "~/Content/sweetalert.css",
                    "~/Content/management-site.css",
                    "~/Content/slider.css")
                .Include("~/Content/icheck/_all.css", new CssRewriteUrlTransform())
                .Include("~/Content/icheck/flat.css", new CssRewriteUrlTransform())
                .Include("~/Content/icheck/grey.css", new CssRewriteUrlTransform())
                .Include("~/Content/icheck/green.css", new CssRewriteUrlTransform()));

            // Admin LTE
            bundles.Add(new ScriptBundle("~/bundles/adminlte").Include("~/Scripts/adminlte.js"));

            // React
            bundles.Add(new ScriptBundle("~/bundles/react").Include("~/Scripts/react.js"));
        }
    }
}