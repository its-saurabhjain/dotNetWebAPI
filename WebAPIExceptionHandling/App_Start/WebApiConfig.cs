using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace WebAPIExceptionHandling
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            //config.Filters.Add(new Filters.NotImplExceptionFilterAttribute());

            config.Services.Add(typeof(IExceptionLogger), new TraceExceptionLogger()); // webConfiguration is an instance of System.Web.Http.HttpConfiguration
            config.Services.Replace(typeof(IExceptionHandler), new OopsExceptionHandler());
        }
    }
}
