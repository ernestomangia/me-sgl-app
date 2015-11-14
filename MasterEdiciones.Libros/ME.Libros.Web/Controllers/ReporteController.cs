using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ME.Libros.Dominio.General;
using ME.Libros.EF;
using ME.Libros.Repositorios;
using ME.Libros.Servicios.General;
using ME.Libros.Utils.Enums;
using ME.Libros.Web.Models;
using Rotativa;
using Rotativa.Options;

namespace ME.Libros.Web.Controllers
{
    public class ReporteController : Controller
    {
        public ClienteService ClienteService { get; set; }
        public VentaService VentaService { get; set; }


        public ReporteController()
        {
            var modelContainer = new ModelContainer();
            ClienteService = new ClienteService(new EntidadRepository<ClienteDominio>(modelContainer));
            VentaService = new VentaService(new EntidadRepository<VentaDominio>(modelContainer));
            ViewBag.MenuId = 150;
            ViewBag.Title = "Reportes";
        }

        // GET: Default
        public ActionResult Index()
        {
            return View(new ReporteViewModel());
        }

        public ActionResult PlanillaCobrador()
        {
            var footer = "--footer-right \"[date] [time]\" " +
                         "--footer-line --footer-font-size \"8\" " +
                         "--footer-font-name \"calibri light\"" +
                         "--header-left='[webpage]'";
            return new ActionAsPdf("PlanillaCobradorPDF")
            {
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4,
                PageMargins = new Margins(5, 10, 10, 10),
                CustomSwitches = "--print-media-type " + footer,
            };
        }

        public ActionResult PlanillaCobradorPDF()
        {
            var planillas = new List<PlanillaCobradorViewModel>();
            using (VentaService)
            {
                planillas.AddRange(VentaService.ListarAsQueryable()
                    .Where(v => v.Estado == EstadoVenta.Vigente)
                    .GroupBy(v => new {v.Cobrador, v.Cliente.Localidad},
                        v => v,
                        (key, group) => new
                        {
                            key.Cobrador,
                            key.Localidad,
                            Ventas = group.OrderBy(v => v.FechaVenta)
                        })
                    .OrderBy(p => p.Cobrador.Apellido)
                    .ThenBy(p => p.Cobrador.Nombre)
                    .ThenBy(p => p.Localidad.Nombre)
                    .ToList()
                    .Select(p => new PlanillaCobradorViewModel(p.Cobrador, p.Localidad, p.Ventas)));
            }

            return View(planillas);
        }

        public ActionResult VentasPorCobrar()
        {
            var footer = "--footer-right \"[date] [time]\" " +
                         "--footer-line --footer-font-size \"8\" " +
                         "--footer-font-name \"calibri light\"" +
                         "--header-left='[webpage]'";
            return new ActionAsPdf("VentasPorCobrarPDF")
            {
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4,
                PageMargins = new Margins(8, 8, 10, 10),
                CustomSwitches = "--print-media-type " + footer,
            };
        }

        public ActionResult VentasPorCobrarPDF()
        {
            var ventasPorCobrar = new List<VentasPorCobrarViewModel>();
            using (VentaService)
            {
                ventasPorCobrar.AddRange(VentaService.ListarAsQueryable()
                    .Where(v => v.Estado == EstadoVenta.Vigente)
                    .GroupBy(v => new {v.FechaVenta.Year, v.FechaVenta.Month},
                        v => v,
                        (key, group) => new
                        {
                            key.Year,
                            key.Month,
                            Ventas = group.OrderBy(v => v.FechaVenta)
                        })
                    .OrderBy(v => v.Year)
                    .ThenBy(v => v.Month)
                    .ToList()
                    .Select(v => new VentasPorCobrarViewModel(v.Year, v.Month, v.Ventas)));
            }

            return View(ventasPorCobrar);
        }

        public ActionResult VentasAtrasadas()
        {
            var footer = "--footer-right \"[date] [time]\" " +
                         "--footer-line --footer-font-size \"8\" " +
                         "--footer-font-name \"calibri light\"" +
                         "--header-left='[webpage]'";
            return new ActionAsPdf("VentasAtrasadasPDF")
            {
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4,
                PageMargins = new Margins(8, 8, 10, 10),
                CustomSwitches = "--print-media-type " + footer,
            };
        }

        public ActionResult VentasAtrasadasPDF()
        {
            var ventasAtrasadas = new List<VentasAtrasadasViewModel>();
            using (VentaService)
            {
                ventasAtrasadas.AddRange(VentaService.ListarAsQueryable()
                    .Where(v => v.Estado == EstadoVenta.Vigente)
                    .GroupBy(v => new {v.FechaVenta.Year, v.FechaVenta.Month},
                        v => v,
                        (key, group) => new
                        {
                            key.Year,
                            key.Month,
                            Ventas = group.OrderBy(v => v.FechaVenta)
                        })
                    .OrderBy(v => v.Year)
                    .ThenBy(v => v.Month)
                    .ToList()
                    .Select(v => new VentasAtrasadasViewModel(v.Year, v.Month, v.Ventas)));
            }

            return View(ventasAtrasadas);
        }
    }
}