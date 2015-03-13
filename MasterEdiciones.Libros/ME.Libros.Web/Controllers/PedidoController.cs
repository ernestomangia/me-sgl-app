using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ME.Libros.Web.Controllers
{
    public class PedidoController : Controller
    {
        public PedidoController()
        {
            ViewBag.MenuId = 4;
        }

        // GET: Pedido
        public ActionResult Index()
        {
            return View();
        }
    }
}