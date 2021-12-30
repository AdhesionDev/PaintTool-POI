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
using Microsoft.Graphics.Canvas;
using Windows.Graphics.Imaging;
using Windows.UI.Input.Inking;
using Windows.Storage.Streams;
using Windows.UI.Core;

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

        public int canvasRotation = 0;

        // private Stack<StrokeCollection> undo;

        #endregion

        public MainPage()
        {
            this.InitializeComponent();
            AddToolItems();
            InitiallizeColors();
            //addCanvases();

            InitializeCanvas();
        }

        private void UpdateCanvasViewBoxRotation()
        {
            //UIElement container = VisualTreeHelper.GetParent(mainCanvasViewBox) as UIElement;
            //Point relativeLocation = mainCanvasViewBox.TranslatePoint(new Point(0, 0), container);

            //Rotation oldRotation = mainCanvasViewBox.RotationTransition;
            mainCanvasViewBox.RenderTransform = new RotateTransform()
            {
                CenterX = 350,
                CenterY = 350,
                Angle = canvasRotation
            };
        }

        private void addCanvases()
        {
        }



        public void InitiallizeColors()
        {
            mainColorPicker.Color = ValueHolder.penColor;
            UpdatePenAndBackColors();
        }

        private async void UpdatePreview()
        {
            Canvas mainCanvas = (Canvas)mainCanvasGrid.Children[0];
            InkCanvas frist = (InkCanvas)mainCanvas.Children[0];

            //await ApplicationData.Current.TemporaryFolder.CreateFileAsync("test.png", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            // StorageFile file = await StorageFile.GetFileFromPathAsync(ApplicationData.Current.TemporaryFolder.Path + "\\test.png");


            //Debug.WriteLine(file.Path + file.Name + file.Properties);
            // When chosen, picker returns a reference to the selected file.

            // Prevent updates to the file until updates are 
            // finalized with call to CompleteUpdatesAsync.
            // Windows.Storage.CachedFileManager.DeferUpdates(file);
            // Open a file stream for writing.


            CanvasDevice device = CanvasDevice.GetSharedDevice();
            CanvasRenderTarget renderTarget = new CanvasRenderTarget(device, (int)frist.ActualWidth, (int)frist.ActualHeight, 96);

            using (CanvasDrawingSession drawing = renderTarget.CreateDrawingSession())
            {
                drawing.Clear(Colors.Transparent);
                drawing.DrawInk(frist.InkPresenter.StrokeContainer.GetStrokes());
                //drawing.DrawImage();
            }

            InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream();


            //IRandomAccessStream stream = await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);

            await renderTarget.SaveAsync(stream, CanvasBitmapFileFormat.Png);
            await stream.FlushAsync();

            //CanvasBitmapFileFormat.Jpeg, 1f

            // Write the ink strokes to the output stream.
            //using (IOutputStream outputStream = stream.GetOutputStreamAt(0))

            //  await frist.InkPresenter.StrokeContainer.SaveAsync(outputStream);
            //   await outputStream.FlushAsync();


            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.DecodePixelWidth = 600; //match the target Image.Width, not shown
            await bitmapImage.SetSourceAsync(stream);
            previewImage.Source = bitmapImage;
            stream.Dispose();
        }
        /// <summary>
        /// Add tool buttons into the tool box
        /// </summary>
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
        private void InitializeCanvas()
        {
            mainCanvasGrid.Children.Clear();

            Canvas newCanvas = new Canvas();
            mainCanvasGrid.Children.Add(newCanvas);
            newCanvas.Width = 300;
            newCanvas.Height = 300;
            newCanvas.Background = new SolidColorBrush(Colors.White);


            InkCanvas newInkCanvas = new InkCanvas();
            newInkCanvas.Height = newCanvas.Height;
            newInkCanvas.Width = newCanvas.Width;
            newInkCanvas.Tapped += CanvasTapped;
            newInkCanvas.PointerPressed += InkCanvas_PointerPressed;
            newInkCanvas.InkPresenter.UnprocessedInput.PointerPressed += InkCanvas_RawPointerPressed;
            newCanvas.PointerPressed += InkCanvas_PointerPressed;
            newInkCanvas.InkPresenter.StrokesCollected += InkPresenter_StrokesCollected;

            //newInkCanvas.RegisterPropertyChangedCallback+= 
            UpdateInkCanvas(newInkCanvas);
            newCanvas.Children.Add(newInkCanvas);
        }

        private void InkPresenter_StrokesCollected(InkPresenter sender, InkStrokesCollectedEventArgs args)
        {
            UpdatePreview();
        }



        public void CanvasTapped(object sender, TappedRoutedEventArgs e)
        {
            UpdatePreview();
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
        /// <summary>
        /// Exit the program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Windows.ApplicationModel.Core.CoreApplication.Exit();
        }

        /// <summary>
        /// Open a file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                Debug.WriteLine("Operation canceled.");
            }

            onFileReadComplete?.Invoke(file);
        }
        private void UpdatePenAndBackColors()
        {
            backColorRectangle.Fill = new SolidColorBrush(ValueHolder.backColor);
            frontColorRectangle.Fill = new SolidColorBrush(ValueHolder.penColor);

            // Set supported inking device types.
            Canvas mainCanvas = (Canvas)mainCanvasGrid.Children[0];
            InkCanvas mainInkCanvas = (InkCanvas)mainCanvas.Children[0];
            if (mainInkCanvas != null)
            {
                // Set initial ink stroke attributes.
                InkDrawingAttributes drawingAttributes = new InkDrawingAttributes();
                drawingAttributes.Color = ValueHolder.penColor;
                drawingAttributes.IgnorePressure = false;
                drawingAttributes.FitToCurve = true;
                mainInkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
                UpdateInkCanvas(mainInkCanvas);
            }
        }

        private void UpdateInkCanvas(InkCanvas inkCanvas)
        {
            inkCanvas.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Mouse | Windows.UI.Core.CoreInputDeviceTypes.Pen;
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
            mainCanvasScrollViewer.ChangeView(0.5, 0.5, mainCanvasScrollViewer.ZoomFactor + factor);
        }

        private void ZoomInButton_Click(object sender, RoutedEventArgs e)
        {
            ZoomCanvas(0.4f);
        }
        private void ZoomOutButton_Click(object sender, RoutedEventArgs e)
        {
            ZoomCanvas(-0.4f);
        }
        private async void DebugButton_Click(object sender, RoutedEventArgs e)
        {
            UpdatePreview();


        }
        private void RotateCWButton_Click(object sender, RoutedEventArgs e)
        {
            canvasRotation += 10;
            UpdateCanvasViewBoxRotation();
        }
        private void RotateCCWButton_Click(object sender, RoutedEventArgs e)
        {
            canvasRotation -= 10;
            UpdateCanvasViewBoxRotation();
        }

        private void InkCanvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            UpdatePreview();
        }

        private void InkCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            UpdatePreview();
        }
        private void InkCanvas_RawPointerPressed(InkUnprocessedInput e, PointerEventArgs a)
        {
            UpdatePreview();
        }

    }
}
public delegate T GetItem<T>();

