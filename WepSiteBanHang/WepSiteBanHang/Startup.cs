using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WepSiteBanHang.Startup))]
namespace WepSiteBanHang
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
