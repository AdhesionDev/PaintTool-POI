using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

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
            AddToolItems();

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

        private void AddToolItems()
        {
            List<ToolItemGrid> items = new List<ToolItemGrid>();

            items.Add(new ToolItemGrid("\xED63", "Pencel"));
            items.Add(new ToolItemGrid("\xE7E6", "High Light"));
            items.Add(new ToolItemGrid("\xEDFB", "Pen"));
            items.Add(new ToolItemGrid("\xE75C", "消しゴム"));
            items.Add(new ToolItemGrid("\xEF3C", "Color Picker"));
            items.Add(new ToolItemGrid("\xE759", "Move"));
            items.Add(new ToolItemGrid("\xF407", "Selection"));
            items.Add(new ToolItemGrid("\xF408", "Free Selection"));
            items.Add(new ToolItemGrid("\xE710", "Add new Item"));
            toolsGridView.ItemsSource = items;

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

        private async void showAbout()
        {

            ContentDialog aboutDialog = new ContentDialog()
            {
                Title = "About",
                Content = "Adhesion Painting Tool\n Version: Test",
                CloseButtonText = "OK"
            };
            await aboutDialog.ShowAsync();
        }

        private void About_Button_Click(object sender, RoutedEventArgs e)
        {
            showAbout();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            CoreApplication.Exit();
        }


    }



}
public delegate T GetItem<T>();

class ToolItemGrid : Grid
{
    private IconElement iconElement;
    private TextBlock descripution;
    public ToolItemGrid(string iconString, string descriputionString) : this(() =>
        {
            FontIcon icon = new FontIcon()
            {
                FontFamily = new FontFamily("Segoe MDL2 Assets"),
                Glyph = iconString
            };
            return icon;
        },
        () =>
        {
            TextBlock descriputionText = new TextBlock()
            {
                Text = descriputionString
            };
            return descriputionText;
        })
    {
    }
    public ToolItemGrid(IconElement icon, TextBlock descripution)
    {
        this.SetIconAndText(icon, descripution);
    }

    public ToolItemGrid(GetItem<IconElement> getIcon, GetItem<TextBlock> getText)
    {
        IconElement iconElement = getIcon?.Invoke();
        TextBlock textBlock = getText?.Invoke();
        this.SetIconAndText(iconElement, textBlock);
    }


    private void SetIconAndText(IconElement icon, TextBlock descripution)
    {
        this.iconElement = icon;
        this.descripution = descripution;

        this.iconElement.SetValue(Grid.ColumnProperty, 0);
        this.iconElement.VerticalAlignment = VerticalAlignment.Center;

        this.descripution.SetValue(Grid.ColumnProperty, 1);
        this.descripution.TextWrapping = TextWrapping.WrapWholeWords;
        this.descripution.FontSize = 8;

        ColumnDefinition iconColumn = new ColumnDefinition();
        this.ColumnDefinitions.Add(iconColumn);
        ColumnDefinition descriputionColumn = new ColumnDefinition();
        this.ColumnDefinitions.Add(descriputionColumn);
        this.SetValue(MarginProperty, new Thickness(10, 0, 10, 0));
        this.Width = 64;

        this.Children.Add(icon);
        this.Children.Add(descripution);
    }
}