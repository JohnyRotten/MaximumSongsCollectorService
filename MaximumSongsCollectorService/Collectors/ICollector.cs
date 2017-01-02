using System.Collections.Generic;

namespace MaximumSongsCollectorService.Collectors
{
    public interface ICollector
    {
        Dictionary<string, List<string>> NewSongs { get; }
    }
}
