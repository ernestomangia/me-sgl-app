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
    public class RendicionController : BaseController<RendicionDominio>
    {
        private RendicionService RendicionService { get; set; }
        private CobradorService CobradorService { get; set; }
        private LocalidadService LocalidadService { get; set; }

        public RendicionController()
        {
            var modelContainer = new ModelContainer();
            RendicionService = new RendicionService(new EntidadRepository<RendicionDominio>(modelContainer));
            CobradorService = new CobradorService(new EntidadRepository<CobradorDominio>(modelContainer));
            LocalidadService = new LocalidadService(new EntidadRepository<LocalidadDominio>(modelContainer));
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
                MontoFacturado = rendicionViewModel.MontoFacturado,
                MontoNeto = rendicionViewModel.MontoNeto,
                Comision = rendicionViewModel.Comision,
                MontoComision = rendicionViewModel.MontoComision,
            };

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
        }

        #endregion
    }
}