using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using ME.Libros.Dominio.General;
using ME.Libros.EF;
using ME.Libros.Repositorios;
using ME.Libros.Servicios.General;
using ME.Libros.Web.Helpers;
using ME.Libros.Web.Models;

namespace ME.Libros.Web.Controllers
{
    public class VentaItemController : Controller
    {
        public ProductoService ProductoService { get; set; }

        public VentaItemController()
        {
            var modelContainer = new ModelContainer();
            ProductoService = new ProductoService(new EntidadRepository<ProductoDominio>(modelContainer));
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

            return View("~/Views/VentaItem/Crear.cshtml", ventaViewModel);
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
    }
}