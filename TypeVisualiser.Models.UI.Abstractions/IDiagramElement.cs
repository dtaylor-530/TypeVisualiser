using GalaSoft.MvvmLight;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using TypeVisualiser.Geometry;

namespace TypeVisualiser.Library
{
    public interface IDiagramElement : ICleanup
    {
        IDiagramContent DiagramContent { get; }
        Area Area { get; }
        double Width { get; set; }
        double Height { get; set; }
        Point TopLeft { get; set; }
        Func<IDiagramElement, bool, bool> IsVisibleAdditionalLogic { get; set; }
        bool Show { get; set; }
        IEnumerable<IDiagramElement> RelatedDiagramElements { get; }
        int ZOrder { get; set; }

        void AdjustCoordinatesAfterCanvasExpansion(double smallestX, double smallestY);
        void CenterOnPoint(Point centre);

        void RefreshPosition();
        void RegisterPositionDependency(IEnumerable<IDiagramElement> dependentElements, Func<Area, ProximityTestResult> isOverlappingFunction);
    }
}