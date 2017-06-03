using IdentityServer3.Core.Services.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdentityServer3.Core.ViewModels;
using System.IO;
using System.Threading.Tasks;

namespace OAuthServer
{
    public class CustomViewService: DefaultViewService
    {

        public CustomViewService(DefaultViewServiceOptions config, IViewLoader viewLoader)
            :base(config, viewLoader)
        {
                
        }
        public override Task<Stream> Error(ErrorViewModel model)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, System.Text.Encoding.UTF8);
            writer.Write($"<h1>Error  Occured! </h1>{model.ErrorMessage}");
            writer.Flush();

            stream.Seek(0, SeekOrigin.Begin);
            return Task.FromResult<Stream>(stream);
        }
    }
}