using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kore.Files.Models;
using Kuriimu2_Avalonia.Interfaces;

namespace Kuriimu2_Avalonia.Views
{
    public class ImageView : UserControl, IFileEditor
    {
        public ImageView(KoreFileInfo kfi)
        {
            this.InitializeComponent();

            KoreFile = kfi;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public KoreFileInfo KoreFile { get; set; }
    }
}
