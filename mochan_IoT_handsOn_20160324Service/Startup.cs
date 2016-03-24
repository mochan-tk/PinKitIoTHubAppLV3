using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(mochan_IoT_handsOn_20160324Service.Startup))]

namespace mochan_IoT_handsOn_20160324Service
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}