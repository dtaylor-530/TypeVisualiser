using TypeVisualiser.Abstractions;
using TypeVisualiser.Library;
using TypeVisualiser.WPF.Common;

namespace TypeVisualiser.Model
{
    public class DiagramElementFactory : IDiagramElementFactory
    {
        public IDiagramElement Create(Guid diagramId, IDiagramContentFunctionality diagramContent, IMessenger messenger)
        {
            return new DiagramElement(diagramId, diagramContent, messenger);
        }

        public IDiagramElement Create(Guid diagramId, IDiagramContentFunctionality diagramContent, IMessenger messenger, DiagramElementConstruction construction)
        {
            return new DiagramElement(diagramId, diagramContent, messenger)
            {
                Show = construction.Show,
                TopLeft = construction.TopLeft,
            };
        }
    }
}