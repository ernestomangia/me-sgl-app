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
    public class EditorialController : BaseController<EditorialDominio>
    {
        public EditorialService EditorialService { get; set; }

        public EditorialController()
        {
            var modelContainer = new ModelContainer();
            EditorialService = new EditorialService(new EntidadRepository<EditorialDominio>(modelContainer));
            ViewBag.MenuId = 5;
            ViewBag.Title = "Editoriales";
        }

        // GET: /Editorial/
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Id = TempData["Id"];
            ViewBag.Mensaje = TempData["Mensaje"];

            var editoriales = new List<EditorialViewModel>();
            using (EditorialService)
            {
                editoriales.AddRange(EditorialService.Listar()
                    .ToList()
                    .Select(e => new EditorialViewModel(e))
                    .Where(e => e.Id != 1));
            }

            return View(editoriales);
        }

        [HttpGet]
        public ActionResult Crear()
        {
            var editorialViewModel = new EditorialViewModel();
            return View(editorialViewModel);
        }

        [HttpPost]
        public ActionResult Crear(EditorialViewModel editorialViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return View(editorialViewModel);
            }

            long resultado = 0;
            try
            {
                using (this.EditorialService)
                {
                    var editorialDominio = new EditorialDominio
                                               {
                                                   FechaAlta = DateTime.Now,
                                                   Nombre = editorialViewModel.Nombre,
                                                   Descripcion = editorialViewModel.Descripcion,
                                               };

                    resultado = this.EditorialService.Guardar(editorialDominio);
                    if (resultado <= 0)
                    {
                        foreach (var error in this.EditorialService.ModelError)
                        {
                            this.ModelState.AddModelError(error.Key, error.Value);
                        }
                    }
                    else
                    {
                        this.TempData["Id"] = editorialDominio.Id;
                        this.TempData["Mensaje"] = string.Format(Messages.EntidadNueva, Messages.LaEditorial, editorialDominio.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log
                this.ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
            }

            return resultado > 0
                ? (ActionResult)RedirectToAction("Index")
                : View(editorialViewModel);
        }

        [HttpGet]
        public JsonResult Eliminar(int id)
        {
            try
            {
                using (EditorialService)
                {
                    EditorialService.Eliminar(EditorialService.GetPorId(id));
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

            return new JsonResult
            {
                Data = new { Success = ModelState.IsValid, Errors = ModelState.GetErrors() },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult Modificar(int id)
        {
            var editorialViewModel = new EditorialViewModel();
            try
            {
                using (EditorialService)
                {
                    var editorialDominio = EditorialService.GetPorId(id);
                    editorialViewModel = new EditorialViewModel(editorialDominio);
                }
            }
            catch (Exception ex)
            {
                // Log
                ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
            }

            return View(editorialViewModel);
        }

        [HttpPost]
        public ActionResult Modificar(EditorialViewModel editorialViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return View(editorialViewModel);
            }

            long resultado = 0;
            try
            {

                using (EditorialService)
                {
                    var editorialDominio = EditorialService.GetPorId(editorialViewModel.Id);
                    editorialDominio.Nombre = editorialViewModel.Nombre;
                    editorialDominio.Descripcion = editorialViewModel.Descripcion;

                    resultado = EditorialService.Guardar(editorialDominio);
                    if (resultado <= 0)
                    {
                        foreach (var error in EditorialService.ModelError)
                        {
                            this.ModelState.AddModelError(error.Key, error.Value);
                        }
                    }
                    else
                    {
                        TempData["Id"] = editorialDominio.Id;
                        TempData["Mensaje"] = string.Format(Messages.EntidadModificada, Messages.LaEditorial, editorialDominio.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log
                this.ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
            }

            return resultado > 0
                ? (ActionResult)RedirectToAction("Index")
                : View(editorialViewModel);
        }

        [HttpGet]
        public ActionResult Detalle(int id)
        {
            EditorialViewModel editorialViewModel;
            using (EditorialService)
            {
                editorialViewModel = new EditorialViewModel(EditorialService.GetPorId(id));
            }

            return View(editorialViewModel);
        }
    }
}
