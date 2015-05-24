using System.ServiceModel;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using proactima.jsonobject;

namespace Contracts
{
    [ServiceContract]
    public interface IStoreAndLoadJson
    {
        [OperationContract]
        Task<JsonObject> LoadAsync(string id);
        [OperationContract]
        Task<JsonObject> StoreAsync(string id, string obj);
    }
}