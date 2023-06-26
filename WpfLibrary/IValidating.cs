using System.Collections.Generic;

namespace WpfLibrary
{
    public interface IValidating
    {
        bool IsValid { get; }
        IReadOnlyDictionary<string, List<string>> GetLastErrors();
    }
}