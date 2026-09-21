using System.Diagnostics;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;
using ValidationException = Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions.ValidationException;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Filters
{
    public class ModelAttributeValidation : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //if (actionContext.ModelState.Count() == 0)
            //{
            //    throw new ValidationException(Constants.ErrorMessages.MODEL_WAS_NULL);
            //}

            if (actionContext.ModelState.IsValid == false)
            { 
                throw new ValidationException(actionContext.ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList());
            }
        }
    }
}
