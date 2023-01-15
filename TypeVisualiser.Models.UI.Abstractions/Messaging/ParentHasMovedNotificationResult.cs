using System.Windows;
using TypeVisualiser.WPF.Common;

namespace TypeVisualiser.Models.UI.Abstractions.Messaging
{
    /// <summary>
    /// A result class for <see cref="IDiagramContentFunctionality.NotifyDiagramContentParentHasMoved"/>.
    /// Passes information back to the <see cref="DiagramElement"/> from the <see cref="IDiagramContentFunctionality"/>.
    /// </summary>
    public class ParentHasMovedNotificationResult //: IParentHasMovedNotificationResult
    {
        public ParentHasMovedNotificationResult()
        {
        }

        public ParentHasMovedNotificationResult(Point newTopLeft)
        {
            LayoutChangesRequired = true;
            NewTopLeftLocation = newTopLeft;
            NewZOrder = 1;
        }

        /// <summary>
        /// Gets or sets a value indicating whether layout changes are required.
        /// This basically means the <see cref="NewTopLeftLocation"/> and/or the <see cref="NewZOrder"/> have been set with suggested new values.
        /// </summary>
        public bool LayoutChangesRequired { get; private set; }

        /// <summary>
        /// An option to return a new top left.
        /// </summary>
        /// <value>
        /// The new top left location.
        /// </value>
        public Point NewTopLeftLocation { get; private set; }

        /// <summary>
        /// An option to return a new Z order.
        /// </summary>
        /// <value>
        /// The new Z order.
        /// </value>
        public int NewZOrder { get; set; }
    }
}