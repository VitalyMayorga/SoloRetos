using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebProyecto.Startup))]
namespace WebProyecto
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
