using System.Web.Http;
using Owin;

namespace EndpointPoc
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
               "DefaultApi",
               "{controller}/{id}",
               new { id = RouteParameter.Optional });

            config.Formatters.Add(new MessagePackMediaTypeFormatter()); 

            var server = new HttpServer(config);
            app.UseWebApi(server);
        }
    }
}