﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;
using ME.Libros.EF;
using ME.Libros.Repositorios;
using ME.Libros.Servicios.General;
using ME.Libros.Web.Helpers;
using ME.Libros.Web.Models;

namespace ME.Libros.Web.Controllers
{
    public class ProductoController : BaseController<ProductoDominio>
    {
        public ProductoService ProductoService { get; set; }
        public EditorialService EditorialService { get; set; }
        public RubroService RubroService { get; set; }

        public ProductoController()
        {
            var modelConteiner = new ModelContainer();
            ProductoService = new ProductoService(new EntidadRepository<ProductoDominio>(modelConteiner));
            EditorialService = new EditorialService(new EntidadRepository<EditorialDominio>(modelConteiner));
            RubroService = new RubroService(new EntidadRepository<RubroDominio>(modelConteiner));
            ViewBag.MenuId = 6;
            Service = new ProductoService(new EntidadRepository<ProductoDominio>(modelConteiner));
            ViewBag.Title = "Productos";
        }

        //
        // GET: /Producto/
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Id = TempData["Id"];
            ViewBag.Mensaje = TempData["Mensaje"];

            var productos = new List<ProductoViewModel>();

            using (ProductoService)
            {
                productos.AddRange(ProductoService.Listar()
                    .ToList()
                    .Select(p => new ProductoViewModel(p)));
            }

            return View(productos);

        }

        [HttpGet]
        public ActionResult Crear()
        {
            var productoViewModel = new ProductoViewModel();
            PrepareModel(productoViewModel);

            return View(productoViewModel);
        }

        [HttpPost]
        public ActionResult Crear(ProductoViewModel productoViewModel)
        {
            ModelState.Remove("Rubro.Nombre");
            ModelState.Remove("Rubro.Descripcion");
            ModelState.Remove("Editorial.Nombre");
            ModelState.Remove("Editorial.Descripcion");

            long resultado = 0;
            if (ModelState.IsValid)
            {
                try
                {
                    using (ProductoService)
                    {
                        var productoDominio = new ProductoDominio
                        {
                            FechaAlta = DateTime.Now,
                            Nombre = productoViewModel.Nombre,
                            Descripcion = productoViewModel.Descripcion,
                            Stock = productoViewModel.Stock,
                            CodigoBarra = productoViewModel.CodigoBarra,
                            PrecioCosto = productoViewModel.PrecioCosto,
                            PrecioVenta = productoViewModel.PrecioVenta,

                            Rubro = RubroService.GetPorId(productoViewModel.Rubro.Id),
                            Editorial = EditorialService.GetPorId(productoViewModel.Editorial.Id),
                        };
                        resultado = ProductoService.Guardar(productoDominio);
                        if (resultado <= 0)
                        {
                            foreach (var error in ProductoService.ModelError)
                            {
                                ModelState.AddModelError(error.Key, error.Value);
                            }
                        }
                        else
                        {
                            TempData["Id"] = productoDominio.Id;
                            TempData["Mensaje"] = string.Format(Messages.EntidadNueva, "El producto", productoDominio.Id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
                }
            }

            PrepareModel(productoViewModel);

            return resultado > 0
                ? (ActionResult)RedirectToAction("Index")
                : View(productoViewModel);

        }

        [HttpGet]
        public JsonResult Eliminar(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (ProductoService)
                    {
                        ProductoService.Eliminar(ProductoService.GetPorId(id));
                    }
                }
                catch (DbUpdateException ex)
                {
                    var sqlException = ex.GetBaseException() as SqlException;

                    if (sqlException != null && sqlException.Number == 547)
                    {
                        ModelState.AddModelError("Error", ErrorMessages.DatosAsociados);
                    }
                    else
                    {
                        ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
                }
            }

            return new JsonResult
            {
                Data = new { Success = ModelState.IsValid, Errors = ModelState.GetErrors() },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult Modificar(int id)
        {
            ProductoViewModel productoViewModel;
            using (ProductoService)
            {
                productoViewModel = new ProductoViewModel(ProductoService.GetPorId(id));
            }
            PrepareModel(productoViewModel);

            return View(productoViewModel);
        }

        [HttpPost]
        public ActionResult Modificar(ProductoViewModel productoViewModel)
        {
            long resultado = 0;

            ModelState.Remove("Editorial.Nombre");
            ModelState.Remove("Rubro.Nombre");

            if (ModelState.IsValid)
            {
                using (ProductoService)
                {
                    var productoDominio = ProductoService.GetPorId(productoViewModel.Id);
                    productoDominio.Nombre = productoViewModel.Nombre;
                    productoDominio.Descripcion = productoViewModel.Descripcion;
                    productoDominio.Stock = productoViewModel.Stock;
                    productoDominio.PrecioCosto = productoViewModel.PrecioCosto;
                    productoDominio.PrecioVenta = productoViewModel.PrecioVenta;
                    productoDominio.Rubro = RubroService.GetPorId(productoViewModel.Rubro.Id);
                    productoDominio.Editorial = EditorialService.GetPorId((productoViewModel.Editorial.Id));

                    resultado = ProductoService.Guardar(productoDominio);
                    if (resultado <= 0)
                    {
                        foreach (var error in ProductoService.ModelError)
                        {
                            ModelState.AddModelError(error.Key, error.Value);
                        }
                    }
                    else
                    {
                        TempData["Id"] = productoDominio.Id;
                        TempData["Mensaje"] = string.Format(Messages.EntidadModificada, "El producto", productoDominio.Id);
                    }
                }
            }

            PrepareModel(productoViewModel);

            return resultado > 0
                ? (ActionResult)RedirectToAction("Index")
                : View(productoViewModel);
        }

        [HttpGet]
        public ActionResult Detalle(int id)
        {
            ProductoViewModel productoViewModel;
            using (ProductoService)
            {
                productoViewModel = new ProductoViewModel(ProductoService.GetPorId(id));
            }

            PrepareModel(productoViewModel);

            return View(productoViewModel);
        }

 
        #region Private Methods

        private void PrepareModel(ProductoViewModel productoViewModel)
        {
            productoViewModel.Editoriales = new SelectList(EditorialService.Listar()
                .Select(e => new EditorialViewModel(e))
                .ToList(), "Id", "Nombre");

            productoViewModel.Rubros = new SelectList(RubroService.Listar()
                .Select(r => new RubroViewModel(r))
                .ToList(), "Id", "Nombre");
        }
        #endregion

    }
}