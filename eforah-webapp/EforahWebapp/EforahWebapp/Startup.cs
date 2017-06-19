using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EforahWebapp.Startup))]
namespace EforahWebapp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
