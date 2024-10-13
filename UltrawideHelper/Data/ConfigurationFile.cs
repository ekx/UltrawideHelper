using System.Collections.Generic;

namespace UltrawideHelper.Data;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable ClassNeverInstantiated.Global

public class ConfigurationFile
{
    public bool AutoStart { get; init; }

    public bool AutoUpdate { get; init; }

    public bool AutoApplyProfiles { get; init; }

    public bool HideTaskbar { get; init; }
    
    public bool HideTaskbarWhenProfileActive { get; init; }
    
    public string MuteFocusedApplicationShortcut { get; init; }

    public List<ShortcutProfile> ShortcutProfiles { get; init; }

    public List<WindowProfile> WindowProfiles { get; init; }
}