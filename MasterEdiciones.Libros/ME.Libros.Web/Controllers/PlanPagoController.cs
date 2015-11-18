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
using ME.Libros.Utils.Enums;
using ME.Libros.Web.Extensions;
using ME.Libros.Web.Models;

namespace ME.Libros.Web.Controllers
{
    public class PlanPagoController : BaseController<PlanPagoDominio>
    {
        public PlanPagoService PlanPagoService { get; set; }
        public VentaService VentaService { get; set; }

        public PlanPagoController()
        {
            var modelContainer = new ModelContainer();
            PlanPagoService = new PlanPagoService(new EntidadRepository<PlanPagoDominio>(modelContainer));
            VentaService = new VentaService(new EntidadRepository<VentaDominio>(modelContainer));
        }

        //
        // GET: /PlanPago/
        public ActionResult Index()
        {
            ViewBag.Id = TempData["Id"];
            ViewBag.Mensaje = TempData["Mensaje"];

            var planPagos = new List<PlanPagoViewModel>();
            using (PlanPagoService)
            {
                planPagos.AddRange(PlanPagoService.ListarAsQueryable()
                    .Where(p => p.Tipo == TipoPlanPago.Financiado)
                    .ToList()
                    .Select(p => new PlanPagoViewModel(p)));
            }

            return View(planPagos);
        }

        [HttpGet]
        public ActionResult Crear()
        {
            var planPagoViewModel = new PlanPagoViewModel();
            return View(planPagoViewModel);
        }

        [HttpPost]
        public ActionResult Crear(PlanPagoViewModel planPagoViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(planPagoViewModel);
            }

            long resultado = 0;
            try
            {
                using (PlanPagoService)
                {
                    var planPagoDominio = new PlanPagoDominio
                    {
                        FechaAlta = DateTime.Now,
                        Nombre = planPagoViewModel.Nombre,
                        Descripcion = planPagoViewModel.Descripcion,
                        CantidadCuotas = planPagoViewModel.CantidadCuotas,
                        MontoCuota = planPagoViewModel.MontoCuota,
                        Monto = planPagoViewModel.CantidadCuotas * planPagoViewModel.MontoCuota,
                        Tipo = TipoPlanPago.Financiado
                    };

                    resultado = PlanPagoService.Guardar(planPagoDominio);
                    if (resultado <= 0)
                    {
                        foreach (var error in PlanPagoService.ModelError)
                        {
                            ModelState.AddModelError(error.Key, error.Value);
                        }
                    }
                    else
                    {
                        TempData["Id"] = planPagoDominio.Id;
                        TempData["Mensaje"] = string.Format(Messages.EntidadNueva, Messages.ElPlanPago, planPagoDominio.Id);
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
                : View(planPagoViewModel);
        }

        [HttpGet]
        public JsonResult Eliminar(int id, string redirectUrl)
        {
            var isRedirect = !string.IsNullOrEmpty(redirectUrl);

            try
            {
                using (PlanPagoService)
                {
                    var planPagoDominio = PlanPagoService.GetPorId(id);
                    PlanPagoService.Eliminar(planPagoDominio);

                    if (isRedirect)
                    {
                        TempData["Id"] = planPagoDominio.Id;
                        TempData["Mensaje"] = string.Format(Messages.EntidadEliminada, Messages.ElPlanPago, planPagoDominio.Id);
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;

                if (sqlException != null && sqlException.Number == 547)
                {
                    ModelState.AddModelError("Error", string.Format(ErrorMessages.DatosAsociados, Messages.ElPlanPago));
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

        public ActionResult Modificar(int id)
        {
            PlanPagoViewModel planPagoViewModel;
            using (PlanPagoService)
            {

                var planPagoDominio = PlanPagoService.GetPorId(id);
                planPagoViewModel = new PlanPagoViewModel(planPagoDominio);

                if (VentaService.ListarAsQueryable().Any(p => p.PlanPago.Id == planPagoDominio.Id))
                {
                    planPagoViewModel.Modificable = true;
                }

            }

            return View(planPagoViewModel);
        }

        [HttpPost]
        public ActionResult Modificar(PlanPagoViewModel planPagoViewModel)
        {
            long resultado = 0;
            if (ModelState.IsValid)
            {
                try
                {
                    using (PlanPagoService)
                    {
                        var planPagoDominio = PlanPagoService.GetPorId(planPagoViewModel.Id);
                        planPagoDominio.Nombre = planPagoViewModel.Nombre;
                        planPagoDominio.Descripcion = planPagoViewModel.Descripcion;

                        if (!planPagoViewModel.Modificable)
                        {
                            planPagoDominio.CantidadCuotas = planPagoViewModel.CantidadCuotas;
                            planPagoDominio.MontoCuota = planPagoViewModel.MontoCuota;
                            planPagoDominio.Monto = planPagoViewModel.CantidadCuotas * planPagoViewModel.MontoCuota;
                        }

                        resultado = PlanPagoService.Guardar(planPagoDominio);
                        if (resultado <= 0)
                        {
                            foreach (var error in PlanPagoService.ModelError)
                            {
                                ModelState.AddModelError(error.Key, error.Value);
                            }
                        }
                        else
                        {
                            TempData["Id"] = planPagoDominio.Id;
                            TempData["Mensaje"] = string.Format(Messages.EntidadModificada, Messages.ElPlanPago, planPagoDominio.Id);
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
                : View(planPagoViewModel);
        }

        [HttpGet]
        public JsonResult Get(int id)
        {
            var planPagoViewModel = new PlanPagoViewModel(PlanPagoService.GetPorId(id));

            return new JsonResult
            {
                Data = planPagoViewModel,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}