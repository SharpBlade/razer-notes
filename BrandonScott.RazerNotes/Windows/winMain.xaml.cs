using System.Windows;
using BrandonScott.RazerNotes.Lib;

namespace BrandonScott.RazerNotes.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NoteManager _noteManager;
        private Note _note;

        public MainWindow()
        {
            InitializeComponent();

            _noteManager = new NoteManager("notes");

            if (_noteManager.Count > 0)
                _note = _noteManager[0];
            else
            {
                _note = new Note("Note Title", "Note Content");
                _noteManager.Add(_note);
            }

            NoteTitleBox.Text = _note.Title;
            NoteContentBox.Text = _note.Content;
        }

        private void SaveNoteButton_Click(object sender, RoutedEventArgs e)
        {
            _note.Title = NoteTitleBox.Text;
            _note.Content = NoteContentBox.Text;
            _noteManager.Save();
        }
    }
}
