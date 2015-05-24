using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using proactima.jsonobject;

namespace EndpointPoc.Controller
{
    public class JsonController : ApiController
    {
        public async Task<IHttpActionResult> Get(string id)
        {
            await Task.Delay(250);

            var response = Data.Load(id);
            if (response.Success)
                return Ok(response.Json);
            return NotFound();
        }

        public async Task<IHttpActionResult> Put(string id, JObject obj)
        {
            await Task.Delay(250);

            var json = JsonObject.FromJObject(obj);
            return Ok(Data.Store(id, json));
        }
    }
}