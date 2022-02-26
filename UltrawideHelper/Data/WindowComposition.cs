using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UltrawideHelper.Data
{
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

        public bool HasWindowStyle { get { return WindowStyles != null && WindowStyles.Count > 0; } }

        public bool HasExtendedWindowStyle { get { return ExtendedWindowStyles != null && ExtendedWindowStyles.Count > 0; } }

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

            foreach (var style in LookupTables.WindowStyles)
            {
                if ((windowStyle & style.Value) != 0)
                {
                    WindowStyles.Add(style.Key);
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

            foreach (var style in LookupTables.ExtendedWindowStyles)
            {
                if ((extendedWindowStyle & style.Value) != 0)
                {
                    ExtendedWindowStyles.Add(style.Key);
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
    }
}
