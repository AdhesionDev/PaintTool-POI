using Windows.UI.Input.Inking;
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
            //this.InitializeComponent();
            this.InitializeComponent();


            /*          inkCanvas.InkPresenter.InputDeviceTypes =
            Windows.UI.Core.CoreInputDeviceTypes.Mouse |
            Windows.UI.Core.CoreInputDeviceTypes.Pen |
            Windows.UI.Core.CoreInputDeviceTypes.Touch;

                      // Set initial ink stroke attributes.
                      InkDrawingAttributes drawingAttributes = new InkDrawingAttributes();
                      drawingAttributes.Color = Windows.UI.Colors.Black;
                      drawingAttributes.IgnorePressure = false;
                      drawingAttributes.FitToCurve = true;
                      inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
                      */
        }

        /// <summary>
        /// Call this method for some debug behaviour.
        /// </summary>
        private void Debug_Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Get singleton var: {SampleClass.Instance.sampleVar}");
        }

        private void colorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
        {/*
            InkDrawingAttributes drawingAttributes = inkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Color = sender.Color;
            inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
            */
        }
    }
}
