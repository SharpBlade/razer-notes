using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BrandonScott.RazerNotes.Lib;

namespace BrandonScott.RazerNotes.Windows
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class NoteWindow : Window
    {
        private readonly Note _note;

        public NoteWindow() : this(new Note("Tap to add note title", "Tap to add note content"))
        {

        }

        public NoteWindow(Note note)
        {
            InitializeComponent();

            _note = note;

            NoteTitleBox.Text = _note.Title;
            NoteContentBox.Text = _note.Content;
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            _note.Title = NoteTitleBox.Text;
            _note.Content = NoteContentBox.Text;
            Application.Current.MainWindow = new NotesWindow(_note);
            Close();
            Application.Current.MainWindow.Show();
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = new NotesWindow();
            Close();
            Application.Current.MainWindow.Show();
        }
    }
}
