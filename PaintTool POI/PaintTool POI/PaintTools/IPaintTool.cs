using PaintTool_POI.DataTypes;
using Windows.Foundation;
using PaintTool_POI.DataTypes;

namespace PaintTool_POI.PaintTools
{
    interface IPaintTool : IColorTool
    {
        /// <summary>
        /// Call when the paint tool is selected
        /// </summary>
        void OnSelect();
        /// <summary>
        /// Call when the paint tool revive a pointer input
        /// </summary>
        /// <param name="position"></param>
        /// <param name="pressure"></param>
        /// <param name="canvasImage"></param>
        void OnPenDown(Point position, float pressure, PixelImage pixelImage);
        /// <summary>
        /// Call when the paint tool is reviving an input.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="pressure"></param>
        /// <param name="canvasImage"></param>
        void OnPaint(Point position, float pressure, PixelImage canvasImage);
        /// <summary>
        /// Call when the input is ended.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="pressure"></param>
        /// <param name="canvasImage"></param>
        void OnPenUp(Point position, float pressure, PixelImage canvasImage);
        /// <summary>
        /// Call when the paint toll is unselected.
        /// </summary>
        void OnUnselet();


    }
}
