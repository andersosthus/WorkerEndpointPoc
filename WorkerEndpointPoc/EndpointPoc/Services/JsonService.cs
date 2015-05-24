using System.Collections.Concurrent;
using System.Threading.Tasks;
using Contracts;
using Newtonsoft.Json.Linq;
using proactima.jsonobject;

namespace EndpointPoc.Services
{
    public class JsonService : IStoreAndLoadJson
    {
        private static readonly ConcurrentDictionary<string, JsonObject> Data =
            new ConcurrentDictionary<string, JsonObject>();

        public async Task<JsonObject> LoadAsync(string id)
        {
            await Task.Delay(250);
            if (Data.ContainsKey(id))
                return Data[id];
            return new JsonObject();
        }

        public async Task<JsonObject> StoreAsync(string id, string obj)
        {
            await Task.Delay(250);
            var json = JsonObject.Parse(obj);
            Data[id] = json;
            return Data[id];
        }
    }
}