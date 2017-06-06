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

[assembly: OwinStartup(typeof(OAuth.Mvc.Startup_AuthCodeFlow))]
namespace OAuth.Mvc
{
    public class Startup_AuthCodeFlow
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
                AuthenticationType = "Cookies"

            });
            // using middle ware for Authorizatiuon code flow -- if using this comment the code in HomeController--Index
            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = "socialnetwork_code",
                Authority = "http://OAuthServer/",
                //Authority = "http://localhost/OAuthServer/",
                RedirectUri = "http://localhost:26654/",
                //ResponseType = "code",
                ResponseType = "code id_token",  //use a hybrid flow.
                //Scope = "openid profile",
                Scope = "openid profile offline_access", //offline access is required for refresh token
                PostLogoutRedirectUri = "http://localhost:26654/",
                SignInAsAuthenticationType = "Cookies",

                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    /*
                    SecurityTokenValidated = notification =>
                    {
                        var identity = notification.AuthenticationTicket.Identity;
                        identity.AddClaim(new Claim("id_token", notification.ProtocolMessage.IdToken));
                        identity.AddClaim(new Claim("access_token", notification.ProtocolMessage.AccessToken));

                        notification.AuthenticationTicket =
                        new AuthenticationTicket(identity, notification.AuthenticationTicket.Properties);
                        return Task.FromResult(0);
                    },
                    */
                    AuthorizationCodeReceived = async notification => 
                    {
                        var requestResponse = await OidcClient.CallTokenEndpointAsync(
                            new Uri("http://OAuthServer/connect/token"),
                            new Uri("http://localhost:26654/"),
                            notification.Code,
                            "socialnetwork_code",
                            "secret");
                        var identity = notification.AuthenticationTicket.Identity;
                        identity.AddClaim(new Claim("access_token", requestResponse.AccessToken));
                        identity.AddClaim(new Claim("id_token", requestResponse.IdentityToken));
                        identity.AddClaim(new Claim("refresh_token", requestResponse.RefreshToken));

                        notification.AuthenticationTicket = new AuthenticationTicket(identity, notification.AuthenticationTicket.Properties);
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
