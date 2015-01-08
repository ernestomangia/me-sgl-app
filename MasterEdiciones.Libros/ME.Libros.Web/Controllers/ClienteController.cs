using System.Net;
using System.Web.Mvc;

namespace ME.Libros.Web.Controllers
{
    using ME.Libros.Web.Models;

    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(WebRequestMethods.Http.Get)]
        public ActionResult Crear()
        {
            return View();
        }

        [AcceptVerbs(WebRequestMethods.Http.Post)]
        public ActionResult Crear(ClienteViewModel model)
        {
            return View();
        }
    }
}