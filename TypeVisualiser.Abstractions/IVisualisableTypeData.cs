using TypeVisualiser.Model;

namespace TypeVisualiser.Model.Persistence
{
    public interface IVisualisableTypeData
    {
        string AssemblyFileName { get; set; }
        string AssemblyFullName { get; set; }
        string AssemblyName { get; set; }
        int ConstructorCount { get; set; }
        int EnumMemberCount { get; set; }
        int EventCount { get; set; }
        int FieldCount { get; set; }
        string FullName { get; set; }
        string Id { get; set; }
        int LinesOfCode { get; set; }
        string LinesOfCodeToolTip { get; set; }
        int MethodCount { get; set; }
        ModifiersData Modifiers { get; set; }
        string Name { get; set; }
        string Namespace { get; set; }
        int PropertyCount { get; set; }
        bool Show { get; set; }
        SubjectOrAssociate SubjectOrAssociate { get; set; }
        string ToolTipName { get; set; }
    }
}