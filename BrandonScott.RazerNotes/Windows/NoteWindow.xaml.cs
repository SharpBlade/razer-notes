using System.Windows;
using System.Windows.Controls;
using BrandonScott.RazerNotes.Lib;
using Sharparam.SharpBlade.Native;
using Sharparam.SharpBlade.Razer.Events;

namespace BrandonScott.RazerNotes.Windows
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class NoteWindow
    {
        private readonly Note _note;
        private bool _clearTitle;
        private bool _clearContent;

        private CaretManager _caretManager;

        public NoteWindow() : this(new Note("Tap to add note title", "Tap to add note content"))
        {
            _clearTitle = true;
            _clearContent = true;
        }

        public NoteWindow(Note note)
        {
            InitializeComponent();

            _note = note;
           
            NoteTitleBox.Text = _note.Title;
            NoteContentBox.Text = _note.Content;
#if RAZER
            SharpBladeHelper.Manager.Touchpad.SetWindow(this);
            SharpBladeHelper.Manager.DisableDynamicKey(RazerAPI.DynamicKeyType.DK1);
            SharpBladeHelper.Manager.DisableDynamicKey(RazerAPI.DynamicKeyType.DK2);
            SharpBladeHelper.Manager.DisableDynamicKey(RazerAPI.DynamicKeyType.DK3);
            SharpBladeHelper.Manager.DisableDynamicKey(RazerAPI.DynamicKeyType.DK5);
            SharpBladeHelper.Manager.DisableDynamicKey(RazerAPI.DynamicKeyType.DK10);
            SharpBladeHelper.Manager.DynamicKeyEvent += ManagerDynamicKeyEvent;
            SharpBladeHelper.Manager.EnableDynamicKey(RazerAPI.DynamicKeyType.DK1, @".\Resources\RazerNotesSave.png");
            SharpBladeHelper.Manager.EnableDynamicKey(RazerAPI.DynamicKeyType.DK2, @".\Resources\RazerNotesBack.png");

            SharpBladeHelper.Manager.Touchpad.EnableGesture(RazerAPI.GestureType.Tap);
            SharpBladeHelper.Manager.Touchpad.Gesture += TouchpadOnGesture;

            RenderPoll.RenderWindow = this;
            RenderPoll.Start();
#endif               
        }

        private void DisposeCaretManager()
        {
            if (_caretManager == null)
                return;

            _caretManager.Dispose();
            _caretManager = null;
        }

        private void SaveNote()
        {
            DisposeCaretManager();

            _note.Title = NoteTitleBox.Text;
            _note.Content = NoteContentBox.Text;

#if RAZER
            SharpBladeHelper.Manager.Touchpad.ClearWindow();
            SharpBladeHelper.Manager.DynamicKeyEvent -= ManagerDynamicKeyEvent;
            SharpBladeHelper.Manager.SetKeyboardCapture(false);
            SharpBladeHelper.Manager.Touchpad.DisableGesture(RazerAPI.GestureType.Tap);
            SharpBladeHelper.Manager.Touchpad.Gesture -= TouchpadOnGesture;
            RenderPoll.Stop();
#endif

            Application.Current.MainWindow = new NotesWindow(_note);
            Close();
            Application.Current.MainWindow.Show();           
        }

        private void Back()
        {
            DisposeCaretManager();

#if RAZER
            SharpBladeHelper.Manager.Touchpad.ClearWindow();
            SharpBladeHelper.Manager.DynamicKeyEvent -= ManagerDynamicKeyEvent;
            SharpBladeHelper.Manager.SetKeyboardCapture(false);
            SharpBladeHelper.Manager.Touchpad.DisableGesture(RazerAPI.GestureType.Tap);
            SharpBladeHelper.Manager.Touchpad.Gesture -= TouchpadOnGesture;
            RenderPoll.Stop();
#endif

            Application.Current.MainWindow = new NotesWindow();
            Close();
            Application.Current.MainWindow.Show();
        }   

        private void ManagerDynamicKeyEvent(object sender, DynamicKeyEventArgs e)
        {
            if (e.State != RazerAPI.DynamicKeyState.Down)
                return;

            switch (e.KeyType)
            {
                case RazerAPI.DynamicKeyType.DK1:
                    SaveNote();
                    break;
                case RazerAPI.DynamicKeyType.DK2:
                    Back();
                    break;
            }
        }

        private void TouchpadOnGesture(object sender, GestureEventArgs gestureEventArgs)
        {
            if (gestureEventArgs.GestureType != RazerAPI.GestureType.Tap)
                return;

            var x = gestureEventArgs.X;
            var y = gestureEventArgs.Y;

            var titlePosition = NoteTitleBox.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0));
            var contentPosition = NoteContentBox.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0));

            var capturing = SharpBladeHelper.Manager.KeyboardCapture;

            if (x >= titlePosition.X && x <= titlePosition.X + NoteTitleBox.Width &&
                y >= titlePosition.Y && y <= titlePosition.Y + NoteTitleBox.Height)
            {
                SharpBladeHelper.Manager.StartWPFControlKeyboardCapture(NoteTitleBox);
                if (_clearTitle)
                {
                    NoteTitleBox.Clear();
                    _clearTitle = false;
                }
                _caretManager = new CaretManager(NoteTitleBox, NoteTitleBox.Text.Length);
                HighlightInput(NoteTitleBox);
                UnhighlightInput(NoteContentBox);
            }
            else if (x >= contentPosition.X && x <= contentPosition.X + NoteContentBox.Width &&
                     y >= contentPosition.Y && y <= contentPosition.Y + NoteContentBox.Height)
            {
                SharpBladeHelper.Manager.StartWPFControlKeyboardCapture(NoteContentBox, false);
                if (_clearContent)
                {
                    NoteContentBox.Clear();
                    _clearContent = false;
                }
                _caretManager = new CaretManager(NoteContentBox, NoteContentBox.Text.Length);
                HighlightInput(NoteContentBox);
                UnhighlightInput(NoteTitleBox);
            }
            else if (capturing)
            {
                SharpBladeHelper.Manager.SetKeyboardCapture(false);
                DisposeCaretManager();
                UnhighlightInput(NoteContentBox);
                UnhighlightInput(NoteTitleBox);
            }
        }

        private void HighlightInput(TextBox input)
        {
            input.BorderThickness = new Thickness(4, 4, 4, 4);
        }

        private void UnhighlightInput(TextBox input)
        {
            input.BorderThickness = new Thickness(2, 2, 2, 2);
        }
    }
}
