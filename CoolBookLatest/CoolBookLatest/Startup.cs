using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CoolBookLatest.Startup))]
namespace CoolBookLatest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
