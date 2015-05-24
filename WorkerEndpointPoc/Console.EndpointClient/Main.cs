using System.ServiceModel;
using System.Threading.Tasks;
using Contracts;
using Newtonsoft.Json;
using proactima.jsonobject;

namespace EndpointClient
{
    public class Main
    {
        private const string ServiceUrl = "net.tcp://127.0.0.1:90/LoanCalculator";
        private const int Counter = 3;

        private static IStoreAndLoadJson GetAProxy()
        {
            var binding = new NetTcpBinding(SecurityMode.None);
            var endpointAddress
                = new EndpointAddress(ServiceUrl);

            return new ChannelFactory<IStoreAndLoadJson>
                (binding, endpointAddress).CreateChannel();
        }

        public async Task ServiceCallsAsync()
        {
            var proxy = GetAProxy();

            System.Console.WriteLine("Store some values");
            var json = new JsonObject
            {
                {"title", "service"}
            };
            var jsonString = JsonConvert.SerializeObject(json);
            for (var i = 0; i < Counter; i++)
                await proxy.StoreAsync("service" + i, jsonString);


            System.Console.WriteLine("Load values back");
            for (var i = 0; i < Counter; i++)
            {
                var result = await proxy.LoadAsync("service" + i);
                System.Console.WriteLine(result);
            }
        }

        public Task HttpCallsAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
