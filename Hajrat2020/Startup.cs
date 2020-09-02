using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Hajrat2020.Startup))]
namespace Hajrat2020
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
