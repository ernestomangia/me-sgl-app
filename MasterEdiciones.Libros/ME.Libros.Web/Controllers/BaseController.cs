using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ME.Libros.Web.Controllers
{
    using System.Data.Entity.Validation;

    public class BaseController : Controller
    {
        //protected T service;

        public bool Probar<TEntity>(TEntity entity, Action<TEntity> action)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    action(entity);
                    return true;
                }
            }
            catch (DbEntityValidationException DBEx)
            {
                foreach (var error in DBEx.EntityValidationErrors.SelectMany(validationError => validationError.ValidationErrors))
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Ha ocurrido un error. Por favor comuníquese con el administrador.", ex.Message);
            }

            return false;
        }
    }
}