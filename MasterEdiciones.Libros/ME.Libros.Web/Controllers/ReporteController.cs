using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ME.Libros.Dominio.General;
using ME.Libros.EF;
using ME.Libros.Repositorios;
using ME.Libros.Servicios.General;
using ME.Libros.Web.Models;
using Rotativa;
using Rotativa.Options;

namespace ME.Libros.Web.Controllers
{
    public class ReporteController : Controller
    {
        public ClienteService ClienteService { get; set; }

        public ReporteController()
        {
            var modelContainer = new ModelContainer();
            ClienteService = new ClienteService(new EntidadRepository<ClienteDominio>(modelContainer));
            ViewBag.Title = "Reportes";
        }

        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Clientes()
        {
            var footer = "--footer-right \"[date] [time]\" --footer-center \"[page] de [toPage]\" " +
                         "--footer-line --footer-font-size \"8\" " +
                         "--footer-spacing 15 --footer-font-name \"calibri light\"";
            return new ActionAsPdf("ClientesPDF")
            {
                //FileName = "ChequeraVentaNro" + id + ".pdf",
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4,
                PageMargins = new Margins(10, 10, 10, 10),
                //CustomSwitches = "--print-media-type --default-header --custom-header \" -- \" \\"
                CustomSwitches = "--print-media-type " + footer,
                UserName = "Emangia" /*User.Identity.Name*/
            };
        }

        public ActionResult ClientesPDF()
        {
            var clientes = new List<ClienteViewModel>();
            using (ClienteService)
            {
                clientes.AddRange(ClienteService.Listar()
                    .OrderBy(c => c.Apellido)
                    .ThenBy(c => c.Nombre)
                    .ToList()
                    .Select(c => new ClienteViewModel(c)));
            }
            return View(clientes);
        }
    }
}