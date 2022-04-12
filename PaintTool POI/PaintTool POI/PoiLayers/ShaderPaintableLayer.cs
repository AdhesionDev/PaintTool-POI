using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace PaintTool_POI.PoiLayers
{
    internal class ShaderPaintableLayer : IPaintableLayer
    {
        public ShaderPaintableLayer()
        {
        }

        public void DrawLine(Vector2 from, Vector2 to, Color fillColor, float strokWidth)
        {
            throw new NotImplementedException();
            /**((BasicCurvePen)currentTool).OnDraw += (refer0, refer1, refer2, currPos, lastPressure, currentPressure) =>
            {
                float distance2 = (float)Math.Sqrt(((refer1.X - refer2.X) * (refer1.X - refer2.X) + (refer1.Y - refer2.Y) * (refer1.Y - refer2.Y)));
                //print("distance2: " + distance2);


                float2 lastFPos = new float2(-1000000, -1);
                for (float f = 0; f < distance2 + step; f += step)
                {
                    //print("F: " + f);
                    float normalF = (f * 100) / (distance2 * 100);

                    float2 currFPos = GetSplinePoint(refer0, refer1, refer2, currPos, normalF);

                    if (normalF >= 1)
                    {
                        GraphicsDevice.Default.For(4000, 4000, new DrawLineShader(renderTexture, lastFPos, refer2, 20f * currentPressure));
                        break;
                    }
                    if (lastFPos.X == -1000000)
                    {
                        //lastFPos = refer1;
                        //GraphicsDevice.Default.For(4000, 4000, new DrawLineShader(renderTexture, refer1, currFPos, 20f * currentPressure));
                    }
                    else
                    {
                        GraphicsDevice.Default.For(4000, 4000, new DrawLineShader(renderTexture, lastFPos, currFPos, 20f * currentPressure));
                    }

                    lastFPos = currFPos;
                }



                //print("Out for");
            };
            
             
             
             
        public Float2 GetSplinePoint(List<Float2> points, float t)
        {
            int portion0, portion1, portion2, portion3;
            portion1 = (int)t + 1;
            portion2 = portion1 + 1;
            portion3 = portion1 + 2;
            portion0 = portion1 - 1;

            t = t - (int)t;

            float t2 = t * t;
            float t3 = t * t * t;

            float factor0 = 0.5f * ((-t3) + 2 * t2 - t);
            float factor1 = 0.5f * (3 * t3 - 5 * t2 + 2);
            float factor2 = 0.5f * (-3 * t3 + 4 * t2 + t);
            float factor3 = 0.5f * (t3 - t2);

            float tx =
                points[portion0].X * factor0 +
                points[portion1].X * factor1 +
                points[portion2].X * factor2 +
                points[portion3].X * factor3;
            float ty =
                points[portion0].Y * factor0 +
                points[portion1].Y * factor1 +
                points[portion2].Y * factor2 +
                points[portion3].Y * factor3;
            return new Float2(tx, ty);
        }
        public Float2 GetSplinePoint(Float2 p0, Float2 p1, Float2 p2, Float2 p3, float t)
        {
            int portion0, portion1, portion2, portion3;
            portion1 = (int)t + 1;
            portion2 = portion1 + 1;
            portion3 = portion1 + 2;
            portion0 = portion1 - 1;

            t = t - (int)t;

            float t2 = t * t;
            float t3 = t * t * t;

            float factor0 = 0.5f * ((-t3) + 2 * t2 - t);
            float factor1 = 0.5f * (3 * t3 - 5 * t2 + 2);
            float factor2 = 0.5f * (-3 * t3 + 4 * t2 + t);
            float factor3 = 0.5f * (t3 - t2);

            float tx =
                p0.X * factor0 +
                p1.X * factor1 +
                p2.X * factor2 +
                p3.X * factor3;
            float ty =
                p0.Y * factor0 +
                p1.Y * factor1 +
                p2.Y * factor2 +
                p3.Y * factor3;
            return new Float2(tx, ty);
        }
             
             
             **/
        }

        public IPaintableLayer SetSize(int width, int height)
        {
            throw new NotImplementedException();
        }
    }
}
