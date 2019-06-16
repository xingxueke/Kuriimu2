using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Kuriimu2_Avalonia.Styles
{
    public class ToolBarButton : UserControl
    {
        public ToolBarButton()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
