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

        ReadWriteTexture2D<Rgba32, Float4> renderTexture;

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

            renderTexture = GraphicsDevice.Default.AllocateReadWriteTexture2D<Rgba32, Float4>(4000, 4000);

            mainPanel.IsDynamicResolutionEnabled = true;

            mainPanel.ShaderRunner = new RenderRunner()
            {
                texture = renderTexture
            };

            InitializeCanvas(4000, 4000);
            AddToolItems();
            InitiallizeColors();
            InitiallizeTools();
        }

        public sealed class RenderRunner : IShaderRunner
        {
            public ReadWriteTexture2D<Rgba32, Float4>? texture;

            public void Execute(IReadWriteTexture2D<Float4> texture, TimeSpan timespan)
            {
                GraphicsDevice.Default.ForEach(texture, new RenderShader(this.texture));
            }
        }

        [EmbeddedBytecode(8, 8, 1)]
        internal readonly partial struct RenderShader : IPixelShader<float4>
        {
            public readonly IReadWriteTexture2D<float4> texture;

            public RenderShader(IReadWriteTexture2D<float4> texture)
            {
                this.texture = texture;
            }

            public Float4 Execute()
            {
                return texture[(int2)(ThreadIds.Normalized.XY * 4000)];
            }
        }

        [EmbeddedBytecode(8, 8, 1)]
        public readonly partial struct DrawShader : IPixelShader<float4>
        {
            public readonly IReadWriteTexture2D<float4> texture;

            public readonly float2 coord;

            public DrawShader(IReadWriteTexture2D<float4> texture, float2 coord)
            {
                this.texture = texture;
                this.coord = coord;
            }

            public Float4 Execute()
            {
                float4 color = new float4(0, 0, 0, 1);
                if (Hlsl.Distance(ThreadIds.XY, coord) < 200)
                {
                    color = new float4(1, 1, 1, 1);
                }
                return color;
            }
        }

        [EmbeddedBytecode(8, 8, 1)]
        public readonly partial struct DrawLineShader : IComputeShader
        {
            public readonly IReadWriteTexture2D<float4> texture;

            public readonly float2 startPos;

            public readonly float2 endPos;
            public readonly float thikness;

            public DrawLineShader(IReadWriteTexture2D<Float4> texture, Float2 startPos, Float2 endPos, float thikness)
            {
                this.texture = texture;
                this.startPos = startPos;
                this.endPos = endPos;
                this.thikness = thikness;
            }

            /// <summary>
            /// The method to get the inverse of a matrix
            /// </summary>
            /// <param name="matrix">the input matrix</param>
            /// <returns>the inverse of the matrix</returns>

            private float2x2 inverse(float2x2 matrix)
            {
                // Get the determinent
                float det = matrix.M11 * matrix.M22 - matrix.M12 * matrix.M21;

                // Using the 2x2 matrix inverse formula.
                return new float2x2(matrix.M22 * det, -matrix.M12 * det, -matrix.M21 * det, matrix.M11 * det);
            }

            public void Execute()
            {
                // Roatation matrix that rotate 90 counter-clockwise.
                float2x2 rot = new float2x2(0, -1, 1, 0);

                // Getting the orthorgonal basis
                float2 _u1 = endPos - startPos;
                float2 u1 = _u1 / Hlsl.Length(_u1);
                float2 u2 = Hlsl.Mul(rot, u1);

                // Forming a subspace
                float2x2 S_E = new float2x2(u1.X, u2.X, u1.Y, u2.Y);
                float2x2 E_S = inverse(S_E);

                // Transforming current uv coord to S basis.
                float2 v_S = Hlsl.Mul(E_S, ThreadIds.XY);

                // Transforming start and end pos coord to S basis.
                float2 startPos_S = Hlsl.Mul(E_S, startPos);
                float2 endPos_S = Hlsl.Mul(E_S, endPos);


                if (Hlsl.Abs(v_S.Y - startPos_S.Y) < thikness && v_S.X > startPos_S.X && v_S.X < endPos_S.X)
                {
                    texture[ThreadIds.XY] = new float4(1, 0, 0, 1);
                }
                else if (Hlsl.Distance(v_S, startPos_S) < thikness || Hlsl.Distance(v_S, endPos_S) < thikness)
                {
                    texture[ThreadIds.XY] = new float4(1, 0, 0, 1);
                }

                //return color;
            }
        }

        private void DebugButton_Click(object sender, RoutedEventArgs e)
        {
            print("Shader");
            //GraphicsDevice.Default.ForEach(renderTexture, new DrawLineShader(renderTexture, new Float2(500, 500), new Float2(500, 1000), 20f));
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
            this.currentTool = new BasicCurvePen();
            currentTool.penColor = ValueHolder.penColor;
            currentTool.OnSelect();

            // TODO: 移除这个野蛮的方案在未来
            // Temp register draw method

            float step = 10;

            ((BasicCurvePen)currentTool).OnDraw += (refer0, refer1, refer2, currPos, lastPressure, currentPressure) =>
            {
                float distance2 = (float)Math.Sqrt(((refer1.X - refer2.X) * (refer1.X - refer2.X) + (refer1.Y - refer2.Y) * (refer1.Y - refer2.Y)));
                //print("distance2: " + distance2);


                float2 lastFPos = new float2(-1000000, -1);
                for (float f = 0; f < distance2 + step; f += step)
                {
                    //print("F: " + f);
                    float normalF = (f * 100) / (distance2 * 100);

                    float2 currFPos = GetSplinePoint(refer0, refer1, refer2, currPos, normalF);

                    if (normalF >= 1)
                    {
                        GraphicsDevice.Default.For(4000, 4000, new DrawLineShader(renderTexture, lastFPos, refer2, 20f * currentPressure));
                        break;
                    }
                    if (lastFPos.X == -1000000)
                    {
                        //lastFPos = refer1;
                        //GraphicsDevice.Default.For(4000, 4000, new DrawLineShader(renderTexture, refer1, currFPos, 20f * currentPressure));
                    }
                    else
                    {
                        GraphicsDevice.Default.For(4000, 4000, new DrawLineShader(renderTexture, lastFPos, currFPos, 20f * currentPressure));
                    }

                    lastFPos = currFPos;
                }



                //print("Out for");
            };
        }

        public void DrawSplineWithLines()
        {

        }


        public Float2 GetSplinePoint(List<Float2> points, float t)
        {
            int portion0, portion1, portion2, portion3;
            portion1 = (int)t + 1;
            portion2 = portion1 + 1;
            portion3 = portion1 + 2;
            portion0 = portion1 - 1;
            //print(portion0.ToString());
            //print(portion1.ToString());
            //print(portion2.ToString());
            //print(portion3.ToString());

            t = t - (int)t;

            float t2 = t * t;
            float t3 = t * t * t;

            float factor0 = 0.5f * ((-t3) + 2 * t2 - t);
            float factor1 = 0.5f * (3 * t3 - 5 * t2 + 2);
            float factor2 = 0.5f * (-3 * t3 + 4 * t2 + t);
            float factor3 = 0.5f * (t3 - t2);

            float tx =
                points[portion0].X * factor0 +
                points[portion1].X * factor1 +
                points[portion2].X * factor2 +
                points[portion3].X * factor3;
            float ty =
                points[portion0].Y * factor0 +
                points[portion1].Y * factor1 +
                points[portion2].Y * factor2 +
                points[portion3].Y * factor3;
            return new Float2(tx, ty);
        }
        public Float2 GetSplinePoint(Float2 p0, Float2 p1, Float2 p2, Float2 p3, float t)
        {
            int portion0, portion1, portion2, portion3;
            portion1 = (int)t + 1;
            portion2 = portion1 + 1;
            portion3 = portion1 + 2;
            portion0 = portion1 - 1;
            //print(portion0.ToString());
            //print(portion1.ToString());
            //print(portion2.ToString());
            //print(portion3.ToString());

            t = t - (int)t;

            float t2 = t * t;
            float t3 = t * t * t;

            float factor0 = 0.5f * ((-t3) + 2 * t2 - t);
            float factor1 = 0.5f * (3 * t3 - 5 * t2 + 2);
            float factor2 = 0.5f * (-3 * t3 + 4 * t2 + t);
            float factor3 = 0.5f * (t3 - t2);

            float tx =
                p0.X * factor0 +
                p1.X * factor1 +
                p2.X * factor2 +
                p3.X * factor3;
            float ty =
                p0.Y * factor0 +
                p1.Y * factor1 +
                p2.Y * factor2 +
                p3.Y * factor3;
            return new Float2(tx, ty);
        }
        public Float2 GetSplinePoint2(Float2 p0, Float2 p1, Float2 p2, Float2 p3, float t)
        {
            //print(p0.ToString());
            //print(p1.ToString());
            //print(p2.ToString());
            //print(p3.ToString());

            float x = 0.5f * (((t * t * t) * (-p0.X + 3 * p1.X - 3 * p2.X + p3.X)) + ((t * t) * (2 * p0.X - 5 * p1.X + 4 * p2.X - p3.X)) + (t * (-p0.X + p2.X)) + (2 * p1.X));
            float y = 0.5f * (((t * t * t) * (-p0.Y + 3 * p1.Y - 3 * p2.Y + p3.Y)) + ((t * t) * (2 * p0.Y - 5 * p1.Y + 4 * p2.Y - p3.Y)) + (t * (-p0.Y + p2.Y)) + (2 * p1.Y));
            //print(x + ", " + y);
            //print(result.ToString());
            return new Float2(x, y);
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

// Get item
// Wrote by Fish ball
// DO NOT delete
