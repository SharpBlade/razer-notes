using System.Windows;
using BrandonScott.RazerNotes.Lib;
using BrandonScott.RazerNotes.ViewModels;

namespace BrandonScott.RazerNotes.Windows
{
    /// <summary>
    /// Interaction logic for winNotes.xaml
    /// </summary>
    public partial class NotesWindow
    {
        public NotesWindow()
        {
            InitializeComponent();

#if RAZER
            SharpBladeHelper.Manager.Touchpad.SetWindow(this);
#endif
        }

        public NotesWindow(Note note) : this()
        {
            var notesVm = (NotesViewModel) DataContext;
            if (!notesVm.Notes.Contains(note))
                notesVm.Notes.Add(note);
            else
                notesVm.Notes.Save();
        }

        private void NewClick(object sender, RoutedEventArgs e)
        {
#if RAZER
            SharpBladeHelper.Manager.Touchpad.ClearWindow();
#endif
            Application.Current.MainWindow = new NoteWindow();
            Close();
            Application.Current.MainWindow.Show();
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = NotesListBox.SelectedItem;

            if (selectedItem == null)
                return;

            ((NotesViewModel) DataContext).Notes.Remove((Note) selectedItem);
        }

        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = NotesListBox.SelectedItem;

            if (selectedItem == null)
                return;

            Application.Current.MainWindow = new NoteWindow((Note) selectedItem);
            Close();
            Application.Current.MainWindow.Show();
        }
    }
}
