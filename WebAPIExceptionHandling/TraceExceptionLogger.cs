using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace WebAPIExceptionHandling
{
    public class TraceExceptionLogger:ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            base.Log(context);
            Trace.TraceError(context.ExceptionContext.Exception.ToString());
        }
    }
}