using PaintTool_POI.DataTypes;
using PaintTool_POI.UIElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;

namespace PaintTool_POI.PaintTools
{
    internal class BasicCurvePen : IPaintToolWithUI
    {
        public Color penColor { get; set; }
        public Color backColor { get; set; }

        public Action<float2, float2, float2, float2, float, float> OnDraw;

        float2 refer0;
        float2 refer1;
        float2 refer2;
        //float2 referFuture;
        float lastPressure;

        public BasicCurvePen()
        {
            refer0 = new float2(-114, -1);
            refer1 = new float2(-114, -1);
            refer2 = new float2(-114, -1);
        }


        public UIElement GetToolBoxItem()
        {
            return new PaintToolItemGrid("\xEDFB", "Calligraphy Pen");
        }

        public UIElement GetToolSettings()
        {
            return new PaintToolItemGrid("\xEDFB", "Calligraphy Pen");
        }

        public void OnPaint(Point position, float pressure, PixelImage canvasImage)
        {
            float2 currentPos = new float2((float)position.X, (float)position.Y);
            if (refer0.X == -114)
            {

            }
            else if (refer1.X == -114)
            {

            }
            else if (refer2.X == -114)
            {
                //float2 futurePos = (currentPos - lastPos) + currentPos;
            }
            else
            {
                OnDraw?.Invoke(refer0, refer1, refer2, currentPos, lastPressure, pressure);
            }

            refer0 = refer1;
            refer1 = refer2;
            refer2 = currentPos;
            lastPressure = pressure;
        }

        public void OnPenDown(Point position, float pressure, PixelImage pixelImage)
        {
            //throw new NotImplementedException();
        }

        public void OnPenUp(Point position, float pressure, PixelImage canvasImage)
        {
            refer0 = new float2(-114, -1);
            refer1 = new float2(-114, -1);
            refer2 = new float2(-114, -1);
        }

        public void OnSelect()
        {

        }

        public void OnUnselet()
        {
            throw new NotImplementedException();
        }
    }
}
