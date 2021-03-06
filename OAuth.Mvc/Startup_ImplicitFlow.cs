﻿using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.Tasks;
[assembly: OwinStartup(typeof(OAuth.Mvc.Startup_ImplicitFlow))]
namespace OAuth.Mvc
{ 
    public class Startup_ImplicitFlow
    {
        /// <summary>
        /// Demonstrate use of Implicit OAuth Flow
        /// User is directed to OAut Server Login when accessing resource which is marked[Authorize]
        /// On sucessful login from AuthServer, Access Token is generated and shared with the user also. The user is redirected to the
        /// application along with the access_token, which directs the user to the resourse server.
        /// The client_secrect is not shared and there is no refresh token generated.
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {

             /*
             1) uninstall previously installed System.IdentityModel.Tokens nuget package
                Uninstall-Package System.IdentityModel.Tokens
             2) uninstall latest System.IdentityModel.Tokens.Jwt nuget package
                Uninstall-Package System.IdentityModel.Tokens.Jwt
             3) install System.IdentityModel.Tokens.Jwt version 4.0.2.206221351 (latest stable)
                Install-Package System.IdentityModel.Tokens.Jwt -Version 4.0.2.206221351
             4) add reference (not nuget!) to .NET framework assembly System.IdentityModel.
                Right click on project -> References -> Add reference -> Assemblies -> Framework -> select System.IdentityModel 4.0.0.0    
             */
            //Install-Package System.IdentityModel.Tokens.Jwt
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            //Install-Package Microsoft.Owin.Security.Cookies 
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"

            });
            //This middleware is for Implict flow
            //Install-Package Microsoft.Owin.Security.OpenIdConnect
            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = "socialnetwork_implicit",
                Authority = "http://OAuthServer/",
                //Authority = "http://localhost/OAuthServer/",
                RedirectUri = "http://localhost:26654/",
                ResponseType = "token id_token",
                Scope = "openid profile",
                PostLogoutRedirectUri = "http://localhost:26654/",
                SignInAsAuthenticationType = "Cookies",

                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = notification =>
                    {
                        var identity = notification.AuthenticationTicket.Identity;
                        identity.AddClaim(new Claim("id_token", notification.ProtocolMessage.IdToken));
                        identity.AddClaim(new Claim("access_token", notification.ProtocolMessage.AccessToken));

                        notification.AuthenticationTicket =
                        new AuthenticationTicket(identity, notification.AuthenticationTicket.Properties);
                        return Task.FromResult(0);
                    },
                    RedirectToIdentityProvider = notification =>
                    {
                        if (notification.ProtocolMessage.RequestType !=
                            Microsoft.IdentityModel.Protocols.OpenIdConnectRequestType.LogoutRequest)
                        {
                            return Task.FromResult(0);
                        }
                        notification.ProtocolMessage.IdTokenHint =
                            notification.OwinContext.Authentication.User.FindFirst("id_token").Value;
                        return Task.FromResult(0);
                    }
                }
            });
        }
    }
}
