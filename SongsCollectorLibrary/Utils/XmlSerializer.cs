using System.IO;
using System.Text;

namespace SongsCollectorLibrary.Utils
{
    public class XmlSerializer<T> : ISerializer<T> where T : new()
    {
        public T Deserialize(string content)
        {
            using (var stream = new StringReader(content))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stream);
            }
        }

        public string Serialize(T item)
        {
            var sb = new StringBuilder();
            using (var stream = new StringWriter(sb))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                serializer.Serialize(stream, item);
                return sb.ToString();
            }
        }
    }
}
