using Avalonia;
using Avalonia.Input.Raw;
using Avalonia.Markup.Xaml;

namespace Kuriimu2_Avalonia
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        /// <summary>
        /// Register's the services needed by Avalonia.
        /// </summary>
        public override void RegisterServices()
        {
            base.RegisterServices();

            // A main window border, not present in the xaml, is hit when checking for dropability
            // This border has AllowDrop disabled and will disable drag'n'drop application wide due to that issue
            // TODO: Raise issue on Avalonia github
            // TODO: Remove own DragDropDevice after issue is solved
            AvaloniaLocator.CurrentMutable.Bind<IDragDropDevice>().ToConstant(K2DragDropDevice.Instance);
        }
    }
}
