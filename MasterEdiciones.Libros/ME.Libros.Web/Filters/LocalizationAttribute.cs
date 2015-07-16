using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ME.Libros.Web.Filters
{
    public class LocalizationAttribute : ActionFilterAttribute
    {
        private const string DefaultLanguage = "es-AR";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Resolve and set the current culture
            var culture = ResolveCulture(filterContext);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            base.OnActionExecuting(filterContext);
        }

        #region Private Methods

        public static CultureInfo ResolveCulture(ControllerContext filterContext)
        {
            // Priority 1: from a language cookie
            // Priority 2: Get culture from web config
            var languageConfig = ((System.Web.Configuration.GlobalizationSection)System.Configuration.ConfigurationManager.GetSection("system.web/globalization")).Culture;
            var languageCookie = HttpContext.Current.Request.Cookies["Language"];
            var culture = languageCookie != null
                              ? languageCookie.Value
                              : !string.IsNullOrEmpty(languageConfig)
                                    ? languageConfig
                                    : DefaultLanguage;

            return CultureInfo.CreateSpecificCulture(culture);
        }

        #endregion
    }
}