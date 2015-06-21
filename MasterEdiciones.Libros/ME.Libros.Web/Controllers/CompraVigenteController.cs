using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ME.Libros.Dominio.General;
using ME.Libros.EF;
using ME.Libros.Repositorios;
using ME.Libros.Servicios.General;
using ME.Libros.Utils.Enums;
using ME.Libros.Web.Models;

namespace ME.Libros.Web.Controllers
{
    public class CompraVigenteController : Controller
    {
        public CompraService CompraService { get; set; }
        public ProveedorService ProveedorService { get; set; }
        public ProductoService ProductoService { get; set; }

        public CompraVigenteController()
        {
            var modelContainer = new ModelContainer();
            ProductoService = new ProductoService(new EntidadRepository<ProductoDominio>(modelContainer));
            CompraService = new CompraService(new EntidadRepository<CompraDominio>(modelContainer), ProductoService);
            ProveedorService = new ProveedorService(new EntidadRepository<ProveedorDominio>(modelContainer));
            ViewBag.Title = "Pagadas";
            ViewBag.MenuId = 27;
        }

        //
        // GET: /CompraVigente/
        public ActionResult Index(EstadoCompra? estado)
        {
            ViewBag.Id = TempData["Id"];
            ViewBag.Mensaje = TempData["Mensaje"];

            var compras = new List<CompraViewModel>();
            var view = "";
            using (CompraService)
            {
                if (estado == null)
                {
                    compras.AddRange(CompraService.ListarAsQueryable()
                    .OrderByDescending(c => c.FechaCompra)
                    .ThenByDescending(c => c.Id)
                    .ToList()
                    .Select(c => new CompraViewModel(c)));
                    ViewBag.Title = "Todas";
                    ViewBag.MenuId = 26;
                    view = "~/Views/Compra/Index.cshtml";
                }
                else
                {
                    compras.AddRange(CompraService.ListarAsQueryable()
                    .Where(c => c.Estado == estado)
                    .OrderByDescending(c => c.FechaCompra)
                    .ThenByDescending(c => c.Id)
                    .ToList()
                    .Select(c => new CompraViewModel(c)));

                    switch (estado)
                    {
                        case EstadoCompra.Pagada:
                            ViewBag.Title = "Pagadas";
                            ViewBag.MenuId = 27;
                            view = "~/Views/CompraPagada/Index.cshtml";
                            break;
                        case EstadoCompra.Anulada:
                            ViewBag.Title = "Anuladas";
                            ViewBag.MenuId = 28;
                            view = "~/Views/CompraAnulada/Index.cshtml";
                            break;

                    }
                }
            }

            return View(view, compras);
        }

	}
}