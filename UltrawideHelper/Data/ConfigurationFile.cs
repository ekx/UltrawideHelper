using System.Collections.Generic;

namespace UltrawideHelper.Data
{
    public class ConfigurationFile
    {
        public bool AutoStart { get; set; }

        public bool AutoApplyProfiles { get; set; }

        public bool HideTaskbar { get; set; }

        public List<ShortcutProfile> ShortcutProfiles { get; set; }

        public List<WindowProfile> WindowProfiles { get; set; }
    }
}
