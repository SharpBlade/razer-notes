using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BrandonScott.RazerNotes.Lib
{
    public class Note
    {
        public string Title;
        public string Content;
        public readonly DateTime Created;
        public DateTime Updated;

        public Note(string title, string content)
        {
            Title = title;
            Content = content;
            Created = DateTime.Now;
            Updated = Created;
        }

        public override string ToString()
        {
            return String.Format("{0}-{1}",
                                 Path.GetInvalidFileNameChars().Aggregate(Title,
                                    (current, c) => current.Replace(c, '_')),
                                Created.ToString("yyyyMMddHHmmss"));
        }
    }
}
