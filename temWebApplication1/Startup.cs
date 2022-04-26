using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(temWebApplication1.Startup))]
namespace temWebApplication1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
