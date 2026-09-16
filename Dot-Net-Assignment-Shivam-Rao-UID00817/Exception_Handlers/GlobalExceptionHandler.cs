using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.ExceptionHandling;
using System.Diagnostics;
using System.Web.Configuration;
using System.Net.Http;
using System.Net;
using System.Web.Http.Results;
using System.Web.Http;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions;
using ValidationException = Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions.ValidationException;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Exception_Handlers
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            HttpStatusCode statusCode = 0;
            if(context.Exception is ConflictException)
            {
                statusCode = HttpStatusCode.Conflict;
            }
            else if(context.Exception is ValidationException)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }
            else
            {
                statusCode = HttpStatusCode.BadRequest;
            }

            context.Result = new NegotiatedContentResult<List<string>>(
                statusCode ,
                new List<string>(context.Exception.Message.Split('^')) ,
                context.RequestContext.Configuration.Services.GetContentNegotiator() ,
                context.Request ,
                context.RequestContext.Configuration.Formatters
            );
        }

        private class ErrorMessageResult : IHttpActionResult
        {
            private readonly HttpResponseMessage _httpResponseMessage;

            public ErrorMessageResult(HttpResponseMessage httpResponseMessage)
            {
                _httpResponseMessage = httpResponseMessage;
            }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(_httpResponseMessage);
            }
        }
    }
}
