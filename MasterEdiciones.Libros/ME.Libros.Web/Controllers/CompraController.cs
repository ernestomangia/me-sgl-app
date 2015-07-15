using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
    public class CompraController : Controller
    {
        public CompraService CompraService { get; set; }
        public ProveedorService ProveedorService { get; set; }
        public ProductoService ProductoService { get; set; }

        public CompraController()
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
                            view = "~/Views/Compra/CompraPagada/Index.cshtml";
                            break;
                        case EstadoCompra.Anulada:
                            ViewBag.Title = "Anuladas";
                            ViewBag.MenuId = 28;
                            view = "~/Views/Compra/CompraAnulada/Index.cshtml";
                            break;

                    }
                }
            }

            return View(view, compras);
        }

        public ActionResult Crear()
        {
            var compraViewModel = new CompraViewModel();
            PrepareModel(compraViewModel);

            return View(compraViewModel);
        }

        [HttpPost]
        public ActionResult Crear(CompraViewModel compraViewModel)
        {
            if (!ModelState.IsValid)
            {
                PrepareModel(compraViewModel);
              //  SetMenuVigente();
                return View(compraViewModel);
            }

            long resultado = 0;
            try
            {
                using (CompraService)
                {
                    var compraDominio = new CompraDominio
                    {
                        FechaAlta = DateTime.Now,
                        FechaCompra = compraViewModel.FechaCompra,
                        Proveedor = ProveedorService.GetPorId(compraViewModel.ProveedorId),
                        Estado = EstadoCompra.Pagada,
                        MontoComprado = compraViewModel.MontoComprado,
                        MontoCalculado = compraViewModel.MontoCalculado,
                        NroFactura = compraViewModel.NroFactura,
                        NroRemito = compraViewModel.NroRemito
                       // VentaItems = new List<VentaItemDominio>(),
                    };

                    //foreach (var ventaItemViewModel in compraViewModel.Items)
                    //{
                    //    var producto = ProductoService.GetPorId(ventaItemViewModel.ProductoId);
                    //    compraDominio.VentaItems.Add(new VentaItemDominio
                    //    {
                    //        FechaAlta = DateTime.Now,
                    //        Cantidad = ventaItemViewModel.Cantidad,
                    //        Producto = producto,
                    //        PrecioVentaVendido = ventaItemViewModel.PrecioVentaVendido,
                    //        MontoVendido = ventaItemViewModel.MontoItemVendido
                    //    });
                    //}

                    resultado = CompraService.Guardar(compraDominio);
                    if (resultado <= 0)
                    {
                        foreach (var error in CompraService.ModelError)
                        {
                            ModelState.AddModelError(error.Key, error.Value);
                        }
                    }
                    else
                    {
                        TempData["Id"] = compraDominio.Id;
                        TempData["Mensaje"] = string.Format(Messages.EntidadNueva, Messages.LaCompra, compraDominio.Id);
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

            PrepareModel(compraViewModel);
       //     SetMenuVigente();
            return View(compraViewModel);
        }

        #region Private Methods

        private void PrepareModel(CompraViewModel compraViewModel)
        {
            compraViewModel.Proveedores = new SelectList(ProveedorService.Listar()
                .ToList()
                .Select(v => new { Id = v.Id, Text = v.Id + " - " + v.Cuil }), "Id", "Text");
        }

        #endregion

    }
}