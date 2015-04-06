using System;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using ME.Libros.Dominio;
using ME.Libros.Servicios;

namespace ME.Libros.Web.Controllers
{
    using System.Data.Entity.Validation;

    public class BaseController<T> : Controller where T : BaseDominio
    {
        protected AbstractService<T> Service;

        public bool ExecuteAction<TEntity>(TEntity entity, Action<TEntity> action)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (Service)
                    {
                        action(entity);
                        return true;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var error in ex.EntityValidationErrors.SelectMany(validationError => validationError.ValidationErrors))
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;

                if (sqlException != null && sqlException.Number == 547)
                {
                    ModelState.AddModelError("Error", ErrorMessages.EliminarCliente);
                }
                else
                {
                    ModelState.AddModelError("Error", ErrorMessages.ErrorSistema);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Ha ocurrido un error. Por favor comuníquese con el administrador.", ex.Message);
            }

            return false;
        }

        //public bool ExecuteFunction<TEntity>(TEntity entity, Func<int>(TEntity) action)
        //{
        //    try
        //    {
        //        if (this.ModelState.IsValid)
        //        {
        //            //action(entity);
        //            return true;
        //        }
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        foreach (var error in ex.EntityValidationErrors.SelectMany(validationError => validationError.ValidationErrors))
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
    }
}