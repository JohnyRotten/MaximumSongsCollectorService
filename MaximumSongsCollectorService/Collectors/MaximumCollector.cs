using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace MaximumSongsCollectorService.Collectors
{
    class MaximumCollector : ICollector
    {
        private const string _url = "http://maximum.ru/currenttrack.aspx?station=maximum";

        private class MaximumJsonConfig
        {
            public class CurrentTrack
            {
                public string Artist { get; set; }
                public string Song { get; set; }
                public DateTime StartTime { get; set; }
            }

            public class HistoryTrack
            {
                public string Artist { get; set; }
                public string Song { get; set; }
                public DateTime StartTime { get; set; }
            }

            public CurrentTrack Current { get; set; }
            public List<HistoryTrack> History { get; set; }
        }

        public Dictionary<string, List<string>> NewSongs
        {
            get
            {
                var map = new Dictionary<string, List<string>>();
                var root = JsonConvert.DeserializeObject<MaximumJsonConfig>(GetJson());
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
        }

        private string GetJson()
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

    }
}
