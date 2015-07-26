using System.Web.Mvc;

using ME.Libros.Dominio.General;
using ME.Libros.EF;
using ME.Libros.Repositorios;
using ME.Libros.Servicios.General;
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
            //ViewBag.MenuId = 13;
            ViewBag.Title = "Login";
        }

        //
        // GET: /Login/
        public ActionResult Index()
        {
            //var logins = new List<LoginViewModel>();
            //using (LoginService)
            //{
            //    logins.AddRange(LoginService.Listar()
            //        .ToList()
            //        .Select(r => new LoginViewModel(r)));
            //}

            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                //TODO: Almacenar cookie Recordar usuario y contraseña
                //TODO: Validar si usuario y contraseña son correctos

                return RedirectToAction("Index", "Cliente");
            }

            return View("Index", loginViewModel);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}