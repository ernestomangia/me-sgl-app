using System.Web.Mvc;

namespace ME.Libros.Web.Controllers
{
    public class VentaController : Controller
    {
        // GET: Ventas
        public VentaController()
        {
            ViewBag.MenuId = 5;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}