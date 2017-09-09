using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(kuvera108.Startup))]
namespace kuvera108
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
