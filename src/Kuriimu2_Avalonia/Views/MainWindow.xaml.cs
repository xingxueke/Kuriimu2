using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kuriimu2_Avalonia.ViewModels;

namespace Kuriimu2_Avalonia.Views
{
    public class MainWindow : Window
    {
        private MainWindowViewModel _dataContext;

        private DockPanel _root;
        private Window _window;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            _window = this.Find<Window>("MainWindow");
            _root = this.Find<DockPanel>("Root");
            _root.AddHandler(DragDrop.DropEvent, MainWindow_OnDrop);

            DataContext = _dataContext = new MainWindowViewModel(_window);

        }

        private void MainWindow_OnClose(object sender, CancelEventArgs e)
        {
            _dataContext.CloseAllTabs();
        }

        private void MainWindow_OnDrop(object sender, DragEventArgs e)
        {
            //_dataContext.FileDrop(e);
        }

        private void NewMenu_OnClick(object sender, RoutedEventArgs e)
        {
            //_dataContext.NewButton();
        }

        private void OpenMenu_OnHotKey()
        {
            _dataContext.OpenButton();
        }

        private void OpenMenu_OnClick(object sender, RoutedEventArgs e)
        {
            _dataContext.OpenButton();
        }

        private void OpenTypeMenu_OnClick(object sender, RoutedEventArgs e)
        {
            //_dataContext.OpenTypeButton();
        }

        private void SaveMenu_OnClick(object sender, RoutedEventArgs e)
        {
            //_dataContext.SaveButton();
        }

        private void SaveAsMenu_OnClick(object sender, RoutedEventArgs e)
        {
            //_dataContext.SaveAsButton();
        }

        private void ExitMenu_OnClick(object sender, RoutedEventArgs e)
        {
            _dataContext.CloseAllTabs();
            Close();
        }

        private void Items_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //_dataContext.TabChanged(e);
        }

        private void Item_OnClick(object sender, RoutedEventArgs e)
        {
            _dataContext.CloseTab(sender as UserControl);
        }
    }
}
