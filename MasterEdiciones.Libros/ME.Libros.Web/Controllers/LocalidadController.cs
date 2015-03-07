using System;
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
    public class LocalidadController : Controller
    {
        private LocalidadService LocalidadService { get; set; }
        private ProvinciaService ProvinciaService { get; set; }

        public LocalidadController()
        {
            var modelContainer = new ModelContainer();
            LocalidadService = new LocalidadService(new EntidadRepository<LocalidadDominio>(modelContainer));
            ProvinciaService = new ProvinciaService(new EntidadRepository<ProvinciaDominio>(modelContainer));
        }

        // GET: Localidad
        public ActionResult Index()
        {
            var localidades = new List<LocalidadViewModel>();
            using (LocalidadService)
            {
                //localidades.AddRange(LocalidadService.Listar().Select(l => new LocalidadViewModel
                //{
                //    Nombre = l.Nombre,
                //    Provincia = new ProvinciaViewModel
                //    {
                //        Id = l.Provincia.Id,
                //        Nombre = l.Provincia.Nombre
                //    },
                //}));
            }

            return View(localidades);
        }

        [HttpGet]
        public PartialViewResult Crear()
        {
            var model = new LocalidadViewModel();
            //model.Provincias = ProvinciaService.Listar().Select(p => new ProvinciaViewModel(p)).ToList();
            model.Provincias = new SelectList(ProvinciaService.Listar().Select(p => new ProvinciaViewModel(p)).ToList(), "Id", "Nombre");
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Crear(LocalidadViewModel model)
        {
            using (LocalidadService)
            {
                try
                {
                    LocalidadService.Guardar(new LocalidadDominio
                    {
                        FechaAlta = DateTime.Now,
                        Nombre = model.Nombre,
                        Provincia = ProvinciaService.GetPorId(model.Provincia.Id)
                    });
                }
                catch (Exception ex)
                {
                    //ModelState.AddModelError("Error al guardar el Cliente", "El cliente no se guardó.");
                    return PartialView(model);
                }
            }
            TempData["Mensaje"] = "La localidad fue creada exitosamente";
            return RedirectToAction("Index", "Administracion");
        }
    }
}