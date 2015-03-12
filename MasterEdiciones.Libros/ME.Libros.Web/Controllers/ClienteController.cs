using System;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Collections.Generic;

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
        private ProvinciaService ProvinciaService { get; set; }
        private LocalidadService LocalidadService { get; set; }

        public ClienteController()
        {
            var modelContainer = new ModelContainer();
            ClienteService = new ClienteService(new EntidadRepository<ClienteDominio>(modelContainer));
            ProvinciaService = new ProvinciaService(new EntidadRepository<ProvinciaDominio>(modelContainer));
            LocalidadService = new LocalidadService(new EntidadRepository<LocalidadDominio>(modelContainer));
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
                    clientes.AddRange(ClienteService.Listar().ToList().Select(c => new ClienteViewModel(c)));
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
            var model = new ClienteViewModel
            {
                Provincias = new SelectList(ProvinciaService.Listar().Select(p => new ProvinciaViewModel(p)).ToList(), "Id", "Nombre"),
                Localidades = new SelectList(new List<LocalidadViewModel>())
            };

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult Crear(ClienteViewModel model)
        {
            var exito = false;
            var mensajeError = new Dictionary<string, string>();
            try
            {
                using (ClienteService)
                {
                    var id = ClienteService.Guardar(new ClienteDominio
                                                        {
                                                            FechaAlta = DateTime.Now,
                                                            Nombre = model.Nombre,
                                                            Apellido = model.Apellido,
                                                            Cuil = model.Cuil,
                                                            Barrio = model.Barrio,
                                                            Direccion = model.Direccion,
                                                            Localidad = LocalidadService.GetPorId(model.LocalidadId),
                                                            //new LocalidadDominio
                                                            //            {
                                                            //                FechaAlta = DateTime.Now,
                                                            //                Nombre = "Paraná",
                                                            //                Provincia = new ProvinciaDominio
                                                            //                            {
                                                            //                                FechaAlta = DateTime.Now,
                                                            //                                Nombre = "Entre Rios"
                                                            //                            }
                                                            //            },
                                                            Sexo = model.Sexo,
                                                            Email = model.Email,
                                                            TelefonoFijo = model.TelefonoFijo,
                                                            Celular = model.Celular
                                                        });
                    if (id > 0)
                    {
                        exito = true;
                    }
                    else
                    {
                        mensajeError = ClienteService.ModelError;
                    }
                }
            }
            catch (Exception ex)
            {
                mensajeError.Add("Error del sistema.", ex.Message);
            }

            return new JsonResult
            {
                Data = new { success = exito, mensajes = mensajeError }
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
                var clienteViewModel = new ClienteViewModel(clienteDominio)
                {
                    Provincias = new SelectList(ProvinciaService.Listar().Select(p => new ProvinciaViewModel(p)).ToList(), "Id", "Nombre"),
                    Localidades = new SelectList(LocalidadService.Listar(l => l.Provincia.Id == clienteDominio.Localidad.Provincia.Id).ToList().Select(l => new LocalidadViewModel(l)), "Id", "Nombre")
                };

                return PartialView(clienteViewModel);
            }

            // Handle and log error
        }

        [HttpPost]
        public JsonResult Modificar(ClienteViewModel clienteViewModel)
        {
            var exito = false;
            var mensajeError = new Dictionary<string, string>();

            using (ClienteService)
            {
                var clienteDominio = ClienteService.GetPorId(clienteViewModel.Id);
                clienteDominio.Nombre = clienteViewModel.Nombre;
                clienteDominio.Apellido = clienteViewModel.Apellido;
                clienteDominio.Cuil = clienteViewModel.Cuil;
                clienteDominio.Barrio = clienteViewModel.Barrio;
                clienteDominio.Direccion = clienteViewModel.Direccion;
                clienteDominio.Localidad = LocalidadService.GetPorId(clienteViewModel.LocalidadId);
                clienteDominio.Sexo = clienteViewModel.Sexo;
                clienteDominio.Email = clienteViewModel.Email;
                clienteDominio.TelefonoFijo = clienteViewModel.TelefonoFijo;
                clienteDominio.Celular = clienteViewModel.Celular;

                var id = ClienteService.Guardar(clienteDominio);
                if (id > 0)
                {
                    exito = true;
                }
                else
                {
                    mensajeError = ClienteService.ModelError;
                }
            }
            return new JsonResult
            {
                Data = new { success = exito, mensajes = mensajeError },
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
                    Localidad = new LocalidadViewModel(clienteDominio.Localidad),
                    Sexo = clienteDominio.Sexo,
                    Email = clienteDominio.Email,
                    TelefonoFijo = clienteDominio.TelefonoFijo,
                    Celular = clienteDominio.Celular
                };

                return PartialView(clienteViewModel);
            }

            // handle try catch and log
        }

        public JsonResult ListarLocalidades(int id)
        {
            var localidades = new List<LocalidadViewModel>();
            using (LocalidadService)
            {
                localidades.AddRange(LocalidadService.Listar(l => l.Provincia.Id == id).ToList().Select(l => new LocalidadViewModel(l)));
            }
            
            return new JsonResult
            {
                Data = localidades,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}