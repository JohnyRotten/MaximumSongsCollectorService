using Newtonsoft.Json;

namespace SongsCollectorLibrary.Utils
{
    public class JsonSerializer<T> : ISerializer<T> where T : new()
    {
        public T Deserialize(string content)
        {
            return JsonConvert.DeserializeObject<T>(content);
        }

        public string Serialize(T item)
        {
            return JsonConvert.SerializeObject(item);
        }
    }
}