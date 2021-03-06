﻿using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

using ME.Libros.EF;
using ME.Libros.Servicios.General;
using ME.Libros.Web.Extensions;
using ME.Libros.Web.Models;
using ME.Libros.Dominio.General;
using ME.Libros.Repositorios;

namespace ME.Libros.Web.Controllers
{
    public class ClienteController : BaseController<ClienteDominio>
    {
        public ClienteService ClienteService { get; set; }
        public ProvinciaService ProvinciaService { get; set; }
        public LocalidadService LocalidadService { get; set; }
        public IvaService IvaService { get; set; }

        public ClienteController()
        {
            var modelContainer = new ModelContainer();
            ClienteService = new ClienteService(new EntidadRepository<ClienteDominio>(modelContainer));
            ProvinciaService = new ProvinciaService(new EntidadRepository<ProvinciaDominio>(modelContainer));
            LocalidadService = new LocalidadService(new EntidadRepository<LocalidadDominio>(modelContainer));
            IvaService = new IvaService(new EntidadRepository<IvaDominio>(modelContainer));
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
            clienteViewModel.Codigo = ClienteService.GetCodigoCorrelativo();

            return View(clienteViewModel);
        }

        [HttpPost]
        public ActionResult Crear(ClienteViewModel clienteViewModel)
        {
            if (!ModelState.IsValid)
            {
                PrepareModel(clienteViewModel);
                return View(clienteViewModel);
            }

            var clienteDominio = new ClienteDominio
            {
                FechaAlta = DateTime.Now,
                Codigo = clienteViewModel.Codigo,
                Nombre = clienteViewModel.Nombre,
                Apellido = clienteViewModel.Apellido,
                Cuil = clienteViewModel.Cuil,
                FechaNacimiento = clienteViewModel.FechaNacimiento.HasValue ? clienteViewModel.FechaNacimiento.Value : (DateTime?)null,
                Direccion = clienteViewModel.Direccion,
                Comentario = clienteViewModel.Comentario,
                Email = clienteViewModel.Email,
                TelefonoFijo = clienteViewModel.TelefonoFijo,
                Celular = clienteViewModel.Celular,
                Celular2 = clienteViewModel.Celular2,
                Localidad = LocalidadService.GetPorId(clienteViewModel.LocalidadId),
                Iva = IvaService.GetPorId(clienteViewModel.IvaId)
            };

            long resultado = 0;
            try
            {
                using (ClienteService)
                {
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
                        TempData["Id"] = clienteDominio.Codigo;
                        TempData["Mensaje"] = string.Format(Messages.EntidadNueva, Messages.ElCliente, clienteDominio.Codigo);
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;
                if (sqlException != null && sqlException.Number == 2601)
                {
                    ModelState.AddModelError("Error", string.Format(ErrorMessages.CuilRepetido, clienteViewModel.Cuil, "cliente"));
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

            if (resultado == 0)
            {
                PrepareModel(clienteViewModel);
            }

            return resultado > 0
                ? (ActionResult)RedirectToAction("Index")
                : View(clienteViewModel);
        }

        [HttpGet]
        public JsonResult Eliminar(int id, string redirectUrl)
        {
            var isRedirect = !string.IsNullOrEmpty(redirectUrl);

            try
            {
                using (ClienteService)
                {
                    var clienteDominio = ClienteService.GetPorId(id);
                    ClienteService.Eliminar(clienteDominio);

                    if (isRedirect)
                    {
                        TempData["Id"] = clienteDominio.Codigo;
                        TempData["Mensaje"] = string.Format(Messages.EntidadEliminada, Messages.ElCliente, clienteDominio.Codigo);
                    }
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

            return new JsonResult
            {
                Data = new
                {
                    Success = ModelState.IsValid,
                    Errors = ModelState.GetErrors(),
                    isRedirect,
                    redirectUrl
                },
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
            if (!ModelState.IsValid)
            {
                PrepareModel(clienteViewModel);
                return View(clienteViewModel);
            }

            long resultado = 0;

            try
            {
                using (ClienteService)
                {
                    var clienteDominio = ClienteService.GetPorId(clienteViewModel.Id);
                    clienteDominio.Nombre = clienteViewModel.Nombre;
                    clienteDominio.Apellido = clienteViewModel.Apellido;
                    clienteDominio.Cuil = clienteViewModel.Cuil;
                    clienteDominio.FechaNacimiento = clienteViewModel.FechaNacimiento.HasValue
                        ? clienteViewModel.FechaNacimiento.Value
                        : (DateTime?)null;
                    clienteDominio.Direccion = clienteViewModel.Direccion;
                    clienteDominio.Comentario = clienteViewModel.Comentario;
                    clienteDominio.Localidad = LocalidadService.GetPorId(clienteViewModel.LocalidadId);
                    clienteDominio.TelefonoFijo = clienteViewModel.TelefonoFijo;
                    clienteDominio.Celular = clienteViewModel.Celular;
                    clienteDominio.Celular2 = clienteViewModel.Celular2;
                    clienteDominio.Email = clienteViewModel.Email;
                    clienteDominio.Iva = IvaService.GetPorId(clienteViewModel.IvaId);

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
                        TempData["Id"] = clienteDominio.Codigo;
                        TempData["Mensaje"] = string.Format(Messages.EntidadModificada, Messages.ElCliente, clienteDominio.Codigo);
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;
                if (sqlException != null && sqlException.Number == 2601)
                {
                    ModelState.AddModelError("Error",
                        string.Format(ErrorMessages.CuilRepetido, clienteViewModel.Cuil));
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

        public ActionResult Get(string query)
        {
            var clientes = new List<AutocompleteViewModel>();
            using (ClienteService)
            {
                clientes.AddRange(ClienteService.ListarPorNombre(query)
                    .ToList()
                    .Select(c => new AutocompleteViewModel
                    {
                        Id = c.Id,
                        Name = string.Format("{0} {1}", c.Nombre, c.Apellido),
                        Description = string.Format("{0} <br/> {1} - {2}, {3}", c.Iva.Codigo == 3 ? c.Cuil.ToString() : c.Dni.ToString(), c.Direccion, c.Localidad.Nombre, c.Localidad.Provincia.Nombre)
                    }));
            }

            return Json(clientes, JsonRequestBehavior.AllowGet);
        }

        #region Private Methods

        private void PrepareModel(ClienteViewModel clienteViewModel)
        {
            clienteViewModel.Provincias = new SelectList(ProvinciaService.Listar()
                .Select(p => new ProvinciaViewModel(p))
                .ToList(), "Id", "Nombre");

            var localidades = new List<LocalidadViewModel>();
            if (clienteViewModel.ProvinciaId > 0)
            {
                localidades.AddRange(LocalidadService.Listar(l => l.Provincia.Id == clienteViewModel.ProvinciaId)
                    .ToList()
                    .Select(l => new LocalidadViewModel(l)));
            }
            clienteViewModel.Localidades = new SelectList(localidades, "Id", "Nombre");

            clienteViewModel.Ivas = new SelectList(IvaService.Listar()
                .Select(p => new IvaViewModel(p))
                .ToList(), "Id", "Nombre");
        }

        #endregion
    }
}