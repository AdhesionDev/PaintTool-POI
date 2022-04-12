using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Input.Inking;
using Windows.UI.Core;
using PaintTool_POI.DataTypes;
using PaintTool_POI.PaintTools;
using Microsoft.Graphics.Canvas;
using Windows.UI.Input;
using Windows.UI.Xaml.Input;
using ComputeSharp.Uwp;
using ComputeSharp;
using ComputeSharp.__Internals;
using System.IO;
using Windows.ApplicationModel;
using PaintTool_POI.Canvas;

namespace PaintTool_POI
{
    /// <summary>
    /// Main page of POI
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Private Field

        IPaintTool currentTool;
        IPaintableLayers layers;

        #endregion

        public int canvasRotation = 0;

        public MainPage()
        {
            this.InitializeComponent();



            InitializeCanvas(4000, 4000);
            AddToolItems();
            InitiallizeColors();
            InitiallizeTools();
        }




        private void DebugButton_Click(object sender, RoutedEventArgs e)
        {
            print("Debug");
        }




        private void InitiallizeTools()
        {
            currentTool.OnSelect();
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
        /// Add tool buttons into the tool box
        /// </summary>
        private void AddToolItems()
        {
            List<UIElement> items = new List<UIElement>();


            toolsGridView.ItemsSource = items;
        }
        private void InitializeCanvas(int x, int y)
        {

        }

        private void About_Button_Click(object sender, RoutedEventArgs e)
        {
            UIElements.AboutPopup.ShowAbout();
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
            //OnFileReadComplete?.Invoke(file);
        }
        private void UpdatePenAndBackColors()
        {
            backColorRectangle.Fill = new SolidColorBrush(ValueHolder.backColor);
            frontColorRectangle.Fill = new SolidColorBrush(ValueHolder.penColor);
            if (currentTool != null)
            {
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

        private void print(string m)
        {
            Debug.WriteLine(m);
        }

        private void mainCanvasScrollViewer_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Pen)
            {
            }
        }

        private void mainCanvasScrollViewer_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Pen)
            {
            }
        }

        private void mainCanvasScrollViewer_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Pen)
            {
                (mainCanvasScrollViewer.Content as UIElement).ManipulationMode = ManipulationModes.None;
            }
        }

        private void mainCanvasScrollViewer_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
        }

        private void mainCanvasScrollViewer_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
        }
    }
}
