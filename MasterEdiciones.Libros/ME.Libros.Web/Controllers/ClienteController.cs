using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;

using ME.Libros.EF;
using ME.Libros.Servicios.General;
using ME.Libros.Web.Models;
using ME.Libros.Dominio.General;
using ME.Libros.Repositorios;

namespace ME.Libros.Web.Controllers
{
    public class ClienteController : BaseController
    {
        public ClienteService ClienteService { get; set; }
        private ProvinciaService ProvinciaService { get; set; }
        private LocalidadService LocalidadService { get; set; }

        //public bool Probar<T>(T entity, Action<T> action)
        //{
        //    try
        //    {
        //        if (this.ModelState.IsValid)
        //        {
        //            action(entity);
        //            return true;
        //        }
        //    }
        //    catch (DbEntityValidationException DBEx)
        //    {
        //        foreach (var error in DBEx.EntityValidationErrors.SelectMany(validationError => validationError.ValidationErrors))
        //        {
        //            ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("Ha ocurrido un error. Por favor comuníquese con el administrador.", ex.Message);
        //    }

        //    return false;
        //}

        public ClienteController()
        {
            var modelContainer = new ModelContainer();
            ClienteService = new ClienteService(new EntidadRepository<ClienteDominio>(modelContainer));
            ProvinciaService = new ProvinciaService(new EntidadRepository<ProvinciaDominio>(modelContainer));
            LocalidadService = new LocalidadService(new EntidadRepository<LocalidadDominio>(modelContainer));
            ViewBag.MenuId = 1;
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
                clientes.AddRange(ClienteService.Listar().ToList().Select(c => new ClienteViewModel(c)));
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
                                                     Barrio = clienteViewModel.Barrio,
                                                     Direccion = clienteViewModel.Direccion,
                                                     Manzana = clienteViewModel.Manzana,
                                                     Piso = clienteViewModel.Piso,
                                                     Sexo = clienteViewModel.Sexo,
                                                     Email = clienteViewModel.Email,
                                                     TelefonoFijo = clienteViewModel.TelefonoFijo,
                                                     Celular = clienteViewModel.Celular,
                                                     Localidad =
                                                         LocalidadService.GetPorId(
                                                             clienteViewModel.Localidad.Id),
                                                 };
                        Probar(clienteDominio, ClienteService.Guardar2);

                        clienteViewModel.Id = ClienteService.Guardar(clienteDominio);
                        if (clienteViewModel.Id <= 0)
                        {
                            foreach (var error in ClienteService.ModelError)
                            {
                                ModelState.AddModelError(error.Key, error.Value);
                            }
                        }
                        else
                        {
                            TempData["Id"] = clienteViewModel.Id;
                            TempData["Mensaje"] = string.Format(Messages.EntidadNueva, "El cliente");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log
                    ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
                }
            }

            PrepareModel(clienteViewModel);

            return clienteViewModel.Id > 0
                    ? (ActionResult)RedirectToAction("Index")
                    : View(clienteViewModel);
        }

        [HttpGet]
        public JsonResult Eliminar(int id)
        {
            var resultadoViewModel = new ResultadoViewModel();
            if (ModelState.IsValid)
            {
                try
                {
                    using (ClienteService)
                    {
                        ClienteService.Eliminar(ClienteService.GetPorId(id));
                    }
                    resultadoViewModel.Success = true;
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var error in ex.EntityValidationErrors.SelectMany(validationError => validationError.ValidationErrors))
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                    //resultadoViewModel.Messages.Add("Error", ErrorMessages.ErrorSistema);
                }
                catch (DbUpdateException ex)
                {
                    var sqlException = ex.GetBaseException() as SqlException;

                    if (sqlException != null)
                    {
                        resultadoViewModel.Messages.Add("Error", sqlException.Number == 547 ? ErrorMessages.EliminarCliente : ErrorMessages.ErrorSistema);
                    }
                    else
                    {
                        resultadoViewModel.Messages.Add("Error", ErrorMessages.ErrorSistema);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);

                    //resultadoViewModel.Messages.Add("Error", ErrorMessages.ErrorSistema);
                }
            }
            //else
            //{
            //    foreach (var error in ModelState)
            //    {
            //        resultadoViewModel.Messages.Add(error.Key, error.Value);
            //    }
            //}


            return Json(ModelState, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult Modificar(int id)
        {
            var clienteViewModel = new ClienteViewModel();
            try
            {
                using (ClienteService)
                {
                    var clienteDominio = ClienteService.GetPorId(id);
                    clienteViewModel = new ClienteViewModel(clienteDominio);
                }
            }
            catch (Exception ex)
            {
                // Log
                ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
            }
            PrepareModel(clienteViewModel);

            return View(clienteViewModel);
        }

        [HttpPost]
        public ActionResult Modificar(ClienteViewModel clienteViewModel)
        {
            long id = 0;
            if (ModelState.IsValid)
            {
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

                    id = ClienteService.Guardar(clienteDominio);
                    if (id <= 0)
                    {
                        foreach (var error in ClienteService.ModelError)
                        {
                            ModelState.AddModelError(error.Key, error.Value);
                        }
                    }
                    else
                    {
                        TempData["Id"] = clienteViewModel.Id;
                        TempData["Mensaje"] = string.Format(Messages.EntidadModificada, "El cliente Nº " + id);
                    }
                }
            }

            PrepareModel(clienteViewModel);

            return clienteViewModel.Id > 0
                    ? (ActionResult)RedirectToAction("Index")
                    : View(clienteViewModel);
        }

        [HttpGet]
        public ActionResult Detalle(int id)
        {
            using (ClienteService)
            {
                var clienteDominio = ClienteService.GetPorId(id);
                var clienteViewModel = new ClienteViewModel(clienteDominio);
                PrepareModel(clienteViewModel);

                return View(clienteViewModel);
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