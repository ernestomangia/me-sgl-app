
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using ME.Libros.Dominio.General;
using ME.Libros.EF;
using ME.Libros.Repositorios;
using ME.Libros.Servicios.General;
using ME.Libros.Utils.Enums;
using ME.Libros.Web.Helpers;
using ME.Libros.Web.Models;

namespace ME.Libros.Web.Controllers
{
    public class VentaVigenteController : Controller
    {
        public VentaService VentaService { get; set; }
        public ClienteService ClienteService { get; set; }

        public VentaVigenteController()
        {
            var modelContainer = new ModelContainer();
            VentaService = new VentaService(new EntidadRepository<VentaDominio>(modelContainer));
            ClienteService = new ClienteService(new EntidadRepository<ClienteDominio>(modelContainer));
            ViewBag.Title = "Vigentes";
            ViewBag.MenuId = 20;
        }

        // GET: Todas
        public ActionResult Index()
        {
            ViewBag.Id = TempData["Id"];
            ViewBag.Mensaje = TempData["Mensaje"];

            var ventas = new List<VentaViewModel>();
            using (VentaService)
            {
                ventas.AddRange(VentaService.Listar()
                    .OrderByDescending(v => v.FechaVenta)
                    .ToList()
                    .Select(v => new VentaViewModel(v)));
            }

            return View(ventas);
        }

        public JsonResult ListarVentas(long idCliente)
        {

            var ventas = new List<VentaViewModel>();
            using (VentaService)
            {
                ventas.AddRange(VentaService.ListarAsQueryable()
                    .Where(v=>v.Cliente.Id==idCliente)
                    .OrderByDescending(v => v.FechaVenta)
                    .ToList()
                    .Select(v => new VentaViewModel(v)));
            }

            return new JsonResult
            {
                Data = ventas,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        public ActionResult Crear()
        {
            var ventaViewModel = new VentaViewModel();
            PrepareModel(ventaViewModel);

            return View(ventaViewModel);
        }

        [HttpPost]
        public ActionResult Crear(VentaViewModel ventaViewModel)
        {
            long resultado = 0;
            ModelState.RemoveFor<ClienteViewModel>(c => c.Localidad.Nombre);
            ModelState.RemoveFor<ClienteViewModel>(c => c.Localidad.ProvinciaId);
            ModelState.RemoveFor<ClienteViewModel>(c => c.Nombre);
            ModelState.RemoveFor<ClienteViewModel>(c => c.Apellido);
            ModelState.RemoveFor<ClienteViewModel>(c => c.Cuil);
            ModelState.RemoveFor<ClienteViewModel>(c => c.Direccion);
            ModelState.RemoveFor<ClienteViewModel>(c => c.Localidad.Id);
            ModelState.RemoveFor<ProductoViewModel>(p => p.Nombre);
            if (ModelState.IsValid)
            {
                try
                {
                    using (VentaService)
                    {
                        var ventaDominio = new VentaDominio
                        {
                            FechaAlta = DateTime.Now,
                            FechaVenta = ventaViewModel.FechaVenta,
                            FechaCobro = ventaViewModel.FechaCobro,
                            Cliente = ClienteService.GetPorId(ventaViewModel.ClienteId),
                            Estado = EstadoVenta.Vigente,
                        };
                        resultado = VentaService.Guardar(ventaDominio);
                        if (resultado <= 0)
                        {
                            foreach (var error in VentaService.ModelError)
                            {
                                ModelState.AddModelError(error.Key, error.Value);
                            }
                        }
                        else
                        {
                            TempData["Id"] = ventaDominio.Id;
                            TempData["Mensaje"] = string.Format(Messages.EntidadNueva, Messages.LaVenta, ventaDominio.Id);
                        }
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var error in ex.EntityValidationErrors.SelectMany(validationError => validationError.ValidationErrors))
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
                }
            }

            if (resultado == 0)
            {
                PrepareModel(ventaViewModel);
            }

            return resultado > 0
                ? (ActionResult)RedirectToAction("Index")
                : View(ventaViewModel);
        }

        public ActionResult Modificar()
        {
            var ventaViewModel = new VentaViewModel();
            PrepareModel(ventaViewModel);

            return View(ventaViewModel);
        }

        public JsonResult Eliminar()
        {
            return new JsonResult
            {
                Data = new { Success = ModelState.IsValid, Errors = ModelState.GetErrors() },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        #region Private Methods

        private void PrepareModel(VentaViewModel ventaViewModel)
        {
            //ventaViewModel.Clientes = new SelectList(ClienteService.Listar()
            //    .ToList()
            //    .Select(c => new ClienteViewModel(c)), "Id", "Nombre");
            ventaViewModel.Clientes = new SelectList(ClienteService.Listar()
                .ToList()
                .Select(c => new { Id = c.Id, Text = c.Id + " - " + c.Cuil }), "Id", "Text");
        }

        #endregion
    }
}