using System;
using System.Threading;
using System.Threading.Tasks;

namespace EndpointClient
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start");
            Console.ReadLine();
            var cts = new CancellationTokenSource();
            System.Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            RunAsync(cts.Token).Wait(cts.Token);
 
        }

        private static async Task RunAsync(CancellationToken token)
        {
            var main = new Main();
            await main.ServiceCallsAsync();
            await main.HttpCallsAsync();
        }

       
    }
}
