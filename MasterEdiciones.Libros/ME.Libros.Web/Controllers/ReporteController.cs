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
        public LocalidadService LocalidadService { get; set; }

        public ReporteController()
        {
            var modelContainer = new ModelContainer();
            ClienteService = new ClienteService(new EntidadRepository<ClienteDominio>(modelContainer));
            VentaService = new VentaService(new EntidadRepository<VentaDominio>(modelContainer));
            LocalidadService = new LocalidadService(new EntidadRepository<LocalidadDominio>(modelContainer));
            ViewBag.MenuId = 150;
            ViewBag.Title = "Reportes";
            var menu = new Menues();
            ViewBag.Menues = menu.GetMenues();
        }

        // GET: Default
        public ActionResult Index()
        {
            var reporteViewModel = new ReporteViewModel();
            var localidades = LocalidadService.Listar()
                .ToList()
                .Select(l => new LocalidadViewModel(l));
            reporteViewModel.Localidades = new SelectList(localidades, "Id", "Nombre");

            var cantidadPorEstado = VentaService.ListarAsQueryable().GroupBy(v => new { v.Estado },
                v => v,
                (key, group) => new
                {
                    key.Estado,
                    Cantidad = group.Count()
                }).ToList();

            reporteViewModel.CantidadVigentes =
                cantidadPorEstado.SingleOrDefault(x => x.Estado == EstadoVenta.Vigente) == null
                    ? 0
                    : cantidadPorEstado.Single(x => x.Estado == EstadoVenta.Vigente).Cantidad;
            reporteViewModel.CantidadPagadas = cantidadPorEstado.SingleOrDefault(x => x.Estado == EstadoVenta.Pagada) == null
                    ? 0
                    : cantidadPorEstado.Single(x => x.Estado == EstadoVenta.Pagada).Cantidad;
            reporteViewModel.CantidadAnuladas = cantidadPorEstado.SingleOrDefault(x => x.Estado == EstadoVenta.Anulada) == null
                    ? 0
                    : cantidadPorEstado.Single(x => x.Estado == EstadoVenta.Anulada).Cantidad;
            return View(reporteViewModel);
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
                    .GroupBy(v => new { v.Cobrador, v.Cliente.Localidad },
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
                    .GroupBy(v => new { v.FechaVenta.Year, v.FechaVenta.Month },
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
                    .Where(v => v.Estado == EstadoVenta.Vigente && v.Cuotas.Any(c => c.Estado == EstadoCuota.Atrasada))
                    .GroupBy(v => new { v.FechaVenta.Year, v.FechaVenta.Month },
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