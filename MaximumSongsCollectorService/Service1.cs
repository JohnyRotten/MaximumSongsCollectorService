using System;
using System.ServiceProcess;
using System.Threading;

namespace MaximumSongsCollectorService
{
    public partial class SongsCollectorService : ServiceBase
    {
        private volatile bool _running;

        public SongsCollectorService()
        {
            InitializeComponent();
            ServiceName = nameof(SongsCollectorService);
        }

        protected override void OnStart(string[] args)
        {
            Logger.Log("Service starting.");
            _running = true;
            ThreadPool.QueueUserWorkItem(new WaitCallback(ServiceWorkerThread));
            Logger.Log("Service started.");
        }

        private void ServiceWorkerThread(object state)
        {
            var worker = Worker.Instance;
            while (_running)
            {
                try
                {
                    worker.SaveUpdatesSongs();
                    Thread.Sleep(TimeSpan.FromMinutes(3));
                }
                catch (Exception e)
                {
                    Logger.Log("Error: {0}", e);
                }
            }
            Logger.Log("Service stopped.");
        }

        protected override void OnStop()
        {
            Logger.Log("Service stopping.");
            _running = false;
        }
    }
}
