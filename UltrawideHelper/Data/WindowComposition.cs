using System;
using System.Collections.Generic;

namespace UltrawideHelper.Data;

public class WindowComposition : IEquatable<WindowComposition>
{
    public int PositionX { get; set; }

    public int PositionY { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public int DelayMilliseconds { get; set; }
        
    public bool MuteWhileNotFocused { get; set; }

    public List<string> WindowStyles { get; set; }

    public List<string> ExtendedWindowStyles { get; set; }

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
        WindowStyles = new List<string>();

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
        ExtendedWindowStyles = new List<string>();

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
}