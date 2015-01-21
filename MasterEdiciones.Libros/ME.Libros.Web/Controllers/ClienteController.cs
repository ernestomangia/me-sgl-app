using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Collections.Generic;
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
        // GET: Cliente
        public ActionResult Index()
        {
            ViewBag.Mensaje = TempData["Mensaje"] ?? "";
            var modelContainer = new ModelContainer();
            var clientes = new List<ClienteViewModel>();
            using (var servicio = new ClienteService(new EntidadRepository<ClienteDominio>(modelContainer)))
            {
                clientes.AddRange(servicio.Listar().Select(c => new ClienteViewModel
                                                                {
                                                                    Id = c.Id,
                                                                    Nombre = c.Nombre,
                                                                    Apellido = c.Apellido,
                                                                    Cuil = c.Cuil
                                                                }));

            }
            //var clientes = new List<ClienteViewModel>
            //                   {
            //                       new ClienteViewModel
            //                           {
            //                               Id = "100",
            //                               Nombre = "Juan",
            //                               Apellido = "Perez",
            //                               Cuil = "20-00000000-9"
            //                           },
            //                       new ClienteViewModel
            //                           {
            //                               Id = "101",
            //                               Nombre = "Pepe",
            //                               Apellido = "Sanchez",
            //                               Cuil = "20-00000000-9"
            //                           },
            //                        new ClienteViewModel
            //                           {
            //                               Id = "102",
            //                               Nombre = "Denis",
            //                               Apellido = "Gonzales",
            //                               Cuil = "20-00000000-9"
            //                           }
            //                   };
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
            var modelContainer = new ModelContainer();
            using (var servicio = new ClienteService(new EntidadRepository<ClienteDominio>(modelContainer)))
            {
                servicio.Crear(new ClienteDominio
                {
                    FechaAlta = DateTime.Now,
                    Codigo = "1000",
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    Cuil = model.Cuil,
                    Barrio = model.Barrio,
                    Direccion = model.Direccion,
                    Localidad = new LocalidadDominio
                    {
                        FechaAlta = DateTime.Now,
                        Nombre = "Paraná",
                        Provincia = new ProvinciaDominio
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