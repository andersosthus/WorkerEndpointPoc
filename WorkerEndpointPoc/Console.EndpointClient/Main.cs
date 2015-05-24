using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.Threading.Tasks;
using Contracts;
using Newtonsoft.Json;
using proactima.jsonobject;

namespace EndpointClient
{
    public static class Main
    {
        private const string ServiceUrl = "net.tcp://127.0.0.1:91/StoreAndLoadJson";
        private const string HttpUrl = "http://127.0.0.1:81/json/";
        private const int Counter = 1000;

        private static IStoreAndLoadJson GetAProxy()
        {
            var binding = new NetTcpBinding(SecurityMode.None)
            {
                OpenTimeout = TimeSpan.FromMinutes(10),
                SendTimeout = TimeSpan.FromMinutes(10),
                MaxConnections = 5000,
                ListenBacklog = 5000,
                PortSharingEnabled = false
            };
            var endpointAddress = new EndpointAddress(ServiceUrl);

            var channelFactory = new ChannelFactory<IStoreAndLoadJson>
                (binding, endpointAddress);
            var channel = channelFactory.CreateChannel();
            return channel;
        }

        public static async Task ServiceCallsAsync()
        {
            var proxy = GetAProxy();
            await proxy.StoreAsync("0", new JsonObject());
            await proxy.LoadAsync("0");
            var ids = CreateIds("service");
            var json = CreateJsonObject();
            Console.WriteLine("Running Service {0} times", Counter);
            var watch = Stopwatch.StartNew();

            await Task.WhenAll(ids
                .Select(i => proxy.StoreAsync("service" + i, json)));

            await Task.WhenAll(ids
                .Select(i => proxy.LoadAsync("service" + i)));

            watch.Stop();
            Console.WriteLine("Net.Tcp took: " + watch.Elapsed);
        }

        public static async Task HttpCallsAsync()
        {
            var proxy = new HttpClient();
            var ids = CreateIds("http");
            var json = CreateJsonObject();

            await proxy.PutAsync(HttpUrl + "0", CreateStringContent(JsonConvert.SerializeObject(json)));
            await proxy.GetStringAsync(HttpUrl + "0");

            Console.WriteLine("Running Http {0} times", Counter);
            var watch = Stopwatch.StartNew();

            await Task.WhenAll(ids
                .Select(i => proxy.PutAsync(HttpUrl + i, CreateStringContent(JsonConvert.SerializeObject(json)))));

            var stringResults = await Task.WhenAll(ids
                .Select(i => proxy.GetStringAsync(HttpUrl + i)));

            var objs = stringResults.Select(result => JsonObject.Parse(result)).ToList();
            

            watch.Stop();
            if (objs.Count != Counter)
                Console.WriteLine("funky...");
            Console.WriteLine("Http took: " + watch.Elapsed);
        }

        private static JsonObject CreateJsonObject()
        {
            var json = new JsonObject
            {
                {"title", "service"},
                {"title1", "service"},
                {"title2", "service"},
                {"title3", "service"},
                {"title4", "service"},
                {"title5", "service"},
                {"title6", "service"},
                {"title7", "service"},
                {"title8", "service"},
                {"title9", "service"},
                {"title10", "service"},
                {"title11", "service"},
            };
            return json;
        }

        private static IList<string> CreateIds(string name)
        {
            return Enumerable.Range(1, Counter).Select(c => String.Format("{0}_{1}", name, c)).ToList();
        }

        private static StringContent CreateStringContent(string jsonString)
        {
            var stringContent = new StringContent(jsonString);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return stringContent;
        }
    }
}