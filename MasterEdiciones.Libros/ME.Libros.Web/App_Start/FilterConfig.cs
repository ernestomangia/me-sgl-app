using System.Web.Mvc;

using ME.Libros.Web.Filters;

namespace ME.Libros.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new AuthorizeAttribute());
            filters.Add(new CustomHandleErrorAttribute());
            filters.Add(new LocalizationAttribute());
        }
    }
}