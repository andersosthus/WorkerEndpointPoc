using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Contracts;
using proactima.jsonobject;

namespace EndpointPoc.Controller
{
    public class ProtoController : ApiController
    {
        public async Task<HttpResponseMessage> Get(string id)
        {
            await Task.Delay(250);

            var response = Data.Load(id);
            var data = new ProtoObj(response.Json);

            if (!response.Success)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            var bytes = data.Serialize();

            var result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(bytes);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            return result;
        }

        public async Task<IHttpActionResult> Put(string id)
        {
            var data = await Request.Content.ReadAsByteArrayAsync();
            await Task.Delay(250);

            var obj = ProtoObj.Deserialize(data);

            var json = obj.JsonObj;
            var jsonObj = json as JsonObject;
            return Ok(Data.Store(id, jsonObj));
        }
    }
}