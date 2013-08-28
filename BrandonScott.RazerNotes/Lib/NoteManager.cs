using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BrandonScott.RazerNotes.Lib
{
    public class NoteManager
    {
        private List<Note> _notes;

        public string StoragePath { get; private set; }
        public StorageMethod StorageMethod { get; private set; }

        public int Count { get { return _notes.Count; } }

        public Note this[int index]
        {
            get
            {
                if (index < 0 || index >= _notes.Count)
                    throw new ArgumentOutOfRangeException("index");
                return _notes[index];
            }
        }

        public NoteManager(string storagePath, StorageMethod method = StorageMethod.Json)
        {
            StoragePath = storagePath;
            StorageMethod = method;

            Load();
        }

        public void Load()
        {
            switch (StorageMethod)
            {
                case StorageMethod.Json:
                    _notes = LoadJson(StoragePath);
                    break;
                default:
                    _notes = new List<Note>();
                    break;
            }
        }

        public static List<Note> LoadJson(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var files = Directory.GetFiles(path, "*.json");

            return files.Select(JsonHelper.Deserialize<Note>).ToList();
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

        public static void SaveJson(List<Note> notes, string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            foreach (var note in notes)
                JsonHelper.Serialize(note, Path.Combine(path, note + ".json"));
        }

        public void Add(Note note)
        {
            _notes.Add(note);
        }
    }
}
