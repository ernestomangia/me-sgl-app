using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

using ME.Libros.Dominio.General;
using ME.Libros.EF;
using ME.Libros.Repositorios;
using ME.Libros.Servicios.General;
using ME.Libros.Web.Extensions;
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
                zonas.AddRange(ZonaService.Listar()
                    .ToList()
                    .Select(z => new ZonaViewModel(z))
                    .Where(z => z.Id != 1));
            }

            return View(zonas);
        }

        public ZonaController()
        {
            var modelContainer = new ModelContainer();
            ZonaService = new ZonaService(new EntidadRepository<ZonaDominio>(modelContainer));
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
            if (!ModelState.IsValid)
            {
                return View(zonaViewModel);
            }

            long resultado = 0;
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
                        TempData["Mensaje"] = string.Format(Messages.EntidadNueva, Messages.LaZona, zonaDominio.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log
                ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
            }

            return resultado > 0
                    ? (ActionResult)RedirectToAction("Index")
                    : View(zonaViewModel);
        }

        [HttpGet]
        public JsonResult Eliminar(int id, string redirectUrl)
        {
            var isRedirect = !string.IsNullOrEmpty(redirectUrl);
            try
            {
                using (ZonaService)
                {
                    var zonaDominio = ZonaService.GetPorId(id);
                    ZonaService.Eliminar(zonaDominio);

                    if (isRedirect)
                    {
                        TempData["Id"] = zonaDominio.Id;
                        TempData["Mensaje"] = string.Format(Messages.EntidadEliminada, Messages.LaZona, zonaDominio.Id);
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;

                if (sqlException != null && sqlException.Number == 547)
                {
                    ModelState.AddModelError("Error", string.Format(ErrorMessages.DatosAsociados, Messages.LaZona));
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

            return new JsonResult
            {
                Data = new
                {
                    Success = ModelState.IsValid,
                    Errors = ModelState.GetErrors(),
                    isRedirect,
                    redirectUrl
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult Modificar(int id)
        {
            ZonaViewModel zonaViewModel;
            using (ZonaService)
            {
                var zonaDominio = ZonaService.GetPorId(id);
                zonaViewModel = new ZonaViewModel(zonaDominio);
            }

            return View(zonaViewModel);
        }

        [HttpPost]
        public ActionResult Modificar(ZonaViewModel zonaViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(zonaViewModel);
            }

            long resultado = 0;
            try
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
                        TempData["Mensaje"] = string.Format(Messages.EntidadModificada, Messages.LaZona, zonaDominio.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
            }

            return resultado > 0
                    ? (ActionResult)RedirectToAction("Index")
                    : View(zonaViewModel);
        }
    }
}