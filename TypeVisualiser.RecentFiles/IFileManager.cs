using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;
using TypeVisualiser.Model;
using TypeVisualiser.Model.Persistence;
using TypeVisualiser.RecentFiles;

namespace TypeVisualiser.Models.Abstractions
{
    public interface IFileManager<T>: IFileManager where T : IVisualisableType
    {
        T LoadDemoType();
        Task<T> LoadFromDiagramFileAsync(ITypeVisualiserLayoutFile layout);
        Task<T> LoadFromRecentFileAsync(RecentFile recentFileData);
        Task<T> LoadFromVisualisableTypeAsync(IVisualisableType type);
        Task<T> LoadTypeAsync(Type type);
        T RefreshSubject(Type subjectType);
    }

    public interface IFileManager
    {
        IRecentFiles RecentFiles { get; }
        void ChooseAssembly();
        void Initialise();
        Assembly LoadAssembly(string assemblyFileName = "");
        ITypeVisualiserLayoutFile LoadDiagram();
        Type RefreshType(string fullTypeName, string assemblyFileName);
        void SaveDiagram(IPersistentFileData layoutData);
    }
}