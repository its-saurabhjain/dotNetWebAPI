using System;
using Microsoft.Owin;
using Owin;
using IdentityServer3.Core.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using IdentityServer3.EntityFramework;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Models;
using System.Collections.Generic;
using OAuth.Data.Repositories;
using System.Data.SqlClient;

[assembly: OwinStartup(typeof(OAuthServer.Startup))]
namespace OAuthServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var entityFrameworkOptions = new EntityFrameworkServiceOptions
            {
                ConnectionString =
                            ConfigurationManager.ConnectionStrings["OAuthSvr.DB"].ConnectionString
            };
            
            var inMemoryManager = new InMemoryManager();
            SetupClients(inMemoryManager.GetClients(), entityFrameworkOptions);
            SetupScopes(inMemoryManager.GetScopes(), entityFrameworkOptions);

            var userRepository = new UserRepository(
                ()=> new SqlConnection(ConfigurationManager.ConnectionStrings["User.DB"].ConnectionString));

            var factory = new IdentityServerServiceFactory();
            factory.RegisterConfigurationServices(entityFrameworkOptions);
            factory.RegisterOperationalServices(entityFrameworkOptions);
            factory.UserService = new Registration<IUserService>(typeof(OAuthUserService));
            factory.Register(new Registration<IUserRepository>(userRepository));

            new TokenCleanup(entityFrameworkOptions, 1).Start();
            var scer = new X509Certificate2(@"C:\OpenSSL\OAuthServer\OAuthServerCA.pem");
            
            var options = new IdentityServerOptions
            {
                SigningCertificate = LoadCertificate(),
                RequireSsl = false,
                Factory = factory
            };
            
            app.UseIdentityServer(options);
           
        }

        private void SetupScopes(IEnumerable<Scope> scopes, 
                                EntityFrameworkServiceOptions entityFrameworkOptions)
        {
            
            using (var context =
                new ScopeConfigurationDbContext(entityFrameworkOptions.ConnectionString,
                                                    entityFrameworkOptions.Schema)) {
                if (context.Scopes.Local.Count != 0) return;
                foreach (var scope in scopes)
                {
                    context.Scopes.Add(scope.ToEntity());
                }
                //context.SaveChanges();
             }
        }

        private void SetupClients(IEnumerable<Client> clients, 
                            EntityFrameworkServiceOptions entityFrameworkOptions)
        {
            using (var context =
                new ClientConfigurationDbContext(entityFrameworkOptions.ConnectionString,
                                                    entityFrameworkOptions.Schema))
            {
                if (context.Clients.Local.Count!= 0) return;
                foreach (var client in clients) {

                    context.Clients.Add(client.ToEntity());
                }
                //context.SaveChanges();

            }
        }

        public X509Certificate2 LoadCertificate()
        {

            ///IIS Certificate
            //string thumbPrint =  "97742ba4e8c9cb2359cffe0afc7932c88fc1a2a4";
            //var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            // Starting with the .NET Framework 4.6, X509Store implements IDisposable.
            // On older .NET, store.Close should be called.
            //string thumbPrint = "‎4f25ba84cad38b2aca82d6ab78bf1b9916002563";
            string thumbPrint = "a6e684cdffdf7a8efdca124a81d977c4e8b2cd3d";
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);

            store.Open(OpenFlags.ReadOnly);
            var certCollection = store.Certificates.Find(X509FindType.FindByThumbprint, thumbPrint, validOnly: false);
            if (certCollection.Count == 0)
            {
                store.Close();
                throw new Exception("No certificate found containing the specified thumbprint.");
            }
            
            return certCollection[0];
        }
    }
}
