namespace SongsCollectorLibrary.Utils
{
    public interface ISerializer<T> where T : new()
    {
        T Deserialize(string content);
        string Serialize(T item);
    }
}