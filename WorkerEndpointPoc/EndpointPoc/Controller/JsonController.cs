using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using proactima.jsonobject;

namespace EndpointPoc.Controller
{
    public class JsonController : ApiController
    {
        private static readonly ConcurrentDictionary<string, JsonObject> Data = new ConcurrentDictionary<string, JsonObject>();

        public async Task<IHttpActionResult> Get(string id)
        {
            await Task.Delay(250);
            if (Data.ContainsKey(id))
                return Ok(Data[id]);
            return NotFound();
        }

        public async Task<IHttpActionResult> Put(string id, JObject obj)
        {
            await Task.Delay(250);
            var json = JsonObject.FromJObject(obj);
            Data[id] = json;
            return Ok(Data[id]);
        }
    }
}