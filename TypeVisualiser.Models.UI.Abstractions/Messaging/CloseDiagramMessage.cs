namespace TypeVisualiser.Models.UI.Abstractions.Messaging
{
    using System;

    public class CloseDiagramMessage : SingleDiagramOrientedMessage
    {
        public CloseDiagramMessage(Guid diagramId) : base(diagramId) { }
    }
}