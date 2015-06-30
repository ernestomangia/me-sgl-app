using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

using ME.Libros.Dominio.General;
using ME.Libros.EF;
using ME.Libros.Repositorios;
using ME.Libros.Servicios.General;
using ME.Libros.Utils.Enums;
using ME.Libros.Web.Helpers;
using ME.Libros.Web.Models;

namespace ME.Libros.Web.Controllers
{
    public class CobradorController : BaseController<CobradorDominio>
    {
        public CobradorService CobradorService { get; set; }
        public LocalidadService LocalidadService { get; set; }
        public ProvinciaService ProvinciaService { get; set; }
        public VentaService VentaService { get; set; }

        public CobradorController()
        {
            var modelContainer = new ModelContainer();
            CobradorService = new CobradorService(new EntidadRepository<CobradorDominio>(modelContainer));
            LocalidadService = new LocalidadService(new EntidadRepository<LocalidadDominio>(modelContainer));
            ProvinciaService = new ProvinciaService(new EntidadRepository<ProvinciaDominio>(modelContainer));
            VentaService = new VentaService(new EntidadRepository<VentaDominio>(modelContainer));
            ViewBag.MenuId = 2;
            ViewBag.Title = "Cobradores";
        }

        //
        // GET: /Cobrador/
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Id = TempData["Id"];
            ViewBag.Mensaje = TempData["Mensaje"];

            var cobradores = new List<CobradorViewModel>();
            using (CobradorService)
            {
                cobradores.AddRange(CobradorService.Listar()
                    .OrderBy(c => c.Apellido)
                    .ThenBy(c => c.Nombre)
                    .ToList()
                    .Select(c => new CobradorViewModel(c)));
            }

            return View(cobradores);
        }

        [HttpGet]
        public ActionResult Crear()
        {
            var cobradorViewModel = new CobradorViewModel();
            PrepareModel(cobradorViewModel);

            return View(cobradorViewModel);
        }

        [HttpPost]
        public ActionResult Crear(CobradorViewModel cobradorViewModel)
        {
            long resultado = 0;
            var localidadIds = new List<string>();
            if (!string.IsNullOrEmpty(Request.Form["localidadesAsignadas_dualList"]))
            {
                localidadIds = Request.Form["localidadesAsignadas_dualList"].Split(',').ToList();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (CobradorService)
                    {
                        var cobradorDominio = new CobradorDominio
                        {
                            FechaAlta = DateTime.Now,
                            Nombre = cobradorViewModel.Nombre,
                            Apellido = cobradorViewModel.Apellido,
                            Dni = cobradorViewModel.Dni,
                            PorcentajeComision = cobradorViewModel.PorcentajeComision,
                            Direccion = cobradorViewModel.Direccion,
                            TelefonoFijo = cobradorViewModel.TelefonoFijo,
                            Celular = cobradorViewModel.Celular,
                            Email = cobradorViewModel.Email,
                            Localidad = LocalidadService.GetPorId(cobradorViewModel.LocalidadId),
                            Localidades = new List<LocalidadDominio>(),
                        };

                        foreach (var localidadId in localidadIds)
                        {
                            cobradorDominio.Localidades.Add(LocalidadService.GetPorId((Convert.ToInt64(localidadId))));
                        }

                        resultado = CobradorService.Guardar(cobradorDominio);

                        if (resultado <= 0)
                        {
                            foreach (var error in CobradorService.ModelError)
                            {
                                ModelState.AddModelError(error.Key, error.Value);
                            }
                        }
                        else
                        {
                            TempData["Id"] = cobradorDominio.Id;
                            TempData["Mensaje"] = string.Format(Messages.EntidadNueva, Messages.ElCobrador, cobradorDominio.Id);
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    var sqlException = ex.GetBaseException() as SqlException;
                    if (sqlException != null && sqlException.Number == 2601)
                    {
                        ModelState.AddModelError("Error",
                            string.Format(ErrorMessages.DniRepetido, cobradorViewModel.Dni, "cobrador"));
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
                PrepareModel(cobradorViewModel, localidadIds);
            }

            return resultado > 0
                ? (ActionResult)RedirectToAction("Index")
                : View(cobradorViewModel);
        }

        [HttpGet]
        public ActionResult Modificar(int id)
        {
            CobradorViewModel cobradorViewModel;
            using (CobradorService)
            {
                cobradorViewModel = new CobradorViewModel(CobradorService.GetPorId(id));
            }
            PrepareModel(cobradorViewModel);

            return View(cobradorViewModel);
        }

        [HttpPost]
        public ActionResult Modificar(CobradorViewModel cobradorViewModel)
        {
            if (!ModelState.IsValid)
            {
                PrepareModel(cobradorViewModel);
                return View(cobradorViewModel);
            }

            var localidadIds = new List<string>();
            if (!string.IsNullOrEmpty(Request.Form["localidadesAsignadas_dualList"]))
            {
                localidadIds = Request.Form["localidadesAsignadas_dualList"].Split(',').ToList();
            }
            var nombreLocalidades = string.Empty;
            long resultado = 0;

            try
            {
                var cobradorDominio = CobradorService.GetPorId(cobradorViewModel.Id);
                var i = 0;
                foreach (var localidadAsignada in cobradorDominio.Localidades)
                {
                    if (!localidadIds.Contains(localidadAsignada.Id.ToString()))
                    {
                        if (VentaService.ListarAsQueryable().Any(v => (v.Cliente.Localidad.Id == localidadAsignada.Id
                                                                       && v.Cobrador.Id == cobradorDominio.Id
                                                                       && v.Estado == EstadoVenta.Vigente)))
                        {
                            nombreLocalidades += ", " + localidadAsignada.Nombre;
                            i++;
                        }
                    }
                }

                if (i > 0)
                {
                    nombreLocalidades = nombreLocalidades.Substring(2, nombreLocalidades.Length - 2);

                    if (i == 1)
                    {
                        ModelState.AddModelError("localidad",
                            "La localidad: " + nombreLocalidades +
                            " debe estar asignada ya que existen ventas asociadas al cobrador que intenta modificar");
                    }
                    else if (i > 1)
                    {
                        ModelState.AddModelError("localidades",
                            "Las localidades: " + nombreLocalidades +
                            " deben estar asignadas ya que existen ventas asociadas al cobrador que intenta modificar");
                    }
                }
                else
                {
                    using (CobradorService)
                    {
                        cobradorDominio.Nombre = cobradorViewModel.Nombre;
                        cobradorDominio.Apellido = cobradorViewModel.Apellido;
                        cobradorDominio.Dni = cobradorViewModel.Dni;
                        cobradorDominio.PorcentajeComision = cobradorViewModel.PorcentajeComision;
                        cobradorDominio.Direccion = cobradorViewModel.Direccion;
                        cobradorDominio.TelefonoFijo = cobradorViewModel.TelefonoFijo;
                        cobradorDominio.Celular = cobradorViewModel.Celular;
                        cobradorDominio.Email = cobradorViewModel.Email;
                        cobradorDominio.Localidad = LocalidadService.GetPorId(cobradorViewModel.LocalidadId);
                        cobradorDominio.Localidades.Clear();

                        foreach (var localidad in localidadIds)
                        {
                            cobradorDominio.Localidades.Add(LocalidadService.GetPorId((Convert.ToInt64(localidad))));
                        }

                        resultado = CobradorService.Guardar(cobradorDominio);
                        if (resultado <= 0)
                        {
                            foreach (var error in CobradorService.ModelError)
                            {
                                ModelState.AddModelError(error.Key, error.Value);
                            }
                        }
                        else
                        {
                            TempData["Id"] = cobradorViewModel.Id;
                            TempData["Mensaje"] = string.Format(Messages.EntidadModificada, Messages.ElCobrador,
                                cobradorViewModel.Id);
                        }
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;
                if (sqlException != null && sqlException.Number == 2601)
                {
                    ModelState.AddModelError("Error",
                        string.Format(ErrorMessages.DniRepetido, cobradorViewModel.Dni));
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
                PrepareModel(cobradorViewModel, localidadIds);
            }

            return resultado > 0
                ? (ActionResult)RedirectToAction("Index")
                : View(cobradorViewModel);
        }

        [HttpGet]
        public JsonResult Eliminar(int id)
        {
            try
            {
                using (CobradorService)
                {
                    var cobradorDominio = CobradorService.GetPorId(id);
                    cobradorDominio.Localidades.Clear();
                    CobradorService.Eliminar(cobradorDominio);
                }
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;

                if (sqlException != null && sqlException.Number == 547)
                {
                    ModelState.AddModelError("Error", string.Format(ErrorMessages.DatosAsociados, Messages.ElCobrador));
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
                Data = new { Success = ModelState.IsValid, Errors = ModelState.GetErrors() },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        #region Private Methods

        private void PrepareModel(CobradorViewModel cobradorViewModel, List<string> localidadesAsignadasIds = null)
        {
            cobradorViewModel.LocalidadesNoAsignadas = new List<LocalidadViewModel>();
            var localidades = LocalidadService.Listar().ToList();
            if (localidadesAsignadasIds != null)
            {
                foreach (var id in localidadesAsignadasIds)
                { 
                    cobradorViewModel.LocalidadesAsignadas.Add(new LocalidadViewModel(localidades.First(l => l.Id.ToString() == id)));
                }
            }
            
            foreach (var localidad in localidades.Where(localidad => !cobradorViewModel.LocalidadesAsignadas.Select(l => l.Id).Contains(localidad.Id)))
            {
                // Si la localidad no esta asignada, agregarla a la lista de no asignadas
                cobradorViewModel.LocalidadesNoAsignadas.Add(new LocalidadViewModel(localidad));
            }

            cobradorViewModel.Provincias = new SelectList(ProvinciaService.Listar()
                .Select(p => new ProvinciaViewModel(p))
                .ToList(), "Id", "Nombre");

            var localidadViewModels = new List<LocalidadViewModel>();
            if (cobradorViewModel.ProvinciaId > 0)
            {
                localidadViewModels.AddRange(localidades.Where(l => l.Provincia.Id == cobradorViewModel.ProvinciaId)
                    .ToList()
                    .Select(l => new LocalidadViewModel(l)));
            }
            cobradorViewModel.Localidades = new SelectList(localidadViewModels, "Id", "Nombre");
        }

        #endregion
    }
}