﻿using System;
using System.Collections.Generic;
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
    public class CobroController : BaseController<CobroDominio>
    {
        public CobroService CobroService { get; set; }
        private VentaService VentaService { get; set; }
        public ClienteService ClienteService { get; set; }

        public CobroController()
        {
            var modelContainer = new ModelContainer();
            CobroService = new CobroService(new EntidadRepository<CobroDominio>(modelContainer));
            VentaService = new VentaService(new EntidadRepository<VentaDominio>(modelContainer));
            ClienteService = new ClienteService(new EntidadRepository<ClienteDominio>(modelContainer));
            Service = new CobroService(new EntidadRepository<CobroDominio>(modelContainer));
        }

        //
        // GET: /Cobrador/
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Id = TempData["Id"];
            ViewBag.Mensaje = TempData["Mensaje"];

            var cobros = new List<CobroViewModel>();
            using (CobroService)
            {
                cobros.AddRange(CobroService.Listar()
                    .OrderBy(c => c.Id)
                    .ThenBy(c => c.FechaCobro)
                    .ToList()
                    .Select(c => new CobroViewModel(c)));
            }

            return View(cobros);
        }

        [HttpGet]
        public ActionResult Crear()
        {
            var cobroViewModel = new CobroViewModel();
            PrepareModel(cobroViewModel);

            return View(cobroViewModel);
        }

        [HttpPost]
        public ActionResult Crear(CobroViewModel cobroViewModel)
        {

            long resultado = 0;

            if (ModelState.IsValid)
            {
                try
                {
                    using (CobroService)
                    {
                        var cobroDominio = new CobroDominio
                        {
                            FechaAlta = DateTime.Now,
                            Monto = cobroViewModel.Monto,
                            FechaCobro = cobroViewModel.FechaCobro,
                            Estado = EstadoCobro.Cobrado,
                        };

                        resultado = CobroService.Guardar(cobroDominio);
                        if (resultado <= 0)
                        {
                            foreach (var error in CobroService.ModelError)
                            {
                                ModelState.AddModelError(error.Key, error.Value);
                            }
                        }
                        else
                        {
                            TempData["Id"] = cobroDominio.Id;
                            TempData["Mensaje"] = string.Format(Messages.EntidadNueva, Messages.ElCobro, cobroDominio.Id);
                        }
                    }
                }

                catch (Exception ex)
                {
                    ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
                }
            }

            if (resultado == 0)
            {
                PrepareModel(cobroViewModel);
            }

            return resultado > 0
                ? (ActionResult)RedirectToAction("Index")
                : View(cobroViewModel);
        }

        [HttpGet]
        public JsonResult Eliminar(int id)
        {
            var cobroDominio = new CobroDominio();

            if (ModelState.IsValid)
            {

                using (CobroService)
                {
                    cobroDominio = CobroService.GetPorId(id);
                    cobroDominio.Estado = EstadoCobro.Anulado;
                    CobroService.Guardar(cobroDominio);
                }
            }

            return new JsonResult
            {
                Data = new
                {
                    Success = ModelState.IsValid,
                    Errors = ModelState.GetErrors(),
                    cobroDominio.Estado
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };
        }

        [HttpGet]
        public ActionResult Modificar(int id)
        {
            CobroViewModel cobroViewModel;
            using (CobroService)
            {
                cobroViewModel = new CobroViewModel(CobroService.GetPorId(id));
            }
            PrepareModel(cobroViewModel);

            return View(cobroViewModel);
        }

        public PartialViewResult VerCobros(long cuotaId)
        {
            var cobros = new List<CobroViewModel>();
            using (CobroService)
            {
                cobros.AddRange(CobroService.ListarAsQueryable()
                    .Where(c => c.Cuotas.Any(cuota => cuota.Id == cuotaId))
                    .ToList()
                    .Select(cobro => new CobroViewModel(cobro)
                    {
                        Cobrador = new CobradorViewModel(cobro.Rendicion.Cobrador)
                    }));
            }

            return PartialView(cobros);
        }

        #region Private Methods

        private void PrepareModel(CobroViewModel cobroViewModel)
        {
            //cobroViewModel.Clientes = new SelectList(ClienteService.Listar()
            //    .ToList()
            //    .Select(c => new ClienteViewModel(c))
            //    .Select(c => new { Id = c.Id, Text = c.Id + " - " + c.Apellido + ", " + c.Nombre }), "Id", "Text");

            //var ventas = new List<Object>();
            //if (cobroViewModel.Venta.Cliente.Id > 0)
            //{
            //    ventas.AddRange(VentaService.Listar(c=>c.Cliente.Id == cobroViewModel.ClienteId)
            //                                                        .ToList()
            //                                                        .Select(c => new VentaViewModel(c))
            //                                                        .Select(c=> new {Id=c.Id, Text=c.Id + " - " + c.FechaVenta.ToString("dd/MM/yyyy")})


            //                                                        );
            //}
            //cobroViewModel.Ventas = new SelectList(ventas, "Id","Text");
        }

        #endregion

    }
}