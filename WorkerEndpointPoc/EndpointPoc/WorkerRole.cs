using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace EndpointPoc
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        private IDisposable _app = null;

        public override void Run()
        {
            Trace.TraceInformation("EndpointPoc is running");

            try
            {
                RunAsync(cancellationTokenSource.Token).Wait();
            }
            finally
            {
                runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            var endpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["HTTP"];
            var baseUri = String.Format("{0}://{1}",
                endpoint.Protocol, endpoint.IPEndpoint);

            var startupMessage = String.Format("Starting OWIN at {0}", baseUri);
            Trace.TraceInformation(startupMessage, "Information");

            _app = WebApp.Start<Startup>(new StartOptions(baseUri));

            Trace.TraceInformation("EndpointPoc has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("EndpointPoc is stopping");

            cancellationTokenSource.Cancel();
            runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("EndpointPoc has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }
    }
}