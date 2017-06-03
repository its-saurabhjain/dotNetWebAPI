using System;
using Microsoft.Owin;
using Owin;
using IdentityServer3.Core.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;

//[assembly: OwinStartup(typeof(OAuthServer.Startup))]
namespace OAuthServer
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            //X509Certificate2 cert = new X509Certificate2(@"C:\OpenSSL\bin\rootCA.p12", "1974");
            //byte[] certBytes = cert.GetRawCertData();
            //string certString = Convert.ToBase64String(certBytes);

            //var certificate = Convert.FromBase64String(ConfigurationManager.AppSettings["SigningCertificate"]);

            var inMemoryManager = new InMemoryManager();
            var factory = new IdentityServerServiceFactory()
                .UseInMemoryUsers(inMemoryManager.GetUsers())
                .UseInMemoryScopes(inMemoryManager.GetScopes())
                .UseInMemoryClients(inMemoryManager.GetClients());

            var scer = new X509Certificate2(@"C:\OpenSSL\OAuthServer\OAuthServerCA.pem");
            
            var options = new IdentityServerOptions
            {
                SigningCertificate = LoadCertificate(),
                RequireSsl = false,
                Factory = factory
            };
            
            app.UseIdentityServer(options);
            
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
