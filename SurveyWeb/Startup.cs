using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SurveyWeb.Startup))]
namespace SurveyWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
