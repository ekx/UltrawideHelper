namespace UltrawideHelper.Data;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable ClassNeverInstantiated.Global

public class WindowProfile
{
    public string TitleRegex { get; init; }

    public string ProcessName { get; init; }
        
    public WindowComposition WindowComposition { get; init; }
}