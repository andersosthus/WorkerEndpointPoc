using System.Web.Http;
using Owin;

namespace EndpointPoc
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.UseNinjectMiddlewareForWebApi(() => kernel, config, new HttpServer(config));

            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
               "DefaultApi",
               "{controller}/{id}",
               new { id = RouteParameter.Optional });
            
            var server = new HttpServer(config);
            app.UseWebApi(server);
        }
    }
}