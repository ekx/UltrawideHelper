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

    public uint GetModifier()
    {
        var keys = KeyCombination.Split('+');
        var result = 0U;

        foreach (var key in keys.SkipLast(1))
        {
            result |= LookupTables.Modifiers[key];
        }    

        return result;
    }

    public uint GetKey()
    {
        var keys = KeyCombination.Split('+');
        return LookupTables.Keys[keys.Last()];
    }
}