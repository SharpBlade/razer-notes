using System;
using System.Collections.Generic;
using System.IO;

namespace BrandonScott.RazerNotes.Lib
{
    public class Settings
    {
        private Dictionary<string, object> _settings; 

        public static readonly Settings AppSettings = new Settings("settings.json");

        public readonly string StorageFile;

        public object this[string key]
        {
            get { return _settings.ContainsKey(key) ? _settings[key] : null; }
            set { Set(key, value); }
        }

        public object this[string key, object def]
        {
            get { return Get(key, def); }
        }

        public Settings(string file)
        {
            _settings = new Dictionary<string, object>();

            StorageFile = file;

            if (File.Exists(file))
                Load();
        }

        public void Load()
        {
            _settings = JsonHelper.Deserialize<Dictionary<string, object>>(StorageFile);
        }

        public void Save()
        {
            JsonHelper.Serialize(_settings, StorageFile);
        }

        public bool Has(string key)
        {
            return _settings.ContainsKey(key);
        }

        public object Get(string key)
        {
            if (!_settings.ContainsKey(key))
                throw new KeyNotFoundException("Couldn't find the specified key in dictionary: " + key);

            return _settings[key];
        }

        public object Get(string key, object def)
        {
            if (!Has(key))
                Set(key, def);
            
            return Get(key);
        }

        public T Get<T>(string key)
        {
            return (T) Get(key);
        }

        public T Get<T>(string key, T def)
        {
            if (!Has(key))
                Set(key, def);
            
            return Get<T>(key);
        }

        public bool TryGet<T>(string key, out T result)
        {
            bool success;

            try
            {
                result = Get<T>(key);
                success = true;
            }
            catch (Exception)
            {
                result = default(T);
                success = false;
            }

            return success;
        }

        public void Set(string key, object value)
        {
            if (_settings.ContainsKey(key))
                _settings.Add(key, value);
            else
                _settings[key] = value;

            Save();
        }
    }
}
