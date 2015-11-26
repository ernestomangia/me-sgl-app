using System.Web.Mvc;
using ME.Libros.Servicios.Configuracion;
using ME.Libros.Web.Models;

namespace ME.Libros.Web.Controllers
{
    public class ConfiguracionController : Controller
    {
        public ConfiguracionService ConfiguracionService { get; set; }
        public ConfiguracionController()
        {
            var navigationBarViewModel = new NavigationBarViewModel();
            ViewBag.NavigationBar = navigationBarViewModel;
            ViewBag.Title = "Configuraciones";
            ConfiguracionService = new ConfiguracionService();
        }

        // GET: Configuracion
        public ActionResult Index()
        {
            ViewBag.Mensaje = TempData["Mensaje"];
            return View(new ConfiguracionViewModel());
        }

        [HttpPost]
        public ActionResult CreateBackup(ConfiguracionViewModel configuracionViewModel)
        {
            ConfiguracionService.CreateBackup(configuracionViewModel.Ruta);
            TempData["Mensaje"] = "El backup de la BD se generó exitosamente";
            return RedirectToAction("Index");
        }
    }
}