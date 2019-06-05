using System;
using System.Collections.Generic;
using System.Text;

namespace Kuriimu2_Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public bool SaveButtonsEnabled { get; }

        public bool TextEditorToolsVisible { get; }

        public bool TextEditorCanExportFiles { get; }

        public bool TextEditorCanImportFiles { get; }
    }
}
