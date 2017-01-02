using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SongsCollectorLibrary.Models
{
    [Magic]
    public class Song : INotifyPropertyChanged, IComparable<Song>
    {
        public string Title { get; set; }

        private int? _rate;

        public int? Rate
        {
            get { return _rate; }
            set
            {
                if (value > 5) _rate = 5;
                else if (value < 0) _rate = 0;
                else _rate = value;
            }
        }

        public bool IsHidden { get; set; }

        public override string ToString() => Title;
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName]string propName = "") 
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        public int CompareTo(Song other) => 
            string.Compare(Title, other.Title, StringComparison.Ordinal);

        public override int GetHashCode() => Title.GetHashCode() ^ 0x1fa32f;

        public override bool Equals(object obj)
        {
            var other = obj as Song;
            if (ReferenceEquals(obj, null))
                return false;
            return other != null && Title == other.Title;
        }

    }
}
