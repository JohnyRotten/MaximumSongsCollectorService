using System;
using System.IO;

namespace SongsCollectorLibrary.Utils
{
    public static class Logger
    {        
        private static readonly object _mutex = new Object();

        public static void Log(string format, params object[] args)
        {
            var msg = string.Format("{0}\t{1}", DateTime.Now, string.Format(format, args));
            Console.WriteLine(msg);
            lock (_mutex)
            using (var stream = new StreamWriter(new FileStream(Constants.ServiceLogPath, FileMode.Append, FileAccess.Write)))
            {
                stream.WriteLine(msg);
            }
        }
    }
}
