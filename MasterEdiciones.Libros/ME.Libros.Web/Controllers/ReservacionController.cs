using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ME.Libros.Dominio.General;
using ME.Libros.EF;
using ME.Libros.Repositorios;
using ME.Libros.Servicios.General;
using ME.Libros.Web.Helpers;
using ME.Libros.Web.Models;

namespace ME.Libros.Web.Controllers
{
    public class ReservacionController : Controller
    {
        public VentaService VentaService { get; set; }
        public ClienteService ClienteService { get; set; }

        public ReservacionController()
        {
            var modelContainer = new ModelContainer();
            VentaService = new VentaService(new EntidadRepository<VentaDominio>(modelContainer));
            ClienteService = new ClienteService(new EntidadRepository<ClienteDominio>(modelContainer));
            ViewBag.Title = "Reservaciones";
            ViewBag.MenuId = 20;
        }

        // GET: Reservacion
        public ActionResult Index()
        {
            ViewBag.Id = TempData["Id"];
            ViewBag.Mensaje = TempData["Mensaje"];

            var ventas = new List<VentaViewModel>();
            using (VentaService)
            {
                //ventas.AddRange(VentaService.Listar()
                //    .OrderBy(v => v.FechaVenta)
                //    .ToList()
                //    .Select(v => new VentaViewModel(v)));
            }

            return View(ventas);
        }

        public ActionResult Crear()
        {
            var ventaViewModel = new VentaViewModel();
            PrepareModel(ventaViewModel);

            return View(ventaViewModel);
        }

        [HttpPost]
        public ActionResult Crear(VentaViewModel ventaViewModel)
        {
            //ModelState.RemoveFor<VentaViewModel>(v => v.Cliente);

            PrepareModel(ventaViewModel);

            return View(ventaViewModel);
        }

        public ActionResult Modificar()
        {
            var ventaViewModel = new VentaViewModel();
            PrepareModel(ventaViewModel);

            return View(ventaViewModel);
        }

        public JsonResult Eliminar()
        {
            return new JsonResult
            {
                Data = new { Success = ModelState.IsValid, Errors = ModelState.GetErrors() },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        #region Private Methods

        private void PrepareModel(VentaViewModel ventaViewModel)
        {
            //ventaViewModel.Clientes = new SelectList(ClienteService.Listar()
            //    .ToList()
            //    .Select(c => new ClienteViewModel(c)), "Id", "Nombre");
            ventaViewModel.Clientes = new SelectList(ClienteService.Listar()
                .ToList()
                .Select(c => new { Id = c.Id, Text = c.Id + " - " + c.Cuil }), "Id", "Text");
        }

        #endregion
    }
}