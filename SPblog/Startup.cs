using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SPblog.Startup))]
namespace SPblog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
