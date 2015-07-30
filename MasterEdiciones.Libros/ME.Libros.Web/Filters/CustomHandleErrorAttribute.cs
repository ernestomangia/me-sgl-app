using System.Web;
using ME.Libros.Api.Logging;
using ME.Libros.Logging;
using System.Web.Mvc;

namespace ME.Libros.Web.Filters
{
    public class CustomHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }

            var logguer = new Logger();
            var exception = filterContext.Exception;
            logguer.Log(exception.Message, SeveridadLog.Error);

            var httpException = filterContext.Exception as HttpException;

            //Set the view correctly depending if it's an AJAX request or not
            if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        error = true,
                    }
                };
            }
            else
            {
                var action = filterContext.RouteData.Values["action"].ToString();
                var controller = filterContext.RouteData.Values["controller"].ToString();
                var handleErrorAttribute = new HandleErrorInfo(filterContext.Exception, controller, action);
                var viewName = (httpException != null && httpException.GetHttpCode() == 404) 
                    ? "NotFound" 
                    : "Error";
                
                filterContext.Result = new ViewResult
                {
                    ViewName = viewName,
                    ViewData = new ViewDataDictionary(handleErrorAttribute),
                };
            }
            //If it's not a httpException, just set the status code as 500
            filterContext.HttpContext.Response.StatusCode = httpException != null
                ? httpException.GetHttpCode()
                : 500;

            filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}