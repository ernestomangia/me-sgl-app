using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using ME.Libros.Web.Models;

namespace ME.Libros.Web.Controllers
{
    public class MenuLateralController : Controller
    {
        // GET: Admnistracion
        public PartialViewResult Index(List<MenuViewModel> subMenues)
        {
            //var menues = new Menues(id);
            //ViewBag.Menues = menues.MenuViewModels.OrderBy(m => m.Posicion).ToList();
            ViewBag.Menues = subMenues.OrderBy(m => m.Posicion).ToList();
            return PartialView();
        }
    }
}