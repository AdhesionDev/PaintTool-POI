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
using Windows.UI.Input;
using Windows.Media;


/// <summary>
/// <see cref="InkCanvas"/>
/// </summary>


namespace PaintTool_POI
{
    /// <summary>
    /// Main page of POI
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Private Field

        private readonly Action<StorageFile> OnFileReadComplete;


        #endregion


        public int canvasRotation = 0;

        Button button;
        InkCanvas newInkCanvas;
        Image canvasImage = new Image();


        WriteableBitmap writeableBitmap;

        public MainPage()
        {
            this.InitializeComponent();

            InitializeCanvas();
            AddToolItems();
            InitiallizeColors();
            //addCanvases();

        }
        /// <summary>
        /// Change the rotation of the canvas
        /// </summary>
        private void UpdateCanvasViewBoxRotation()
        {
            mainCanvasViewBox.RenderTransform = new RotateTransform()
            {
                CenterX = 350,
                CenterY = 350,
                Angle = canvasRotation
            };
        }
        /// <summary>
        /// Initialize Color settings.
        /// </summary>
        public void InitiallizeColors()
        {
            UpdatePenAndBackColors();
        }

        /// <summary>
        /// Update preview image
        /// </summary>
        private async void UpdatePreview()
        {

            CanvasDevice device = CanvasDevice.GetSharedDevice();
            CanvasRenderTarget renderTarget = new CanvasRenderTarget(device, (int)newInkCanvas.ActualWidth, (int)newInkCanvas.ActualHeight, 96);

            using (CanvasDrawingSession drawing = renderTarget.CreateDrawingSession())
            {
                drawing.Clear(Colors.Transparent);
                drawing.DrawInk(newInkCanvas.InkPresenter.StrokeContainer.GetStrokes());
                //drawing.DrawImage();
            }

            InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream();

            await renderTarget.SaveAsync(stream, CanvasBitmapFileFormat.Gif);
            await stream.FlushAsync();

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.DecodePixelWidth = 300; //match the target Image.Width, not shown
            await bitmapImage.SetSourceAsync(stream);
            previewImage.Source = bitmapImage;
            newInkCanvas.Opacity = 0.1f;
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

            newInkCanvas = new InkCanvas();
            newInkCanvas.Height = newCanvas.Height;
            newInkCanvas.Width = newCanvas.Width;
            newInkCanvas.Opacity = 0.2;
            newInkCanvas.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen;
            newInkCanvas.InkPresenter.StrokeInput.StrokeContinued += StrokeInput_StrokeContinuedAsync;
            newCanvas.Children.Add(newInkCanvas);


            writeableBitmap = new WriteableBitmap(300, 300);
            canvasImage.Source = writeableBitmap;
            canvasImage.Stretch = Stretch.None;


            SoftwareBitmap softwareBitmap = new SoftwareBitmap(BitmapPixelFormat.Bgra8, 300, 300, BitmapAlphaMode.Premultiplied);

            //InMemoryRandomAccessStream
            //InMemoryRandomAccessStream



            //print(writeableBitmap.PixelBuffer.ToArray()[0].ToString());

        }
        public void CanvasTapped(object sender, TappedRoutedEventArgs e)
        {
            UpdatePreview();
        }


        private async void AboutDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            MediaElement element = new MediaElement()
            {
                Source = new Uri("ms-appx:///Assets/Output.mp3")
            };
            //var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Asset");
            //var file = await folder.GetFileAsync("MySound.wav");
            //var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            //element.SetSource(stream, "");
            element.Play();
        }

        private void About_Button_Click(object sender, RoutedEventArgs e)
        {
            MainPageSub.ShowAbout();
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
            print("Opening file");
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
                print("Picked photo: " + file.Name);
            }
            else
            {
                print("Operation canceled.");
            }

            OnFileReadComplete?.Invoke(file);
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
                //UpdateInkCanvas(mainInkCanvas);
            }
        }
        private void SwapColorButton_Click(object sender, RoutedEventArgs e)
        {
            Color back = ValueHolder.backColor;
            ValueHolder.backColor = ValueHolder.penColor;
            ValueHolder.penColor = back;
            UpdatePenAndBackColors();
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
        private void DebugButton_Click(object sender, RoutedEventArgs e)
        {
            InkCanvas debugCanvas = new InkCanvas();
            Canvas canvas = (Canvas)mainCanvasGrid.Children[0];
            Canvas.SetZIndex(canvas, -1);
            canvas.Children.Add(debugCanvas);
            InkCanvas inkCanvas = (InkCanvas)canvas.Children[0];

        }
        private async void StrokeInput_StrokeContinuedAsync(InkStrokeInput sender, PointerEventArgs args)
        {
            //print(sender.InkPresenter.UnprocessedInput.ToString());
            //IReadOnlyList<InkStroke> inkStrokes = sender.InkPresenter.StrokeContainer.GetStrokes();
            //print(inks[inks.Count - 1].GetInkPoints()[inks[inks.Count - 1].GetInkPoints().Count - 1].Pressure.ToString());
            //IReadOnlyList<InkPoint> inkPoints = inkStrokes[inkStrokes.Count - 1].GetInkPoints();


            Point point = args.CurrentPoint.Position;
            //sender.InkPresenter.UnprocessedInput.PointerMoved += UnprocessedInput_PointerMoved2;
            //sender.InkPresenter.StrokeContainer.GetStrokes
            float pressure = args.CurrentPoint.Properties.Pressure;

            DataReader dataReader = DataReader.FromBuffer(writeableBitmap.PixelBuffer);
            print("Pen: [" + point.ToString() + "]" + "| Pressure: " + pressure.ToString());


        }

        private void print(string m)
        {
            Debug.WriteLine(m);
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
        private void mainColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
        {
            ValueHolder.penColor = sender.Color;
            UpdatePenAndBackColors();
        }
    }
}
//Get item
//Wrote by Fishball
// DO NOT delete
public delegate T GetItem<T>();

