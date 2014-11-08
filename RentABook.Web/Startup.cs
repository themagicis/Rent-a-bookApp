using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RentABook.Web.Startup))]
namespace RentABook.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
