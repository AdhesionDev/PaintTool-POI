using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintTool_POI.PoiLayers
{
    internal interface IPaintableLayer : IPaintable
    {

        public IPaintableLayer SetSize(int width, int height);
    }
}
