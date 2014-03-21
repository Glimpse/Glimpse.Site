using System.Web;
using System.Web.Optimization;

namespace Glimpse.Site
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/sitejs").Include(
                      "~/Scripts/jquery-{version}.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/site.js"));
             
            bundles.Add(new StyleBundle("~/content/sitecss").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
