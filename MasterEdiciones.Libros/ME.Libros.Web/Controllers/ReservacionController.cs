using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ME.Libros.Web.Helpers;
using ME.Libros.Web.Models;

namespace ME.Libros.Web.Controllers
{
    public class ReservacionController : Controller
    {
        public ReservacionController()
        {
            ViewBag.MenuId = 20;
        }

        // GET: Reservacion
        public ActionResult Index()
        {
            return View(new List<VentaViewModel>());
        }

        public ActionResult Crear()
        {
            return View(new VentaViewModel());
        }

        public ActionResult Modificar()
        {
            return View(new VentaViewModel());
        }

        public JsonResult Eliminar()
        {
            return new JsonResult
            {
                Data = new { Success = ModelState.IsValid, Errors = ModelState.GetErrors() },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}