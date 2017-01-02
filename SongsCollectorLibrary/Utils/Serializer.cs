using SongsCollectorLibrary.Utils;
using System;
using System.IO;
using System.Xml.Serialization;

namespace SongsCollectorLibrary.Utils
{
    public static class Serializer
    {

        public static T Get<T>(string path) where T : new()
        {
            try
            {
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                Logger.Log("Error: {0}", e);
            }
            return new T();
        }

        public static void Set<T>(string path, T item) where T : new()
        {
            try
            {
                using (var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(stream, item);
                }
            }
            catch (Exception e)
            {
                Logger.Log("Error: {0}", e);
            }
        }

    }
}
