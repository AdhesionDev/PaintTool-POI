using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace PaintTool_POI
{
    internal interface IPaintable
    {
        void DrawLine(Point2D from, Point2D to, Color fillColor, float strokWidth);

        //void DrawCurve(Vector2 from, Vector2 to, Vector2 controlPoint1, Vector2 controlPoint2, Color fillColor, float strokWidth);
    }
}
