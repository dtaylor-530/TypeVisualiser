namespace TypeVisualiser.Model.Persistence
{
    public interface ITypeVisualiserLayoutFile
    {
        string AssemblyFileName { get; }
        IVisualisableTypeData Subject { get; set; }
        bool HideTrivialTypes { get; }
    }
}