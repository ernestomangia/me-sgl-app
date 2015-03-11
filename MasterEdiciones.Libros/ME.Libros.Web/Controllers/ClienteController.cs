using System;
using System.Linq;
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
        [HttpGet]
        public PartialViewResult Index()
        {
            var clientes = new List<ClienteViewModel>();
            try
            {
                using (ClienteService)
                {
                    clientes.AddRange(ClienteService.Listar().Select(c => new ClienteViewModel(c)));
                }
            }
            catch (Exception ex)
            {
                // Loguear
            }

            return PartialView(clientes);
        }

        [HttpGet]
        public PartialViewResult Crear()
        {
            var model = new ClienteViewModel();
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult Crear(ClienteViewModel model)
        {
            var exito = false;
            var mensajeError = "";
            try
            {
                using (ClienteService)
                {
                    var id = ClienteService.Guardar(new ClienteDTO
                    {
                        FechaAlta = DateTime.Now,
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
                    exito = id > 0;
                }
            }
            catch (Exception ex)
            {
                mensajeError = ex.Message;
            }

            return new JsonResult
            {
                Data = new { success = exito, mensaje = mensajeError }
            };
        }

        [HttpGet]
        public JsonResult Eliminar(int id)
        {
            var exito = false;
            var mensajeError = "";
            try
            {
                using (ClienteService)
                {
                    ClienteService.Eliminar(ClienteService.GetPorId(id));
                }
                exito = true;
            }
            catch (Exception ex)
            {
                mensajeError = ex.Message;
            }

            return new JsonResult
            {
                Data = new { success = exito, mensaje = mensajeError },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public PartialViewResult Modificar(int id)
        {
            using (ClienteService)
            {
                var clienteDominio = ClienteService.GetPorId(id);

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

            // Handle and log error
        }

        [HttpPost]
        public JsonResult Modificar(ClienteViewModel clienteViewModel)
        {
            var exito = false;
            var mensajeError = string.Empty;

            using (ClienteService)
            {
                var id = ClienteService.Guardar(new ClienteDTO
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
                exito = id > 0;
            }

            return new JsonResult
            {
                Data = new { success = exito, mensaje = mensajeError },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public PartialViewResult Detalle(int id)
        {
            using (ClienteService)
            {
                var clienteDominio = ClienteService.GetPorId(id);

                var clienteViewModel = new ClienteViewModel
                {
                    Id = clienteDominio.Id,
                    Nombre = clienteDominio.Nombre,
                    Apellido = clienteDominio.Apellido,
                    Cuil = clienteDominio.Cuil,
                    Barrio = clienteDominio.Barrio,
                    Direccion = clienteDominio.Direccion,
                    Localidad = clienteDominio.Localidad.Nombre,
                    Sexo = clienteDominio.Sexo,
                    Email = clienteDominio.Email,
                    TelefonoFijo = clienteDominio.TelefonoFijo,
                    Celular = clienteDominio.Celular
                };

                return PartialView(clienteViewModel);
            }

            // handle try catch and log
        }
    }
}