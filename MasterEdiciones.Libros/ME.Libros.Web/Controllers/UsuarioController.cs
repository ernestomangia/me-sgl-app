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
    public class UsuarioController : BaseController<UsuarioDominio>
    {
        public UsuarioService UsuarioService { get; set; }

        public UsuarioController()
        {
            var modelContainer = new ModelContainer();
            UsuarioService = new UsuarioService(new EntidadRepository<UsuarioDominio>(modelContainer));
            ViewBag.MenuId = 11;
            ViewBag.Title = "Usuarios";
        }

        //
        // GET: /Usuario/
        public ActionResult Index()
        {
            ViewBag.Id = TempData["Id"];
            ViewBag.Mensaje = TempData["Mensaje"];

            var usuarios = new List<UsuarioViewModel>();
            using (UsuarioService)
            {
                usuarios.AddRange(UsuarioService.Listar()
                    .ToList()
                    .Select(r => new UsuarioViewModel(r)));
            }

            return View(usuarios);
        }

        [HttpGet]
        public ActionResult Crear()
        {
            var usuarioViewModel = new UsuarioViewModel();
            return View(usuarioViewModel);
        }

        [HttpPost]
        public ActionResult Crear(UsuarioViewModel usuarioViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (UsuarioService)
                    {
                        var usuarioDominio = new UsuarioDominio
                        {
                            FechaAlta = DateTime.Now,
                            Nombre = usuarioViewModel.Nombre,
                            Contrasena = usuarioViewModel.Contrasena,
                        };

                        usuarioViewModel.Id = UsuarioService.Guardar(usuarioDominio);
                        if (usuarioViewModel.Id <= 0)
                        {
                            foreach (var error in UsuarioService.ModelError)
                            {
                                ModelState.AddModelError(error.Key, error.Value);
                            }
                        }
                        else
                        {
                            TempData["Id"] = usuarioViewModel.Id;
                            TempData["Mensaje"] = string.Format(Messages.EntidadNueva, "El usuario", usuarioDominio.Id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log
                    ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
                }
            }

            return usuarioViewModel.Id > 0
                    ? (ActionResult)RedirectToAction("Index")
                    : View(usuarioViewModel);
        }

        [HttpGet]
        public JsonResult Eliminar(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (UsuarioService)
                    {
                        UsuarioService.Eliminar(UsuarioService.GetPorId(id));
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
            var usuarioViewModel = new UsuarioViewModel();
            try
            {
                using (UsuarioService)
                {
                    var usuarioDominio = UsuarioService.GetPorId(id);
                    usuarioViewModel = new UsuarioViewModel(usuarioDominio);
                }
            }
            catch (Exception ex)
            {
                // Log
                ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
            }

            return View(usuarioViewModel);
        }

        [HttpPost]
        public ActionResult Modificar(UsuarioViewModel usuarioViewModel)
        {
            long resultado = 0;
            if (ModelState.IsValid)
            {
                using (UsuarioService)
                {
                    var usuarioDominio = UsuarioService.GetPorId(usuarioViewModel.Id);
                    usuarioDominio.Nombre = usuarioViewModel.Nombre;
                    usuarioDominio.Contrasena = usuarioViewModel.Contrasena;

                    resultado = UsuarioService.Guardar(usuarioDominio);
                    if (resultado <= 0)
                    {
                        foreach (var error in UsuarioService.ModelError)
                        {
                            ModelState.AddModelError(error.Key, error.Value);
                        }
                    }
                    else
                    {
                        TempData["Id"] = usuarioViewModel.Id;
                        TempData["Mensaje"] = string.Format(Messages.EntidadModificada, "El usuario", usuarioViewModel.Id);
                    }
                }
            }


            return usuarioViewModel.Id > 0
                    ? (ActionResult)RedirectToAction("Index")
                    : View(usuarioViewModel);
        }

        [HttpGet]
        public ActionResult Detalle(int id)
        {
            UsuarioViewModel usuarioViewModel;
            using (UsuarioService)
            {
                usuarioViewModel = new UsuarioViewModel(UsuarioService.GetPorId(id));
            }

            return View(usuarioViewModel);
        }
    }
}