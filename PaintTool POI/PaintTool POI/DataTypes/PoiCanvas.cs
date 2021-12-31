using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace PaintTool_POI.DataTypes
{
    internal class PoiCanvas : Canvas
    {
        public PoiCanvas(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;

        }

        public void AddLayer(PixelImage pixelImage)
        {
            Rectangle newLayer = new Rectangle()
            {
                Width = pixelImage.Width,
                Height = pixelImage.Height
            };
            newLayer.Fill = new PixelImageBrush(pixelImage);
            this.Children.Add(newLayer);
        }
        public void AddLayer()
        {
            PixelImage pixelImage = new PixelImage((int)Width, (int)Height);
            this.AddLayer(pixelImage);
        }
        public PixelImage GetLayerImage(int index)
        {

            PixelImage image = GetLayerBrush(index).Source;
            return image;
        }
        public PixelImageBrush GetLayerBrush(int index)
        {
            Rectangle layer = (Rectangle)Children[0];
            PixelImageBrush brush = (PixelImageBrush)layer.Fill;
            return brush;
        }

    }
}
