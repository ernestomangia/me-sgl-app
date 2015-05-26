using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ME.Libros.Dominio.General;
using ME.Libros.EF;
using ME.Libros.Repositorios;
using ME.Libros.Servicios.General;
using ME.Libros.Web.Models;

namespace ME.Libros.Web.Controllers
{
    public class PlanPagoController : BaseController<PlanPagoDominio>
    {
        //
        // GET: /PlanPago/

        public PlanPagoService PlanPagoService { get; set; }

       
        public ActionResult Index()
        {
            ViewBag.Id = TempData["Id"];
            ViewBag.Mensaje = TempData["Mensaje"];

            var planPagos = new List<PlanPagoViewModel>();
            using (PlanPagoService)
            {
                planPagos.AddRange(PlanPagoService.Listar()
                    .ToList()
                    .Select(p => new PlanPagoViewModel(p)));
            }

            return View(planPagos);

        }

        public PlanPagoController()
        {
            var modelContainer = new ModelContainer();
            PlanPagoService = new PlanPagoService(new EntidadRepository<PlanPagoDominio>(modelContainer));
            ViewBag.MenuId = 24;
            ViewBag.Title = "Planes de Pago";
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
                        Monto = planPagoViewModel.Monto,
                        Tipo = planPagoViewModel.Tipo,
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

	}
}