using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Composition;
using System;
using Windows.Graphics.DirectX;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace PixelImage
{
    /// <summary>
    /// A BGRA pixel.
    /// </summary>
    public struct Pixel
    {
        public byte B;
        public byte G;
        public byte R;
        public byte A;
    }

    /// <summary>
    /// A BGRA pixel image.
    /// </summary>
    public class PixelImage
    {
        private const int bytesPerPixel = 4;

        /// <summary>
        /// The image bytes.
        /// </summary>
        public byte[] Pixels
        {
            private set;
            get;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PixelImage(int width, int height)
        {
            SetSize(width, height);
        }

        /// <summary>
        /// The image width.
        /// </summary>
        public int Width { private set; get; }

        /// <summary>
        /// The image height.
        /// </summary>
        public int Height { private set; get; }

        /// <summary>
        /// Resize the image.
        /// </summary>
        public void SetSize(int width, int height)
        {
            Width = width;
            Height = height;
            Pixels = new byte[width * height * bytesPerPixel];
        }

        /// <summary>
        /// Set an image pixel.
        /// </summary>
        public void SetPixel(int x, int y, Pixel pixel)
        {
            var offset = (x + y * Width) * bytesPerPixel;

            Pixels[offset + 0] = pixel.B;
            Pixels[offset + 1] = pixel.G;
            Pixels[offset + 2] = pixel.R;
            Pixels[offset + 3] = pixel.A;

            ImageModified?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Get an image pixel.
        /// </summary>
        public Pixel GetPixel(int x, int y)
        {
            var offset = (x + y * Width) * bytesPerPixel;

            return new Pixel()
            {
                B = Pixels[offset + 0],
                G = Pixels[offset + 1],
                R = Pixels[offset + 2],
                A = Pixels[offset + 3],
            };
        }

        /// <summary>
        /// Set a block of image pixels from a source byte array.
        /// </summary>
        public void SetPixels(int dx, int dy, byte[] source, int sw, int sh)
        {
            int srcOffset = 0;
            int srcBytesPerRow = sw * bytesPerPixel;
            int dstOffset = (dx + dy * Width) * bytesPerPixel;
            int dstBytesPerRow = Width * bytesPerPixel;

            while (sh > 0)
            {
                Buffer.BlockCopy(source, srcOffset, Pixels, dstOffset, srcBytesPerRow);
                srcOffset += srcBytesPerRow;
                dstOffset += dstBytesPerRow;
                --sh;
            }

            ImageModified?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires after the image has been modified.
        /// </summary>
        public event EventHandler ImageModified;
    }

    /// <summary>
    /// A XAML Brush that uses a modifiable PixelImage as its source.
    /// </summary>
    public class PixelImageBrush : XamlCompositionBrushBase
    {
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
            UpdateBrush();
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
        private void UpdateBrush(bool create = false)
        {
            if (source == null)
            {
                // No source Image, so do nothing.
                return;
            }

            if (create)
            {
                // Create a new CompositionSurfaceBrush.
                var device = CanvasDevice.GetSharedDevice();
                var compositor = Window.Current.Compositor;
                var graphicsDevice = CanvasComposition.CreateCompositionGraphicsDevice(compositor, device);
                var surfaceSize = new Windows.Foundation.Size(source.Width, source.Height);
                var surface = graphicsDevice.CreateDrawingSurface(surfaceSize, DirectXPixelFormat.B8G8R8A8UIntNormalized, DirectXAlphaMode.Premultiplied);
                var brush = compositor.CreateSurfaceBrush(surface);
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
            var compositionSurfaceBrush = (CompositionSurfaceBrush)CompositionBrush;
            var compositionDrawingSurface = (CompositionDrawingSurface)(compositionSurfaceBrush.Surface);
            var canvasDrawingSession = CanvasComposition.CreateDrawingSession(compositionDrawingSurface);
            var canvasBitmap = CanvasBitmap.CreateFromBytes(canvasDrawingSession.Device, source.Pixels, source.Width, source.Height, DirectXPixelFormat.B8G8R8A8UIntNormalized);
            try
            {
                canvasDrawingSession.DrawImage(canvasBitmap);
            }
            catch (Exception ex)
            {
                // If it's because the device was lost recreate the brush, otherwise rethrow.
                if (!canvasDrawingSession.Device.IsDeviceLost(ex.HResult)) throw;
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
