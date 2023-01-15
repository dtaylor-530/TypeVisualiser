using System.Windows;
using TypeVisualiser.Library;
using TypeVisualiser.WPF.Common;

namespace TypeVisualiser.Model
{
    public interface IConnectionLine: IDiagramContentFunctionality
    {
        string Style { get; set; }
        double Thickness { get; set; }
        double FromAngle { get; }
        double ToAngle { get; }
        Point From { get; }
        IDiagramContentFunctionality PointingAt { get; }
        Point To { get; }
    }
}