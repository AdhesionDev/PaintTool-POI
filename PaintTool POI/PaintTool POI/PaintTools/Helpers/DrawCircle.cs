using PaintTool_POI.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace PaintTool_POI.PaintTools.Helpers
{
    internal class DrawCircle
    {
        public static void DrawBasicCircle(Point center, float radius, PixelImage pixelImage, Color color)
        {
            //DrawBasicCircle(center, radius, pixelImage, color);
        }
        public static void DrawBasicCircle(Point center, double radius, PixelImage pixelImage, Color color)
        {
            for (double x = -radius; x <= radius; x++)
            {
                for (double y = -radius; y < radius; y++)
                {
                    if (x * y <= radius * radius)
                    {
                        pixelImage.SetPixel((int)x, (int)y, color);
                    }
                }
            }
        }
    }
}
