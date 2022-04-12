using PaintTool_POI.DataTypes;
using Windows.Foundation;
using PaintTool_POI.DataTypes;
using System.Numerics;
using PaintTool_POI.PoiLayers;

namespace PaintTool_POI
{
    interface IPaintTool : IColorSubscriber
    {
        /// <summary>
        /// Call when the paint tool is selected
        /// </summary>

        /// <summary>
        /// Call when the paint tool revive a pointer input
        /// </summary>
        /// <param name="position"></param>
        /// <param name="pressure"></param>
        /// <param name="canvasImage"></param>
        void OnPenDown(Point2D position, float pressure, IPaintableLayers layers);
        /// <summary>
        /// Call when the paint tool is reviving an input.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="pressure"></param>
        /// <param name="canvasImage"></param>
        void OnPaint(Point2D position, float pressure, IPaintableLayers layers);
        /// <summary>
        /// Call when the input is ended.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="pressure"></param>
        /// <param name="canvasImage"></param>
        void OnPenUp(Point2D position, float pressure, IPaintableLayers layers);
        /// <summary>
        /// Call when the paint toll is unselected.
        /// </summary>
        void OnUnselet();
        void OnSelect();
    }
}
