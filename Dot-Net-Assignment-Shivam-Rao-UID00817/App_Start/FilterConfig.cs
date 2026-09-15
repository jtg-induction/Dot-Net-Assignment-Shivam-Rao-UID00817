using System.Web;
using System.Web.Mvc;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
