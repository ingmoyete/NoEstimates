using referenceArchitecture.ui.Core.Filters;
using System.Web;
using System.Web.Mvc;

namespace referenceArchitecture.ui
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ResourceFilter());
            filters.Add(new CustomHandleError());
        }
    }
}
