using System.Linq;
using System.Web.Mvc;

using ME.Libros.Web.Models;

namespace ME.Libros.Web.Controllers
{
    public class MenuLateralController : Controller
    {
        // GET: Admnistracion
        public PartialViewResult Index(int id)
        {
            var menues = new Menues(id);
            ViewBag.Menues = menues.MenuViewModels.OrderBy(m => m.Posicion).ToList();
            return PartialView();
        }
    }
}