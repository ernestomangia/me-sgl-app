using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace ME.Libros.Web.Extensions
{
    public static class ModelStateExtensions
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