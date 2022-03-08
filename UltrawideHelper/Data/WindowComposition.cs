using System;
using System.Collections.Generic;

namespace UltrawideHelper.Data;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable ClassNeverInstantiated.Global

public class WindowComposition : IEquatable<WindowComposition>
{
    public int PositionX { get; init; }

    public int PositionY { get; init; }

    public int Width { get; init; }

    public int Height { get; init; }
    
    public int DelayMilliseconds { get; init; }

    public bool MuteWhileNotFocused { get; init; }
    
    public List<string> WindowStyles { get; init; } = new();

    public List<string> ExtendedWindowStyles { get; init; } = new();
    
    public bool HasWindowStyle => WindowStyles is { Count: > 0 };

    public bool HasExtendedWindowStyle => ExtendedWindowStyles is { Count: > 0 };

    public uint GetWindowStyle()
    {
        var result = 0U;

        if (WindowStyles == null) return result;

        foreach (var style in WindowStyles)
        {
            result |= LookupTables.WindowStyles[style];
        }    

        return result;
    }

    public void SetWindowStyle(uint windowStyle)
    {
        WindowStyles.Clear();

        foreach (var (key, value) in LookupTables.WindowStyles)
        {
            if ((windowStyle & value) != 0)
            {
                WindowStyles.Add(key);
            }
        }
    }

    public uint GetExtendedWindowStyle()
    {
        var result = 0U;

        if (ExtendedWindowStyles == null) return result;

        foreach (var style in ExtendedWindowStyles)
        {
            result |= LookupTables.ExtendedWindowStyles[style];
        }

        return result;
    }

    public void SetExtendedWindowStyle(uint extendedWindowStyle)
    {
        ExtendedWindowStyles.Clear();

        foreach (var (key, value) in LookupTables.ExtendedWindowStyles)
        {
            if ((extendedWindowStyle & value) != 0)
            {
                ExtendedWindowStyles.Add(key);
            }
        }
    }

    public bool Equals(WindowComposition other)
    {
        if (other == null) return false;

        if (PositionX != other.PositionX) return false;
        if (PositionY != other.PositionY) return false;
        if (Width != other.Width) return false;
        if (Height != other.Height) return false;
        if (GetWindowStyle() != other.GetWindowStyle()) return false;
        if (GetExtendedWindowStyle() != other.GetExtendedWindowStyle()) return false;

        return true;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as WindowComposition);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(PositionX, PositionY, Width, Height, WindowStyles, ExtendedWindowStyles);
    }
}