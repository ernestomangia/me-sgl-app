using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using ME.Libros.Dominio.General;
using ME.Libros.EF;
using ME.Libros.Repositorios;
using ME.Libros.Servicios.General;
using ME.Libros.Web.Models;

namespace ME.Libros.Web.Controllers
{
    public class ProvinciaController : Controller
    {
        public ProvinciaService ProvinciaService { get; set; }

        public ProvinciaController()
        {
            var modelContainer = new ModelContainer();
            ProvinciaService = new ProvinciaService(new EntidadRepository<ProvinciaDominio>(modelContainer));
        }

        // GET: Provincia
        public ActionResult Index()
        {
            var provincias = new List<ProvinciaViewModel>();
            using (ProvinciaService)
            {
                provincias.AddRange(ProvinciaService.Listar().Select(p => new ProvinciaViewModel
                {
                    Id = p.Id,
                    Nombre = p.Nombre
                }));
            }

            return View(provincias);
        }
    }
}