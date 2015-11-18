using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

using ME.Libros.Dominio.General;
using ME.Libros.EF;
using ME.Libros.Repositorios;
using ME.Libros.Servicios.General;
using ME.Libros.Web.Extensions;
using ME.Libros.Web.Models;

namespace ME.Libros.Web.Controllers
{
    public class LocalidadController : BaseController<LocalidadDominio>
    {
        private LocalidadService LocalidadService { get; set; }
        private ProvinciaService ProvinciaService { get; set; }
        public ZonaService ZonaService { get; set; }

        public LocalidadController()
        {
            var modelContainer = new ModelContainer();
            LocalidadService = new LocalidadService(new EntidadRepository<LocalidadDominio>(modelContainer));
            ProvinciaService = new ProvinciaService(new EntidadRepository<ProvinciaDominio>(modelContainer));
            ZonaService = new ZonaService(new EntidadRepository<ZonaDominio>(modelContainer));
            ViewBag.MenuId = 3;
            ViewBag.Title = "Localidades";
        }

        // GET: Localidad
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Id = TempData["Id"];
            ViewBag.Mensaje = TempData["Mensaje"];

            var localidades = new List<LocalidadViewModel>();
            using (LocalidadService)
            {
                localidades.AddRange(LocalidadService.Listar()
                    .OrderBy(l => l.Nombre)
                    .ToList()
                    .Select(l => new LocalidadViewModel(l)));
            }

            return View(localidades);
        }

        [HttpGet]
        public ActionResult Crear()
        {
            var model = new LocalidadViewModel();
            PrepareModel(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult Crear(LocalidadViewModel localidadViewModel)
        {
            if (!ModelState.IsValid)
            {
                this.PrepareModel(localidadViewModel);
                return View(localidadViewModel);
            }

            long resultado = 0;
            try
            {
                using (LocalidadService)
                {
                    var localidadDominio = new LocalidadDominio
                                             {
                                                 FechaAlta = DateTime.Now,
                                                 Nombre = localidadViewModel.Nombre,
                                                 CodigoPostal = localidadViewModel.CodigoPostal,
                                                 Provincia = ProvinciaService.GetPorId(localidadViewModel.ProvinciaId),
                                                 Zona = ZonaService.GetPorId(localidadViewModel.ZonaId)
                                             };

                    resultado = LocalidadService.Guardar(localidadDominio);
                    if (resultado <= 0)
                    {
                        foreach (var error in LocalidadService.ModelError)
                        {
                            ModelState.AddModelError(error.Key, error.Value);
                        }
                    }
                    else
                    {
                        TempData["Id"] = localidadDominio.Id;
                        TempData["Mensaje"] = string.Format(Messages.EntidadNueva, Messages.LaLocalidad, localidadDominio.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
            }

            if (resultado == 0)
            {
                PrepareModel(localidadViewModel);
            }

            return resultado > 0
                ? (ActionResult)RedirectToAction("Index")
                : View(localidadViewModel);
        }

        [HttpGet]
        public JsonResult Eliminar(int id, string redirectUrl)
        {
            var isRedirect = !string.IsNullOrEmpty(redirectUrl);

            try
            {
                using (LocalidadService)
                {
                    var localidadDominio = LocalidadService.GetPorId(id);
                    LocalidadService.Eliminar(localidadDominio);

                    if (isRedirect)
                    {
                        TempData["Id"] = localidadDominio.Id;
                        TempData["Mensaje"] = string.Format(Messages.EntidadEliminada, Messages.LaLocalidad, localidadDominio.Id);
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;

                if (sqlException != null && sqlException.Number == 547)
                {
                    ModelState.AddModelError("Error", string.Format(ErrorMessages.DatosAsociados, Messages.LaLocalidad));
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
            LocalidadViewModel localidadViewModel;
            using (LocalidadService)
            {
                localidadViewModel = new LocalidadViewModel(LocalidadService.GetPorId(id));
            }
            PrepareModel(localidadViewModel);

            return View(localidadViewModel);
        }

        [HttpPost]
        public ActionResult Modificar(LocalidadViewModel localidadViewModel)
        {
            if (!ModelState.IsValid)
            {
                PrepareModel(localidadViewModel);
                return View(localidadViewModel);
            }

            long resultado = 0;
            try
            {
                using (LocalidadService)
                {
                    var localidadDominio = LocalidadService.GetPorId(localidadViewModel.Id);
                    localidadDominio.Nombre = localidadViewModel.Nombre;
                    localidadDominio.CodigoPostal = localidadViewModel.CodigoPostal;
                    localidadDominio.Provincia = ProvinciaService.GetPorId(localidadViewModel.ProvinciaId);
                    localidadDominio.Zona = ZonaService.GetPorId(localidadViewModel.ZonaId);

                    resultado = LocalidadService.Guardar(localidadDominio);
                    if (resultado <= 0)
                    {
                        foreach (var error in LocalidadService.ModelError)
                        {
                            ModelState.AddModelError(error.Key, error.Value);
                        }
                    }
                    else
                    {
                        TempData["Id"] = localidadDominio.Id;
                        TempData["Mensaje"] = string.Format(Messages.EntidadModificada, Messages.LaLocalidad, localidadDominio.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
            }

            if (resultado == 0)
            {
                PrepareModel(localidadViewModel);
            }

            return resultado > 0
                ? (ActionResult)RedirectToAction("Index")
                : View(localidadViewModel);
        }

        public JsonResult ListarLocalidades(int id)
        {
            var localidades = new List<LocalidadViewModel>();
            using (LocalidadService)
            {
                localidades.AddRange(LocalidadService
                    .Listar(l => l.Provincia.Id == id)
                    .ToList()
                    .Select(l => new LocalidadViewModel(l)));
            }

            return new JsonResult
            {
                Data = localidades,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        #region Private Methods

        private void PrepareModel(LocalidadViewModel localidadViewModel)
        {
            localidadViewModel.Provincias = new SelectList(ProvinciaService.Listar()
                    .Select(p => new ProvinciaViewModel(p))
                    .ToList(),
                    "Id",
                    "Nombre");

            localidadViewModel.Zonas = new SelectList(ZonaService.Listar()
                .Select(z => new ZonaViewModel(z))
                .ToList(), "Id", "Nombre");
        }

        #endregion
    }
}