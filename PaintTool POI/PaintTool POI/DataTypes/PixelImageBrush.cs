using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Composition;
using System;
using Windows.Foundation;
using Windows.Graphics.DirectX;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace PaintTool_POI.DataTypes
{
    public class PixelImageBrush : XamlCompositionBrushBase
    {
        public PixelImageBrush(PixelImage pixel)
        {
            this.Source = pixel;
        }


        /// <summary>
        /// Set the source image.
        /// </summary>
        public PixelImage Source
        {
            set
            {
                if (source != value)
                {
                    if (source != null) source.ImageModified -= Source_ImageModified;
                    source = value;
                    if (source != null) source.ImageModified += Source_ImageModified;
                    UpdateBrush();
                }
            }
            get
            {
                return source;
            }
        }
        private PixelImage source;

        /// <summary>
        /// Update the composition brush surface when the image is modified.
        /// </summary>
        private void Source_ImageModified(object sender, EventArgs e)
        {
            //UpdateBrushAsync();
        }

        /// <summary>
        /// Set the brush stretch mode.
        /// </summary>
        public CompositionStretch StretchMode
        {
            set
            {
                if (stretchMode != value)
                {
                    stretchMode = value;
                    var brush = (CompositionSurfaceBrush)CompositionBrush;
                    if (brush != null) brush.Stretch = value;
                }
            }
            get
            {
                return stretchMode;
            }
        }
        private CompositionStretch stretchMode = CompositionStretch.None;

        /// <summary>
        /// Set the brush interpolation mode.
        /// </summary>
        public CompositionBitmapInterpolationMode InterpolationMode
        {
            set
            {
                if (interpolationMode != value)
                {
                    interpolationMode = value;
                    var brush = (CompositionSurfaceBrush)CompositionBrush;
                    if (brush != null) brush.BitmapInterpolationMode = interpolationMode;
                }
            }
            get
            {
                return interpolationMode;
            }
        }
        private CompositionBitmapInterpolationMode interpolationMode = CompositionBitmapInterpolationMode.NearestNeighbor;

        /// <summary>
        /// Create or update the composition brush with the current image contents.
        /// </summary>
        public void UpdateBrush(bool create = false)
        {
            if (source == null)
            {
                // No source Image, so do nothing.
                return;
            }

            else if (create)
            {
                // Create a new CompositionSurfaceBrush.
                CanvasDevice device = CanvasDevice.GetSharedDevice();
                Compositor compositor = Window.Current.Compositor;
                CompositionGraphicsDevice graphicsDevice = CanvasComposition.CreateCompositionGraphicsDevice(compositor, device);
                Size surfaceSize = new Size(source.Width, source.Height);
                CompositionDrawingSurface surface = graphicsDevice.CreateDrawingSurface(surfaceSize, DirectXPixelFormat.B8G8R8A8UIntNormalized, DirectXAlphaMode.Premultiplied);
                CompositionSurfaceBrush brush = compositor.CreateSurfaceBrush(surface);
                brush.BitmapInterpolationMode = interpolationMode;
                brush.Stretch = stretchMode;
                CompositionBrush = brush;
            }
            else if (CompositionBrush == null)
            {
                // If we have no CompositionSurfaceBrush to update, do nothing.
                return;
            }

            // Note that the CanvasDevice from the CanvasDrawingDession is used when creating the
            // CanvasBitmap, rather than the device returned by CanvasDevice.GetSharedDevice() in
            // the code below. This is because the shared device occasionally changes (and for no
            // obvious reason, i.e. not device loss or other clear event). This causes a "Objects
            // used together must be created from the same factory instance" exception during XAML
            // tree rendering.

            // Update the CompositionSurfaceBrush.
            CompositionSurfaceBrush compositionSurfaceBrush = (CompositionSurfaceBrush)CompositionBrush;
            CompositionDrawingSurface compositionDrawingSurface = (CompositionDrawingSurface)(compositionSurfaceBrush.Surface);
            CanvasDrawingSession canvasDrawingSession = CanvasComposition.CreateDrawingSession(compositionDrawingSurface);
            CanvasBitmap canvasBitmap;
            canvasBitmap = CanvasBitmap.CreateFromBytes(canvasDrawingSession.Device, source.Pixels, source.Width, source.Height, DirectXPixelFormat.B8G8R8A8UIntNormalized);

            try
            {

                canvasDrawingSession.DrawImage(canvasBitmap);
            }
            catch (Exception e)
            {
                // If it's because the device was lost recreate the brush, otherwise rethrow.
                //if (!canvasDrawingSession.Device.IsDeviceLost(ex.HResult)) throw;
                UpdateBrush(true);
            }
            finally
            {
                if (canvasBitmap != null) canvasBitmap.Dispose();
                if (canvasDrawingSession != null) canvasDrawingSession.Dispose();
            }
        }

        /// <summary>
        /// Brush connected to visual tree, create the CompositionBrush.
        /// </summary>
        protected override void OnConnected()
        {
            UpdateBrush(true);
        }

        /// <summary>
        /// Brush disconnected from visual tree, destroy the CompositionBrush.
        /// </summary>
        protected override void OnDisconnected()
        {
            if (CompositionBrush != null)
            {
                CompositionBrush.Dispose();
                CompositionBrush = null;
            }
        }
    }
}
