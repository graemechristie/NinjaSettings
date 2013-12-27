using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NinjaSettings.Example.Autofac.Startup))]
namespace NinjaSettings.Example.Autofac
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
