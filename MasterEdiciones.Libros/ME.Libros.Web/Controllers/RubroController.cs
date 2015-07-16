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
    public class RubroController : BaseController<RubroDominio>
    {
        //
        // GET: /Rubro/
        public RubroService RubroService { get; set; }
        public ActionResult Index()
        {
            ViewBag.Id = TempData["Id"];
            ViewBag.Mensaje = TempData["Mensaje"];

            var rubros = new List<RubroViewModel>();
            using (RubroService)
            {
                rubros.AddRange(RubroService.Listar()
                    .ToList()
                    .Select(r => new RubroViewModel(r))
                    .Where(r => r.Id != 1));
            }

            return View(rubros);
        }

        public RubroController()
        {
            var modelContainer = new ModelContainer();
            RubroService = new RubroService(new EntidadRepository<RubroDominio>(modelContainer));
            ViewBag.MenuId = 4;
            ViewBag.Title = "Rubros";
        }

        [HttpGet]
        public ActionResult Crear()
        {
            var rubroViewModel = new RubroViewModel();
            return View(rubroViewModel);
        }

        [HttpPost]
        public ActionResult Crear(RubroViewModel rubroViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(rubroViewModel);
            }

            long resultado = 0;
            try
            {
                using (RubroService)
                {
                    var rubroDominio = new RubroDominio
                    {
                        FechaAlta = DateTime.Now,
                        Nombre = rubroViewModel.Nombre,
                        Descripcion = rubroViewModel.Descripcion,
                    };

                    resultado = RubroService.Guardar(rubroDominio);
                    if (resultado <= 0)
                    {
                        foreach (var error in RubroService.ModelError)
                        {
                            ModelState.AddModelError(error.Key, error.Value);
                        }
                    }
                    else
                    {
                        TempData["Id"] = rubroDominio.Id;
                        TempData["Mensaje"] = string.Format(Messages.EntidadNueva, Messages.ElRubro, rubroDominio.Id);
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
                : View(rubroViewModel);
        }

        [HttpGet]
        public JsonResult Eliminar(int id)
        {
            try
            {
                using (RubroService)
                {
                    RubroService.Eliminar(RubroService.GetPorId(id));
                }
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;

                if (sqlException != null && sqlException.Number == 547)
                {
                    ModelState.AddModelError("Error", string.Format(ErrorMessages.DatosAsociados, Messages.ElRubro));
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
                Data = new { Success = ModelState.IsValid, Errors = ModelState.GetErrors() },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult Modificar(int id)
        {
            RubroViewModel rubroViewModel;
            using (RubroService)
            {
                var rubroDominio = RubroService.GetPorId(id);
                rubroViewModel = new RubroViewModel(rubroDominio);
            }

            return View(rubroViewModel);
        }

        [HttpPost]
        public ActionResult Modificar(RubroViewModel rubroViewModel)
        {
            long resultado = 0;
            if (ModelState.IsValid)
            {
                try
                {
                    using (RubroService)
                    {
                        var rubroDominio = RubroService.GetPorId(rubroViewModel.Id);
                        rubroDominio.Nombre = rubroViewModel.Nombre;
                        rubroDominio.Descripcion = rubroViewModel.Descripcion;

                        resultado = RubroService.Guardar(rubroDominio);
                        if (resultado <= 0)
                        {
                            foreach (var error in RubroService.ModelError)
                            {
                                ModelState.AddModelError(error.Key, error.Value);
                            }
                        }
                        else
                        {
                            TempData["Id"] = rubroDominio.Id;
                            TempData["Mensaje"] = string.Format(Messages.EntidadModificada, Messages.ElRubro, rubroDominio.Id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
                }
            }

            return resultado > 0
                ? (ActionResult)RedirectToAction("Index")
                : View(rubroViewModel);
        }

        [HttpGet]
        public ActionResult Detalle(int id)
        {
            RubroViewModel rubroViewModel;
            using (RubroService)
            {
                rubroViewModel = new RubroViewModel(RubroService.GetPorId(id));
            }

            return View(rubroViewModel);
        }
    }
}