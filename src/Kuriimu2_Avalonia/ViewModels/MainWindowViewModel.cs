using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Avalonia.Controls;
using Kontract;
using Kontract.Interfaces.Font;
using Kontract.Interfaces.Image;
using Kontract.Interfaces.Text;
using Kore;
using Kore.Files;
using Kore.Files.Models;
using Kuriimu2_Avalonia.Interfaces;
using Kuriimu2_Avalonia.Views;
using ReactiveUI;

namespace Kuriimu2_Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly Window _window;
        private readonly PluginLoader _pluginLoader = new PluginLoader("plugins");
        private readonly FileManager _fileManager;

        public IEnumerable Items { get; set; }

        public MainWindowViewModel(Window window)
        {
            _window = window;

            // Assign plugin loading event handler.
            _fileManager = new FileManager(_pluginLoader);
            _fileManager.IdentificationFailed += FileIdentificationFailed;
        }

        #region Tab controls

        public void CloseAllTabs()
        {
            if (Items == null)
                return;

            foreach (UserControl control in Items)
                CloseTab(control);
        }

        public void CloseTab(UserControl tab)
        {
            switch (tab)
            {
                case IFileEditor editor:
                    _fileManager.CloseFile(editor.KoreFile);
                    break;
            }

            RemoveTab(tab);
        }

        private void AddTab(UserControl control)
        {
            var items = Items.Cast<object>().ToList();
            items.Add(control);
            Items = items;
        }

        private void RemoveTab(UserControl control)
        {
            var items = Items.Cast<object>().ToList();
            items.Remove(control);
            Items = items;
        }

        #endregion

        #region Events

        // TODO: Implement FileIdentification
        private void FileIdentificationFailed(object sender, IdentificationFailedEventArgs e)
        {
            //var pe = new SelectAdapterViewModel(e.BlindAdapters.ToList(), _fileManager, _pluginLoader, e.FileName);
            //_windows.Add(pe);

            //if (_wm.ShowDialog(pe) == true)
            //{
            //    e.SelectedAdapter = pe.Adapter;

            //    if (pe.RememberMySelection)
            //    {
            //        // TODO: Do magic
            //    }
            //}
        }

        #endregion

        public async void OpenButton()
        {
            var ofd = new OpenFileDialog
            {
                Filters = _fileManager.AvaloniaFileFilters.Select(x => new FileDialogFilter { Name = x.Key, Extensions = x.Value }).ToList(),
                AllowMultiple = true
            };

            var result = await ofd.ShowAsync(_window);
            if (result == null || !result.Any())
                return;

            foreach (var file in result)
                LoadFile(file);
        }

        private bool LoadFile(string filename)
        {
            KoreFileInfo kfi = null;

            try
            {
                kfi = _fileManager.LoadFile(new KoreLoadInfo(File.OpenRead(filename), filename));
            }
            catch (LoadFileException ex)
            {
                // TODO: Notifications for exception
                //MessageBox.Show(ex.ToString(), "Open File", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            ActivateTab(kfi);

            return true;
        }

        private void ActivateTab(KoreFileInfo kfi)
        {
            if (kfi == null) return;

            switch (kfi.Adapter)
            {
                case ITextAdapter txt2:
                    AddTab(new ImageView());
                    break;
                case IImageAdapter img:
                    //ActivateItem(new ImageEditorViewModel(_fileManager, kfi));
                    break;
                case IFontAdapter fnt:
                    //ActivateItem(new FontEditorViewModel(kfi));
                    break;
            }
        }
    }
}
