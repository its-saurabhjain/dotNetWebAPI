using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace OAuthServer
{
    public class InMemoryManager
    {
        public List<InMemoryUser> GetUsers() {
            return new List<InMemoryUser> {
                new InMemoryUser {
                     Subject= "saurabh.jain2@conduent.com",
                     Username = "saurabh.jain2@conduent.com",
                     Password = "saurabhjain",
                     Claims = new [] {
                            new Claim(ClaimTypes.Name, "saurabh.jain2@conduent.com")
                     },         
                }};
        }
        public IEnumerable<Scope> GetScopes() {
            return new [] {

                       StandardScopes.OpenId,
                       StandardScopes.Profile,
                       StandardScopes.OfflineAccess,
                       new Scope {
                           Name = "read",
                           DisplayName = "Read User Data"
                       },
              };
        }
        public IEnumerable<Client> GetClients() {
            return new[] {
                new Client {
                    ClientId = "socialnetwork",
                    ClientSecrets = new List<Secret> {
                        new Secret("secret".Sha256())
                    },
                    ClientName="SocialNetwork",
                    Flow = Flows.ResourceOwner,
                    AllowedScopes = new List<string>
                    {
                        Constants.StandardScopes.OpenId,
                        "read"
                    },
                    Enabled = true,
                },
                new Client {
                    ClientId = "socialnetwork_implicit",
                    ClientSecrets = new List<Secret> {
                        new Secret("secret".Sha256())
                    },
                    ClientName="SocialNetwork",
                    Flow = Flows.Implicit,
                    AllowedScopes = new List<string>
                    {
                        Constants.StandardScopes.OpenId,
                        Constants.StandardScopes.Profile,
                        "read"
                    },
                    RedirectUris = new List<string>
                    {
                        "http://localhost:26654/"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "http://localhost:26654/"
                    },
                    Enabled = true,
                }
            };
        }
    }
}