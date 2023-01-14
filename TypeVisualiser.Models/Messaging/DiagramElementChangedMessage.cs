using System;
using TypeVisualiser.Library;
using TypeVisualiser.Model;

namespace TypeVisualiser.Messaging
{
    public class DiagramElementChangedMessage : SingleDiagramOrientedMessage
    {
        public DiagramElementChangedMessage(Guid diagramId) : base(diagramId)
        {
        }

        public IDiagramElement ChangedElement { get; set; }
    }
}