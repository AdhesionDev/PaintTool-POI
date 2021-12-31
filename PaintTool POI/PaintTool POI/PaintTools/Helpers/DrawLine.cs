using PaintTool_POI.DataTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace PaintTool_POI.PaintTools.Helpers
{
    class DrawLine
    {
        public static void BaiscLine(Point from, Point to, int thickness, PixelImage pixelImage, Color color)
        {

            //Debug.WriteLine("LastPos: " + from.ToString());

            Vector2 vector = new Vector2((float)(to.X - from.X), (float)(to.Y - from.Y));
            float maxLength = Math.Max(Math.Abs(vector.X), Math.Abs(vector.Y));

            for (int times = 0; times <= maxLength; times++)
            {
                double protion = times / maxLength;
                Vector2 currentPoint = vector * (float)protion;

                for (int x = -thickness; x < thickness; x++)
                {
                    for (int y = -thickness; y <= thickness; y++)
                    {
                        pixelImage.SetPixel(x + (int)from.X + (int)currentPoint.X,
                            y + (int)from.Y + (int)currentPoint.Y, color);
                    }
                }
            }
        }
        public static void BaiscLineWithPressureChange(Point from, Point to, int thickness, float fromPressure, float toPressure, PixelImage pixelImage, Color color)
        {

            //Debug.WriteLine("LastPos: " + from.ToString());

            Vector2 vector = new Vector2((float)(to.X - from.X), (float)(to.Y - from.Y));
            float maxLength = Math.Max(Math.Abs(vector.X), Math.Abs(vector.Y));

            float pressureDiff = toPressure - fromPressure;

            for (int times = 0; times <= maxLength; times++)
            {
                double protion = times / maxLength;
                Vector2 currentPoint = vector * (float)protion;

                float currentThickness = ((float)protion * pressureDiff + fromPressure) * thickness;

                for (float x = -currentThickness; x < currentThickness; x++)
                {
                    for (float y = -currentThickness; y <= currentThickness; y++)
                    {
                        pixelImage.SetPixel(
                            (int)(x + from.X + currentPoint.X),
                            (int)(y + from.Y + currentPoint.Y),
                            color
                            );
                    }
                }
            }
        }

        public static void RoundLine(Point from, Point to, int thickness, float fromPressure, float toPressure, PixelImage pixelImage, Color color)
        {

            //Debug.WriteLine("LastPos: " + from.ToString());

            Vector2 vector = new Vector2((float)(to.X - from.X), (float)(to.Y - from.Y));
            float maxLength = Math.Max(Math.Abs(vector.X), Math.Abs(vector.Y));

            float pressureDiff = toPressure - fromPressure;

            for (int times = 0; times <= maxLength; times++)
            {
                double protion = times / maxLength;
                Vector2 currentPoint = vector * (float)protion;

                float currentThickness = ((float)protion * pressureDiff + fromPressure) * thickness;

                for (float x = -currentThickness; x < currentThickness; x++)
                {
                    for (float y = -currentThickness; y <= currentThickness; y++)
                    {
                        if (currentThickness * currentThickness > (x * x + y * y))
                        {
                            pixelImage.SetPixel(
                                (int)(x + from.X + currentPoint.X),
                                (int)(y + from.Y + currentPoint.Y),
                                color
                                );
                        }

                    }
                }
            }
        }

    }
}
