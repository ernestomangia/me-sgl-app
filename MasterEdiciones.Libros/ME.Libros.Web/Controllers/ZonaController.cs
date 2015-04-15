using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ME.Libros.Dominio.General;
using ME.Libros.EF;
using ME.Libros.Repositorios;
using ME.Libros.Servicios.General;
using ME.Libros.Web.Helpers;
using ME.Libros.Web.Models;

namespace ME.Libros.Web.Controllers
{
    public class ZonaController : BaseController<ZonaDominio>
    {
        //
        // GET: /Zona/
        public ZonaService ZonaService { get; set; }
        public ActionResult Index()
        {
            ViewBag.Id = TempData["Id"];
            ViewBag.Mensaje = TempData["Mensaje"];

            var zonas = new List<ZonaViewModel>();
            using (ZonaService)
            {
                zonas.AddRange(ZonaService.Listar().ToList().Select(z => new ZonaViewModel(z)));
            }

            return View(zonas);

        }

        public ZonaController()
        {
            var modelContainer = new ModelContainer();
            ZonaService = new ZonaService(new EntidadRepository<ZonaDominio>(modelContainer));
            ViewBag.MenuId = 7;
            ViewBag.Title = "Zonas";
        }
        
        [HttpGet]
        public ActionResult Crear()
        {
            var zonaViewModel = new ZonaViewModel();

            return View(zonaViewModel);
        }

        [HttpPost]
        public ActionResult Crear(ZonaViewModel zonaViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (ZonaService)
                    {
                        var zonaDominio = new ZonaDominio
                        {
                            FechaAlta = DateTime.Now,
                            Nombre = zonaViewModel.Nombre,
                            Descripcion = zonaViewModel.Descripcion,
                        };

                        zonaViewModel.Id = ZonaService.Guardar(zonaDominio);
                        if (zonaViewModel.Id <= 0)
                        {
                            foreach (var error in ZonaService.ModelError)
                            {
                                ModelState.AddModelError(error.Key, error.Value);
                            }
                        }
                        else
                        {
                            TempData["Id"] = zonaDominio.Id;
                            TempData["Mensaje"] = string.Format(Messages.EntidadNueva, "La zona", zonaDominio.Id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log
                    ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
                }
            }

            return zonaViewModel.Id > 0
                    ? (ActionResult)RedirectToAction("Index")
                    : View(zonaViewModel);
        }

        [HttpGet]
        public JsonResult Eliminar(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (ZonaService)
                    {
                        ZonaService.Eliminar(ZonaService.GetPorId(id));
                    }
                }
                catch (DbUpdateException ex)
                {
                    var sqlException = ex.GetBaseException() as SqlException;

                    if (sqlException != null && sqlException.Number == 547)
                    {
                        ModelState.AddModelError("Error", ErrorMessages.DatosAsociados);
                    }
                    else
                    {
                        ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
                }
            }

            return new JsonResult
            {
                Data = new { Success = ModelState.IsValid, Errors = ModelState.GetErrors() },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult Modificar(int id)
        {
            var zonaViewModel = new ZonaViewModel();
            try
            {
                using (ZonaService)
                {
                    var zonaDominio = ZonaService.GetPorId(id);
                    zonaViewModel = new ZonaViewModel(zonaDominio);
                }
            }
            catch (Exception ex)
            {
                // Log
                ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
            }

            return View(zonaViewModel);
        }

        [HttpPost]
        public ActionResult Modificar(ZonaViewModel zonaViewModel)
        {
            long resultado = 0;
            if (ModelState.IsValid)
            {
                using (ZonaService)
                {
                    var zonaDominio = ZonaService.GetPorId(zonaViewModel.Id);
                    zonaDominio.Nombre = zonaViewModel.Nombre;
                    zonaDominio.Descripcion = zonaViewModel.Descripcion;

                    resultado = ZonaService.Guardar(zonaDominio);
                    if (resultado <= 0)
                    {
                        foreach (var error in ZonaService.ModelError)
                        {
                            ModelState.AddModelError(error.Key, error.Value);
                        }
                    }
                    else
                    {
                        TempData["Id"] = zonaDominio.Id;
                        TempData["Mensaje"] = string.Format(Messages.EntidadModificada, "La zona", zonaDominio.Id);
                    }
                }
            }


            return zonaViewModel.Id > 0
                    ? (ActionResult)RedirectToAction("Index")
                    : View(zonaViewModel);
        }

        [HttpGet]
        public ActionResult Detalle(int id)
        {
            ZonaViewModel zonaViewModel;
            using (ZonaService)
            {
                zonaViewModel = new ZonaViewModel(ZonaService.GetPorId(id));
            }

            return View(zonaViewModel);
        }

	}
}