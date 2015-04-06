using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
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
    public class RubroController : BaseController
    {
        //
        // GET: /Rubro/

        public RubroService RubroService { get; set; }
        public ActionResult Index()
        {
            ViewBag.Id = TempData["Id"];
            ViewBag.Mensaje = TempData["Mensaje"];

            var rubros = new List<RubroViewModel>();
            using (RubroService)
            {
                rubros.AddRange(RubroService.Listar().ToList().Select(r => new RubroViewModel(r)));
            }

            return View(rubros);

        }

        public RubroController()
        {
            var modelContainer = new ModelContainer();
            RubroService = new RubroService(new EntidadRepository<RubroDominio>(modelContainer));
            ViewBag.MenuId = 4;
        }

        [HttpGet]
        public ActionResult Crear()
        {
            var rubroViewModel = new RubroViewModel();
            return View(rubroViewModel);
        }

        [HttpPost]
        public ActionResult Crear(RubroViewModel rubroViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (RubroService)
                    {
                        var rubroDominio = new RubroDominio
                        {   
                            FechaAlta = DateTime.Now,
                            Nombre = rubroViewModel.Nombre,
                            Descripcion=rubroViewModel.Descripcion,
                        };

                        rubroViewModel.Id = RubroService.Guardar(rubroDominio);
                        if (rubroViewModel.Id <= 0)
                        {
                            foreach (var error in RubroService.ModelError)
                            {
                                ModelState.AddModelError(error.Key, error.Value);
                            }
                        }
                        else
                        {
                            TempData["Id"] = rubroViewModel.Id;
                            TempData["Mensaje"] = string.Format(Messages.EntidadNueva, "El rubro");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log
                    ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
                }
            }

            return rubroViewModel.Id > 0
                    ? (ActionResult)RedirectToAction("Index")
                    : View(rubroViewModel);
        }

        [HttpGet]
        public JsonResult Eliminar(int id)
        {
            var resultadoViewModel = new ResultadoViewModel();
            if (ModelState.IsValid)
            {
                try
                {
                    using (RubroService)
                    {
                        RubroService.Eliminar(RubroService.GetPorId(id));
                    }
                    resultadoViewModel.Success = true;
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var error in ex.EntityValidationErrors.SelectMany(validationError => validationError.ValidationErrors))
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                }
                catch (DbUpdateException ex)
                {
                    var sqlException = ex.GetBaseException() as SqlException;

                    if (sqlException != null)
                    {
                        resultadoViewModel.Messages.Add("Error", sqlException.Number == 547 ? ErrorMessages.EliminarCliente : ErrorMessages.ErrorSistema);
                    }
                    else
                    {
                        resultadoViewModel.Messages.Add("Error", ErrorMessages.ErrorSistema);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
                }
            }
         

            return Json(ModelState, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult Modificar(int id)
        {
            var rubroViewModel = new RubroViewModel();
            try
            {
                using (RubroService)
                {
                    var rubroDominio = RubroService.GetPorId(id);
                    rubroViewModel = new RubroViewModel(rubroDominio);
                }
            }
            catch (Exception ex)
            {
                // Log
                ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
            }

            return View(rubroViewModel);
        }

        [HttpPost]
        public ActionResult Modificar(RubroViewModel rubroViewModel)
        {
            long id = 0;
            if (ModelState.IsValid)
            {
                using (RubroService)
                {
                    var rubroDominio = RubroService.GetPorId(rubroViewModel.Id);
                    rubroDominio.Nombre = rubroViewModel.Nombre;
                    rubroDominio.Descripcion = rubroViewModel.Descripcion;

                    id = RubroService.Guardar(rubroDominio);
                    if (id <= 0)
                    {
                        foreach (var error in RubroService.ModelError)
                        {
                            ModelState.AddModelError(error.Key, error.Value);
                        }
                    }
                    else
                    {
                        TempData["Id"] = rubroViewModel.Id;
                        TempData["Mensaje"] = string.Format(Messages.EntidadModificada, "El rubro Nº " + id);
                    }
                }
            }


            return rubroViewModel.Id > 0
                    ? (ActionResult)RedirectToAction("Index")
                    : View(rubroViewModel);
        }



	}
}