namespace Core
{
    public interface IPlugin
    {
        string Name { get; }
        string Description { get; }
        string GetXamlResourceDictionary();
    }
}