using System.Linq;
using UltrawideHelper.Data;

namespace UltrawideHelper.Shortcuts;

public static class ShortcutHelper
{
    public static uint GetModifier(string keyCombination)
    {
        var keys = keyCombination.Split('+');
        var result = 0U;

        foreach (var key in keys.SkipLast(1))
        {
            result |= LookupTables.Modifiers[key];
        }    

        return result;
    }

    public static uint GetKey(string keyCombination)
    {
        var keys = keyCombination.Split('+');
        return LookupTables.Keys[keys.Last()];
    }
}