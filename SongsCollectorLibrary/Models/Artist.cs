using System;
using System.Collections.Generic;

namespace SongsCollectorLibrary.Models
{
    [Serializable]
    public class Artist
    {        
        public string Name { get; set; }
        public HashSet<string> Songs { get; set; } = new HashSet<string>();
    }
}
