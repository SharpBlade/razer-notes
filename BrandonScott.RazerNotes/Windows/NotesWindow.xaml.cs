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
            SharpBladeHelper.Manager.DynamicKeyEvent += Manager_DynamicKeyEvent;
            SharpBladeHelper.Manager.EnableDynamicKey(Sharparam.SharpBlade.Native.RazerAPI.DynamicKeyType.DK1, "C:\\Users\\Brandon\\Pictures\\RazerNotes\\RazerNotesAdd.png");
            SharpBladeHelper.Manager.EnableDynamicKey(Sharparam.SharpBlade.Native.RazerAPI.DynamicKeyType.DK2, "C:\\Users\\Brandon\\Pictures\\RazerNotes\\RazerNotesEdit.png");
            SharpBladeHelper.Manager.EnableDynamicKey(Sharparam.SharpBlade.Native.RazerAPI.DynamicKeyType.DK3, "C:\\Users\\Brandon\\Pictures\\RazerNotes\\RazerNotesDelete.png");
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
            NewNote();
        }
        private void NewNote()
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
            DeleteNote();
        }
        private void DeleteNote()
        {
            var selectedItem = NotesListBox.SelectedItem;

            if (selectedItem == null)
                return;

            ((NotesViewModel)DataContext).Notes.Remove((Note)selectedItem);
        }
        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            EditNote();
        }
        private void EditNote()
        {
            var selectedItem = NotesListBox.SelectedItem;

            if (selectedItem == null)
                return;

            Application.Current.MainWindow = new NoteWindow((Note)selectedItem);
            Close();
            Application.Current.MainWindow.Show();
        }
        private void ParameterFufill(object sender, System.EventArgs e)
        {
            //To fufill parameter
        }

        private void Manager_DynamicKeyEvent(object sender, Sharparam.SharpBlade.Razer.Events.DynamicKeyEventArgs e)
        {
            switch (e.KeyType)
            {
                case Sharparam.SharpBlade.Native.RazerAPI.DynamicKeyType.DK1:
                    NewNote();
                    break;
                case Sharparam.SharpBlade.Native.RazerAPI.DynamicKeyType.DK2:
                    EditNote();
                    break;
                case Sharparam.SharpBlade.Native.RazerAPI.DynamicKeyType.DK3:
                    DeleteNote();
                    break;
            }
        }

    }
}
