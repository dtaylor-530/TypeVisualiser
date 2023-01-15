using System;
using TypeVisualiser.Library;
using TypeVisualiser.Model;
using TypeVisualiser.Models.UI.Abstractions.Messaging;

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