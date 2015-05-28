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
        public PartialViewResult CrearItem(List<VentaItemViewModel> ventaItemViewModels)
        {
            var productoIds = ventaItemViewModels.Select(vi => vi.ProductoId).ToList();
            var productos = ProductoService.ListarAsQueryable()
                                            .Where(p => !productoIds.Contains(p.Id))
                                            .ToList();

            var ventaViewModel = new VentaItemViewModel
            {
                Productos = new SelectList(productos, "Id", "Nombre")
            };

            return PartialView("~/Views/VentaItem/Crear.cshtml", ventaViewModel);
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

                        //Validar stock
                        if (!ProductoService.VerificarStock(productoDominio, ventaItemViewModel.Cantidad))
                        {
                            foreach (var error in ProductoService.ModelError)
                            {
                                ModelState.AddModelError(error.Key, error.Value);
                            }
                        }
                        else
                        {
                            ventaItemViewModel.Producto = new ProductoViewModel(productoDominio);
                            ventaItemViewModel.PrecioVenta = ventaItemViewModel.Producto.PrecioVenta;
                            ventaItemViewModel.Monto = ventaItemViewModel.Cantidad * ventaItemViewModel.Producto.PrecioVenta;
                        }
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
                //JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}