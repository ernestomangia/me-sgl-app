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
    public class VentaController : Controller
    {
        public VentaService VentaService { get; set; }
        public ClienteService ClienteService { get; set; }
        public CobradorService CobradorService { get; set; }
        public VendedorService VendedorService { get; set; }
        public ProductoService ProductoService { get; set; }
        public PlanPagoService PlanPagoService { get; set; }

        public VentaController()
        {
            var modelContainer = new ModelContainer();
            ProductoService = new ProductoService(new EntidadRepository<ProductoDominio>(modelContainer));
            VentaService = new VentaService(new EntidadRepository<VentaDominio>(modelContainer), ProductoService);
            ClienteService = new ClienteService(new EntidadRepository<ClienteDominio>(modelContainer));
            CobradorService = new CobradorService(new EntidadRepository<CobradorDominio>(modelContainer));
            VendedorService = new VendedorService(new EntidadRepository<VendedorDominio>(modelContainer));
            PlanPagoService = new PlanPagoService(new EntidadRepository<PlanPagoDominio>(modelContainer));
        }

        // GET: Todas
        // [Route("Venta/{estado?}")]
        public ActionResult Index(EstadoVenta? estado)
        {
            ViewBag.Id = TempData["Id"];
            ViewBag.Mensaje = TempData["Mensaje"];

            var ventas = new List<VentaViewModel>();
            var subFolder = "";
            var title = "Todas";
            var menuId = 23;
            using (VentaService)
            {
                if (estado == null)
                {
                    // Listar todas
                    ventas.AddRange(VentaService.ListarAsQueryable()
                    .OrderByDescending(v => v.FechaVenta)
                    .ThenByDescending(v => v.Id)
                    .ToList()
                    .Select(v => new VentaViewModel(v)));

                    Session.Add("MenuTodas", true);
                }
                else
                {
                    Session.Remove("MenuTodas");
                    ventas.AddRange(VentaService.ListarAsQueryable()
                    .Where(v => v.Estado == estado)
                    .OrderByDescending(v => v.FechaVenta)
                    .ThenByDescending(v => v.Id)
                    .ToList()
                    .Select(v => new VentaViewModel(v)));

                    switch (estado)
                    {
                        case EstadoVenta.Vigente:
                            title = "Vigentes";
                            menuId = 20;
                            subFolder = "Vigente";
                            break;
                        case EstadoVenta.Pagada:
                            title = "Pagadas";
                            menuId = 21;
                            subFolder = "Pagada";
                            break;
                        case EstadoVenta.Anulada:
                            title = "Anuladas";
                            menuId = 22;
                            subFolder = "Anulada";
                            break;

                    }
                    subFolder += "/";
                }
            }
            ViewBag.Title = title;
            ViewBag.MenuId = menuId;

            return View(subFolder + "Index", ventas);
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

        public ActionResult Crear()
        {
            var ventaViewModel = new VentaViewModel();
            PrepareModel(ventaViewModel);
            SetMenuVigente();

            return View(ventaViewModel);
        }

        [HttpPost]
        public ActionResult Crear(VentaViewModel ventaViewModel)
        {
            if (!ModelState.IsValid)
            {
                PrepareModel(ventaViewModel);
                SetMenuVigente();
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
                        Vendedor = VendedorService.GetPorId(ventaViewModel.VendedorId),
                        Estado = EstadoVenta.Vigente,
                        MontoVendido = ventaViewModel.MontoVendido,
                        Saldo = ventaViewModel.MontoVendido,
                        PlanPago = PlanPagoService.GetPorId(ventaViewModel.PlanPagoId),
                        VentaItems = new List<VentaItemDominio>(),
                        Cuotas = new List<CuotaDominio>()
                    };

                    foreach (var ventaItemViewModel in ventaViewModel.Items)
                    {
                        var producto = ProductoService.GetPorId(ventaItemViewModel.ProductoId);
                        ventaDominio.VentaItems.Add(new VentaItemDominio
                        {
                            FechaAlta = DateTime.Now,
                            Cantidad = ventaItemViewModel.Cantidad,
                            Producto = producto,
                            PrecioVentaVendido = ventaItemViewModel.PrecioVentaVendido,
                            MontoVendido = ventaItemViewModel.MontoItemVendido
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
            SetMenuVigente();
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

            if (Session["MenuTodas"] == null)
            {
                switch (ventaViewModel.Estado)
                {
                    case EstadoVenta.Vigente:
                        SetMenuVigente();
                        break;
                    case EstadoVenta.Pagada:
                        ViewBag.Title = "Pagadas";
                        ViewBag.MenuId = 21;
                        break;
                    case EstadoVenta.Anulada:
                        ViewBag.Title = "Anuladas";
                        ViewBag.MenuId = 22;
                        break;
                }
            }
            else
            {
                ViewBag.Title = "Todas";
                ViewBag.MenuId = 23;
            }

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

        public PartialViewResult VerCuotas(List<CuotaViewModel> cuotaViewModels)
        {
            return PartialView(cuotaViewModels);
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
                .Select(v => new { Id = v.Id, Text = v.Id + " - " + v.Dni }), "Id", "Text");

            ventaViewModel.PlanesPago = new SelectList(PlanPagoService.Listar()
                .ToList()
                .Select(p => new PlanPagoViewModel(p)), "Id", "Nombre");
        }

        private void SetMenuVigente()
        {
            ViewBag.Title = "Vigentes";
            ViewBag.MenuId = 20;
        }

        #endregion
    }
}