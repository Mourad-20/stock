using System.Web;
using System.Web.Mvc;
using webCaisse.Filters;

namespace webCaisse
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
