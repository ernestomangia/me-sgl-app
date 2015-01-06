using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ME.Libros.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Hoem
        public ActionResult Index()
        {
            return View();
        }
    }
}