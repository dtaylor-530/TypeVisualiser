using System;
using TypeVisualiser.Geometry;

namespace TypeVisualiser.Demo.Infrastructure
{
    public interface IAreaCalculater
    {
        Area Calculate(double actualWidth, double actualHeight, Area subjectArea, Func<Area, ProximityTestResult> overlapsWithOthers);
    }
}