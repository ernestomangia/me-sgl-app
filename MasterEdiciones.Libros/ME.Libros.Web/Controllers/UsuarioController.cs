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
using ME.Libros.Web.Extensions;
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
        [ValidateAntiForgeryToken]
        public ActionResult Crear(UsuarioViewModel usuarioViewModel)
        {
            long resultado = 0;
            if (ModelState.IsValid)
            {
                var usuarioDominio = new UsuarioDominio
                {
                    FechaAlta = DateTime.Now,
                    Nombre = usuarioViewModel.Nombre,
                    Apellido = usuarioViewModel.Apellido,
                    UserName = usuarioViewModel.UserName,
                    Password = usuarioViewModel.Password,
                    Email = usuarioViewModel.Email,
                    EmailConfirmado = false,
                    CantidadIntentosFallidos = 0,
                    Habilitado = usuarioViewModel.Habilitado
                };

                try
                {
                    using (UsuarioService)
                    {
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
                            TempData["Id"] = usuarioDominio.Id;
                            TempData["Mensaje"] = string.Format(Messages.EntidadNueva, Messages.ElUsuario, usuarioDominio.Id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log
                    ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
                }
            }

            return resultado > 0
                ? (ActionResult)RedirectToAction("Index")
                : View(usuarioViewModel);
        }

        [HttpGet]
        public JsonResult Eliminar(int id, string redirectUrl)
        {
            var isRedirect = !string.IsNullOrEmpty(redirectUrl);
            try
            {
                using (UsuarioService)
                {
                    var usuarioDominio = UsuarioService.GetPorId(id);
                    UsuarioService.Eliminar(usuarioDominio);

                    if (isRedirect)
                    {
                        TempData["Id"] = usuarioDominio.Id;
                        TempData["Mensaje"] = string.Format(Messages.EntidadEliminada, Messages.ElUsuario, usuarioDominio.Id);
                    }
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
        [ValidateAntiForgeryToken]
        public ActionResult Modificar(UsuarioViewModel usuarioViewModel)
        {
            long resultado = 0;
            if (ModelState.IsValid)
            {
                using (UsuarioService)
                {
                    var usuarioDominio = UsuarioService.GetPorId(usuarioViewModel.Id);
                    usuarioDominio.Nombre = usuarioViewModel.Nombre;
                    usuarioDominio.Apellido = usuarioViewModel.Apellido;
                    usuarioDominio.Email = usuarioViewModel.Email;
                    usuarioDominio.EmailConfirmado = usuarioViewModel.EmailConfirmado;
                    usuarioDominio.Habilitado = usuarioViewModel.Habilitado;
                    usuarioDominio.UserName = usuarioViewModel.UserName;
                    usuarioDominio.Password = usuarioViewModel.Password;
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
                        TempData["Mensaje"] = string.Format(Messages.EntidadModificada, Messages.ElUsuario, usuarioViewModel.Id);
                    }
                }
            }

            return resultado > 0
                ? (ActionResult)RedirectToAction("Index")
                : View(usuarioViewModel);
        }
    }
}