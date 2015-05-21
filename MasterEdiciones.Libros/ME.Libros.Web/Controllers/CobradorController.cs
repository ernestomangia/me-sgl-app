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
using ME.Libros.Web.Models;

namespace ME.Libros.Web.Controllers
{
    public class CobradorController : BaseController<CobradorDominio>
    {
        public CobradorService CobradorService { get; set; }
        private LocalidadService LocalidadService { get; set; }
        private ProvinciaService ProvinciaService { get; set; }

        public CobradorController()
        {
            var modelContainer = new ModelContainer();
            CobradorService = new CobradorService(new EntidadRepository<CobradorDominio>(modelContainer));
            LocalidadService = new LocalidadService(new EntidadRepository<LocalidadDominio>(modelContainer));
            ProvinciaService = new ProvinciaService(new EntidadRepository<ProvinciaDominio>(modelContainer));
            ViewBag.MenuId = 2;
            ViewBag.Title = "Cobradores";
            Service = new CobradorService(new EntidadRepository<CobradorDominio>(modelContainer));
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
                            Localidad = LocalidadService.GetPorId(cobradorViewModel.LocalidadId),
                            Localidades = new List<LocalidadDominio>(),
                        };

                        var varlocalidades = Request.Form["localidadesAsignadas_dualList"].Split(',');

                        foreach (var localidad in varlocalidades)
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
                        ModelState.AddModelError("Error", string.Format(ErrorMessages.DniRepetidoCobrador, cobradorViewModel.Dni));
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
                PrepareModel(cobradorViewModel);
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

            long resultado = 0;
            try
            {
                var cobradorDominio = CobradorService.GetPorId(cobradorViewModel.Id);
                using (CobradorService)
                {

                    cobradorDominio.Nombre = cobradorViewModel.Nombre;
                    cobradorDominio.Apellido = cobradorViewModel.Apellido;
                    cobradorDominio.Dni = cobradorViewModel.Dni;
                    cobradorDominio.Localidad = LocalidadService.GetPorId(cobradorViewModel.LocalidadId);
                    cobradorDominio.Localidades.Clear();

                    var varlocalidades = Request.Form["localidadesAsignadas_dualList"].Split(',');

                    foreach (var localidad in varlocalidades)
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
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;
                if (sqlException != null && sqlException.Number == 2601)
                {
                    ModelState.AddModelError("Error",
                        string.Format(ErrorMessages.DniRepetidoCobrador, cobradorViewModel.Dni));
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
                PrepareModel(cobradorViewModel);
            }

            return resultado > 0
                ? (ActionResult)RedirectToAction("Index")
                : View(cobradorViewModel);
        }

        #region Private Methods

        private void PrepareModel(CobradorViewModel cobradorViewModel)
        {
            cobradorViewModel.Localidades = new SelectList(LocalidadService.Listar().ToList()
                .Select(l => new LocalidadViewModel(l))
                .ToList(), "Id", "Nombre");

            cobradorViewModel.Provincias = new SelectList(ProvinciaService.Listar()
               .Select(p => new ProvinciaViewModel(p))
               .ToList(), "Id", "Nombre");


            var localidades = new List<LocalidadViewModel>();
            if (cobradorViewModel.ProvinciaId > 0)
            {
                localidades.AddRange(LocalidadService.Listar(l => l.Provincia.Id == cobradorViewModel.ProvinciaId)
                                                                    .ToList()
                                                                    .Select(l => new LocalidadViewModel(l)));
            }
        }

        #endregion
    }
}