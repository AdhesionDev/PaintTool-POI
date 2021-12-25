using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace AdhesionTekPaintTool
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            inkCanvas.InkPresenter.InputDeviceTypes =
  Windows.UI.Core.CoreInputDeviceTypes.Mouse |
  Windows.UI.Core.CoreInputDeviceTypes.Pen |
  Windows.UI.Core.CoreInputDeviceTypes.Touch;

        }

        /// <summary>
        /// Call this method for some debug behaviour.
        /// </summary>
        private void Debug_Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Debug btn clicked.");
        }
    }
}
