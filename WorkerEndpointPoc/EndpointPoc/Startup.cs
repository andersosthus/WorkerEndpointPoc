using System.Web.Http;
using Contracts;
using Owin;
using ProtoBuf.Meta;

namespace EndpointPoc
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            RuntimeTypeModel.Default.Add(typeof(ProtoObj), false).SetSurrogate(typeof(ProtoObjSurrogate));

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