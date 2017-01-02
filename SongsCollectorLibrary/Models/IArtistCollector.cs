using System.Collections.ObjectModel;

namespace SongsCollectorLibrary.Models
{
    public interface IArtistCollector
    {
        ObservableCollection<Artist> Artists { get; set; }
    }
}