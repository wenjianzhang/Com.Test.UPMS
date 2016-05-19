using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Com.Test.UPMS.Web.Startup))]

namespace Com.Test.UPMS.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}