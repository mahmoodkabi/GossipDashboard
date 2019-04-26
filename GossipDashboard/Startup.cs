using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GossipDashboard.Startup))]
namespace GossipDashboard
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
