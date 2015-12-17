using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using ME.Libros.Api.Logging;
using ME.Libros.Dominio.General;
using ME.Libros.EF;
using ME.Libros.Repositorios;
using ME.Libros.Servicios.General;
using ME.Libros.Web.Extensions;
using ME.Libros.Web.Models;

namespace ME.Libros.Web.Controllers
{
    public class LoginController : Controller
    {
        public UsuarioService UsuarioService { get; set; }

        public LoginController()
        {
            var modelContainer = new ModelContainer();
            UsuarioService = new UsuarioService(new EntidadRepository<UsuarioDominio>(modelContainer));
            var navigationBarViewModel = new NavigationBarViewModel();
            ViewBag.NavigationBar = navigationBarViewModel;
            ViewBag.Title = "Login";
        }

        //
        // GET: /Login/
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var usuario = UsuarioService.ListarAsQueryable().SingleOrDefault(u => u.UserName.Equals(loginViewModel.Usuario) && u.Password.Equals(loginViewModel.Password));
                if (usuario != null)
                {
                    FormsAuthentication.SetAuthCookie(loginViewModel.Usuario, loginViewModel.Recordarme);
                    LogHelper.Log("LOGIN - User: " + loginViewModel.Usuario, SeveridadLog.Info);

                    if (Url.IsLocalUrl(returnUrl)
                       && returnUrl.Length > 1
                       && returnUrl.StartsWith("/")
                       && !returnUrl.StartsWith("//")
                       && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Cliente");
                }

                ModelState.AddModelError(string.Empty, ErrorMessages.InvalidUserPassword);
            }

            return View(loginViewModel);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Cliente");
        }
    }
}