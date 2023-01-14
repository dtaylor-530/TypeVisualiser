namespace TypeVisualiser.RecentFiles
{
    public interface IXamlService
    {
        object Load(string fullFileName);
        string Save(List<RecentFile> recentFiles);
    }
}