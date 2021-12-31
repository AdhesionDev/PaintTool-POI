using PaintTool_POI.DataTypes;
using PaintTool_POI.UIElements;
using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;

namespace PaintTool_POI.PaintTools
{
    internal class BasicPencil : IPaintToolWithUI
    {
        int thickness = 100;

        public ColorTypes type { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Color backColor { get; set; }
        public Color penColor { get; set; }

        public BasicPencil(Color penColor)
        {
            this.penColor = penColor;
        }

        public void OnPaint(Point position, float pressure, PixelImage pixelImage)
        {
            int intPressure = (int)(pressure * thickness);
            for (int i = -intPressure; i < intPressure + 1; i++)
            {
                for (int j = -intPressure; j < intPressure + 1; j++)
                {
                    pixelImage.SetPixel((int)position.X + i, (int)position.Y + j, penColor);
                }
            }
        }

        public void OnPenDown(Point position, float pressure, PixelImage canvasImage)
        {

        }

        public void OnPenUp(Point position, float pressure, PixelImage canvasImage)
        {

        }

        public void OnSelect()
        {

        }

        public void OnUnselet()
        {

        }

        public UIElement GetToolBoxItem()
        {
            return new PaintToolItemGrid("\xE7E6", "High Light");
        }

        public UIElement GetToolSettings()
        {
            throw new NotImplementedException();
        }

        public void SetColor(Color color)
        {
            this.penColor = color;
        }
    }
}
