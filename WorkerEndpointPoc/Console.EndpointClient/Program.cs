using System;
using System.Threading;
using System.Threading.Tasks;
using Contracts;
using ProtoBuf.Meta;

namespace EndpointClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start");
            Console.ReadLine();
            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            RuntimeTypeModel.Default.Add(typeof(ProtoObj), false).SetSurrogate(typeof(ProtoObjSurrogate));

            RunAsync(cts.Token).Wait(cts.Token);
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }

        private static async Task RunAsync(CancellationToken token)
        {
            await EndpointClient.Main.MsgPackHttpCallsAsync();
            await EndpointClient.Main.ServiceCallsAsync();
            await EndpointClient.Main.HttpCallsAsync();
            await EndpointClient.Main.ProtoCallsAsync();
        }
    }
}
