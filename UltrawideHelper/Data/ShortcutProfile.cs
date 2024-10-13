using System.Linq;

namespace UltrawideHelper.Data;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable ClassNeverInstantiated.Global

public class ShortcutProfile
{
    public int Id { get; init; }

    public string KeyCombination { get; init; }
        
    public WindowComposition WindowComposition { get; init; }
}