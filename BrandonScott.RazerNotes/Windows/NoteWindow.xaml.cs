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
using Sharparam.SharpBlade.Native;
using Sharparam.SharpBlade.Razer.Events;

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
#if RAZER
            SharpBladeHelper.Manager.Touchpad.SetWindow(this);
            SharpBladeHelper.Manager.DisableDynamicKey(RazerAPI.DynamicKeyType.DK1);
            SharpBladeHelper.Manager.DisableDynamicKey(RazerAPI.DynamicKeyType.DK2);
            SharpBladeHelper.Manager.DisableDynamicKey(RazerAPI.DynamicKeyType.DK3);
            SharpBladeHelper.Manager.DisableDynamicKey(RazerAPI.DynamicKeyType.DK5);
            SharpBladeHelper.Manager.DisableDynamicKey(RazerAPI.DynamicKeyType.DK10);
            SharpBladeHelper.Manager.DynamicKeyEvent += Manager_DynamicKeyEvent;
            SharpBladeHelper.Manager.EnableDynamicKey(RazerAPI.DynamicKeyType.DK1, @".\Resources\RazerNotesSave.png");
            SharpBladeHelper.Manager.EnableDynamicKey(RazerAPI.DynamicKeyType.DK2, @".\Resources\RazerNotesBack.png");

            SharpBladeHelper.Manager.Touchpad.Gesture += TouchpadOnGesture;

            RenderPoll.RenderWindow = this;
            RenderPoll.Start();
#endif
        }

        private void SaveNote()
        {
            _note.Title = NoteTitleBox.Text;
            _note.Content = NoteContentBox.Text;
#if RAZER
            SharpBladeHelper.Manager.Touchpad.ClearWindow();
            SharpBladeHelper.Manager.DynamicKeyEvent -= Manager_DynamicKeyEvent;
            SharpBladeHelper.Manager.Touchpad.Gesture -= TouchpadOnGesture;
            RenderPoll.Stop();
#endif
            Application.Current.MainWindow = new NotesWindow(_note);
            Close();
            Application.Current.MainWindow.Show();           
        }

        private void Back()
        {
#if RAZER
            SharpBladeHelper.Manager.Touchpad.ClearWindow();
            SharpBladeHelper.Manager.DynamicKeyEvent -= Manager_DynamicKeyEvent;
            SharpBladeHelper.Manager.Touchpad.Gesture -= TouchpadOnGesture;
            RenderPoll.Stop();
#endif
            Application.Current.MainWindow = new NotesWindow();
            Close();
            Application.Current.MainWindow.Show();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            SaveNote();
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            Back();
        }

        private void Manager_DynamicKeyEvent(object sender, DynamicKeyEventArgs e)
        {
            if (e.State == RazerAPI.DynamicKeyState.Down)
            {
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
        }

        private void TouchpadOnGesture(object sender, GestureEventArgs gestureEventArgs)
        {
            if (gestureEventArgs.GestureType != RazerAPI.GestureType.Tap)
                return;

            var x = gestureEventArgs.X;
            var y = gestureEventArgs.Y;

            var titlePosition = NoteTitleBox.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0));
            var contentPosition = NoteTitleBox.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0));

            var capturing = SharpBladeHelper.Manager.KeyboardCapture;

            if (x >= titlePosition.X && x <= titlePosition.X + NoteTitleBox.Width &&
                y >= titlePosition.Y && y <= titlePosition.Y + NoteTitleBox.Height)
            {
                if (!capturing)
                    SharpBladeHelper.Manager.StartWPFControlKeyboardCapture(NoteTitleBox);
            }
            else if (x >= contentPosition.X && x <= contentPosition.X + NoteContentBox.Width &&
                     y >= contentPosition.Y && y <= contentPosition.Y + NoteContentBox.Height)
            {
                if (!capturing)
                    SharpBladeHelper.Manager.StartWPFControlKeyboardCapture(NoteContentBox);
            }
            else if (capturing)
                SharpBladeHelper.Manager.SetKeyboardCapture(false);
        }
    }
}
