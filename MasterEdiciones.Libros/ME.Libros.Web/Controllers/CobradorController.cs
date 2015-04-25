using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ME.Libros.Dominio.General;
using ME.Libros.EF;
using ME.Libros.EF.Mapeos;
using ME.Libros.Repositorios;
using ME.Libros.Servicios.General;
using ME.Libros.Web.Helpers;
using ME.Libros.Web.Models;

namespace ME.Libros.Web.Controllers
{
    public class CobradorController : BaseController<CobradorDominio>
    {

        public  CobradorService CobradorService { get; set; }
        private LocalidadService LocalidadService { get; set; }

        public CobradorController()
        {
            var modelContainer = new ModelContainer();
            CobradorService = new CobradorService(new EntidadRepository<CobradorDominio>(modelContainer));
            LocalidadService = new LocalidadService(new EntidadRepository<LocalidadDominio>(modelContainer));
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
            ModelState.RemoveFor<CobradorViewModel>(c => c.Localidad.Nombre);
    
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
                            Localidad = LocalidadService.GetPorId(cobradorViewModel.Localidad.Id),
                            Localidades = new List<LocalidadDominio>(),
                        };

                        var varlocalidades = Request.Form["duallistbox_demo1[]"].Split(',');


                        
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
                        ModelState.AddModelError("Error", string.Format(ErrorMessages.DniRepetido, cobradorViewModel.Dni));
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

        #region Private Methods

        private void PrepareModel(CobradorViewModel cobradorViewModel)
        {
            cobradorViewModel.Localidades = new SelectList(LocalidadService.Listar().ToList()
                .Select(l => new LocalidadViewModel(l))
                .ToList(), "Id", "Nombre");    
        }
        #endregion

	}
}