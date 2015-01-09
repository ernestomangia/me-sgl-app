using System.Net;
using System.Web.Mvc;
using System.Collections.Generic;

using ME.Libros.Web.Models;

namespace ME.Libros.Web.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index()
        {
            var clientes = new List<ClienteViewModel>
                               {
                                   new ClienteViewModel
                                       {
                                           Codigo = "100",
                                           Nombre = "Juan",
                                           Apellido = "Perez",
                                           Cuil = "20-00000000-9"
                                       },
                                   new ClienteViewModel
                                       {
                                           Codigo = "101",
                                           Nombre = "Pepe",
                                           Apellido = "Sanchez",
                                           Cuil = "20-00000000-9"
                                       },
                               };
            return View(clientes);
        }

        [AcceptVerbs(WebRequestMethods.Http.Get)]
        public PartialViewResult Crear()
        {
            var model = new ClienteViewModel();
            return PartialView(model);
        }

        [AcceptVerbs(WebRequestMethods.Http.Post)]
        public ActionResult Crear(ClienteViewModel model)
        {
            return View();
        }
    }
}