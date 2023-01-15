using System;
using TypeVisualiser.Geometry;
using TypeVisualiser.WPF.Common;

namespace TypeVisualiser.Models.Abstractions
{
    public interface ICalculatedPositionDiagramContent : IDiagramContentFunctionality
    {
        /// <summary>
        /// Proposes a position for the diagram content.
        /// </summary>
        /// <param name="actualWidth">The actual width of the associate UI element.</param>
        /// <param name="actualHeight">The actual height of the associate UI element.</param>
        /// <param name="subjectArea">The subject area.</param>
        /// <param name="overlapsWithOthers">The function delegate that determines if the proposed area overlaps with others.</param>
        /// <returns>A proposed area to move the content to.</returns>
        Area ProposePosition(
            double actualWidth,
            double actualHeight,
            Area subjectArea,
            Func<Area, ProximityTestResult> overlapsWithOthers);
    }
}