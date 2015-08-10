using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

using ME.Libros.EF;
using ME.Libros.Servicios.General;
using ME.Libros.Web.Extensions;
using ME.Libros.Web.Models;
using ME.Libros.Dominio.General;
using ME.Libros.Repositorios;

namespace ME.Libros.Web.Controllers
{
    public class ProveedorController : BaseController<ProveedorDominio>
    {
        public ProveedorService ProveedorService { get; set; }
        private ProvinciaService ProvinciaService { get; set; }
        private LocalidadService LocalidadService { get; set; }

        public ProveedorController()
        {
            var modelContainer = new ModelContainer();
            ProveedorService = new ProveedorService(new EntidadRepository<ProveedorDominio>(modelContainer));
            ProvinciaService = new ProvinciaService(new EntidadRepository<ProvinciaDominio>(modelContainer));
            LocalidadService = new LocalidadService(new EntidadRepository<LocalidadDominio>(modelContainer));
            ViewBag.MenuId = 10;
            ViewBag.Title = "Proveedores";
            Service = new ProveedorService(new EntidadRepository<ProveedorDominio>(modelContainer));
        }

        // GET: Proveedor
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Id = TempData["Id"];
            ViewBag.Mensaje = TempData["Mensaje"];

            var proveedores = new List<ProveedorViewModel>();
            using (ProveedorService)
            {
                proveedores.AddRange(ProveedorService.Listar()
                    .OrderBy(c => c.RazonSocial)
                    .ToList()
                    .Select(c => new ProveedorViewModel(c)));
            }

            return View(proveedores);
        }

        [HttpGet]
        public ActionResult Crear()
        {
            var proveedorViewModel = new ProveedorViewModel();
            PrepareModel(proveedorViewModel);

            return View(proveedorViewModel);
        }

        [HttpPost]
        public ActionResult Crear(ProveedorViewModel proveedorViewModel)
        {
            if (!ModelState.IsValid)
            {
                this.PrepareModel(proveedorViewModel);
                return View(proveedorViewModel);
            }

            long resultado = 0;
            try
            {
                using (ProveedorService)
                {
                    var proveedorDominio = new ProveedorDominio
                                             {
                                                 FechaAlta = DateTime.Now,
                                                 RazonSocial = proveedorViewModel.RazonSocial,
                                                 Cuil = proveedorViewModel.Cuil,
                                                 Direccion = proveedorViewModel.Direccion,
                                                 Email = proveedorViewModel.Email,
                                                 TelefonoFijo = proveedorViewModel.TelefonoFijo,
                                                 Celular = proveedorViewModel.Celular,
                                                 Localidad = LocalidadService.GetPorId(proveedorViewModel.LocalidadId),
                                             };
                    resultado = ProveedorService.Guardar(proveedorDominio);
                    if (resultado <= 0)
                    {
                        foreach (var error in ProveedorService.ModelError)
                        {
                            ModelState.AddModelError(error.Key, error.Value);
                        }
                    }
                    else
                    {
                        TempData["Id"] = proveedorDominio.Id;
                        TempData["Mensaje"] = string.Format(Messages.EntidadNueva, Messages.ElProveedor, proveedorDominio.Id);
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;
                if (sqlException != null && sqlException.Number == 2601)
                {
                    ModelState.AddModelError("Error", string.Format(ErrorMessages.CuilRepetido, proveedorViewModel.Cuil));
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

            if (resultado == 0)
            {
                PrepareModel(proveedorViewModel);
            }

            return resultado > 0
                ? (ActionResult)RedirectToAction("Index")
                : View(proveedorViewModel);
        }

        [HttpGet]
        public JsonResult Eliminar(int id)
        {
            try
            {
                using (ProveedorService)
                {
                    ProveedorService.Eliminar(ProveedorService.GetPorId(id));
                }
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;

                if (sqlException != null && sqlException.Number == 547)
                {
                    ModelState.AddModelError("Error", string.Format(ErrorMessages.DatosAsociados, Messages.ElProveedor));
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

            return new JsonResult
            {
                Data = new { Success = ModelState.IsValid, Errors = ModelState.GetErrors() },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult Modificar(int id)
        {
            ProveedorViewModel proveedorViewModel;
            using (ProveedorService)
            {
                proveedorViewModel = new ProveedorViewModel(ProveedorService.GetPorId(id));
            }
            PrepareModel(proveedorViewModel);

            return View(proveedorViewModel);
        }

        [HttpPost]
        public ActionResult Modificar(ProveedorViewModel proveedorViewModel)
        {
            if (!ModelState.IsValid)
            {
                PrepareModel(proveedorViewModel);
                return View(proveedorViewModel);
            }

            long resultado = 0;

            try
            {
                using (ProveedorService)
                {
                    var proveedorDominio = ProveedorService.GetPorId(proveedorViewModel.Id);
                    proveedorDominio.RazonSocial = proveedorViewModel.RazonSocial;
                    proveedorDominio.Cuil = proveedorViewModel.Cuil;
                    proveedorDominio.Direccion = proveedorViewModel.Direccion;
                    proveedorDominio.Localidad = LocalidadService.GetPorId(proveedorViewModel.LocalidadId);
                    proveedorDominio.TelefonoFijo = proveedorViewModel.TelefonoFijo;
                    proveedorDominio.Celular = proveedorViewModel.Celular;
                    proveedorDominio.Email = proveedorViewModel.Email;

                    resultado = ProveedorService.Guardar(proveedorDominio);
                    if (resultado <= 0)
                    {
                        foreach (var error in ProveedorService.ModelError)
                        {
                            ModelState.AddModelError(error.Key, error.Value);
                        }
                    }
                    else
                    {
                        TempData["Id"] = proveedorViewModel.Id;
                        TempData["Mensaje"] = string.Format(Messages.EntidadModificada, Messages.ElProveedor,
                            proveedorViewModel.Id);
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;
                if (sqlException != null && sqlException.Number == 2601)
                {
                    ModelState.AddModelError("Error",
                        string.Format(ErrorMessages.CuilRepetido, proveedorViewModel.Cuil));
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

            if (resultado == 0)
            {
                PrepareModel(proveedorViewModel);
            }

            return resultado > 0
                ? (ActionResult)RedirectToAction("Index")
                : View(proveedorViewModel);
        }

        #region Private Methods

        private void PrepareModel(ProveedorViewModel proveedorViewModel)
        {
            proveedorViewModel.Provincias = new SelectList(ProvinciaService.Listar()
                .Select(p => new ProvinciaViewModel(p))
                .ToList(), "Id", "Nombre");

            var localidades = new List<LocalidadViewModel>();
            if (proveedorViewModel.ProvinciaId > 0)
            {
                localidades.AddRange(LocalidadService.Listar(l => l.Provincia.Id == proveedorViewModel.ProvinciaId)
                                                                    .ToList()
                                                                    .Select(l => new LocalidadViewModel(l)));
            }
            proveedorViewModel.Localidades = new SelectList(localidades, "Id", "Nombre");
        }

        #endregion
    }
}