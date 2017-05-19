using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Security.Cryptography.X509Certificates;
using Autofac.Integration.WebApi;
using Autofac;
using System.Reflection;
using Microsoft.Owin.Security.Jwt;
using System.IdentityModel.Tokens;
using System.Configuration;
using IdentityServer3.AccessTokenValidation;

[assembly: OwinStartup(typeof(WebAPIRouting.Startup))]

namespace WebAPIRouting
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            var webApiConfiguration = new HttpConfiguration();
            
            webApiConfiguration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{userName}/{password}",
                defaults: new { userName = RouteParameter.Optional, password = RouteParameter.Optional });

            var certificate = new X509Certificate2(@"C:\OpenSSL\bin\rootCA.p12", "1974");
            //var publickey = Convert.ToBase64String(certificate.PublicKey.EncodedKeyValue.RawData);

            //var cer = new X509Certificate2(Convert.FromBase64String(publickey));
            //var builder = new ContainerBuilder();
            //builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            //builder.RegisterModule();
            //var container = builder.Build();
            //webApiConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            /*
            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AllowedAudiences = new[] { "http://localhost:15638/resources" },
                TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidAudience = "http://localhost:15638/resources",
                    ValidIssuer = "http://localhost:15638/",
                    IssuerSigningKey = new X509SecurityKey(certificate)
                }
            });
            */
            //Install - Package IdentityServer3.AccessTokenValidation
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "http://localhost:15638/"

            });
            app.UseWebApi(webApiConfiguration);
        }
    }
}
