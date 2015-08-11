using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ME.Libros.Dominio.General;
using ME.Libros.EF;
using ME.Libros.Repositorios;
using ME.Libros.Servicios.General;
using ME.Libros.Web.Models;

namespace ME.Libros.Web.Controllers
{
    public class RendicionController : BaseController<RendicionDominio>
    {
        public RendicionService RendicionService { get; set; }

        public RendicionController()
        {
            var modelContainer = new ModelContainer();
            RendicionService = new RendicionService(new EntidadRepository<RendicionDominio>(modelContainer));
            ViewBag.MenuId = 100;
            ViewBag.Title = "Rendiciones";
        }

        // GET: Rendicion
        public ActionResult Index()
        {
            ViewBag.Id = TempData["Id"];
            ViewBag.Mensaje = TempData["Mensaje"];

            var rendiciones = new List<RendicionViewModel>();
            using (RendicionService)
            {
                rendiciones.AddRange(RendicionService.Listar()
                    .OrderBy(r => r.Periodo)
                    .ThenBy(r => r.Cobrador.Apellido)
                    .ThenBy(r => r.Cobrador.Nombre)
                    .ThenBy(r => r.Zona.Nombre)
                    .ToList()
                    .Select(r => new RendicionViewModel(r)));
            }
            return View(rendiciones);
        }
    }
}