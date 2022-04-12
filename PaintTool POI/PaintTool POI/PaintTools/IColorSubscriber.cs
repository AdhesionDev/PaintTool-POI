using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace PaintTool_POI
{
    internal interface IColorSubscriber
    {
        void UpdateColor(Color newColor);
    }
}
