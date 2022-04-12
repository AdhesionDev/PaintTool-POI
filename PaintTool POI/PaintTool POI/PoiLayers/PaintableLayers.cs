using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace PaintTool_POI.PoiLayers
{
    internal class PaintableLayers : IPaintableLayers
    {
        List<IPaintableLayer> layers;
        public PaintableLayers()
        {
            layers = new List<IPaintableLayer>();
            layers.Add(new PaintableLayer());
        }


        public IPaintableLayer GetActiveLayer()
        {
            return layers[0];
        }

        public UIElement GetUIElement()
        {
            return ((CanvasPaintableLayer)layers[0]).canvas;
        }

        public IPaintableLayers SetSize(int width, int height)
        {
            for (int i = 0; i < layers.Count; i++)
            {
                layers[i].SetSize(width, height);
            }
            return this;
        }
    }
}
