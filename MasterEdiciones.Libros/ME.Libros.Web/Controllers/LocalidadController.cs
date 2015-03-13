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
            ViewBag.MenuId = 3;
        }

        // GET: Localidad
        [HttpGet]
        public ActionResult Index()
        {
            var localidades = new List<LocalidadViewModel>();
            using (LocalidadService)
            {
                localidades.AddRange(LocalidadService.Listar().ToList().Select(l => new LocalidadViewModel(l)));
            }

            return View(localidades);
        }

        [HttpGet]
        public ActionResult Crear()
        {
            var model = new LocalidadViewModel
                            {
                                Provincias = new SelectList(this.ProvinciaService.Listar().Select(p => new ProvinciaViewModel(p)).ToList(), "Id", "Nombre")
                            };

            return View(model);
        }

        [HttpPost]
        public JsonResult Crear(LocalidadViewModel model)
        {
            var exito = false;
            var mensajeError = "";
            try
            {
                using (LocalidadService)
                {
                    var id = LocalidadService.Guardar(new LocalidadDominio
                                                          {
                                                              FechaAlta = DateTime.Now,
                                                              Nombre = model.Nombre,
                                                              Provincia = ProvinciaService.GetPorId(model.Provincia.Id)
                                                          });
                    exito = id > 0;
                }
            }
            catch (Exception ex)
            {
                mensajeError = ex.Message;
            }

            return new JsonResult
            {
                Data = new { success = exito, mensaje = mensajeError }
            };
        }
    }
}