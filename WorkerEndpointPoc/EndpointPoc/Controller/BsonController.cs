using System.Threading.Tasks;
using System.Web.Http;
using proactima.jsonobject;

namespace EndpointPoc.Controller
{
    public class BsonController : ApiController
    {
        public async Task<IHttpActionResult> Get(string id)
        {
            await Task.Delay(250);

            var response = Data.Load(id);
            if (response.Success)
                return Ok(response.Json);
            return NotFound();
        }

        public async Task<IHttpActionResult> Put(string id, JsonObject json)
        {
            await Task.Delay(250);

            return Ok(Data.Store(id, json));
        }
    }
}