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
        public CobradorService CobradorService { get; set; }
        public VendedorService VendedorService { get; set; }
        public ProductoService ProductoService { get; set; }

        public VentaVigenteController()
        {
            var modelContainer = new ModelContainer();
            ProductoService = new ProductoService(new EntidadRepository<ProductoDominio>(modelContainer));
            VentaService = new VentaService(new EntidadRepository<VentaDominio>(modelContainer), ProductoService);
            ClienteService = new ClienteService(new EntidadRepository<ClienteDominio>(modelContainer));
            CobradorService = new CobradorService(new EntidadRepository<CobradorDominio>(modelContainer));
            VendedorService = new VendedorService(new EntidadRepository<VendedorDominio>(modelContainer));
            ViewBag.Title = "Vigentes";
            ViewBag.MenuId = 20;
        }

        // GET: Todas
        public ActionResult Index(EstadoVenta? estado)
        {
            ViewBag.Id = TempData["Id"];
            ViewBag.Mensaje = TempData["Mensaje"];

            var ventas = new List<VentaViewModel>();
            var view = "";
            using (VentaService)
            {
                if (estado == null)
                {
                    ventas.AddRange(VentaService.ListarAsQueryable()
                    .OrderByDescending(v => v.FechaVenta)
                    .ThenByDescending(v => v.Id)
                    .ToList()
                    .Select(v => new VentaViewModel(v)));
                    ViewBag.Title = "Todas";
                    ViewBag.MenuId = 23;
                    view = "~/Views/Venta/Index.cshtml";
                }
                else
                {
                    ventas.AddRange(VentaService.ListarAsQueryable()
                    .Where(v => v.Estado == estado)
                    .OrderByDescending(v => v.FechaVenta)
                    .ThenByDescending(v => v.Id)
                    .ToList()
                    .Select(v => new VentaViewModel(v)));

                    switch (estado)
                    {
                        case EstadoVenta.Vigente:
                            ViewBag.Title = "Vigentes";
                            ViewBag.MenuId = 20;
                            view = "~/Views/VentaVigente/Index.cshtml";
                            break;
                        case EstadoVenta.Pagada:
                            ViewBag.Title = "Pagadas";
                            ViewBag.MenuId = 21;
                            view = "~/Views/VentaPagada/Index.cshtml";
                            break;
                        case EstadoVenta.Anulada:
                            ViewBag.Title = "Anuladas";
                            ViewBag.MenuId = 22;
                            view = "~/Views/VentaAnulada/Index.cshtml";
                            break;
                        
                    }
                }
            }

            return View(view, ventas);
        }

        public JsonResult ListarVentas(long idCliente)
        {
            var ventas = new List<VentaViewModel>();
            using (VentaService)
            {
                ventas.AddRange(VentaService.ListarAsQueryable()
                    .Where(v => v.Cliente.Id == idCliente)
                    .OrderByDescending(v => v.FechaVenta)
                    .ToList()
                    .Select(v => new VentaViewModel(v)));

                return new JsonResult
                {
                    Data = ventas,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        public ActionResult IndexEstado(EstadoVenta estado)
        {
            ViewBag.Id = TempData["Id"];
            ViewBag.Mensaje = TempData["Mensaje"];

            var ventas = new List<VentaViewModel>();
            using (VentaService)
            {
                ventas.AddRange(VentaService.ListarAsQueryable()
                    .Where(v => v.Estado == estado)
                    .OrderByDescending(v => v.FechaVenta)
                    .ToList()
                    .Select(v => new VentaViewModel(v)));
            }

            switch (estado)
            {
                case EstadoVenta.Vigente:
                    {
                        ViewBag.Title = "Vigentes";
                        ViewBag.MenuId = 20;
                        return View("Index", ventas);
                    }
            }
            return View("Index", ventas);
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
            if (!ModelState.IsValid)
            {
                PrepareModel(ventaViewModel);
                return View(ventaViewModel);
            }

            long resultado = 0;
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
                        Cobrador = CobradorService.GetPorId(ventaViewModel.CobradorId),
                        Estado = EstadoVenta.Vigente,
                        MontoVendido = ventaViewModel.MontoVendido,
                        Saldo = ventaViewModel.MontoVendido,
                        VentaItems = new List<VentaItemDominio>(),
                    };

                    foreach (var ventaItemViewModel in ventaViewModel.Items)
                    {
                        var producto = ProductoService.GetPorId(ventaItemViewModel.ProductoId);
                        ventaDominio.VentaItems.Add(new VentaItemDominio
                        {
                            FechaAlta = DateTime.Now,
                            Cantidad = ventaItemViewModel.Cantidad,
                            Producto = producto,
                            PrecioVenta = ventaItemViewModel.PrecioVentaVendido,
                            PrecioCosto = producto.PrecioCosto,
                            Monto = ventaItemViewModel.MontoItemVendido
                        });
                    }

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

            if (resultado > 0)
            {
                return RedirectToAction("Index");
            }

            PrepareModel(ventaViewModel);
            return View(ventaViewModel);
        }

        [HttpGet]
        public ActionResult Modificar(int id)
        {
            VentaViewModel ventaViewModel;
            using (VentaService)
            {
                ventaViewModel = new VentaViewModel(VentaService.GetPorId(id));
            }
            PrepareModel(ventaViewModel);

            return View(ventaViewModel);
        }

        [HttpGet]
        public JsonResult Eliminar(int id)
        {
            try
            {
                using (VentaService)
                {
                    VentaService.AnularVenta(id);
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

        #region Private Methods

        private void PrepareModel(VentaViewModel ventaViewModel)
        {
            ventaViewModel.Clientes = new SelectList(ClienteService.Listar()
                .ToList()
                .Select(c => new { Id = c.Id, Text = c.Id + " - " + c.Cuil }), "Id", "Text");

            ventaViewModel.Cobradores = new SelectList(CobradorService.Listar()
                .ToList()
                .Select(c => new { Id = c.Id, Text = c.Id + " - " + c.Dni }), "Id", "Text");

            ventaViewModel.Vendedores = new SelectList(VendedorService.Listar()
                .ToList()
                .Select(v => new { Id = v.Id, Text = v.Id + " - " + v.Cuil }), "Id", "Text");
        }

        #endregion
    }
}