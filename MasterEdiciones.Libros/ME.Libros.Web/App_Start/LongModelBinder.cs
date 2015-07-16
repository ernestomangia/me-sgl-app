﻿using System;
using System.Globalization;
using System.Web.Mvc;

namespace ME.Libros.Web
{
    public class LongModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var modelState = new ModelState { Value = valueResult };
            object actualValue = null;

            if (valueResult.AttemptedValue != string.Empty)
            {
                try
                {
                    //TODO: Controlar agrupacion de miles
                    actualValue = Int64.Parse(valueResult.AttemptedValue, NumberStyles.AllowThousands, CultureInfo.CurrentCulture);
                }
                catch (FormatException e)
                {
                    modelState.Errors.Add(e);
                }
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);

            return actualValue;
        }
    }
}