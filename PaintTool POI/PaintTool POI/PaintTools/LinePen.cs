using PaintTool_POI.DataTypes;
using PaintTool_POI.PoiLayers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace PaintTool_POI.PaintTools
{
    internal class LinePen : IPaintTool
    {
        Point2D lastPosition;
        Color penColor;
        float brushSize = 20;

        public void UpdateColor(Color newColor)
        {
            this.penColor = newColor;
        }
        void IPaintTool.OnPaint(Point2D position, float pressure, IPaintableLayers layers)
        {
            Debug.WriteLine("LinePen Painting...");
            if (lastPosition != null)
            {
                layers.GetActiveLayer().DrawLine(lastPosition, position, penColor, brushSize * pressure);
            }
            lastPosition = position;
        }


        void IPaintTool.OnPenDown(Point2D position, float pressure, IPaintableLayers layers)
        {
            Debug.WriteLine("LinePen Down");

            lastPosition = position;
        }

        void IPaintTool.OnPenUp(Point2D position, float pressure, IPaintableLayers layers)
        {
            Debug.WriteLine("LinePen Up");
            lastPosition = null;
        }

        void IPaintTool.OnSelect()
        {
            Debug.WriteLine("LinePen selected");
        }

        void IPaintTool.OnUnselet()
        {
            Debug.WriteLine("LinePen unselected");
        }
    }
}
