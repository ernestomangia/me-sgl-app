using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ME.Libros.Dominio.General;
using ME.Libros.EF;
using ME.Libros.Repositorios;
using ME.Libros.Servicios.General;
using ME.Libros.Web.Models;

namespace ME.Libros.Web.Controllers
{
    public class CompraItemController : Controller
    {
        //
        // GET: /CompraItem/
        public ProductoService ProductoService { get; set; }

        public CompraItemController()
        {
            var modelContainer = new ModelContainer();
            ProductoService = new ProductoService(new EntidadRepository<ProductoDominio>(modelContainer));
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
	}
}