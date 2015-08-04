using System;
using System.Web.Mvc;

namespace ME.Libros.Web.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Error(HandleErrorInfo handleErrorInfo)
        {
            if (handleErrorInfo == null)
            {
                handleErrorInfo = new HandleErrorInfo(new Exception("Ha ocurrido un error en el sistema."), "Error", "Error");
            }
            return View(handleErrorInfo);
        }

        public ActionResult NotFound()
        {
            return View();
        }
    }
}