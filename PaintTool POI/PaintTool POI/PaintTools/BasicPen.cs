using PaintTool_POI.DataTypes;
using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using PaintTool_POI.UIElements;
using Windows.UI;
using PaintTool_POI.PaintTools.Helpers;

namespace PaintTool_POI.PaintTools
{

    internal class BasicPen : IPaintToolWithUI
    {
        int interBound;
        int outerBound;
        Point lastPos;
        float lastPressure;

        public ColorTypes type { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Color backColor { get; set; }
        public Color penColor { get; set; }

        public UIElement GetToolBoxItem()
        {
            return new PaintToolItemGrid("\xEDFB", "Calligraphy Pen");
        }

        public UIElement GetToolSettings()
        {
            throw new NotImplementedException();
        }

        public BasicPen(Color color)
        {
            this.penColor = color;
            interBound = 30;
            outerBound = 10;
        }

        public void OnPaint(Point position, float pressure, PixelImage canvasImage)
        {
            if (lastPos.X == -1 && lastPos.Y == -1)
            {

            }
            else
            {
                Helpers.DrawLine.RoundLine(lastPos, position, interBound, lastPressure, pressure, canvasImage, penColor);
            }
            lastPos = position;
            lastPressure = pressure;
        }

        public void OnPenDown(Point position, float pressure, PixelImage pixelImage)
        {
            //DrawCircle.DrawBasicCircle(position, (double)(pressure * interBound), pixelImage, penColor);
        }



        public void OnPenUp(Point position, float pressure, PixelImage canvasImage)
        {
            lastPos = new Point(-1, -1);
            //DrawCircle.DrawBasicCircle(position, pressure * interBound, canvasImage, penColor);
        }

        public void OnSelect()
        {
            lastPos = new Point(-1, -1);
        }

        public void OnUnselet()
        {
            throw new NotImplementedException();
        }
    }
}
