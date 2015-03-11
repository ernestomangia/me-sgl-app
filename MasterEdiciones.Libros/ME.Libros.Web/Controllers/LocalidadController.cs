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
        [HttpGet]
        public PartialViewResult Index()
        {
            var localidades = new List<LocalidadViewModel>();
            using (LocalidadService)
            {
                var test = LocalidadService.GetPorId(1);
                localidades.AddRange(LocalidadService.Listar().ToList().Select(l => new LocalidadViewModel(l)));
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

            return PartialView(localidades);
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
                                                              Provincia = this.ProvinciaService.GetPorId(model.ProvinciaId)
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