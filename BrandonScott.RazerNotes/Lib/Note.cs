using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace BrandonScott.RazerNotes.Lib
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Note : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonProperty("Title")]
        private string _title;

        [JsonProperty("Content")]
        private string _content;

        [JsonProperty("Created")]
        private readonly DateTime _created;

        [JsonProperty("Updated")]
        private DateTime _updated;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                Updated = DateTime.Now;
                OnPropertyChanged("Title");
            }
        }

        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                Updated = DateTime.Now;
                OnPropertyChanged("Content");
            }
        }

        public DateTime Created
        {
            get { return _created; }
        }

        public DateTime Updated
        {
            get { return _updated; }
            set
            {
                _updated = value;
                OnPropertyChanged("Updated");
            }
        }

        private void OnPropertyChanged(string name)
        {
            var func = PropertyChanged;
            if (func != null)
                func(this, new PropertyChangedEventArgs(name));
        }

        public Note(string title, string content)
        {
            Title = title;
            Content = content;
            _created = DateTime.Now;
            _updated = _created;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
