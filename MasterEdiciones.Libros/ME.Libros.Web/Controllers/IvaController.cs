using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
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
    public class IvaController : BaseController<IvaDominio>
    {
        //
        // GET: /Iva/

        public IvaService IvaService { get; set; }

        public IvaController()
        {
            var modelContainer = new ModelContainer();
            IvaService = new IvaService(new EntidadRepository<IvaDominio>(modelContainer));
            ViewBag.MenuId = 12;
            ViewBag.Title = "Iva";
        }

        public ActionResult Index()
        {
            ViewBag.Id = TempData["Id"];
            ViewBag.Mensaje = TempData["Mensaje"];

            var iva = new List<IvaViewModel>();
            using (IvaService)
            {
                iva.AddRange(IvaService.Listar()
                    .ToList()
                    .Select(r => new IvaViewModel(r))
                    );
            }

            return View(iva);
        }

        [HttpGet]
        public ActionResult Crear()
        {
            var ivaViewModel = new IvaViewModel();
            return View(ivaViewModel);
        }

        [HttpPost]
        public ActionResult Crear(IvaViewModel ivaViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (IvaService)
                    {
                        var ivaDominio = new IvaDominio
                        {
                            FechaAlta = DateTime.Now,
                            Nombre = ivaViewModel.Nombre,
                            Alicuota = ivaViewModel.Alicuota,
                        };

                        ivaViewModel.Id = IvaService.Guardar(ivaDominio);
                        if (ivaViewModel.Id <= 0)
                        {
                            foreach (var error in IvaService.ModelError)
                            {
                                ModelState.AddModelError(error.Key, error.Value);
                            }
                        }
                        else
                        {
                            TempData["Id"] = ivaViewModel.Id;
                            TempData["Mensaje"] = string.Format(Messages.EntidadNueva, "El iva", ivaDominio.Id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log
                    ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
                }
            }

            return ivaViewModel.Id > 0
                    ? (ActionResult)RedirectToAction("Index")
                    : View(ivaViewModel);
        }

        [HttpGet]
        public JsonResult Eliminar(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (IvaService)
                    {
                        IvaService.Eliminar(IvaService.GetPorId(id));
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
            var ivaViewModel = new IvaViewModel();
            try
            {
                using (IvaService)
                {
                    var ivaDominio = IvaService.GetPorId(id);
                    ivaViewModel = new IvaViewModel(ivaDominio);
                }
            }
            catch (Exception ex)
            {
                // Log
                ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
            }

            return View(ivaViewModel);
        }

        [HttpPost]
        public ActionResult Modificar(IvaViewModel ivaViewModel)
        {
            long resultado = 0;
            if (ModelState.IsValid)
            {
                using (IvaService)
                {
                    var ivaDominio = IvaService.GetPorId(ivaViewModel.Id);
                    ivaDominio.Nombre = ivaViewModel.Nombre;
                    ivaDominio.Alicuota = ivaViewModel.Alicuota;

                    resultado = IvaService.Guardar(ivaDominio);
                    if (resultado <= 0)
                    {
                        foreach (var error in IvaService.ModelError)
                        {
                            ModelState.AddModelError(error.Key, error.Value);
                        }
                    }
                    else
                    {
                        TempData["Id"] = ivaViewModel.Id;
                        TempData["Mensaje"] = string.Format(Messages.EntidadModificada, "El iva", ivaViewModel.Id);
                    }
                }
            }


            return ivaViewModel.Id > 0
                    ? (ActionResult)RedirectToAction("Index")
                    : View(ivaViewModel);
        }

        [HttpGet]
        public ActionResult Detalle(int id)
        {
            IvaViewModel ivaViewModel;
            using (IvaService)
            {
                ivaViewModel = new IvaViewModel(IvaService.GetPorId(id));
            }

            return View(ivaViewModel);
        }
    }
}