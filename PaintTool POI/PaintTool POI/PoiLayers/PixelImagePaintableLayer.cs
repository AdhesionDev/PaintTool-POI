using PaintTool_POI.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace PaintTool_POI.Canvas
{
    internal class PixelImagePaintableLayer : IPaintableLayer
    {
        PixelImage currentLayer;


        public void DrawLine(Vector2 from, Vector2 to, Color fillColor, float strokWidth)
        {
            //int intPressure = (int)(pressure * thickness);
            for (float i = -strokWidth; i < strokWidth + 1; i++)
            {
                for (float j = -strokWidth; j < strokWidth + 1; j++)
                {
                    currentLayer.SetPixel((int)(from.X + i), (int)(from.Y + j), fillColor);
                }
            }
        }

        public IPaintableLayer SetSize(int width, int height)
        {
            currentLayer = new PixelImage(width, height);
            return this;
        }
    }
}
