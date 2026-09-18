using Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using ValidationException = Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions.ValidationException;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Exception_Handlers
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var errorResponse = new ErrorResponse();
            HttpStatusCode statusCode;
            if (context.Exception is ConflictException conflictException)
            {
                errorResponse.errors.Add(conflictException.Message);
                statusCode = HttpStatusCode.Conflict;
            }
            else if (context.Exception is ValidationException validationException)
            {
                errorResponse.errors = validationException.ValidationMessages;
                statusCode = HttpStatusCode.BadRequest;
            }
            else
            {
                statusCode = HttpStatusCode.InternalServerError;
            }

            context.Result = new NegotiatedContentResult<ErrorResponse>(
                statusCode ,
                errorResponse ,
                context.RequestContext.Configuration.Services.GetContentNegotiator() ,
                context.Request ,
                context.RequestContext.Configuration.Formatters
            );
        }
    }
}
