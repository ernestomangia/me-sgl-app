﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

using ME.Libros.Dominio.General;
using ME.Libros.EF;
using ME.Libros.Repositorios;
using ME.Libros.Servicios.General;
using ME.Libros.Utils.Enums;
using ME.Libros.Web.Helpers;
using ME.Libros.Web.Models;

namespace ME.Libros.Web.Controllers
{
    public class VendedorController : BaseController<VendedorDominio>
    {
        public VendedorService VendedorService { get; set; }
        private LocalidadService LocalidadService { get; set; }
        private ProvinciaService ProvinciaService { get; set; }
        private VentaService VentaService { get; set; }

        public VendedorController()
        {
            var modelContainer = new ModelContainer();
            VendedorService = new VendedorService(new EntidadRepository<VendedorDominio>(modelContainer));
            LocalidadService = new LocalidadService(new EntidadRepository<LocalidadDominio>(modelContainer));
            ProvinciaService = new ProvinciaService(new EntidadRepository<ProvinciaDominio>(modelContainer));
            VentaService = new VentaService(new EntidadRepository<VentaDominio>(modelContainer));
            ViewBag.MenuId = 9;
            ViewBag.Title = "Vendedores";
        }
        //
        // GET: /Cobrador/

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Id = TempData["Id"];
            ViewBag.Mensaje = TempData["Mensaje"];

            var vendedorViewModels = new List<VendedorViewModel>();
            using (VendedorService)
            {
                vendedorViewModels.AddRange(VendedorService.Listar()
                    .OrderBy(c => c.Apellido)
                    .ThenBy(c => c.Nombre)
                    .ToList()
                    .Select(c => new VendedorViewModel(c)));
            }

            return View(vendedorViewModels);
        }

        [HttpGet]
        public ActionResult Crear()
        {
            var vendedorViewModel = new VendedorViewModel();
            PrepareModel(vendedorViewModel);

            return View(vendedorViewModel);
        }

        [HttpPost]
        public ActionResult Crear(VendedorViewModel vendedorViewModel)
        {
            long resultado = 0;
            var localidadIds = new List<string>();
            if (!string.IsNullOrEmpty(Request.Form["localidadesAsignadas_dualList"]))
            {
                localidadIds = Request.Form["localidadesAsignadas_dualList"].Split(',').ToList();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (VendedorService)
                    {
                        var vendedorDominio = new VendedorDominio
                        {
                            FechaAlta = DateTime.Now,
                            Nombre = vendedorViewModel.Nombre,
                            Apellido = vendedorViewModel.Apellido,
                            Dni = vendedorViewModel.Dni,
                            PorcentajeComision = vendedorViewModel.PorcentajeComision,
                            Direccion = vendedorViewModel.Direccion,
                            TelefonoFijo = vendedorViewModel.TelefonoFijo,
                            Celular = vendedorViewModel.Celular,
                            Email = vendedorViewModel.Email,
                            Localidad = LocalidadService.GetPorId(vendedorViewModel.LocalidadId),
                            Localidades = new List<LocalidadDominio>(),
                        };

                        foreach (var localidadId in localidadIds)
                        {
                            vendedorDominio.Localidades.Add(LocalidadService.GetPorId((Convert.ToInt64(localidadId))));
                        }

                        resultado = VendedorService.Guardar(vendedorDominio);

                        if (resultado <= 0)
                        {
                            foreach (var error in VendedorService.ModelError)
                            {
                                ModelState.AddModelError(error.Key, error.Value);
                            }
                        }
                        else
                        {
                            TempData["Id"] = vendedorDominio.Id;
                            TempData["Mensaje"] = string.Format(Messages.EntidadNueva, Messages.ElVendedor, vendedorDominio.Id);
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    var sqlException = ex.GetBaseException() as SqlException;
                    if (sqlException != null && sqlException.Number == 2601)
                    {
                        ModelState.AddModelError("Error",
                            string.Format(ErrorMessages.DniRepetido, vendedorViewModel.Dni, "cobrador"));
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

            if (resultado == 0)
            {
                foreach (var localidadId in localidadIds)
                {
                    vendedorViewModel.LocalidadesAsignadas.Add(new LocalidadViewModel(LocalidadService.GetPorId(Convert.ToInt64(localidadId))));
                }

                PrepareModel(vendedorViewModel);
            }

            return resultado > 0
                ? (ActionResult)RedirectToAction("Index")
                : View(vendedorViewModel);
        }

        [HttpGet]
        public ActionResult Modificar(int id)
        {
            VendedorViewModel vendedorViewModel;
            using (VendedorService)
            {
                vendedorViewModel = new VendedorViewModel(VendedorService.GetPorId(id));
            }
            PrepareModel(vendedorViewModel);

            return View(vendedorViewModel);
        }

        [HttpPost]
        public ActionResult Modificar(VendedorViewModel vendedorViewModel)
        {
            if (!ModelState.IsValid)
            {
                PrepareModel(vendedorViewModel);
                return View(vendedorViewModel);
            }

            var localidadIds = new List<string>();
            if (!string.IsNullOrEmpty(Request.Form["localidadesAsignadas_dualList"]))
            {
                localidadIds = Request.Form["localidadesAsignadas_dualList"].Split(',').ToList();
            }
            var nombreLocalidades = string.Empty;
            long resultado = 0;

            try
            {
                var vendedorDominio = VendedorService.GetPorId(vendedorViewModel.Id);
                var i = 0;
                foreach (var localidadAsignada in vendedorDominio.Localidades)
                {
                    if (localidadIds.Contains(localidadAsignada.Id.ToString()) == false)
                    {
                        if (VentaService.ListarAsQueryable().Any(v => (v.Cliente.Localidad.Id == localidadAsignada.Id
                                                                        && v.Cobrador.Id == vendedorDominio.Id
                                                                        && v.Estado == EstadoVenta.Vigente)))
                        {
                            nombreLocalidades += ", " + localidadAsignada.Nombre;
                            i++;
                        }
                    }
                }

                if (i > 0)
                {
                    nombreLocalidades = nombreLocalidades.Substring(2, nombreLocalidades.Length - 2);

                    if (i == 1)
                    {
                        ModelState.AddModelError("localidad",
                            "La localidad: " + nombreLocalidades +
                            " debe estar asignada ya que existen ventas asociadas al vendedor que intenta modificar");
                    }
                    else if (i > 1)
                    {
                        ModelState.AddModelError("localidades",
                            "Las localidades: " + nombreLocalidades +
                            " deben estar asignadas ya que existen ventas asociadas al vendedor que intenta modificar");
                    }
                }
                else
                {
                    using (VendedorService)
                    {
                        vendedorDominio.Nombre = vendedorViewModel.Nombre;
                        vendedorDominio.Apellido = vendedorViewModel.Apellido;
                        vendedorDominio.Dni = vendedorViewModel.Dni;
                        vendedorDominio.PorcentajeComision = vendedorViewModel.PorcentajeComision;
                        vendedorDominio.Direccion = vendedorViewModel.Direccion;
                        vendedorDominio.TelefonoFijo = vendedorViewModel.TelefonoFijo;
                        vendedorDominio.Celular = vendedorViewModel.Celular;
                        vendedorDominio.Email = vendedorViewModel.Email;
                        vendedorDominio.Localidad = LocalidadService.GetPorId(vendedorViewModel.LocalidadId);
                        vendedorDominio.Localidades.Clear();

                        foreach (var localidad in localidadIds)
                        {
                            vendedorDominio.Localidades.Add(LocalidadService.GetPorId((Convert.ToInt64(localidad))));
                        }

                        resultado = VendedorService.Guardar(vendedorDominio);
                        if (resultado <= 0)
                        {
                            foreach (var error in VendedorService.ModelError)
                            {
                                ModelState.AddModelError(error.Key, error.Value);
                            }
                        }
                        else
                        {
                            TempData["Id"] = vendedorViewModel.Id;
                            TempData["Mensaje"] = string.Format(Messages.EntidadModificada, Messages.ElVendedor, vendedorViewModel.Id);
                        }
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;
                if (sqlException != null && sqlException.Number == 2601)
                {
                    ModelState.AddModelError("Error",
                        string.Format(ErrorMessages.DniRepetido, vendedorViewModel.Dni));
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
                foreach (var localidad in localidadIds)
                {
                    vendedorViewModel.LocalidadesAsignadas.Add(new LocalidadViewModel(LocalidadService.GetPorId(Convert.ToInt64(localidad))));
                }
                PrepareModel(vendedorViewModel);
            }

            return resultado > 0
                ? (ActionResult)RedirectToAction("Index")
                : View(vendedorViewModel);
        }

        [HttpGet]
        public JsonResult Eliminar(int id)
        {
            try
            {
                using (VendedorService)
                {
                    var vendedorDominio = VendedorService.GetPorId(id);
                    vendedorDominio.Localidades.Clear();
                    VendedorService.Eliminar(vendedorDominio);
                }
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;
                if (sqlException != null && sqlException.Number == 547)
                {
                    ModelState.AddModelError("Error", string.Format(ErrorMessages.DatosAsociados, Messages.ElVendedor));
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

        #region Private Methods

        private void PrepareModel(VendedorViewModel vendedorViewModel)
        {
            vendedorViewModel.Localidades = new SelectList(LocalidadService.Listar().ToList()
                .Select(l => new LocalidadViewModel(l))
                .ToList(), "Id", "Nombre");

            vendedorViewModel.Provincias = new SelectList(ProvinciaService.Listar()
               .Select(p => new ProvinciaViewModel(p))
               .ToList(), "Id", "Nombre");


            var localidades = new List<LocalidadViewModel>();
            if (vendedorViewModel.ProvinciaId > 0)
            {
                localidades.AddRange(LocalidadService.Listar(l => l.Provincia.Id == vendedorViewModel.ProvinciaId)
                                                                    .ToList()
                                                                    .Select(l => new LocalidadViewModel(l)));
            }
        }

        #endregion
    }
}