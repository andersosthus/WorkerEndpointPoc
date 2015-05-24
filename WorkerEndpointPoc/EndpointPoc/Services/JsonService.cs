using System.ServiceModel;
using System.Threading.Tasks;
using Contracts;
using proactima.jsonobject;

namespace EndpointPoc.Services
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
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