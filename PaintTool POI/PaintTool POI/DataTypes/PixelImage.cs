using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace PaintTool_POI.DataTypes
{
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
            set;
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
        public void SetPixel(int x, int y, Color pixel)
        {
            int offset = (x + y * Width) * bytesPerPixel;
            if (offset < 0 || offset + 4 > Pixels.Length)
            {
                return;
            }
            Pixels[offset + 0] = pixel.B;
            Pixels[offset + 1] = pixel.G;
            Pixels[offset + 2] = pixel.R;
            Pixels[offset + 3] = pixel.A;

            ImageModified?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Get an image pixel.
        /// </summary>
        public Color GetPixelColor(int x, int y)
        {
            int offset = (x + y * Width) * bytesPerPixel;

            return new Color()
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
        public void SetImage(int dx, int dy, byte[] source, int sourseWhith, int sourceHeigh)
        {
            int srcOffset = 0;
            int srcBytesPerRow = sourseWhith * bytesPerPixel;
            int dstOffset = (dx + dy * Width) * bytesPerPixel;
            int dstBytesPerRow = Width * bytesPerPixel;

            while (sourceHeigh > 0)
            {
                System.Buffer.BlockCopy(source, srcOffset, Pixels, dstOffset, srcBytesPerRow);
                srcOffset += srcBytesPerRow;
                dstOffset += dstBytesPerRow;
                --sourceHeigh;
            }
            ImageModified?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires after the image has been modified.
        /// </summary>
        public event EventHandler ImageModified;
    }
}
