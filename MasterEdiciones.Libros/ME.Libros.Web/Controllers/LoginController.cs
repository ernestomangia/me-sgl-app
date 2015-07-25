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
    public class LoginController : BaseController<LoginDominio>
    {
        public LoginService LoginService { get; set; }

        public LoginController()
        {
            var modelContainer = new ModelContainer();
            LoginService = new LoginService(new EntidadRepository<LoginDominio>(modelContainer));
            ViewBag.MenuId = 13;
            ViewBag.Title = "Login";
        }

        //
        // GET: /Login/
        public ActionResult Index()
        {
            ViewBag.Id = TempData["Id"];
            ViewBag.Mensaje = TempData["Mensaje"];

            var logins = new List<LoginViewModel>();
            using (LoginService)
            {
                logins.AddRange(LoginService.Listar()
                    .ToList()
                    .Select(r => new LoginViewModel(r)));
            }

            return View(logins);
        }

          }
}