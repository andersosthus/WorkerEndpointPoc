using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Newtonsoft.Json;
using proactima.jsonobject;

namespace EndpointClient
{
    public static class Main
    {
        private const string ServiceUrl = "net.tcp://127.0.0.1:91/LoanCalculator";
        private const string HttpUrl = "http://127.0.0.1:81/json/";
        private const int Counter = 3;

        private static IStoreAndLoadJson GetAProxy()
        {
            var binding = new NetTcpBinding(SecurityMode.None);
            var endpointAddress
                = new EndpointAddress(ServiceUrl);

            return new ChannelFactory<IStoreAndLoadJson>
                (binding, endpointAddress).CreateChannel();
        }

        public static async Task ServiceCallsAsync()
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

        public static async Task HttpCallsAsync()
        {
            var proxy = new HttpClient();
            System.Console.WriteLine("Store some values");
            var json = new JsonObject
            {
                {"title", "service"}
            };
            var jsonString = JsonConvert.SerializeObject(json);
            for (var i = 0; i < Counter; i++)
                await proxy.PutAsync(HttpUrl+i, CreateStringContent(jsonString));


            System.Console.WriteLine("Load values back");
            for (var i = 0; i < Counter; i++)
            {
                var result = await proxy.GetStringAsync(HttpUrl + i);
                var obj = JsonObject.Parse(result);
                System.Console.WriteLine(obj);
            }
        }

        private static StringContent CreateStringContent(string jsonString)
        {
            var stringContent = new StringContent(jsonString);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return stringContent;
        }
    }
}
