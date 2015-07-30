using System.Web.Mvc;
using System.Web.Routing;
using ME.Libros.Utils.Enums;

namespace ME.Libros.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Venta",
            //    url: "Venta/{estado}",
            //    defaults: new { controller = "Venta", action = "Index" });

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Cliente", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Login", id = UrlParameter.Optional });
        }
    }
}
