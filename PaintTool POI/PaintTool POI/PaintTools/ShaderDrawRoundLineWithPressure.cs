using ComputeSharp;
using ComputeSharp.__Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintTool_POI.PaintTools
{
    [EmbeddedBytecode(8, 8, 1)]
    public readonly partial struct ShaderDrawRoundLineWithPressure : IPixelShader<float4>
    {
        public readonly IReadWriteTexture2D<float4> texture;


        public readonly float2 lastPos;
        public readonly float2 currentPos;
        public readonly float4 penColor;
        public readonly float lastPressure;
        public readonly float currentPressure;
        public readonly float thickness;


        public ShaderDrawRoundLineWithPressure(IReadWriteTexture2D<float4> texture, float2 lastPos, float currentPos, float4 penColor, float lastPressure, float currentPressure, float thickness)
        {
            this.texture = texture;
            this.lastPos = lastPos;
            this.currentPos = currentPos;
            this.penColor = penColor;
            this.lastPressure = lastPressure;
            this.currentPressure = currentPressure;
            this.thickness = thickness;
        }

        public Float4 Execute()
        {
            float4 result = new float4(1, 1, 1, 0);

            float2 vectorLine = lastPos - currentPos;

            float pressureDiff = lastPressure - currentPressure;


            if (ThreadIds.X < Hlsl.Max(lastPos.X, currentPos.X) ||
                ThreadIds.X > Hlsl.Min(lastPos.X, currentPos.X))
            {

            }

            for (int times = 0; times <= 1; times++)
            {
                double protion = times / 1;
                float currentPoint = 1 * (float)protion;

                float currentThickness = ((float)protion * pressureDiff + pressureDiff) * thickness;

                for (float x = -currentThickness; x < currentThickness; x++)
                {
                    for (float y = -currentThickness; y <= currentThickness; y++)
                    {
                        if (currentThickness * currentThickness > (x * x + y * y))
                        {
                            if (Hlsl.Distance(ThreadIds.XY, lastPos) < currentThickness * currentThickness)
                            {
                                result = new float4(1, 1, 1, 1);
                            }
                        }

                    }
                }
            }



            return result;
        }
    }
}
