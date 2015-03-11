using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ME.Libros.Web.Controllers
{
    public class VentaController : Controller
    {
        // GET: Ventas
        public ActionResult Index()
        {
            return View();
        }
    }
}