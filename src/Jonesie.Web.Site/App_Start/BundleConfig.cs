using System.Web;
using System.Web.Optimization;

namespace Jonesie.Web.Site
{
  public class BundleConfig
  {
    // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
    public static void RegisterBundles(BundleCollection bundles)
    {
      bundles.Add(new ScriptBundle("~/bundles/base").Include(
                  "~/Scripts/lib/jquery-{version}.js",
                  "~/Scripts/lib/underscore*",
                  "~/Scripts/lib/handlebars.js",
                  "~/Scripts/lib/bootstrap.*",
                  "~/Scripts/lib/bootstrap-modal*",
                  "~/Scripts/lib/moment*",
                  "~/Scripts/lib/jquery.signalR*",
                  "~/Scripts/redactor/redactor.js",
                  "~/Scripts/jonesie/app.js"));

      bundles.Add(new ScriptBundle("~/bundles/jonesiegrid").Include(
                  "~/Scripts/lib/backbone*",
                  "~/Scripts/jonesie/JonesieGrid.js"));

      //bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
      //            "~/Scripts/lib/jquery-ui-{version}.js"));

      bundles.Add(new ScriptBundle("~/bundles/forms").Include(
                  "~/Scripts/lib/jquery.form*",
                  "~/Scripts/lib/jquery.unobtrusive*",
                  "~/Scripts/lib/jquery.validate*",
                  "~/Scripts/jonesie/bsforms.js"));

      // Use the development version of Modernizr to develop with and learn from. Then, when you're
      // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
      bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                  "~/Scripts/lib/modernizr-*"));

      bundles.Add(new StyleBundle("~/Content/css").Include(
                  "~/Content/bootstrap.css",
                  "~/Content/redactor/redactor.css",
                  "~/Content/site.css",
                  "~/Content/bootstrap-responsive.css",
                  "~/Content/bootstrap-modal.css",
                  "~/Content/animate-custom.css"));

    }
  }
}