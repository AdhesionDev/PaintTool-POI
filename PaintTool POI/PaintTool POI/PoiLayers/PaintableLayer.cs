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
    internal class PaintableLayer : IPaintableLayer
    {
        public void DrawLine(Point2D from, Point2D to, Color fillColor, float strokWidth)
        {
            throw new NotImplementedException();
        }

        public IPaintableLayer SetSize(int width, int height)
        {
            throw new NotImplementedException();
        }
    }
}
