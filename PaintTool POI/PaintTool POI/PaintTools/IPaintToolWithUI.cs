using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaintTool_POI.UIElements;
namespace PaintTool_POI.PaintTools
{
    interface IPaintToolWithUI : IPaintTool
    {
        /// <summary>
        /// Get UI in tool box.
        /// </summary>
        /// <returns></returns>
        Windows.UI.Xaml.UIElement GetToolBoxItem();
        /// <summary>
        /// Get UI in paint tool settings.
        /// </summary>
        /// <returns></returns>
        Windows.UI.Xaml.UIElement GetToolSettings();
    }
}
