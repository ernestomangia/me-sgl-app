using System.IO;
using System.Net.Mime;
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
            ViewBag.Error = TempData["Error"];
            return View(new ConfiguracionViewModel());
        }

        [HttpPost]
        public ActionResult CreateBackup(ConfiguracionViewModel configuracionViewModel)
        {
            var result = ConfiguracionService.CreateBackup(configuracionViewModel.CarpetaDestino);
            if (result)
            {
                TempData["Mensaje"] = "El backup de la base de datos se generó exitosamente";
                if (string.IsNullOrEmpty(configuracionViewModel.CarpetaDestino))
                {
                    var backupFile = Path.Combine(ConfiguracionService.GetBackupFolder(), ConfiguracionService.BackupName);
                    var fileBytes = System.IO.File.ReadAllBytes(backupFile);
                    return File(fileBytes, MediaTypeNames.Application.Octet, ConfiguracionService.BackupName);
                }
            }
            else
            {
                TempData["Error"] = true;
                TempData["Mensaje"] = "Error al intentar crear el backup de la base de datos";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RestoreBackup(ConfiguracionViewModel configuracionViewModel)
        {
            var result = ConfiguracionService.RestoreBackup(configuracionViewModel.BackupFile);
            if (result)
            {
                TempData["Mensaje"] = "El backup de la base de datos se restauró exitosamente";
            }
            else
            {
                TempData["Error"] = true;
                TempData["Mensaje"] = "Error al intentar restaurar el backup de la base de datos";
            }

            return RedirectToAction("Index");
        }
    }
}