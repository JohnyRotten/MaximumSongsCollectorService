using System;
using System.IO;
using System.Text;

namespace SongsCollectorLibrary.Utils
{
    public class Serializer<T> where T : new()
    {
        private readonly string _path;
        private readonly ISerializer<T> _serializer;

        public Serializer(string path, ISerializer<T> serializer)
        {
            _path = path;
            _serializer = serializer;
        }

        public T Get()
        {
            try
            {
                using (var stream = new FileStream(_path, FileMode.Open, FileAccess.Read))
                {
                    var bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, bytes.Length);
                    return _serializer.Deserialize(Encoding.UTF8.GetString(bytes));
                }
            }
            catch (Exception e)
            {
                Logger.Log("Error: {0}", e);
            }
            return new T();
        }

        public void Set(T item)
        {
            try
            {
                using (var stream = new FileStream(_path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    var bytes = Encoding.UTF8.GetBytes(_serializer.Serialize(item));
                    stream.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception e)
            {
                Logger.Log("Error: {0}", e);
            }
        }
    }
}