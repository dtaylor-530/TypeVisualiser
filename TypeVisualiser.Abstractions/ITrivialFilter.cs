using TypeVisualiser.Library;
using TypeVisualiser.Model;

namespace TypeVisualiser
{
    public interface ITrivialFilter
    {
        bool HideTrivialTypes { get; set; }
        bool HideSystemTypes { get; set; }
        bool HideSecondaryAssociations { get; set; }
        void AddToTrivialTypeList(IVisualisableType type);
        void EditTrivialList();

        /// <summary>
        /// Determines if the given type is deemed a trivial type.
        /// </summary>
        /// <param name="namespaceQualifiedName">The full type name.</param>
        /// <returns>
        /// Returns <c>true</c> if given type is a trivial type, otherwise <c>false</c>
        /// </returns>
        bool IsTrivialType(string namespaceQualifiedName);

        bool IsVisible(IDiagramElement element, bool primaryDiagramElement);
    }
}