using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Kuriimu2_Avalonia.Views
{
    public class ImageView : UserControl
    {
        public ImageView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
