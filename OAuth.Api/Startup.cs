using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.OpenIdConnect;
using System.IdentityModel.Claims;
using Microsoft.Owin.Security;
using System.Security.Cryptography.X509Certificates;
using System.IdentityModel.Tokens;
using System.Web.Http;
using IdentityServer3.AccessTokenValidation;
//Install-Package Microsoft.Owin.Host.SystemWeb
[assembly: OwinStartup(typeof(OAuth.Api.Startup))]

namespace OAuth.Api
{   
    public class Startup
    {
        /// <summary>
        /// Demonstrate use of Resource Owner Credential flow
        /// Using user name and password a token is obtained from the OAuth Server
        /// Postman:-http://localhost/OAuthServer/connect/token
        /// Using that access_token we invoke the API passing Authorization header with Bearer <<token>>
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            var config = GlobalConfiguration.Configuration;
            WebApiConfig.Register(config);

            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            //Install-Package Microsoft.Owin.Security.OpenIdConnect
            // app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            /*
            var certificate = new X509Certificate2(Convert.FromBase64String("MIID/jCCAuagAwIBAgIJAPhn5wdJQ9psMA0GCSqGSIb3DQEBCwUAMIGTMQswCQYDVQQGEwJVUzELMAkGA1UECAwCUEExFTATBgNVBAcMDFBoaWxhZGVscGhpYTERMA8GA1UECgwIQ29uZHVlbnQxDDAKBgNVBAsMA0ZQUzEUMBIGA1UEAwwLT0F1dGhTZXJ2ZXIxKTAnBgkqhkiG9w0BCQEWGnNhdXJhYmguamFpbjJAY29uZHVlbnQuY29tMB4XDTE3MDUyMjIwMzM1OVoXDTIwMDMxMTIwMzM1OVowgZMxCzAJBgNVBAYTAlVTMQswCQYDVQQIDAJQQTEVMBMGA1UEBwwMUGhpbGFkZWxwaGlhMREwDwYDVQQKDAhDb25kdWVudDEMMAoGA1UECwwDRlBTMRQwEgYDVQQDDAtPQXV0aFNlcnZlcjEpMCcGCSqGSIb3DQEJARYac2F1cmFiaC5qYWluMkBjb25kdWVudC5jb20wggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQC5nWAxK06PcOzafrQWz8RlFfaMq4Cr8ude43v1bWXfFSeyKRmUdXeMSif+qSOaFq8f9kU/45DmNMv0JDtYTQDKgY6rXaPDtlGaiC7ub1K+gZQsHv8VJ90YskANMaDbxEHbjUI9VtBd6hgeQdWtzwO0IzQjXK+YDt7gisQGAifRUOWJVxWwu2zDLCMPMq7yGU11uzZ9loVtLm3QzruI9jMmSVAWAejIGVzJpdm/cAovb/be8yj3eygzNPrcTV1UlHj7YiIeLvypO6H2TTRx8VWIw+3AqjgGpNiVJ58tVfzO0v4UyriMQMiGt0TjbxSgyl+KY4g6wABURn0Ms6NdrDATAgMBAAGjUzBRMB0GA1UdDgQWBBQlV+aJxx1e8Q8P5nG5S7KuRfQvKzAfBgNVHSMEGDAWgBQlV+aJxx1e8Q8P5nG5S7KuRfQvKzAPBgNVHRMBAf8EBTADAQH/MA0GCSqGSIb3DQEBCwUAA4IBAQCHF54kLS5S6zRRB14cikm2+5j/yyVQd8rCGdaQIL0lHQAn/fbn6DvrWWoDtXC2Wq40MToMAUENr/jFCubcIgVvdGcQhQMctU4HPkTIZh7WyzY50mLILze+acJUjt6i7J1TE7SM2m2nslP3uZGLr9etMBsZM1JcvHKJtHaCXq+FJpqDw9rPJTqjxpRk90b1S7/cLEXg7/bJquAhiRhYCC34ApvMsoA8qkkDR/PGunwQMygdbChIiBwr29KVq+jrvPT+jEGzp+Rnh8AFlotV4ZXDBomqrcZo1xaeh8Divxx59ABYH4CbpazYp1ZRFiOZEAsNpnzIBpX3IBs5QkrCjr82"));
            //Install - Package Microsoft.Owin.Security.Jwt
            TokenValidationParameters para = new System.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidAudience = "http://localhost:15638/resources",
                ValidIssuer = "http://localhost:15638/",
                IssuerSigningKey = new X509SecurityKey(certificate)
            };
            app.UseJwtBearerAuthentication(new Microsoft.Owin.Security.Jwt.JwtBearerAuthenticationOptions {

                AllowedAudiences = new[] { "http://localhost:15638/resources" },
                TokenValidationParameters= para
            });
            */
            //Install-Package IdentityServer3.AccessTokenValidation
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                //Authority = "http://OAuthserver/" //--This doesn't works, need to know why.
                Authority = "http://localhost/OAuthServer/" //--This works
                //Authority = "http://localhost:15638/" //--This works

            });
            //Install-Package Microsoft.AspNet.WebApi.OwinSelfHost, when there is no global.asax
            app.UseWebApi(config);
            config.EnsureInitialized();
        }
    }
}
