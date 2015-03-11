using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ME.Libros.Web.Models;

namespace ME.Libros.Web.Controllers
{
    public class MenuLateralController : Controller
    {
        // GET: Admnistracion
        public ActionResult Index(int id)
        {
            var helper = new UrlHelper(HttpContext.Request.RequestContext);
            var menues = new List<MenuViewModel>();
            switch (id)
            {
                case 1:
                    menues.Add(new MenuViewModel
                    {
                        Id = 1,
                        Nombre = "Clientes",
                        Link = helper.Action("Index", "Cliente"),
                        Posicion = 10,
                        Seleccionado = true
                    });
                    menues.Add(new MenuViewModel
                    {
                        Id = 2,
                        Nombre = "Cobradores",
                        Link = "#",
                        Posicion = 20
                    });
                    menues.Add(new MenuViewModel
                    {
                        Id = 2,
                        Nombre = "Localidades",
                        Link = helper.Action("Index", "Localidad"),
                        Posicion = 50
                    });
                    break;
                case 2:
                    menues.Add(new MenuViewModel
                    {
                        Id = 1,
                        Nombre = "Pedidos",
                        Link = "#",
                        Posicion = 10,
                        Seleccionado = true
                    });
                    menues.Add(new MenuViewModel
                    {
                        Id = 2,
                        Nombre = "Ventas",
                        Link = "#",
                        Posicion = 20
                    });
                    break;
            }

            ViewBag.Menues = menues.OrderBy(m => m.Posicion).ToList();
            return View();
        }

    }
}