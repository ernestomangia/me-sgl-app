using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using ME.Libros.EF;
using ME.Libros.Servicios.General;
using ME.Libros.Web.Models;
using ME.Libros.Dominio.General;
using ME.Libros.Repositorios;

namespace ME.Libros.Web.Controllers
{
    using System.Runtime.InteropServices;

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
            var clienteViewModel = new ClienteViewModel();
            PrepareModel(clienteViewModel);

            return PartialView(clienteViewModel);
        }

        [HttpPost]
        public JsonResult Crear(ClienteViewModel clienteViewModel)
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
                                                            Nombre = clienteViewModel.Nombre,
                                                            Apellido = clienteViewModel.Apellido,
                                                            Cuil = clienteViewModel.Cuil,
                                                            Barrio = clienteViewModel.Barrio,
                                                            Direccion = clienteViewModel.Direccion,
                                                            Manzana = clienteViewModel.Manzana,
                                                            Piso = clienteViewModel.Piso,
                                                            Sexo = clienteViewModel.Sexo,
                                                            Email = clienteViewModel.Email,
                                                            TelefonoFijo = clienteViewModel.TelefonoFijo,
                                                            Celular = clienteViewModel.Celular,
                                                            Localidad = LocalidadService.GetPorId(clienteViewModel.Localidad.Id),
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
                var clienteViewModel = new ClienteViewModel(clienteDominio);
                PrepareModel(clienteViewModel);

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
                clienteDominio.Localidad = LocalidadService.GetPorId(clienteViewModel.Localidad.Id);
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
                var clienteViewModel = new ClienteViewModel(clienteDominio);
                PrepareModel(clienteViewModel);

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

        #region Private Methods

        private void PrepareModel(ClienteViewModel clienteViewModel)
        {
            clienteViewModel.Provincias = new SelectList(this.ProvinciaService.Listar().Select(p => new ProvinciaViewModel(p)).ToList(),
                                            "Id",
                                            "Nombre");

            var localidades = new List<LocalidadViewModel>();
            if (clienteViewModel.Id > 0)
            {
                localidades.AddRange(LocalidadService.Listar(l => l.Provincia.Id == clienteViewModel.Localidad.Provincia.Id)
                                                                    .ToList()
                                                                    .Select(l => new LocalidadViewModel(l)));
            }
            clienteViewModel.Localidades = new SelectList(localidades, "Id", "Nombre");
        }

        #endregion
    }
}