using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Collections.Generic;

using ME.Libros.DTO.General;
using ME.Libros.EF;
using ME.Libros.Servicios.General;
using ME.Libros.Utils.Enums;
using ME.Libros.Web.Models;
using ME.Libros.Dominio.General;
using ME.Libros.Repositorios;

namespace ME.Libros.Web.Controllers
{
    public class ClienteController : Controller
    {
        public ClienteService ClienteService { get; set; }

        public ClienteController()
        {
            var modelContainer = new ModelContainer();
            ClienteService = new ClienteService(new EntidadRepository<ClienteDominio>(modelContainer));
        }

        // GET: Cliente
        public ActionResult Index()
        {
            ViewBag.Mensaje = TempData["Mensaje"] ?? "";
            var clientes = new List<ClienteViewModel>();
            using (ClienteService)
            {
                clientes.AddRange(ClienteService.Listar().Select(c => new ClienteViewModel(c)));
            }
            
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
            using (ClienteService)
            {
                ClienteService.Guardar(new ClienteDTO
                {
                    FechaAlta = DateTime.Now,
                    Codigo = "1000",
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    Cuil = model.Cuil,
                    Barrio = model.Barrio,
                    Direccion = model.Direccion,
                    Localidad = new LocalidadDTO
                    {
                        FechaAlta = DateTime.Now,
                        Nombre = "Paraná",
                        Provincia = new ProvinciaDTO
                        {
                            FechaAlta = DateTime.Now,
                            Nombre = "Entre Rios"
                        }
                    },
                    Sexo = Sexo.Masculino
                });
            }

            TempData["Mensaje"] = "El cliente fue creado exitosamente";
            
            return RedirectToAction("Index", "Administracion");
        }

        public ActionResult Eliminar(long idCliente)
        {
            return null;
        }
    }
}