using System;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace ME.Libros.Web.Controllers
{
    using System.Data.Entity.Validation;

    public class BaseController : Controller
    {
        //protected T service;

        public bool ExecuteAction<TEntity>(TEntity entity, Action<TEntity> action)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    action(entity);
                    return true;
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

                if (sqlException != null)
                {
                    ModelState.AddModelError("Error", sqlException.Number == 547 ? ErrorMessages.EliminarCliente : ErrorMessages.ErrorSistema);
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

        //public bool ExecuteAction<TEntity>(TEntity entity, Func<int>(TEntity) action)
        //{
        //    try
        //    {
        //        if (this.ModelState.IsValid)
        //        {
        //            action(entity);
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