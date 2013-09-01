using System;
using System.Windows;
using BrandonScott.RazerNotes.Lib;
using BrandonScott.RazerNotes.ViewModels;
using Sharparam.SharpBlade.Native;

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
            SharpBladeHelper.Manager.Touchpad.DisableOSGesture(RazerAPI.GestureType.All);
            SharpBladeHelper.Manager.Touchpad.SetWindow(this);
            
            SharpBladeHelper.ShutdownListener();
        
            SharpBladeHelper.Manager.DynamicKeyEvent += OnDynamicKeyEvent;
            SharpBladeHelper.Manager.DisableDynamicKey(RazerAPI.DynamicKeyType.DK1);
            SharpBladeHelper.Manager.DisableDynamicKey(RazerAPI.DynamicKeyType.DK2);
            SharpBladeHelper.Manager.EnableDynamicKey(RazerAPI.DynamicKeyType.DK1, @".\Resources\RazerNotesAdd.png");
            SharpBladeHelper.Manager.EnableDynamicKey(RazerAPI.DynamicKeyType.DK2, @".\Resources\RazerNotesEdit.png");
            SharpBladeHelper.Manager.EnableDynamicKey(RazerAPI.DynamicKeyType.DK3, @".\Resources\RazerNotesDelete.png");
            SharpBladeHelper.Manager.EnableDynamicKey(RazerAPI.DynamicKeyType.DK5, @".\Resources\RazerNotesDown.png");
            SharpBladeHelper.Manager.EnableDynamicKey(RazerAPI.DynamicKeyType.DK10, @".\Resources\RazerNotesUp.png");
#endif
            if(NotesListBox.Items.Count > 0)
                NotesListBox.SelectedIndex = 0;
#if RAZER
            RenderPoll.RenderWindow = this;
            RenderPoll.Start();
#endif
        }

        public NotesWindow(Note note) : this()
        {
            var notesVm = (NotesViewModel) DataContext;
            if (!notesVm.Notes.Contains(note))
                notesVm.Notes.Add(note);
            else
                notesVm.Notes.Save();

            if (NotesListBox.Items.Count > 0)
                NotesListBox.SelectedIndex = 0;
        }

        private void NewNote()
        {
#if RAZER
            SharpBladeHelper.Manager.Touchpad.ClearWindow();
            SharpBladeHelper.Manager.DynamicKeyEvent -= OnDynamicKeyEvent;
            RenderPoll.Stop();
#endif
            Application.Current.MainWindow = new NoteWindow();
            Close();
            Application.Current.MainWindow.Show();
        }    

        private void DeleteNote()
        {
            var selectedItem = NotesListBox.SelectedItem;
            int selectedIndex = NotesListBox.SelectedIndex;

            if (selectedItem == null)
                return;

            ((NotesViewModel) DataContext).Notes.Remove((Note)selectedItem);

            if (selectedIndex > 0)
                NotesListBox.SelectedIndex = selectedIndex - 1;
            else if (NotesListBox.Items.Count == 1)
                NotesListBox.SelectedIndex = 0;
            else if (selectedIndex == 0 && NotesListBox.Items.Count > 0)
                NotesListBox.SelectedIndex = selectedIndex + 1;
        }

        private void EditNote()
        {
            var selectedItem = NotesListBox.SelectedItem;

            if (selectedItem == null)
                return;
#if RAZER
            SharpBladeHelper.Manager.Touchpad.ClearWindow();
            SharpBladeHelper.Manager.DynamicKeyEvent -= OnDynamicKeyEvent;
            RenderPoll.Stop();
#endif
            Application.Current.MainWindow = new NoteWindow((Note) selectedItem);
            Close();
            Application.Current.MainWindow.Show();
        }

        private void MoveNote(int direction)
        {
            if (NotesListBox.Items.Count == 0 ||
                NotesListBox.SelectedIndex == -1 ||
                (NotesListBox.SelectedIndex == 0 && direction == -1) ||
                (NotesListBox.SelectedIndex == NotesListBox.Items.Count - 1 && direction == 1))
                return;

            NotesListBox.SelectedIndex += direction;
            NotesListBox.ScrollIntoView(NotesListBox.SelectedItem);
        }

        private void MoveNoteUp()
        {
            MoveNote(-1);
        }

        private void MoveNoteDown()
        {
            MoveNote(1);
        }

        private void OnDynamicKeyEvent(object sender, Sharparam.SharpBlade.Razer.Events.DynamicKeyEventArgs e)
        {
            if (e.State == RazerAPI.DynamicKeyState.Down)
            {
                switch (e.KeyType)
                {
                    case RazerAPI.DynamicKeyType.DK1:
                        NewNote();
                        break;
                    case RazerAPI.DynamicKeyType.DK2:
                        EditNote();
                        break;
                    case RazerAPI.DynamicKeyType.DK3:
                        DeleteNote();
                        break;
                    case RazerAPI.DynamicKeyType.DK5:
                        MoveNoteDown();
                        break;
                    case RazerAPI.DynamicKeyType.DK10:
                        MoveNoteUp();
                        break;
                }
            }
        }
    }
}
