using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PaintTool_POI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            AddToolItems();
        }

        private void AddToolItems()
        {
            List<ToolItemGrid> items = new List<ToolItemGrid>();

            items.Add(new ToolItemGrid("\xED63", "Pencil"));
            items.Add(new ToolItemGrid("\xE7E6", "High Light"));
            items.Add(new ToolItemGrid("\xEDFB", "Calligraphy Pen"));
            items.Add(new ToolItemGrid("\xE75C", "消しゴム"));
            items.Add(new ToolItemGrid("\xEF3C", "Color Picker"));
            items.Add(new ToolItemGrid("\xE759", "Move"));
            items.Add(new ToolItemGrid("\xF407", "Rectangular Selecting"));
            items.Add(new ToolItemGrid("\xF408", "Free Selecting"));
            items.Add(new ToolItemGrid("\xE710", "Add new Item"));
            toolsGridView.ItemsSource = items;

        }

        /// <summary>
        /// Call this method for some debug behavior.
        /// </summary>
        private void Debug_Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine($"Get singleton var: {SampleClass.Instance.sampleVar}");
        }
        private async void showAbout()
        {

            ContentDialog aboutDialog = new ContentDialog()
            {
                Title = "About",
                Content = "PaintTool POI\nVersion: Test",
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
            Windows.ApplicationModel.Core.CoreApplication.Exit();
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {

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