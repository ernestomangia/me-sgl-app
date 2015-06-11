using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ME.Libros.Web.Controllers
{
    public class CompraController : Controller
    {
        //
        // GET: /Compra/

        public CompraController()
        {
            ViewBag.MenuId = 25;
        }

        public ActionResult Index()
        {
            return View();
        }
	}
}