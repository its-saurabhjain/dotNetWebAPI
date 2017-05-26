using System;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(PatientDataAPI.Startup))]

namespace PatientDataAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
