using System.ServiceProcess;

namespace MaximumSongsCollectorService
{
    public static class Program
    {
        public static void Main() => ServiceBase.Run(new ServiceBase[] { new SongsCollectorService() });
    }
}
