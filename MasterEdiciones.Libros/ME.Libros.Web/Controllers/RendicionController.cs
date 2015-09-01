using System;
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
    public class RendicionController : BaseController<RendicionDominio>
    {
        private RendicionService RendicionService { get; set; }
        private CobradorService CobradorService { get; set; }
        private LocalidadService LocalidadService { get; set; }
        private VentaService VentaService { get; set; }

        public RendicionController()
        {
            var modelContainer = new ModelContainer();
            CobradorService = new CobradorService(new EntidadRepository<CobradorDominio>(modelContainer));
            LocalidadService = new LocalidadService(new EntidadRepository<LocalidadDominio>(modelContainer));
            VentaService = new VentaService(new EntidadRepository<VentaDominio>(modelContainer));
            RendicionService = new RendicionService(new EntidadRepository<RendicionDominio>(modelContainer), VentaService);

            ViewBag.MenuId = 100;
            ViewBag.Title = "Rendiciones";
        }

        // GET: Rendicion
        public ActionResult Index()
        {
            ViewBag.Id = TempData["Id"];
            ViewBag.Mensaje = TempData["Mensaje"];

            var rendiciones = new List<RendicionViewModel>();
            using (RendicionService)
            {
                rendiciones.AddRange(RendicionService.Listar()
                    .OrderBy(r => r.Periodo)
                    .ThenBy(r => r.Cobrador.Apellido)
                    .ThenBy(r => r.Cobrador.Nombre)
                    .ThenBy(r => r.Localidad.Nombre)
                    .ToList()
                    .Select(r => new RendicionViewModel(r)));
            }
            return View(rendiciones);
        }

        [HttpGet]
        public ActionResult Crear()
        {
            var rendicionViewModel = new RendicionViewModel();
            PrepareModel(rendicionViewModel);

            return View(rendicionViewModel);
        }

        [HttpPost]
        public ActionResult Crear(RendicionViewModel rendicionViewModel)
        {
            if (!ModelState.IsValid)
            {
                PrepareModel(rendicionViewModel);
                return View(rendicionViewModel);
            }

            var rendicionDominio = new RendicionDominio
            {
                FechaAlta = DateTime.Now,
                Periodo = rendicionViewModel.Periodo,
                Cobrador = CobradorService.GetPorId(rendicionViewModel.CobradorId),
                Localidad = LocalidadService.GetPorId(rendicionViewModel.LocalidadId),
                Comision = rendicionViewModel.Comision,
                MontoComision = rendicionViewModel.MontoComision,
                Cobros = new List<CobroDominio>()
            };

            RendicionService.ContabilizarCobros(rendicionDominio, rendicionViewModel.Cobros.Select(DtoHelper.ConvertToDto));

            long resultado = 0;
            try
            {
                using (RendicionService)
                {
                    resultado = RendicionService.Guardar(rendicionDominio);
                }

                if (resultado <= 0)
                {
                    foreach (var error in RendicionService.ModelError)
                    {
                        ModelState.AddModelError(error.Key, error.Value);
                    }
                }
                else
                {
                    TempData["Id"] = rendicionDominio.Id;
                    TempData["Mensaje"] = string.Format(Messages.EntidadNueva, Messages.LaRendicion, rendicionDominio.Id);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
            }

            if (resultado > 0)
            {
                return RedirectToAction("Index");
            }

            PrepareModel(rendicionViewModel);
            return View(rendicionViewModel);
        }

        [HttpGet]
        public ActionResult Modificar(int id)
        {
            RendicionViewModel rendicionViewModel;
            using (RendicionService)
            {
                rendicionViewModel = new RendicionViewModel(RendicionService.GetPorId(id));
            }

            PrepareModel(rendicionViewModel);
            return View(rendicionViewModel);
        }

        [HttpGet]
        public PartialViewResult ListarCobros(int cobradorId, int localidadId)
        {
            var rendicion = new RendicionViewModel();
            if (cobradorId <= 0 || localidadId <= 0)
            {
                return PartialView(rendicion);
            }

            using (VentaService)
            {
                rendicion.Cobros.AddRange(VentaService.ListarAsQueryable()
                    .Where(v => v.Estado == EstadoVenta.Vigente
                                && v.Cobrador.Id == cobradorId
                                && v.Cliente.Localidad.Id == localidadId)
                    .ToList()
                    .Select(v => new CobroViewModel
                    {
                        Venta = new VentaViewModel(v),
                        VentaId = v.Id
                    }));
            }

            using (CobradorService)
            {
                rendicion.Cobrador = new CobradorViewModel(CobradorService.GetPorId(cobradorId));
            }

            return PartialView(rendicion);
        }

        #region Private Methods

        private void PrepareModel(RendicionViewModel rendicionViewModel)
        {
            rendicionViewModel.Cobradores = new SelectList(CobradorService.Listar()
                .ToList()
                .Select(c => new { Id = c.Id, Text = c.Id + " - " + c.Dni }), "Id", "Text");

            rendicionViewModel.Localidades = new SelectList(LocalidadService.Listar()
                .ToList()
                .Select(l => new LocalidadViewModel(l)),
                "Id",
                "Nombre");

            foreach (var cobroViewModel in rendicionViewModel.Cobros)
            {
                cobroViewModel.Venta = new VentaViewModel(VentaService.GetPorId(cobroViewModel.VentaId));
            }
        }

        #endregion
    }
}