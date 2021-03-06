﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using ME.Libros.Dominio.General;
using ME.Libros.EF;
using ME.Libros.Repositorios;
using ME.Libros.Servicios.General;
using ME.Libros.Utils.Enums;
using ME.Libros.Web.Extensions;
using ME.Libros.Web.Models;

namespace ME.Libros.Web.Controllers
{
    public class CompraController : BaseController<CompraDominio>
    {
        public CompraService CompraService { get; set; }
        public ProveedorService ProveedorService { get; set; }
        public ProductoService ProductoService { get; set; }
        public List<MenuViewModel> SubMenues { get; set; }

        public CompraController()
        {
            var modelContainer = new ModelContainer();
            ProductoService = new ProductoService(new EntidadRepository<ProductoDominio>(modelContainer));
            CompraService = new CompraService(new EntidadRepository<CompraDominio>(modelContainer), ProductoService);
            ProveedorService = new ProveedorService(new EntidadRepository<ProveedorDominio>(modelContainer));
            SubMenues = NavigationBarViewModel.MenuViewModels.First(x => x.Controller.Equals("Compra")).Hijos;
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
                    Session.Add("MenuTodasCompra", true);

                    compras.AddRange(CompraService.ListarAsQueryable()
                    .OrderByDescending(c => c.FechaCompra)
                    .ThenByDescending(c => c.Id)
                    .ToList()
                    .Select(c => new CompraViewModel(c)));

                    SetMenuTodas();
                    view = "~/Views/Compra/Index.cshtml";
                }
                else
                {
                    Session.Remove("MenuTodasCompra");
                    compras.AddRange(CompraService.ListarAsQueryable()
                    .Where(c => c.Estado == estado)
                    .OrderByDescending(c => c.FechaCompra)
                    .ThenByDescending(c => c.Id)
                    .ToList()
                    .Select(c => new CompraViewModel(c)));

                    switch (estado)
                    {
                        case EstadoCompra.Pagada:
                            SetMenuPagada();
                            view = "~/Views/Compra/CompraPagada/Index.cshtml";
                            break;
                        case EstadoCompra.Anulada:
                            SetMenuAnulada();
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
            SetMenuPagada();
            return View(compraViewModel);
        }

        [HttpPost]
        public ActionResult Crear(CompraViewModel compraViewModel)
        {
            if (!ModelState.IsValid)
            {
                PrepareModel(compraViewModel);
                SetMenuPagada();
                return View(compraViewModel);
            }

            var compraDominio = new CompraDominio
            {
                FechaAlta = DateTime.Now,
                FechaCompra = compraViewModel.FechaCompra,
                Proveedor = ProveedorService.GetPorId(compraViewModel.ProveedorId),
                Estado = EstadoCompra.Pagada,
                MontoComprado = compraViewModel.MontoComprado,
                MontoCalculado = compraViewModel.MontoCalculado,
                NroFactura = compraViewModel.NroFactura,
                NroRemito = compraViewModel.NroRemito,
                CompraItems = new List<CompraItemDominio>(),
            };

            var i = 1;
            foreach (var compraItemViewModel in compraViewModel.Items)
            {
                var producto = ProductoService.GetPorId(compraItemViewModel.ProductoId);
                compraDominio.CompraItems.Add(new CompraItemDominio
                {
                    FechaAlta = DateTime.Now,
                    Orden = i++,
                    Producto = producto,
                    Cantidad = compraItemViewModel.Cantidad,
                    PrecioCostoComprado = compraItemViewModel.PrecioCostoComprado,
                    MontoComprado = compraItemViewModel.MontoItemComprado
                });
            }

            long resultado = 0;
            try
            {
                using (CompraService)
                {
                    resultado = CompraService.Guardar(compraDominio);
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

            if (resultado > 0)
            {
                return RedirectToAction("Index", new { estado = EstadoCompra.Pagada });
            }

            PrepareModel(compraViewModel);
            SetMenuPagada();
            return View(compraViewModel);
        }

        [HttpGet]
        public ActionResult Modificar(int id)
        {
            CompraViewModel compraViewModel;
            using (CompraService)
            {
                compraViewModel = new CompraViewModel(CompraService.GetPorId(id));
            }
            PrepareModel(compraViewModel);
            SetMenu(compraViewModel.Estado);
            return View(compraViewModel);
        }

        [HttpPost]
        public ActionResult Modificar(CompraViewModel compraViewModel)
        {
            if (!ModelState.IsValid)
            {
                PrepareModel(compraViewModel);
                SetMenu(compraViewModel.Estado);
                return View(compraViewModel);
            }

            long resultado = 0;
            try
            {
                using (CompraService)
                {
                    var compraDominio = CompraService.GetPorId(compraViewModel.Id);
                    compraDominio.NroFactura = compraViewModel.NroFactura;
                    compraDominio.NroRemito = compraViewModel.NroRemito;

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
                        TempData["Mensaje"] = string.Format(Messages.EntidadModificada, Messages.LaCompra, compraDominio.Id);
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
                // Volver al menu desde donde se abrio el modificar
                return RedirectToAction("Index", new
                {
                    estado = Session["MenuTodasCompra"] == null
                        ? compraViewModel.Estado
                        : (EstadoCompra?)null
                });
            }

            PrepareModel(compraViewModel);
            SetMenu(compraViewModel.Estado);
            return View(compraViewModel);
        }

        [HttpGet]
        public JsonResult Eliminar(int id, string redirectUrl)
        {
            var isRedirect = !string.IsNullOrEmpty(redirectUrl);

            try
            {
                using (CompraService)
                {
                    CompraService.AnularCompra(id);

                    if (isRedirect)
                    {
                        var compraDominio = CompraService.GetPorId(id);
                        TempData["Id"] = compraDominio.Id;
                        TempData["Mensaje"] = string.Format(Messages.EntidadAnulada, Messages.LaCompra, compraDominio.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
            }

            return new JsonResult
            {
                Data = new
                {
                    Success = ModelState.IsValid,
                    Errors = ModelState.GetErrors(),
                    isRedirect,
                    redirectUrl
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        #region Private Methods

        private void PrepareModel(CompraViewModel compraViewModel)
        {
            if (compraViewModel.Estado == EstadoCompra.None)
            {
                var i = 1;
                foreach (var item in compraViewModel.Items)
                {
                    item.Producto = new ProductoViewModel(ProductoService.GetPorId(item.ProductoId));
                    item.Orden = i++;
                }
            }
        }

        private void SetMenu(EstadoCompra estadoCompra)
        {
            if (Session["MenuTodasCompra"] == null)
            {
                switch (estadoCompra)
                {
                    case EstadoCompra.Pagada:
                        SetMenuPagada();
                        break;
                    case EstadoCompra.Anulada:
                        SetMenuAnulada();
                        break;
                }

                ViewBag.Estado = estadoCompra;
            }
            else
            {
                SetMenuTodas();
            }
        }

        private void SetMenuPagada()
        {
            NavigationBarViewModel.Title = "Pagadas";
            SubMenues.First(x => x.Nombre.Equals("Pagadas")).Seleccionado = true;
        }

        private void SetMenuAnulada()
        {
            NavigationBarViewModel.Title = "Anuladas";
            SubMenues.First(x => x.Nombre.Equals("Anuladas")).Seleccionado = true;
        }

        private void SetMenuTodas()
        {
            NavigationBarViewModel.Title = "Todas";
            SubMenues.First(x => x.Nombre.Equals("Todas")).Seleccionado = true;
        }

        #endregion
    }
}