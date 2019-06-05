using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Kuriimu2_Avalonia.Views
{
    public class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            AddHandler(DragDrop.DropEvent, Drop);
            AddHandler(DragDrop.DragOverEvent, DragOver);
        }

        private void NewMenu_OnClick(object sender, RoutedEventArgs e)
        {
            ;
        }

        private void OpenMenu_OnClick(object sender, RoutedEventArgs e)
        {
            ;
        }

        private void OpenTypeMenu_OnClick(object sender, RoutedEventArgs e)
        {
            ;
        }

        private void SaveMenu_OnClick(object sender, RoutedEventArgs e)
        {
            ;
        }

        private void SaveAsMenu_OnClick(object sender, RoutedEventArgs e)
        {
            ;
        }

        private void Items_TabChanged(object sender, SelectionChangedEventArgs e)
        {
            ;
        }

        private void ItemButton_OnClose(object sender, RoutedEventArgs e)
        {
            ;
        }

        private void DragOver(object sender, DragEventArgs e)
        {
            // Only allow Copy or Link as Drop Operations.
            e.DragEffects = e.DragEffects & (DragDropEffects.Copy | DragDropEffects.Link);

            // Only allow if the dragged data contains text or filenames.
            if (!e.Data.Contains(DataFormats.Text) && !e.Data.Contains(DataFormats.FileNames))
                e.DragEffects = DragDropEffects.None;
        }

        private void Drop(object sender, DragEventArgs e)
        {
            ;
            //if (e.Data.Contains(DataFormats.Text))
            //    _DropState.Text = e.Data.GetText();
            //else if (e.Data.Contains(DataFormats.FileNames))
            //    _DropState.Text = string.Join(Environment.NewLine, e.Data.GetFileNames());
        }
    }
}
