using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Kuriimu2_Avalonia.Styles
{
    public class Menu : UserControl
    {
        public Menu()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
