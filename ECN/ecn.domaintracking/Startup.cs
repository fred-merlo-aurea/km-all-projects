using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ecn.domaintracking.Startup))]
namespace ecn.domaintracking
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
