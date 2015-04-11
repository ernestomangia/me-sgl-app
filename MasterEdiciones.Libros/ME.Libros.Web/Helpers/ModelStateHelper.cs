using System;
using System.Linq.Expressions;
using System.Collections;
using System.Linq;
using System.Web.Mvc;

namespace ME.Libros.Web.Helpers
{
    public static class ModelStateHelper
    {
        public static IEnumerable GetErrors(this ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return modelState
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray())
                    .Where(m => m.Value.Any());
            }
            return null;
        }

        public static void RemoveFor<TModel>(this ModelStateDictionary modelState, Expression<Func<TModel, object>> expression)
        {
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            if (modelState.ContainsKey(expressionText))
            {
                modelState.Remove(expressionText);
            }
        }
    }
}