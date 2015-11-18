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
    public class GastoController : BaseController<GastoDominio>
    {
        //
        // GET: /Gasto/

        public GastoService GastoService { get; set; }

        public GastoController()
        {
            var modelContainer = new ModelContainer();
            GastoService = new GastoService(new EntidadRepository<GastoDominio>(modelContainer));
        }

        public ActionResult Index()
        {
            ViewBag.Id = TempData["Id"];
            ViewBag.Mensaje = TempData["Mensaje"];

            var gastos = new List<GastoViewModel>();
            using (GastoService)
            {
                gastos.AddRange(GastoService.Listar()
                    .ToList()
                    .Where(r => r.Id != 1)
                    .Select(r => new GastoViewModel(r)));
            }

            return View(gastos);
        }

        [HttpGet]
        public ActionResult Crear()
        {
            var gastoViewModel = new GastoViewModel();
            return View(gastoViewModel);
        }

        [HttpPost]
        public ActionResult Crear(GastoViewModel gastoViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (GastoService)
                    {
                        var gastoDominio = new GastoDominio
                        {
                            FechaAlta = DateTime.Now,
                            Nombre = gastoViewModel.Nombre,
                            Descripcion = gastoViewModel.Descripcion,
                        };

                        gastoViewModel.Id = GastoService.Guardar(gastoDominio);
                        if (gastoViewModel.Id <= 0)
                        {
                            foreach (var error in GastoService.ModelError)
                            {
                                ModelState.AddModelError(error.Key, error.Value);
                            }
                        }
                        else
                        {
                            TempData["Id"] = gastoViewModel.Id;
                            TempData["Mensaje"] = string.Format(Messages.EntidadNueva, "El gasto", gastoDominio.Id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log
                    ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
                }
            }

            return gastoViewModel.Id > 0
                    ? (ActionResult)RedirectToAction("Index")
                    : View(gastoViewModel);
        }

        [HttpGet]
        public JsonResult Eliminar(int id, string redirectUrl)
        {
            var isRedirect = !string.IsNullOrEmpty(redirectUrl);

            try
            {
                using (GastoService)
                {
                    var gastoDominio = GastoService.GetPorId(id);
                    GastoService.Eliminar(gastoDominio);

                    if (isRedirect)
                    {
                        TempData["Id"] = gastoDominio.Id;
                        TempData["Mensaje"] = string.Format(Messages.EntidadEliminada, Messages.ElGasto, gastoDominio.Id);
                    }
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
            var gastoViewModel = new GastoViewModel();
            try
            {
                using (GastoService)
                {
                    var gastoDominio = GastoService.GetPorId(id);
                    gastoViewModel = new GastoViewModel(gastoDominio);
                }
            }
            catch (Exception ex)
            {
                // Log
                ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
            }

            return View(gastoViewModel);
        }

        [HttpPost]
        public ActionResult Modificar(GastoViewModel gastoViewModel)
        {
            long resultado = 0;
            if (ModelState.IsValid)
            {
                using (GastoService)
                {
                    var gastoDominio = GastoService.GetPorId(gastoViewModel.Id);
                    gastoDominio.Nombre = gastoViewModel.Nombre;
                    gastoDominio.Descripcion = gastoViewModel.Descripcion;

                    resultado = GastoService.Guardar(gastoDominio);
                    if (resultado <= 0)
                    {
                        foreach (var error in GastoService.ModelError)
                        {
                            ModelState.AddModelError(error.Key, error.Value);
                        }
                    }
                    else
                    {
                        TempData["Id"] = gastoViewModel.Id;
                        TempData["Mensaje"] = string.Format(Messages.EntidadModificada, "El gasto", gastoViewModel.Id);
                    }
                }
            }


            return gastoViewModel.Id > 0
                    ? (ActionResult)RedirectToAction("Index")
                    : View(gastoViewModel);
        }
    }
}