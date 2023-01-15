using System.Reflection;

namespace TypeVisualiser.Factory
{
    public interface ITypeBuilder
    {
        Type BuildType(string assemblyFile, string fullTypeName);
        Assembly LoadAssembly(string fileName);
    }
}