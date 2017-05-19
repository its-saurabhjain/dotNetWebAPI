using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using IdentityServer3.Core.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;

[assembly: OwinStartup(typeof(OAuthServer.Startup))]

namespace OAuthServer
{
    public class Startup
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
                SigningCertificate = scer,
                RequireSsl = false,
                Factory = factory
            };
            
            app.UseIdentityServer(options);
            
        }

        public X509Certificate2 LoadCertificate()
        {
            string thumbPrint = "‎4f25ba84cad38b2aca82d6ab78bf1b9916002563";
            // Starting with the .NET Framework 4.6, X509Store implements IDisposable.
            // On older .NET, store.Close should be called.
            var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            var certCollection = store.Certificates.Find(X509FindType.FindByThumbprint, thumbPrint, validOnly: false);
            if (certCollection.Count == 0)
                 throw new Exception("No certificate found containing the specified thumbprint.");
            store.Close();
            return certCollection[0];
        }
    }
}
