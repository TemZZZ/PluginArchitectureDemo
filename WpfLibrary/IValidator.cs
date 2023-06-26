using System.Collections.Generic;

namespace WpfLibrary
{
    public interface IValidator<in T>
    {
        IReadOnlyDictionary<string, List<string>> Validate(T validationContext);
    }
}