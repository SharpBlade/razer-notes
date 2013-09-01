using System;
using System.IO;
using Newtonsoft.Json;

namespace BrandonScott.RazerNotes.Lib
{
    public static class JsonHelper
    {
        private static readonly Lazy<JsonSerializer> Serializer = new Lazy<JsonSerializer>();

        public static void Serialize<T>(T data, string file)
        {
            using (var writer = new StreamWriter(file, false))
            {
                using (var jsonWriter = new JsonTextWriter(writer) { Formatting = Formatting.Indented })
                {
                    Serializer.Value.Serialize(jsonWriter, data);
                    jsonWriter.Close();
                }
                writer.Close();
            }
        }

        public static T Deserialize<T>(string file)
        {
            if (!File.Exists(file))
                throw new ArgumentException(@"Specified file does not exist", "file",
                    new FileNotFoundException(@"Couldn't find the specified file", file));

            T result;

            using (var reader = new StreamReader(file))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    result = Serializer.Value.Deserialize<T>(jsonReader);
                    jsonReader.Close();
                }
                reader.Close();
            }

            return result;
        }
    }
}
