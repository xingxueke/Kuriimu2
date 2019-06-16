using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using DynamicData.Binding;
using Kontract;
using Kontract.Interfaces.Font;
using Kontract.Interfaces.Image;
using Kontract.Interfaces.Text;
using Kore;
using Kore.Exceptions.FileManager;
using Kore.Files;
using Kore.Files.Models;
using Kuriimu2_Avalonia.Interfaces;
using Kuriimu2_Avalonia.Views;
using ReactiveUI;

namespace Kuriimu2_Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly PluginLoader _pluginLoader = new PluginLoader("plugins");
        private readonly FileManager _fileManager;

        private IObservableCollection<IFileEditor> _items;
        private bool _saveButtonsEnabled;
        private bool _textEditorCanExportFiles;
        private bool _textEditorCanImportFiles;

        public IObservableCollection<IFileEditor> Items { get => _items; set => this.RaiseAndSetIfChanged(ref _items, value); }
        public bool SaveButtonsEnabled { get => _saveButtonsEnabled; set => this.RaiseAndSetIfChanged(ref _saveButtonsEnabled, value); }
        public bool TextEditorCanExportFiles { get => _textEditorCanExportFiles; set => this.RaiseAndSetIfChanged(ref _textEditorCanExportFiles, value); }
        public bool TextEditorCanImportFiles { get => _textEditorCanImportFiles; set => this.RaiseAndSetIfChanged(ref _textEditorCanImportFiles, value); }

        public MainWindowViewModel()
        {
            // Assign plugin loading event handler.
            _fileManager = new FileManager(_pluginLoader);
            _fileManager.IdentificationFailed += FileIdentificationFailed;

            _items = new ObservableCollectionExtended<IFileEditor>();
        }

        private void OpenMenu_OnHotKey(Window window)
        {
            OpenButton(window);
        }

        #region Tab controls

        public void CloseAllTabs()
        {
            if (Items == null)
                return;

            foreach (var control in Items)
                CloseTab(control);
        }

        public void CloseTab(IFileEditor tab)
        {
            _fileManager.CloseFile(tab.KoreFile);
            RemoveTab(tab);
        }

        public void AddTab(IFileEditor control)
        {
            Items.Add(control);
        }

        public void RemoveTab(IFileEditor control)
        {
            Items.Remove(control);
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

        public async void OpenButton(Window window)
        {
            var ofd = new OpenFileDialog
            {
                Filters = _fileManager.AvaloniaFileFilters.Select(x => new FileDialogFilter { Name = x.Key, Extensions = x.Value }).ToList(),
                AllowMultiple = true
            };

            var result = await ofd.ShowAsync(window);
            if (result == null || !result.Any())
                return;

            foreach (var file in result)
                LoadFile(file);
        }

        public bool LoadFile(string filename)
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
                    AddTab(new ImageView(kfi));
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
