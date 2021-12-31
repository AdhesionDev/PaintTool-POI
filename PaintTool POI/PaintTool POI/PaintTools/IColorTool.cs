using Windows.UI;

namespace PaintTool_POI.PaintTools
{
    interface IColorTool
    {
        Color penColor
        {
            get;
            set;
        }
        Color backColor
        {
            get;
            set;
        }
    }
}
