using System;
using System.Diagnostics;
using System.Net;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Contracts;
using EndpointPoc.Services;
using Microsoft.Owin.Hosting;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace EndpointPoc
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent _runCompleteEvent = new ManualResetEvent(false);

        private IDisposable _app;
        private ServiceHost _serviceHost;

        public override void Run()
        {
            Trace.TraceInformation("EndpointPoc is running");

            try
            {
                RunAsync(_cancellationTokenSource.Token).Wait();
            }
            finally
            {
                _runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            CreateWebHost();
            CreateServiceHost();

            Trace.TraceInformation("EndpointPoc has been started");

            return result;
        }

        private void CreateWebHost()
        {
            var externalEndPoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["HTTP"];
            var endpoint = String.Format("{0}://{1}",
                externalEndPoint.Protocol, externalEndPoint.IPEndpoint);

            var startupMessage = String.Format("Starting OWIN at {0}", endpoint);
            Trace.TraceInformation(startupMessage, "Information");
            _app = WebApp.Start<Startup>(new StartOptions(endpoint));
        }

        private void CreateServiceHost()
        {
            _serviceHost = new ServiceHost(typeof (JsonService));

            var binding = new NetTcpBinding(SecurityMode.None)
            {
                MaxConnections = 500,
                PortSharingEnabled = false,
                ListenBacklog = 500
            };

            var externalEndPoint =
                RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["TCP"];
            var endpoint = String.Format("net.tcp://{0}/LoanCalculator",
                externalEndPoint.IPEndpoint);
            var startupMessage = String.Format("Starting OWIN at {0}", endpoint);

            Trace.TraceInformation(startupMessage, "Information");

            _serviceHost.AddServiceEndpoint(typeof (IStoreAndLoadJson), binding, endpoint);
            _serviceHost.Open();
        }

        public override void OnStop()
        {
            Trace.TraceInformation("EndpointPoc is stopping");

            _cancellationTokenSource.Cancel();
            _runCompleteEvent.WaitOne();
            _app.Dispose();

            base.OnStop();

            Trace.TraceInformation("EndpointPoc has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}