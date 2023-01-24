using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(W2022A5MA.Startup))]

namespace W2022A5MA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
