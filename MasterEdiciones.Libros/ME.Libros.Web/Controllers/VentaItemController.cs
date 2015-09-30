using System;
using System.Collections.Generic;
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
    public class VentaItemController : Controller
    {
        public ProductoService ProductoService { get; set; }
        public VentaItemService VentaItemService { get; set; }

        public VentaItemController()
        {
            var modelContainer = new ModelContainer();
            ProductoService = new ProductoService(new EntidadRepository<ProductoDominio>(modelContainer));
            VentaItemService = new VentaItemService(new EntidadRepository<VentaItemDominio>(modelContainer));
        }

        // GET: VentaItem
        public PartialViewResult Index(List<VentaItemViewModel> ventaItemViewModels)
        {
            return PartialView(ventaItemViewModels);
        }

        [HttpPost]
        public ActionResult CrearItem(List<VentaItemViewModel> ventaItemViewModels)
        {
            if (ventaItemViewModels == null)
            {
                ventaItemViewModels = new List<VentaItemViewModel>();
            }

            var productoIds = ventaItemViewModels.Select(vi => vi.ProductoId).ToList();
            var productos = ProductoService.ListarAsQueryable()
                .Where(p => !productoIds.Contains(p.Id))
                .ToList()
                .Select(p => new ProductoViewModel(p));

            var ventaViewModel = new VentaItemViewModel
            {
                Productos = new SelectList(productos, "Id", "Nombre")
            };

            return View("Crear", ventaViewModel);
        }

        [HttpPost]
        public JsonResult Crear(VentaItemViewModel ventaItemViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (ProductoService)
                    {
                        var productoDominio = ProductoService.GetPorId(ventaItemViewModel.ProductoId);
                        ventaItemViewModel.Producto = new ProductoViewModel(productoDominio);
                        ventaItemViewModel.PrecioCosto = ventaItemViewModel.Producto.PrecioCosto;
                        ventaItemViewModel.PrecioVentaCalculado = productoDominio.PrecioVenta;
                        ventaItemViewModel.PrecioVentaVendido = ventaItemViewModel.PrecioVentaVendido;
                        ventaItemViewModel.MontoItemCalculado = ventaItemViewModel.Cantidad * productoDominio.PrecioVenta;
                        ventaItemViewModel.MontoItemVendido = ventaItemViewModel.MontoItemVendido;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
                }
            }

            return new JsonResult
            {
                Data = new { Success = ModelState.IsValid, Errors = ModelState.GetErrors(), VentaItem = ventaItemViewModel }
            };
        }

        // Dibuja el modal para modificar item cuando el usuario esta creando la venta
        [HttpPost]
        public ActionResult ModificarItem(List<VentaItemViewModel> ventaItemViewModels, int itemIndex)
        {
            // Buscar ProductoId a modificar
            var productoId = ventaItemViewModels.First(vi => vi.Orden - 1 == itemIndex).ProductoId;

            // Listar IDs de los items ya agregados, exceptuar el que se esta modificando
            var productoIdsAgregados = ventaItemViewModels.Where(vi => vi.ProductoId != productoId)
                .Select(vi => vi.ProductoId)
                .ToList();
            var productos = ProductoService.ListarAsQueryable()
                .Where(p => !productoIdsAgregados.Contains(p.Id))
                .ToList()
                .Select(p => new ProductoViewModel(p))
                .ToList();

            var ventaItemViewModel = ventaItemViewModels.First(vi => vi.ProductoId == productoId);
            ventaItemViewModel.Productos = new SelectList(productos, "Id", "Nombre");
            ventaItemViewModel.Producto = productos.First(p => p.Id == productoId);
            ventaItemViewModel.CodigoBarra = ventaItemViewModel.Producto.CodigoBarra;
            ventaItemViewModel.PrecioVentaCalculado = ventaItemViewModel.Producto.PrecioVenta;
            ventaItemViewModel.MontoItemCalculado = ventaItemViewModel.PrecioVentaCalculado * ventaItemViewModel.Cantidad;

            return View("Modificar", ventaItemViewModel);
        }

        // Dibuja el modal para modificar item cuando el la venta ya esta creada
        [HttpGet]
        public ActionResult Modificar(int id)
        {
            VentaItemViewModel ventaItemViewModel;
            using (VentaItemService)
            {
                ventaItemViewModel = new VentaItemViewModel(VentaItemService.GetPorId(id));
            }

            // Listar IDs de los items ya agregados, exceptuar el que se esta modificando
            var productos = ProductoService.ListarAsQueryable()
                .Where(p => p.Id == ventaItemViewModel.ProductoId)
                .ToList()
                .Select(p => new ProductoViewModel(p))
                .ToList();

            ventaItemViewModel.Productos = new SelectList(productos, "Id", "Nombre");
            return View(ventaItemViewModel);
        }

        [HttpPost]
        public JsonResult Modificar(VentaItemViewModel ventaItemViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (ProductoService)
                    {
                        var productoDominio = ProductoService.GetPorId(ventaItemViewModel.ProductoId);
                        ventaItemViewModel.Producto = new ProductoViewModel(productoDominio);
                        ventaItemViewModel.PrecioCosto = ventaItemViewModel.Producto.PrecioCosto;
                        ventaItemViewModel.PrecioVentaCalculado = productoDominio.PrecioVenta;
                        ventaItemViewModel.PrecioVentaVendido = ventaItemViewModel.PrecioVentaVendido;
                        ventaItemViewModel.MontoItemCalculado = ventaItemViewModel.Cantidad * productoDominio.PrecioVenta;
                        ventaItemViewModel.MontoItemVendido = ventaItemViewModel.MontoItemVendido;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
                }
            }

            return new JsonResult
            {
                Data = new
                {
                    Success = ModelState.IsValid,
                    Errors = ModelState.GetErrors(),
                    VentaItem = ventaItemViewModel
                }
            };
        }
    }
}