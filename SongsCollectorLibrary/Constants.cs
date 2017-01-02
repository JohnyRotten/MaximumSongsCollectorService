using System;
using System.IO;

namespace SongsCollectorLibrary
{
    public static class Constants
    {
        public static readonly string ServiceLogPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments),
            "SongsCollectorService.log");

        public static readonly string DbXmlFile = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            "SongsCollector", "artists.xml");

        public static readonly string DbJsonFile = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            "SongsCollector", "artists.json");

    }
}
