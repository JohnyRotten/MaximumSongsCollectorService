using System;
using System.IO;
using System.Text;

namespace MaximumSongsCollectorService
{
    public static class Logger
    {

        private readonly static string LogPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments),
            nameof(SongsCollectorService) + ".log");

        private static readonly object _mutex = new Object();

        public static void Log(string format, params object[] args)
        {
            var msg = string.Format("{0}\t{1}", DateTime.Now, string.Format(format, args));
            Console.WriteLine(msg);
            lock (_mutex)
            using (var stream = new StreamWriter(new FileStream(LogPath, FileMode.Append, FileAccess.Write)))
            {
                stream.WriteLine(msg);
            }
        }
    }
}
