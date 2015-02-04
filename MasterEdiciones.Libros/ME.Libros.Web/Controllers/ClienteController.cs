using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Collections.Generic;

using ME.Libros.DTO.General;
using ME.Libros.EF;
using ME.Libros.Servicios.General;
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

        [HttpGet]
        public PartialViewResult Crear()
        {
            var model = new ClienteViewModel();
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Crear(ClienteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (ClienteService)
            {
                try
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
                        Sexo = model.Sexo,
                        Email = model.Email,
                        TelefonoFijo = model.TelefonoFijo,
                        Celular = model.Celular
                    });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Error al guardar el Cliente", "El cliente no se guardó.");
                    
                    return View(model);
                }
            }

            TempData["Mensaje"] = "El cliente fue creado exitosamente";

            return RedirectToAction("Index", "Administracion");
        }

        [HttpGet]
        public ActionResult Eliminar(int idCliente)
        {
            using (ClienteService)
            {
                ClienteService.Eliminar(ClienteService.GetPorId(idCliente));
            }

            return null;
        }

        [HttpGet]
        public PartialViewResult Modificar(int idCliente)
        {
            using (ClienteService)
            {
                var clienteDominio = ClienteService.GetPorId(idCliente);

                var clienteViewModel = new ClienteViewModel(new ClienteDTO
                {
                    Id = clienteDominio.Id,
                    FechaAlta = clienteDominio.FechaAlta,
                    Nombre = clienteDominio.Nombre,
                    Apellido = clienteDominio.Apellido,
                    Cuil = clienteDominio.Cuil,
                    Barrio = clienteDominio.Barrio,
                    Direccion = clienteDominio.Direccion,
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
                    Sexo = clienteDominio.Sexo,
                    Email = clienteDominio.Email,
                    TelefonoFijo = clienteDominio.TelefonoFijo,
                    Celular = clienteDominio.Celular
                });

                return PartialView(clienteViewModel);
            }
        }

        [HttpPost]
        public ActionResult Modificar(ClienteViewModel clienteViewModel)
        {
            using (ClienteService)
            {
                ClienteService.Guardar(new ClienteDTO
                {
                    Id = clienteViewModel.Id,
                    Nombre = clienteViewModel.Nombre,
                    Apellido = clienteViewModel.Apellido,
                    Cuil = clienteViewModel.Cuil,
                    Barrio = clienteViewModel.Barrio,
                    Direccion = clienteViewModel.Direccion,
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
                    Sexo = clienteViewModel.Sexo,
                    Email = clienteViewModel.Email,
                    TelefonoFijo = clienteViewModel.TelefonoFijo,
                    Celular = clienteViewModel.Celular
                });

                return RedirectToAction("Index", "Administracion");
            }
        }
    }
}