using System.Web;
using System.Web.Mvc;

namespace Ot_Sims_Givebox
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
