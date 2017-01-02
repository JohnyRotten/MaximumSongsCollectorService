using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SongsCollectorLibrary.Utils;
using SongsCollectorLibrary;
using SongsCollectorLibrary.Models;
using MaximumSongsCollectorService.Collectors;

namespace MaximumSongsCollectorService
{
    [Serializable]
    public class Worker
    {
        
        private readonly List<ICollector> Collectors = new List<ICollector>();
        private volatile bool _saved;

        public List<Artist> Artists { get; set; } = new List<Artist>();

        public Worker() { }

        public static Worker Instance => Serializer.Get<Worker>(Constants.DbFile);

        public void UpdateSongs()
        {
            Logger.Log("Updating songs.");
            foreach (var pair in Collectors.SelectMany(c => c.NewSongs))
            {
                var artist = Artists.FirstOrDefault(a => a.Name.ToUpper().Equals(pair.Key.ToUpper()));
                if (artist != null)
                {
                    foreach (var song in pair.Value.Except(artist.Songs))
                    {
                        artist.Songs.Add(song);
                        Logger.Log("Added: {0} - {1}", pair.Key, song);
                        _saved = false;
                    }
                }
                else
                {
                    Artists.Add(new Artist { Name = pair.Key, Songs = new HashSet<string>(pair.Value) });
                    Logger.Log("Added: {0} - {1}", pair.Key, string.Join("|", pair.Value));
                    _saved = false;
                }
            }            
        }

        public void SaveUpdatesSongs()
        {
            UpdateSongs();
            Save();
        }
                
        public void Save()
        {
            if (_saved) return;
            var file = new FileInfo(Constants.DbFile);
            if (!file.Exists)
            {
                Directory.CreateDirectory(file.Directory.FullName);
                Logger.Log("Create dir.");
            }
            Serializer.Set<Worker>(Constants.DbFile, this);
            Logger.Log("Saving");
            _saved = true;
        }

        public void AddCollectors(params ICollector[] collectors)
        {
            Collectors.AddRange(collectors);
        }

    }
}
