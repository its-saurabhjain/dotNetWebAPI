using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebAPI2.Demo.Startup))]
namespace WebAPI2.Demo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
