using System;
using System.Windows;
using TypeVisualiser.Abstractions;
using TypeVisualiser.Library;
using TypeVisualiser.WPF.Common;

namespace TypeVisualiser.Model
{
    public interface IDiagramElementFactory
    {
        IDiagramElement Create(Guid diagramId, IDiagramContentFunctionality diagramContent, IMessenger messenger);
        IDiagramElement Create(Guid diagramId, IDiagramContentFunctionality diagramContent, IMessenger messenger, DiagramElementConstruction construction);
    }

    public class DiagramElementConstruction
    {
        public bool Show { get; set; }
        public Point TopLeft { get; set; }

    }
}