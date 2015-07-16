using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using ME.Libros.Utils.Enums;
using ME.Libros.Web.Models;

namespace ME.Libros.Web.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString CssClassFor(this HtmlHelper html, VentaViewModel model)
        {
            return new MvcHtmlString(string.Format("label-{0}", model != null ? model.Estado == EstadoVenta.Vigente ? "info" : model.Estado == EstadoVenta.Pagada ? "success" : "danger" : string.Empty));
        }

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

        //public static MvcHtmlString TooltipFor(this HtmlHelper html, MetricaViewModel model)
        //{
        //    var htmlBuilder = new StringBuilder();
        //    foreach (var umbral in model.Umbrales.OrderBy(u => u.ValorInferior))
        //    {
        //        var spanTagBuilder = new TagBuilder("span");
        //        spanTagBuilder.AddCssClass("label");
        //        spanTagBuilder.AddCssClass(CssClassFor(html, umbral).ToHtmlString());
        //        spanTagBuilder.InnerHtml = string.Format("{0} {1}", umbral.ValorInferior, model.Unidad);

        //        htmlBuilder.Append(spanTagBuilder);
        //        htmlBuilder.Append("&nbsp;");
        //    }

        //    var divTagBuilder = new TagBuilder("div");
        //    divTagBuilder.AddCssClass("umbral-container");
        //    divTagBuilder.Attributes.Add("style", "display: none");
        //    divTagBuilder.InnerHtml = htmlBuilder.ToString();

        //    return new MvcHtmlString(divTagBuilder.ToString());
        //}
    }
}