using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Input.Inking;

namespace PaintTool_POI.UnusedCodes
{
    internal class LegacyCodes
    {
        /// <summary>
        /// Update preview image
        /// </summary>
        private async void UpdatePreview()
        {

            CanvasDevice device = CanvasDevice.GetSharedDevice();
            //CanvasRenderTarget renderTarget = new CanvasRenderTarget(device, (int)newInkCanvas.ActualWidth, (int)newInkCanvas.ActualHeight, 96);

            //using (CanvasDrawingSession drawing = renderTarget.CreateDrawingSession())
            {
                //drawing.Clear(Colors.Transparent);
                //drawing.DrawInk(newInkCanvas.InkPresenter.StrokeContainer.GetStrokes());
                //drawing.DrawImage();
            }

            InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream();

            //await renderTarget.SaveAsync(stream, CanvasBitmapFileFormat.Gif);
            //await stream.FlushAsync();

            //BitmapImage bitmapImage = new BitmapImage();
            //bitmapImage.DecodePixelWidth = 300; //match the target Image.Width, not shown
            //await bitmapImage.SetSourceAsync(stream);
            //previewImage.Source = bitmapImage;

            //newInkCanvas.Opacity = 0.1f;
            stream.Dispose();
        }


        private async void StrokeInput_StrokeContinued(InkStrokeInput sender, PointerEventArgs args)
        {
            //Windows.Foundation.Point point = args.CurrentPoint.Position;
            //float pressure = args.CurrentPoint.Properties.Pressure;

            //print("Pen: [" + point.ToString() + "]" + "| Pressure: " + pressure.ToString());


            //currentTool.OnPaint(point, pressure, canvas.getLayerImage(0));
            //newInkCanvas.InkPresenter.StrokeContainer.Clear();
        }
    }
}
