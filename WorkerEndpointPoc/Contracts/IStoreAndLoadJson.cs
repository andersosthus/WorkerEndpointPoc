using System.ServiceModel;
using System.Threading.Tasks;
using proactima.jsonobject;

namespace Contracts
{
    [ServiceContract]
    public interface IStoreAndLoadJson
    {
        [OperationContract]
        Task<Response> LoadAsync(string id);

        [OperationContract]
        Task<JsonObject> StoreAsync(string id, JsonObject obj);
    }
}