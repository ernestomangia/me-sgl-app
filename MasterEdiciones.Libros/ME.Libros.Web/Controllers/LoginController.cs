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
    public class LoginController : BaseController<LoginDominio>
    {
        public LoginService LoginService { get; set; }

        public LoginController()
        {
            var modelContainer = new ModelContainer();
            LoginService = new LoginService(new EntidadRepository<LoginDominio>(modelContainer));
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
                //TODO: Almacenar cookie Recordar usuario y contraseña
                //TODO: Validar si usuario y contraseña son correctos

                if (true || Membership.ValidateUser(loginViewModel.Usuario, loginViewModel.Password))
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