﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

using ME.Libros.Dominio.General;
using ME.Libros.EF;
using ME.Libros.Repositorios;
using ME.Libros.Servicios.General;
using ME.Libros.Web.Helpers;
using ME.Libros.Web.Models;

namespace ME.Libros.Web.Controllers
{
    public class LocalidadController : Controller
    {
        private LocalidadService LocalidadService { get; set; }
        private ProvinciaService ProvinciaService { get; set; }
        public ZonaService ZonaService { get; set; }

        public LocalidadController()
        {
            var modelContainer = new ModelContainer();
            LocalidadService = new LocalidadService(new EntidadRepository<LocalidadDominio>(modelContainer));
            ProvinciaService = new ProvinciaService(new EntidadRepository<ProvinciaDominio>(modelContainer));
            ZonaService=new ZonaService(new EntidadRepository<ZonaDominio>(modelContainer));
            ViewBag.MenuId = 3;
            ViewBag.Title = "Localidades";
        }

        // GET: Localidad
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Id = TempData["Id"];
            ViewBag.Mensaje = TempData["Mensaje"];

            var localidades = new List<LocalidadViewModel>();
            using (LocalidadService)
            {
                localidades.AddRange(LocalidadService.Listar()
                    .OrderBy(l => l.Nombre)
                    .ToList()
                    .Select(l => new LocalidadViewModel(l)));
            }

            return View(localidades);
        }

        [HttpGet]
        public ActionResult Crear()
        {
            var model = new LocalidadViewModel();
            PrepareModel(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult Crear(LocalidadViewModel localidadViewModel)
        {
            long resultado = 0;

            ModelState.RemoveFor<LocalidadViewModel>(l => l.Zona.Nombre);

            if (ModelState.IsValid)
            {
                try
                {
                    using (LocalidadService)
                    {
                        var localidadDominio = new LocalidadDominio
                                                 {
                                                     FechaAlta = DateTime.Now,
                                                     Nombre = localidadViewModel.Nombre,
                                                     Provincia = ProvinciaService.GetPorId(localidadViewModel.ProvinciaId),
                                                     Zona=ZonaService.GetPorId(localidadViewModel.Zona.Id)
                                                 };

                        resultado = LocalidadService.Guardar(localidadDominio);
                        if (resultado <= 0)
                        {
                            foreach (var error in LocalidadService.ModelError)
                            {
                                ModelState.AddModelError(error.Key, error.Value);
                            }
                        }
                        else
                        {
                            TempData["Id"] = localidadDominio.Id;
                            TempData["Mensaje"] = string.Format(Messages.EntidadNueva, Messages.LaLocalidad, localidadDominio.Id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
                }
            }

            if (resultado == 0)
            {
                PrepareModel(localidadViewModel);
            }

            return resultado > 0
                ? (ActionResult)RedirectToAction("Index")
                : View(localidadViewModel);
        }

        [HttpGet]
        public JsonResult Eliminar(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (LocalidadService)
                    {
                        LocalidadService.Eliminar(LocalidadService.GetPorId(id));
                    }
                }
                catch (DbUpdateException ex)
                {
                    var sqlException = ex.GetBaseException() as SqlException;

                    if (sqlException != null && sqlException.Number == 547)
                    {
                        ModelState.AddModelError("Error", string.Format(ErrorMessages.DatosAsociados, Messages.LaLocalidad));
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
            LocalidadViewModel localidadViewModel;
            using (LocalidadService)
            {
                localidadViewModel = new LocalidadViewModel(LocalidadService.GetPorId(id));
            }
            PrepareModel(localidadViewModel);

            return View(localidadViewModel);
        }

        [HttpPost]
        public ActionResult Modificar(LocalidadViewModel localidadViewModel)
        {
            long resultado = 0;

            ModelState.RemoveFor<LocalidadViewModel>(l => l.Zona.Nombre);

            if (ModelState.IsValid)
            {
                using (LocalidadService)
                {
                    var localidadDominio = LocalidadService.GetPorId(localidadViewModel.Id);
                    localidadDominio.Nombre = localidadViewModel.Nombre;
                    localidadDominio.Provincia = ProvinciaService.GetPorId(localidadViewModel.ProvinciaId);
                    localidadDominio.Zona = ZonaService.GetPorId(localidadViewModel.Zona.Id);

                    resultado = LocalidadService.Guardar(localidadDominio);
                    if (resultado <= 0)
                    {
                        foreach (var error in LocalidadService.ModelError)
                        {
                            ModelState.AddModelError(error.Key, error.Value);
                        }
                    }
                    else
                    {
                        TempData["Id"] = localidadViewModel.Id;
                        TempData["Mensaje"] = string.Format(Messages.EntidadModificada, Messages.LaLocalidad, localidadViewModel.Id);
                    }
                }
            }

            if (resultado == 0)
            {
                PrepareModel(localidadViewModel);
            }

            return resultado > 0
                ? (ActionResult)RedirectToAction("Index")
                : View(localidadViewModel);
        }

        [HttpGet]
        public ActionResult Detalle(int id)
        {
            LocalidadViewModel localidadViewModel;
            using (LocalidadService)
            {
                localidadViewModel = new LocalidadViewModel(LocalidadService.GetPorId(id));
            }

            PrepareModel(localidadViewModel);

            return View(localidadViewModel);
        }

        public JsonResult ListarLocalidades(int id)
        {
            var localidades = new List<LocalidadViewModel>();
            using (LocalidadService)
            {
                localidades.AddRange(LocalidadService
                    .Listar(l => l.Provincia.Id == id)
                    .ToList()
                    .Select(l => new LocalidadViewModel(l)));
            }

            return new JsonResult
            {
                Data = localidades,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        #region Private Methods

        private void PrepareModel(LocalidadViewModel localidadViewModel)
        {
            localidadViewModel.Provincias = new SelectList(ProvinciaService.Listar()
                    .Select(p => new ProvinciaViewModel(p))
                    .ToList(),
                    "Id",
                    "Nombre");

            localidadViewModel.Zonas= new SelectList(ZonaService.Listar()
                .Select(z=>new ZonaViewModel(z))
                .ToList(),"Id","Nombre");
        }

        #endregion
    }
}