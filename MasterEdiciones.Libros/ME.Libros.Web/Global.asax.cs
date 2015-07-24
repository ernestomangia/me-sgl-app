using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EFlogger.EntityFramework6;

namespace ME.Libros.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            ModelBinders.Binders.Add(typeof(long), new LongModelBinder());
            //EFloggerFor6.Initialize();
        }
    }
}
