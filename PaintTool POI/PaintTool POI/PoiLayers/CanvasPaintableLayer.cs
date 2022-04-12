using PaintTool_POI.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace PaintTool_POI.PoiLayers
{
    internal class CanvasPaintableLayer : IPaintableLayer
    {
        public Canvas canvas;

        CanvasPaintableLayer()
        {
            this.canvas = new Canvas();

        }

        public void DrawLine(Point2D from, Point2D to, Color fillColor, float strokWidth)
        {
            var line = new Line()
            {
                X1 = from.x,
                Y1 = from.y,
                X2 = to.x,
                Y2 = to.y,
            };
            line.Stroke = new SolidColorBrush(fillColor);
            line.StrokeThickness = strokWidth;

            canvas.Children.Add(line);
        }

        public UIElement GetUIElement()
        {
            return canvas;
        }

        public IPaintableLayer SetSize(int width, int height)
        {
            canvas.Width = width;
            canvas.Height = height;
            return this;
        }
    }
}
