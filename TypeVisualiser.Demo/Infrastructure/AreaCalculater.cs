using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TypeVisualiser.Geometry;
using TypeVisualiser.Model;

namespace TypeVisualiser.Demo.Infrastructure
{
    public class AreaCalculater : IAreaCalculater
    {
        public Area Calculate(
            double actualWidth,
            double actualHeight,
            Area subjectArea,
            Func<Area, ProximityTestResult> overlapsWithOthers)
        {

            Point proposedTopLeft = subjectArea.TopLeft.Clone();

            // Suggest directly above
            proposedTopLeft.Offset(0, -(actualHeight + (2.5 * ArrowHead.ArrowWidth)));
            var proposedArea = new Area(proposedTopLeft, actualWidth, actualHeight);

            while (overlapsWithOthers(proposedArea).Proximity == Proximity.Overlapping)
            {
                // Try left
                Area moveLeftProposal = proposedArea;
                do
                {
                    moveLeftProposal = moveLeftProposal.Offset(-(actualWidth + LayoutConstants.MinimumDistanceBetweenObjects), 0);
                }
                while (overlapsWithOthers(moveLeftProposal).Proximity == Proximity.Overlapping);

                // Try right
                Area moveRightProposal = proposedArea;
                Proximity proximity = overlapsWithOthers(moveRightProposal).Proximity;
                while (proximity != Proximity.NotOverlapping)
                {
                    if (proximity == Proximity.Overlapping)
                    {
                        moveRightProposal = moveRightProposal.Offset(actualWidth + LayoutConstants.MinimumDistanceBetweenObjects, 0);
                    }
                    else
                    {
                        moveRightProposal = moveRightProposal.Offset(LayoutConstants.MinimumDistanceBetweenObjects / 2, 0);
                    }

                    proximity = overlapsWithOthers(moveRightProposal).Proximity;
                }

                proposedArea = moveLeftProposal.DistanceToPoint(subjectArea.TopLeft) <= moveRightProposal.DistanceToPoint(subjectArea.TopLeft) ? moveLeftProposal : moveRightProposal;
            }
            return proposedArea;
        }
    }
}
