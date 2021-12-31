﻿using System;
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

namespace PaintTool_POI
{
    /// <summary>
    /// Main page of POI
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Private Field

        PaintTools.IPaintTool currentTool;
        PoiCanvas canvas;


        #endregion

        public int canvasRotation = 0;

        //InkCanvas newInkCanvas;
        PixelImage pixelImage;
        public MainPage()
        {
            this.InitializeComponent();


            //this.inkSync = this.mainInkCanvas.InkPresenter.ActivateCustomDrying();
            //this.mainInkCanvas.InkPresenter.StrokesCollected += InkPresenter_StrokesCollected1; ;
            //this.canvasControl.SizeChanged += OnCanvasControlSizeChanged;
            mainCanvasGrid.PointerEntered += MainCanvasGrid_PointerEntered;
            mainCanvasGrid.PointerReleased += MainCanvasGrid_PointerReleased;
            mainCanvasGrid.PointerPressed += MainCanvasGrid_PointerPressed;


            InitializeCanvas(4000, 4000);
            AddToolItems();
            InitiallizeColors();
            InitiallizeTools();
        }

        private void MainCanvasGrid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            print("Pressed");
            mainCanvasGrid.PointerMoved += MainCanvasGrid_PointerMoved;
            PointerPoint pointerPoint = e.GetCurrentPoint(mainCanvasGrid);
            Windows.Foundation.Point point = pointerPoint.Position;
            float pressure = pointerPoint.Properties.Pressure;
            currentTool.OnPenDown(point, pressure, canvas.GetLayerImage(0));
            canvas.GetLayerBrush(0).UpdateBrush();
            //e.Handled = true;
        }

        private void MainCanvasGrid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            //mainCanvasGrid.PointerMoved += MainCanvasGrid_PointerMoved;
            //e.Handled = true;
        }

        private void MainCanvasGrid_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            mainCanvasGrid.PointerMoved -= MainCanvasGrid_PointerMoved;
            UpdatePreview();
            PointerPoint pointerPoint = e.GetCurrentPoint(mainCanvasGrid);
            Windows.Foundation.Point point = pointerPoint.Position;
            float pressure = pointerPoint.Properties.Pressure;
            currentTool.OnPenUp(point, pressure, canvas.GetLayerImage(0));
            canvas.GetLayerBrush(0).UpdateBrush();
            //e.Handled = true;
        }


        private void UpdatePreview()
        {
            PixelImage pixel = new PixelImage((int)canvas.Height, (int)canvas.Width);
            pixel.Pixels = canvas.GetLayerImage(0).Pixels;
            previewRectangle.Fill = new PixelImageBrush(pixel);
        }

        private void MainCanvasGrid_PointerMoved(object sender, PointerRoutedEventArgs e)
        {

            //print("[ " + intI + " ]");
            //intI++;

            PointerPoint pointerPoint = e.GetCurrentPoint(mainCanvasGrid);
            Windows.Foundation.Point point = pointerPoint.Position;
            float pressure = pointerPoint.Properties.Pressure;
            if (pressure == 0 || (pointerPoint.PointerDevice.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse && !pointerPoint.Properties.IsLeftButtonPressed))
            {
                //print("No pressure!");
                return;
            }

            //print(pointerPoint.PointerDevice.PointerDeviceType.ToString() + ": [" + point.ToString() + "]" + "| Pressure: " + pressure.ToString());

            //print("Point: " + point.ToString());
            currentTool.OnPaint(point, pressure, canvas.GetLayerImage(0));
            canvas.GetLayerBrush(0).UpdateBrush();
            //print("Paint");
            //newInkCanvas.InkPresenter.StrokeContainer.Clear();
            //e.Handled = true;
        }

        private void InitiallizeTools()
        {
            this.currentTool = new BasicPen(ValueHolder.penColor);
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
            pixelImage = new PixelImage(x, y);
            mainCanvasGrid.Width = pixelImage.Width;
            mainCanvasGrid.Height = pixelImage.Height;
            //mainCanvasGrid.CanDrag = true;
            canvas = new PoiCanvas(x, y);


            canvas.AddLayer(pixelImage);
            mainCanvasGrid.Children.Add(canvas);
            previewRectangle.Fill = canvas.GetLayerBrush(0);
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
                currentTool.penColor = ValueHolder.penColor;
                currentTool.backColor = ValueHolder.backColor;
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
                //print("Disabled");
                //(mainCanvasScrollViewer.Content as UIElement).ManipulationMode = ManipulationModes.None;
                //e.Handled = false;
            }
        }

        private void mainCanvasScrollViewer_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Pen)
            {
                //print("Move");
                //(mainCanvasScrollViewer.Content as UIElement).ManipulationMode = ManipulationModes.None;
                //e.Handled = false;
            }
        }

        private void mainCanvasScrollViewer_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Pen)
            {
                //print("Pressed");
                (mainCanvasScrollViewer.Content as UIElement).ManipulationMode = ManipulationModes.None;
                //e.Handled = false;
            }
        }

        private void mainCanvasScrollViewer_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            //(mainCanvasScrollViewer.Content as UIElement).ManipulationMode = ManipulationModes.None;
            //print("Lost");
        }

        private void mainCanvasScrollViewer_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            //print("Canceled");
        }
    }
}

//Get item
//Wrote by Fishball
// DO NOT delete
