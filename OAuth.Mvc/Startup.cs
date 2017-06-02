using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Security.Claims;
using Microsoft.Owin.Security;
using Thinktecture.IdentityModel.Clients;

[assembly: OwinStartup(typeof(OAuth.Mvc.Startup))]
namespace OAuth.Mvc
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            /*
            1) install System.IdentityModel.Tokens.Jwt version 4.0.2.206221351 (latest stable)
               Install-Package System.IdentityModel.Tokens.Jwt -Version 4.0.2.206221351
            2) add reference (not nuget!) to .NET framework assembly System.IdentityModel.
               Right click on project -> References -> Add reference -> Assemblies -> Framework -> select System.IdentityModel 4.0.0.0    
            */
            //Install-Package System.IdentityModel.Tokens.Jwt
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            //Install-Package Microsoft.Owin.Security.Cookies 
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies",
                LoginPath = new PathString("/Home2/Login")

            });
            

        }
    }
}
