using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NguyenVanTuan_BigSchool.Startup))]
namespace NguyenVanTuan_BigSchool
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
