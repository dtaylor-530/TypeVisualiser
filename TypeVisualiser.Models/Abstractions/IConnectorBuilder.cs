using System;
using TypeVisualiser.Geometry;
using TypeVisualiser.Model;

namespace TypeVisualiser.Models.Abstractions
{
    /// <summary>
    /// An interface to encapsulate obtaining the best connection line from area to area.
    /// </summary>
    public interface IConnectorBuilder
    {
        /// <summary>
        /// Calculate the best line connecting <see cref="fromArea"/> to <see cref="destinationArea"/>
        /// </summary>
        ConnectionLine CalculateBestConnection(Area fromArea, Area destinationArea, Func<Area, ProximityTestResult> isOverlappingWithOtherControls);
    }
}
