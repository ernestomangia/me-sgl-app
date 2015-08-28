using System;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EFlogger.EntityFramework6;
using ME.Libros.Api.Logging;
using ME.Libros.Logging;
using ME.Libros.Web.Controllers;

namespace ME.Libros.Web
{
    public class MvcApplication : HttpApplication
    {
        private Logger logguer;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            ModelBinders.Binders.Add(typeof(long), new LongModelBinder());
            //EFloggerFor6.Initialize();
            logguer = new Logger();
            logguer.Log("App Starts", SeveridadLog.Info);
        }
        
        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            //Logging goes here
            logguer = new Logger();
            logguer.Log(exception.Message, SeveridadLog.Error);
            
            var httpException = exception as HttpException;
            var routeData = new RouteData();
            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = "Error";
            var handleErrorAttribute = new HandleErrorInfo(exception, "Error", "Error");
            routeData.Values.Add("handleErrorInfo", handleErrorAttribute);

            if (httpException != null)
            {
                if (httpException.GetHttpCode() == 404)
                {
                    routeData.Values["action"] = "NotFound";
                }
                
                Response.StatusCode = httpException.GetHttpCode();
            }
            else
            {
                Response.StatusCode = 500;
            }

            Server.ClearError();
            Response.Clear();
            Response.ContentType = "text/html";

            // Avoid IIS7 getting involved
            Response.TrySkipIisCustomErrors = true;

            // Execute the error controller
            IController c = new ErrorController();
            c.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        }
    }
}
