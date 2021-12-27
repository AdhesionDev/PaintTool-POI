using System.Collections.Generic;
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
            InkCanvas inkCanvas = new InkCanvas();
            PopulateToolButtons();

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

        private void PopulateToolButtons()
        {
            List<Button> buttons = new List<Button>();
            Button button = new Button();
            button.FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe MDL2 Assets");
            button.Content = "\xED63";
            buttons.Add(button);
            buttons.Add(new ToolButton("ED63"));
            toolsGridView.ItemsSource = buttons;

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
class ToolButton : Button
{

    public ToolButton(string iconString)
    {
        if (string.IsNullOrEmpty(iconString))
        {
            throw new System.ArgumentException($"'{nameof(iconString)}' cannot be null or empty.", nameof(iconString));
        }

        this.FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe MDL2 Assets");
        this.Content = "\\" + "x" + iconString;
    }

}