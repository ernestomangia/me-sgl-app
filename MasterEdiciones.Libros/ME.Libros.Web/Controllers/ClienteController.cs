using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

using ME.Libros.EF;
using ME.Libros.Servicios.General;
using ME.Libros.Web.Helpers;
using ME.Libros.Web.Models;
using ME.Libros.Dominio.General;
using ME.Libros.Repositorios;

namespace ME.Libros.Web.Controllers
{
    public class ClienteController : BaseController<ClienteDominio>
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
            ViewBag.MenuId = 1;
            ViewBag.Title = "Clientes";
            Service = new ClienteService(new EntidadRepository<ClienteDominio>(modelContainer));
        }

        // GET: Cliente
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Id = TempData["Id"];
            ViewBag.Mensaje = TempData["Mensaje"];

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

        [HttpGet]
        public ActionResult Crear()
        {
            var clienteViewModel = new ClienteViewModel();
            PrepareModel(clienteViewModel);

            return View(clienteViewModel);
        }

        [HttpPost]
        public ActionResult Crear(ClienteViewModel clienteViewModel)
        {
            long resultado = 0;
            ModelState.RemoveFor<ClienteViewModel>(c => c.Localidad.Nombre);
            ModelState.RemoveFor<ClienteViewModel>(c => c.Localidad.ProvinciaId);
            if (ModelState.IsValid)
            {
                try
                {
                    using (ClienteService)
                    {
                        var clienteDominio = new ClienteDominio
                                                 {
                                                     FechaAlta = DateTime.Now,
                                                     Nombre = clienteViewModel.Nombre,
                                                     Apellido = clienteViewModel.Apellido,
                                                     Cuil = clienteViewModel.Cuil,
                                                     FechaNacimiento = clienteViewModel.FechaNacimiento.HasValue ? clienteViewModel.FechaNacimiento.Value : (DateTime?)null,
                                                     Direccion = clienteViewModel.Direccion,
                                                     Numero = clienteViewModel.Numero,
                                                     Comentario = clienteViewModel.Comentario,
                                                     Sexo = clienteViewModel.Sexo,
                                                     Email = clienteViewModel.Email,
                                                     TelefonoFijo = clienteViewModel.TelefonoFijo,
                                                     Celular = clienteViewModel.Celular,
                                                     Localidad = LocalidadService.GetPorId(clienteViewModel.Localidad.Id),
                                                 };
                        resultado = ClienteService.Guardar(clienteDominio);
                        if (resultado <= 0)
                        {
                            foreach (var error in ClienteService.ModelError)
                            {
                                ModelState.AddModelError(error.Key, error.Value);
                            }
                        }
                        else
                        {
                            TempData["Id"] = clienteDominio.Id;
                            TempData["Mensaje"] = string.Format(Messages.EntidadNueva, Messages.ElCliente, clienteDominio.Id);
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    var sqlException = ex.GetBaseException() as SqlException;
                    if (sqlException != null && sqlException.Number == 2601)
                    {
                        ModelState.AddModelError("Error", string.Format(ErrorMessages.CuilRepetido, clienteViewModel.Cuil));
                    }
                    else
                    {
                        ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
                }
            }

            if (resultado == 0)
            {
                PrepareModel(clienteViewModel);
            }

            return resultado > 0
                ? (ActionResult)RedirectToAction("Index")
                : View(clienteViewModel);
        }

        [HttpGet]
        public JsonResult Eliminar(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (ClienteService)
                    {
                        ClienteService.Eliminar(ClienteService.GetPorId(id));
                    }
                }
                catch (DbUpdateException ex)
                {
                    var sqlException = ex.GetBaseException() as SqlException;

                    if (sqlException != null && sqlException.Number == 547)
                    {
                        ModelState.AddModelError("Error", string.Format(ErrorMessages.DatosAsociados, Messages.ElCliente));
                    }
                    else
                    {
                        ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
                }
            }

            return new JsonResult
            {
                Data = new { Success = ModelState.IsValid, Errors = ModelState.GetErrors() },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult Modificar(int id)
        {
            ClienteViewModel clienteViewModel;
            using (ClienteService)
            {
                clienteViewModel = new ClienteViewModel(ClienteService.GetPorId(id));
            }
            PrepareModel(clienteViewModel);

            return View(clienteViewModel);
        }

        [HttpPost]
        public ActionResult Modificar(ClienteViewModel clienteViewModel)
        {
            long resultado = 0;
            ModelState.RemoveFor<ClienteViewModel>(c => c.Localidad.Nombre);
            ModelState.RemoveFor<ClienteViewModel>(c => c.Localidad.ProvinciaId);
            
            if (ModelState.IsValid)
            {
                using (ClienteService)
                {
                    var clienteDominio = ClienteService.GetPorId(clienteViewModel.Id);
                    clienteDominio.Nombre = clienteViewModel.Nombre;
                    clienteDominio.Apellido = clienteViewModel.Apellido;
                    clienteDominio.Cuil = clienteViewModel.Cuil;
                    clienteDominio.FechaNacimiento = clienteViewModel.FechaNacimiento.HasValue ? clienteViewModel.FechaNacimiento.Value : (DateTime?)null;
                    clienteDominio.Direccion = clienteViewModel.Direccion;
                    clienteDominio.Numero = clienteViewModel.Numero;
                    clienteDominio.Comentario = clienteViewModel.Comentario;
                    clienteDominio.Localidad = LocalidadService.GetPorId(clienteViewModel.Localidad.Id);
                    clienteDominio.Sexo = clienteViewModel.Sexo;
                    clienteDominio.TelefonoFijo = clienteViewModel.TelefonoFijo;
                    clienteDominio.Celular = clienteViewModel.Celular;
                    clienteDominio.Email = clienteViewModel.Email;

                    resultado = ClienteService.Guardar(clienteDominio);
                    if (resultado <= 0)
                    {
                        foreach (var error in ClienteService.ModelError)
                        {
                            ModelState.AddModelError(error.Key, error.Value);
                        }
                    }
                    else
                    {
                        TempData["Id"] = clienteViewModel.Id;
                        TempData["Mensaje"] = string.Format(Messages.EntidadModificada, Messages.ElCliente, clienteViewModel.Id);
                    }
                }
            }

            if (resultado == 0)
            {
                PrepareModel(clienteViewModel);
            }

            return resultado > 0
                ? (ActionResult)RedirectToAction("Index")
                : View(clienteViewModel);
        }

        [HttpGet]
        public ActionResult Detalle(int id)
        {
            ClienteViewModel clienteViewModel;
            using (ClienteService)
            {
                clienteViewModel = new ClienteViewModel(ClienteService.GetPorId(id));
            }

            PrepareModel(clienteViewModel);

            return View(clienteViewModel);
        }

        #region Private Methods

        private void PrepareModel(ClienteViewModel clienteViewModel)
        {
            clienteViewModel.Provincias = new SelectList(ProvinciaService.Listar()
                .Select(p => new ProvinciaViewModel(p))
                .ToList(), "Id", "Nombre");

            var localidades = new List<LocalidadViewModel>();
            if (clienteViewModel.Localidad.Provincia.Id > 0)
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