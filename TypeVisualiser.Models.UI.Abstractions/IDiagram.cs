using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TypeVisualiser.Models.Abstractions
{
    public interface IDiagram
    {
        Point Centre { get; }
        IDiagramController Controller { get; }
        Guid Id { get; }
        string FullName { get; }
        bool IsLoaded { get; }
        double ContentScale { get; set; }

        event Action Closed;
        double ContentWidth { get; set; }
        double ContentHeight { get; set; }

        void CentreDiagram();
        void ZoomToFit();
    }
}
