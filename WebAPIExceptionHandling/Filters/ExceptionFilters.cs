using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace WebAPIExceptionHandling.Filters
{
    public class NotImplExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is NotImplementedException)
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented)
                {
                    Content = new StringContent($"{"Exception Filter: Method Not Implemented"}"),
                    ReasonPhrase = "No Product ID Found, Exception Filter"
                };
                //throw new HttpResponseException(actionExecutedContext.Response);
            }
        }

    }
}