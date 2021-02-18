using System.Linq;

namespace UltrawideHelper.Data
{
    public class ShortcutProfile
    {
        public int Id { get; set; }

        public string KeyCombination { get; set; }
        
        public WindowComposition WindowComposition { get; set; }

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
}
