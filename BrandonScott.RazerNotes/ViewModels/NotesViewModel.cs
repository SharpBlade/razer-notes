using BrandonScott.RazerNotes.Lib;

namespace BrandonScott.RazerNotes.ViewModels
{
    public class NotesViewModel
    {
        public NoteManager Notes { get; private set; }

        public NotesViewModel()
        {
            Notes = NoteManager.GetManager();
        }
    }
}
