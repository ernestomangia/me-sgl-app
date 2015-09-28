using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ME.Libros.Dominio.General;
using ME.Libros.EF;
using ME.Libros.Repositorios;
using ME.Libros.Servicios.General;
using ME.Libros.Web.Extensions;
using ME.Libros.Web.Models;

namespace ME.Libros.Web.Controllers
{
    public class CompraItemController : Controller
    {
        //
        // GET: /CompraItem/
        public ProductoService ProductoService { get; set; }
        public CompraItemService CompraItemService { get; set; }

        public CompraItemController()
        {
            var modelContainer = new ModelContainer();
            ProductoService = new ProductoService(new EntidadRepository<ProductoDominio>(modelContainer));
            CompraItemService = new CompraItemService(new EntidadRepository<CompraItemDominio>(modelContainer));
        }

        // GET: VentaItem
        public PartialViewResult Index(List<CompraItemViewModel> compraItemViewModels)
        {
            return PartialView(compraItemViewModels);
        }

        [HttpPost]
        public ActionResult CrearItem(List<CompraItemViewModel> compraItemViewModels)
        {
            if (compraItemViewModels == null)
            {
                compraItemViewModels = new List<CompraItemViewModel>();
            }

            var productoIds = compraItemViewModels.Select(vi => vi.ProductoId).ToList();
            var productos = ProductoService.ListarAsQueryable()
                .Where(p => !productoIds.Contains(p.Id))
                .ToList()
                .Select(p => new ProductoViewModel(p));

            var compraViewModel = new CompraItemViewModel
            {
                Productos = new SelectList(productos, "Id", "Nombre")
            };

            return View("~/Views/CompraItem/Crear.cshtml", compraViewModel);
        }

        [HttpPost]
        public JsonResult Crear(CompraItemViewModel compraItemViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (ProductoService)
                    {
                        var productoDominio = ProductoService.GetPorId(compraItemViewModel.ProductoId);
                        compraItemViewModel.Producto = new ProductoViewModel(productoDominio);
                        compraItemViewModel.PrecioCostoAnterior = productoDominio.PrecioCosto;
                        compraItemViewModel.PrecioCostoComprado = compraItemViewModel.PrecioCostoComprado;
                        compraItemViewModel.MontoItemComprado = compraItemViewModel.MontoItemComprado;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
                }
            }

            return new JsonResult
            {
                Data =
                    new
                    {
                        Success = ModelState.IsValid,
                        Errors = ModelState.GetErrors(),
                        CompraItem = compraItemViewModel
                    }
            };
        }

        [HttpPost]
        public ActionResult ModificarItem(List<CompraItemViewModel> compraItemViewModels, int productoId)
        {
            // Listar IDs de los items ya agregados, exceptuar el que se esta modificando
            var productoIds = compraItemViewModels.Where(vi => vi.ProductoId != productoId)
                .Select(vi => vi.ProductoId)
                .ToList();
            var productos = ProductoService.ListarAsQueryable()
                .Where(p => !productoIds.Contains(p.Id))
                .ToList()
                .Select(p => new ProductoViewModel(p))
                .ToList();

            var compraItemViewModel = compraItemViewModels.First(vi => vi.ProductoId == productoId);
            compraItemViewModel.Productos = new SelectList(productos, "Id", "Nombre");
            compraItemViewModel.Producto = productos.First(p => p.Id == productoId);
            compraItemViewModel.CodigoBarra = compraItemViewModel.Producto.CodigoBarra;
            compraItemViewModel.PrecioCostoAnterior = compraItemViewModel.Producto.PrecioCosto;

            return View("Modificar", compraItemViewModel);
        }

        [HttpGet]
        public ActionResult Modificar(int id)
        {
            CompraItemViewModel compraItemViewModel;
            using (CompraItemService)
            {
                compraItemViewModel = new CompraItemViewModel(CompraItemService.GetPorId(id));
            }

            // Listar IDs de los items ya agregados, exceptuar el que se esta modificando
            var productos = ProductoService.ListarAsQueryable()
                .Where(p => p.Id == compraItemViewModel.ProductoId)
                .ToList()
                .Select(p => new ProductoViewModel(p))
                .ToList();

            compraItemViewModel.Productos = new SelectList(productos, "Id", "Nombre");
            return View(compraItemViewModel);
        }

        [HttpPost]
        public JsonResult Modificar(CompraItemViewModel compraItemViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (ProductoService)
                    {
                        var productoDominio = ProductoService.GetPorId(compraItemViewModel.ProductoId);
                        compraItemViewModel.Producto = new ProductoViewModel(productoDominio);
                        compraItemViewModel.PrecioCostoAnterior = productoDominio.PrecioCosto;
                        compraItemViewModel.PrecioCostoComprado = compraItemViewModel.PrecioCostoComprado;
                        compraItemViewModel.MontoItemComprado = compraItemViewModel.MontoItemComprado;
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
                    CompraItem = compraItemViewModel
                }
            };

        }
    }
}