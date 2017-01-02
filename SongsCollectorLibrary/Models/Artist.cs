using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Collections.Specialized;

namespace SongsCollectorLibrary.Models
{
    [Serializable, Magic]
    public class Artist : INotifyPropertyChanged
    {

        public Artist()
        {
            Songs.CollectionChanged += SongsChanged;
        }

        private void SongsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Rate));
        }

        public string Name { get; set; }
        public ObservableCollection<Song> Songs { get; set; } = new ObservableCollection<Song>();

        public double? Rate
        {
            get
            {
                var songs = Songs.Where(s => s.Rate != null).ToList();
                return songs.Any() ? songs.Sum(s => s.Rate)/songs.Count : null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName]string propName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

    }
}
