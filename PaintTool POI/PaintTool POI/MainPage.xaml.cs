using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PaintTool_POI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Private Field

        private Action<StorageFile> onFileReadComplete;

        // private Stack<StrokeCollection> undo;

        #endregion

        public MainPage()
        {
            this.InitializeComponent();
            AddToolItems();
            InitiallizeColors();
        }

        public void InitiallizeColors()
        {
            mainColorPicker.Color = ValueHolder.penColor;
            UpdatePenAndBackColors();
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

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {

        }
        private async void OpenFile_ButtonClick(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Opening file");
            //StorageFile file;
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Clear();
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".bmp");

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file
                Debug.WriteLine("Picked photo: " + file.Name);
            }
            else
            {
                Debug.WriteLine("Operation cancelled.");
            }

            onFileReadComplete?.Invoke(file);
        }
        private void UpdatePenAndBackColors()
        {
            backColorRectangle.Fill = new SolidColorBrush(ValueHolder.backColor);
            frontColorRectangle.Fill = new SolidColorBrush(ValueHolder.penColor);
        }
        private void SwapColorButton_Click(object sender, RoutedEventArgs e)
        {
            Color back = ValueHolder.backColor;
            ValueHolder.backColor = ValueHolder.penColor;
            ValueHolder.penColor = back;
            UpdatePenAndBackColors();
        }

        private void ColorPicker_ColorChanged(Microsoft.UI.Xaml.Controls.ColorPicker sender, Microsoft.UI.Xaml.Controls.ColorChangedEventArgs args)
        {
            ValueHolder.penColor = sender.Color;
            UpdatePenAndBackColors();
        }

        private void MenuFlyoutItem_Click_1(object sender, RoutedEventArgs e)
        {

        }


        private void ZoomCanvas(float factor)
        {
            mainCanvasScrollViewer.ChangeView(0, 0, mainCanvasScrollViewer.ZoomFactor + factor);
        }

        private void ZoomInButton_Click(object sender, RoutedEventArgs e)
        {
            ZoomCanvas(0.4f);
        }
        private void ZoomOutButton_Click(object sender, RoutedEventArgs e)
        {
            ZoomCanvas(-0.4f);
        }
    }
}
public delegate T GetItem<T>();

