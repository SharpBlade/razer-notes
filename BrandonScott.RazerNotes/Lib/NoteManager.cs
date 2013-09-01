using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;

namespace BrandonScott.RazerNotes.Lib
{
    public class NoteManager : ICollection<Note>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private static readonly Dictionary<string, NoteManager> Managers = new Dictionary<string, NoteManager>();

        private ObservableCollection<Note> _notes;

        public string StoragePath { get; private set; }

        public StorageMethod StorageMethod { get; private set; }

        public int Count { get { return _notes.Count; } }

        public bool IsReadOnly { get { return false; } }

        public Note this[int index]
        {
            get { return _notes[index]; }
            set { _notes[index] = value; }
        }

        private NoteManager(string storagePath, StorageMethod method)
        {
            StoragePath = storagePath;
            StorageMethod = method;

            Load();
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action)
        {
            var func = CollectionChanged;
            if (func != null)
                func(this, new NotifyCollectionChangedEventArgs(action, _notes));
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            var func = CollectionChanged;
            if (func != null)
                func(sender, args);
        }

        public static NoteManager GetManager(string storagePath = "notes.json", StorageMethod method = StorageMethod.Json)
        {
            if (!Managers.ContainsKey(storagePath))
                Managers.Add(storagePath, new NoteManager(storagePath, method));
            return Managers[storagePath];
        }

        private void Load()
        {
            switch (StorageMethod)
            {
                case StorageMethod.Json:
                    _notes = LoadJson(StoragePath);
                    OnCollectionChanged(NotifyCollectionChangedAction.Reset);
                    _notes.CollectionChanged += (sender, args) =>
                    {
                        OnCollectionChanged(sender, args);
                        Save();
                    };
                    break;
                default:
                    _notes = new ObservableCollection<Note>();
                    OnCollectionChanged(NotifyCollectionChangedAction.Reset);
                    _notes.CollectionChanged += (sender, args) =>
                    {
                        OnCollectionChanged(sender, args);
                        Save();
                    };
                    break;
            }
        }

        private static ObservableCollection<Note> LoadJson(string path)
        {
            return File.Exists(path) ? JsonHelper.Deserialize<ObservableCollection<Note>>(path) : new ObservableCollection<Note>();
        }

        public void Save()
        {
            switch (StorageMethod)
            {
                case StorageMethod.Json:
                    SaveJson(_notes, StoragePath);
                    break;
                default:
                    throw new Exception("The selected storage method is not supported for saving notes.");
            }
        }

        private static void SaveJson(ObservableCollection<Note> notes, string path)
        {
            JsonHelper.Serialize(notes, path);
        }

        public void Add(Note note)
        {
            _notes.Add(note);
            Save();
        }

        public bool Remove(Note item)
        {
            var result = _notes.Remove(item);
            Save();
            return result;
        }

        public void Clear()
        {
            _notes.Clear();
            Save();
        }

        public bool Contains(Note item)
        {
            return _notes.Contains(item);
        }

        public void CopyTo(Note[] array, int arrayIndex)
        {
            _notes.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Note> GetEnumerator()
        {
            return _notes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
