using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using ValidationException = Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions.ValidationException;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Filters
{
    public class ModelAttributeValidation : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ModelState.IsValid == false)
            {
                string allErrors = string.Join("^" , actionContext.ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList());
                throw new ValidationException(allErrors);
            }
        }
    }
}
