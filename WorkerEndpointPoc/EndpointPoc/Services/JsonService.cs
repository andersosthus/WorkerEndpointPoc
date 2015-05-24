using System.Threading.Tasks;
using Contracts;
using proactima.jsonobject;

namespace EndpointPoc.Services
{
    public class JsonService : IStoreAndLoadJson
    {
        public async Task<Response> LoadAsync(string id)
        {
            await Task.Delay(250);
            return Data.Load(id);
        }

        public async Task<JsonObject> StoreAsync(string id, JsonObject json)
        {
            await Task.Delay(250);
            return Data.Store(id, json);
        }
    }
}