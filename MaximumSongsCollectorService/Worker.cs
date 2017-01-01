using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace MaximumSongsCollectorService
{

    public class Current
    {
        public string Artist { get; set; }
        public string Song { get; set; }
        public DateTime StartTime { get; set; }
    }

    public class History
    {
        public string Artist { get; set; }
        public string Song { get; set; }
        public DateTime StartTime { get; set; }
    }

    public class RootObject
    {
        public Current Current { get; set; }
        public List<History> History { get; set; }
    }

    [Serializable]
    public class Worker
    {
        private static readonly string _artistsFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), 
            "MaximumSongsCollector", "artists.xml");

        private const string _url = "http://maximum.ru/currenttrack.aspx?station=maximum";

        public List<Artist> Artists { get; set; } = new List<Artist>();

        public Worker() { }

        public static Worker Instance => Serializer.Get<Worker>(_artistsFilePath);

        public void UpdateSongs()
        {
            Logger.Log("Updating songs.");
            foreach (var pair in GetUpdatesSongs())
            {
                var artist = Artists.FirstOrDefault(a => a.Name.ToUpper().Equals(pair.Key.ToUpper()));
                if (artist != null)
                {
                    foreach (var song in pair.Value)
                    {
                        artist.Songs.Add(song);
                        Logger.Log("Added: {0} - {1}", pair.Key, song);
                    }
                }
                else
                {
                    Artists.Add(new Artist { Name = pair.Key, Songs = new HashSet<string>(pair.Value) });
                    Logger.Log("Added: {0} - {1}", pair.Key, string.Join("|", pair.Value));
                }
            }            
        }

        public void SaveUpdatesSongs()
        {
            UpdateSongs();
            Save();
        }

        private Dictionary<string, List<string>> GetUpdatesSongs()
        {
            var map = new Dictionary<string, List<string>>();
            var root = JsonConvert.DeserializeObject<RootObject>(GetJson());
            foreach (var item in root.History)
            {
                if (map.ContainsKey(item.Artist))
                {
                    var list = map[item.Artist];
                    if (!list.Contains(item.Song))
                    {
                        map[item.Artist].Add(item.Song);
                    }
                }
                else
                {
                    map[item.Artist] = new List<string> { item.Song };
                }
            }
            return map;
        }

        public string GetJson()
        {
            var data = string.Empty;
            var request = (HttpWebRequest)WebRequest.Create(_url);
            var response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                data = readStream.ReadToEnd();

                response.Close();
                readStream.Close();
            }
            return data;
        }

        public void Save()
        {
            var file = new FileInfo(_artistsFilePath);
            if (!file.Exists)
            {
                Directory.CreateDirectory(file.Directory.FullName);
                Logger.Log("Create dir.");
            }
            Serializer.Set<Worker>(_artistsFilePath, this);
            Logger.Log("Saving");
        }

    }
}
