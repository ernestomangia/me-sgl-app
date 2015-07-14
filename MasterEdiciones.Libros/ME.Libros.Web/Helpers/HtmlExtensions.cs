using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace ME.Libros.Web.Helpers
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString LabelWithTooltipFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }

            var tag = new TagBuilder("label");
            tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
            tag.Attributes.Add("class", htmlAttributes.GetType().GetProperty("class").GetValue(htmlAttributes).ToString());

            var span = new TagBuilder("span");
            span.SetInnerText(labelText);
            span.Attributes.Add("data-toggle", "tooltip");
            span.Attributes.Add("title", htmlAttributes.GetType().GetProperty("title").GetValue(htmlAttributes).ToString());
            span.Attributes.Add("data-placement", htmlAttributes.GetType().GetProperty("data_placement").GetValue(htmlAttributes).ToString());

            // assign <span> to <label> inner html
            tag.InnerHtml = span.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }
    }
}