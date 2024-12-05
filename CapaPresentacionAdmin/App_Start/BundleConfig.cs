using System.Web;
using System.Web.Optimization;

namespace CapaPresentacionAdmin
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new Bundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/complementos").Include(
               "~/Scripts/moment.min.js",
               "~/Scripts/fullcalendar/fullcalendar.min.js",
               "~/Scripts/DataTables/jquery.dataTables.js",
               "~/Scripts/DataTables/dataTables.responsive.js",
               "~/Scripts/loadingoverlay/loadingoverlay.min.js",
               "~/Scripts/sweetalert.min.js",
               "~/Scripts/fontawesome/all.min.js",
               "~/Scripts/scripts.js"));

            /* bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                         "~/Scripts/jquery.validate*"));

             // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información sobre los formularios.  De esta manera estará
             // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
             bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                         "~/Scripts/modernizr-*"));*/

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.bundle.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                          "~/Content/bootstrap.css",
                          "~/Content/DataTables/css/jquery.dataTables.css",
                          "~/Content/DataTables/css/responsive.dataTables.css",
                          "~/Content/fullcalendar.min.css",
                          "~/Content/sweetalert.css",
                          "~/Content/fontawesome/css/all.min.css",
                           "~/Content/CalendarioStyle.css",
                          "~/Content/site.css"));
        }
    }
}
