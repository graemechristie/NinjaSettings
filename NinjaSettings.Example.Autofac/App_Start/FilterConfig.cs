using System.Web;
using System.Web.Mvc;

namespace NinjaSettings.Example.Autofac
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
