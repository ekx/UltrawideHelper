using System.Collections.Generic;

namespace UltrawideHelper.Data;

public static class LookupTables
{
    public static readonly Dictionary<string, uint> Modifiers = new()
    {
        { "ALT", 0x0001 },                              // Either ALT key must be held down.
        { "CONTROL", 0x0002 },                          // Either CTRL key must be held down.
        { "SHIFT", 0x0004 },                            // Either SHIFT key must be held down.
        { "WIN", 0x0008 },                              // Either WINDOWS key was held down. These keys are labeled with the Windows logo. Keyboard shortcuts that involve the WINDOWS key are reserved for use by the operating system.
        { "NOREPEAT", 0x4000 }                          // Changes the hotkey behavior so that the keyboard auto-repeat does not yield multiple hotkey notifications. Not supported on Windows Vista.
    };

    public static readonly Dictionary<string, uint> Keys = new()
    {
        { "LBUTTON", 0x01 },                            // Left mouse button
        { "RBUTTON", 0x02 },                            // Right mouse button
        { "CANCEL", 0x03 },                             // Control-break processing
        { "MBUTTON", 0x04 },                            // Middle mouse button (three-button mouse)
        { "XBUTTON1", 0x05 },                           // X1 mouse button
        { "XBUTTON2", 0x06 },                           // X2 mouse button

        { "BACK", 0x08 },                               // BACKSPACE key
        { "TAB", 0x09 },                                // TAB key

        { "SHIFT", 0x10 },                              // SHIFT key
        { "CONTROL", 0x11 },                            // CTRL key
        { "MENU", 0x12 },                               // ALT key
        { "PAUSE", 0x13 },                              // PAUSE key
        { "CAPITAL", 0x14 },                            // CAPS LOCK key
        { "KANA", 0x15 },                               // IME Kana mode
        { "HANGUL", 0x15 },                             // IME Hangul mode
        { "IME_ON", 0x16 },                             // IME On
        { "JUNJA", 0x17 },                              // IME Junja mode
        { "FINAL", 0x18 },                              // IME final mode
        { "HANJA", 0x19 },                              // IME Hanja mode
        { "KANJI", 0x19 },                              // IME Kanji mode
        { "IME_OFF", 0x1A },                            // IME Off
        { "ESCAPE", 0x1B },                             // ESC key
        { "CONVERT", 0x1C },                            // IME convert
        { "NONCONVERT", 0x1D },                         // IME nonconvert
        { "ACCEPT", 0x1E },                             // IME accept
        { "MODECHANGE", 0x1F },                         // IME mode change request
        { "SPACE", 0x20 },                              // SPACEBAR
        { "PRIOR", 0x21 },                              // PAGE UP key
        { "NEXT", 0x22 },                               // PAGE DOWN key
        { "END", 0x23 },                                // END key
        { "HOME", 0x24 },                               // HOME key
        { "LEFT", 0x25 },                               // LEFT ARROW key
        { "UP", 0x26 },                                 // UP ARROW key
        { "RIGHT", 0x27 },                              // RIGHT ARROW key
        { "DOWN", 0x28 },                               // DOWN ARROW key
        { "SELECT", 0x29 },                             // SELECT key
        { "PRINT", 0x2A },                              // PRINT key
        { "EXECUTE", 0x2B },                            // EXECUTE key
        { "SNAPSHOT", 0x2C },                           // PRINT SCREEN key
        { "INSERT", 0x2D },                             // INS key
        { "DELETE", 0x2E },                             // DEL key
        { "HELP", 0x2F },                               // HELP key

        { "0", 0x30 },                                  // 0 key
        { "1", 0x31 },                                  // 1 key
        { "2", 0x32 },                                  // 2 key
        { "3", 0x33 },                                  // 3 key
        { "4", 0x34 },                                  // 4 key
        { "5", 0x35 },                                  // 5 key
        { "6", 0x36 },                                  // 6 key
        { "7", 0x37 },                                  // 7 key
        { "8", 0x38 },                                  // 8 key
        { "9", 0x39 },                                  // 9 key

        { "A", 0x41 },                                  // A key
        { "B", 0x42 },                                  // B key
        { "C", 0x43 },                                  // C key
        { "D", 0x44 },                                  // D key
        { "E", 0x45 },                                  // E key
        { "F", 0x46 },                                  // F key
        { "G", 0x47 },                                  // G key
        { "H", 0x48 },                                  // H key
        { "I", 0x49 },                                  // I key
        { "J", 0x4A },                                  // J key
        { "K", 0x4B },                                  // K key
        { "L", 0x4C },                                  // L key
        { "M", 0x4D },                                  // M key
        { "N", 0x4E },                                  // N key
        { "O", 0x4F },                                  // O key
        { "P", 0x50 },                                  // P key
        { "Q", 0x51 },                                  // Q key
        { "R", 0x52 },                                  // R key
        { "S", 0x53 },                                  // S key
        { "T", 0x54 },                                  // T key
        { "U", 0x55 },                                  // U key
        { "V", 0x56 },                                  // V key
        { "W", 0x57 },                                  // W key
        { "X", 0x58 },                                  // X key
        { "Y", 0x59 },                                  // Y key
        { "Z", 0x5A },                                  // Z key

        { "LWIN", 0x5B },                               // Left Windows key (Natural keyboard)
        { "RWIN", 0x5C },                               // Right Windows key (Natural keyboard)
        { "APPS", 0x5D },                               // Applications key (Natural keyboard)

        { "SLEEP", 0x5F },                              // Computer Sleep key
        { "NUMPAD0", 0x60 },                            // Numeric keypad 0 key
        { "NUMPAD1", 0x61 },                            // Numeric keypad 1 key
        { "NUMPAD2", 0x62 },                            // Numeric keypad 2 key
        { "NUMPAD3", 0x63 },                            // Numeric keypad 3 key
        { "NUMPAD4", 0x64 },                            // Numeric keypad 4 key
        { "NUMPAD5", 0x65 },                            // Numeric keypad 5 key
        { "NUMPAD6", 0x66 },                            // Numeric keypad 6 key
        { "NUMPAD7", 0x67 },                            // Numeric keypad 7 key
        { "NUMPAD8", 0x68 },                            // Numeric keypad 8 key
        { "NUMPAD9", 0x69 },                            // Numeric keypad 9 key
        { "MULTIPLY", 0x6A },                           // Multiply key
        { "ADD", 0x6B },                                // Add key
        { "SEPARATOR", 0x6C },                          // Separator key
        { "SUBTRACT", 0x6D },                           // Subtract key
        { "DECIMAL", 0x6E },                            // Decimal key
        { "DIVIDE", 0x6F },                             // Divide key

        { "F1", 0x70 },                                 // F1 key
        { "F2", 0x71 },                                 // F2 key
        { "F3", 0x72 },                                 // F3 key
        { "F4", 0x73 },                                 // F4 key
        { "F5", 0x74 },                                 // F5 key
        { "F6", 0x75 },                                 // F6 key
        { "F7", 0x76 },                                 // F7 key
        { "F8", 0x77 },                                 // F8 key
        { "F9", 0x78 },                                 // F9 key
        { "F10", 0x79 },                                // F10 key
        { "F11", 0x7A },                                // F11 key
        { "F12", 0x7B },                                // F12 key
        { "F13", 0x7C },                                // F13 key
        { "F14", 0x7D },                                // F14 key
        { "F15", 0x7E },                                // F15 key
        { "F16", 0x7F },                                // F16 key
        { "F17", 0x80 },                                // F17 key
        { "F18", 0x81 },                                // F18 key
        { "F19", 0x82 },                                // F19 key
        { "F20", 0x83 },                                // F20 key
        { "F21", 0x84 },                                // F21 key
        { "F22", 0x85 },                                // F22 key
        { "F23", 0x86 },                                // F23 key
        { "F24", 0x87 },                                // F24 key

        { "NUMLOCK", 0x90 },                            // NUM LOCK key
        { "SCROLL", 0x91 },                             // SCROLL LOCK key

        { "LSHIFT", 0xA0 },                             // Left SHIFT key
        { "RSHIFT", 0xA1 },                             // Right SHIFT key
        { "LCONTROL", 0xA2 },                           // Left CONTROL key
        { "RCONTROL", 0xA3 },                           // Right CONTROL key
        { "LMENU", 0xA4 },                              // Left MENU key
        { "RMENU", 0xA5 },                              // Right MENU key

        { "BROWSER_BACK", 0xA6 },                       // Browser Back key
        { "BROWSER_FORWARD", 0xA7 },                    // Browser Forward key
        { "BROWSER_REFRESH", 0xA8 },                    // Browser Refresh key
        { "BROWSER_STOP", 0xA9 },                       // Browser Stop key
        { "BROWSER_SEARCH", 0xAA },                     // Browser Search key
        { "BROWSER_FAVORITES", 0xAB },                  // Browser Favorites key
        { "BROWSER_HOME", 0xAC },                       // Browser Start and Home key
        { "VOLUME_MUTE", 0xAD },                        // Volume Mute key
        { "VOLUME_DOWN", 0xAE },                        // Volume Down key
        { "VOLUME_UP", 0xAF },                          // Volume Up key
        { "MEDIA_NEXT_TRACK", 0xB0 },                   // Next Track key
        { "MEDIA_PREV_TRACK", 0xB1 },                   // Previous Track key
        { "MEDIA_STOP", 0xB2 },                         // Stop Media key
        { "MEDIA_PLAY_PAUSE", 0xB3 },                   // Play/Pause Media key
        { "LAUNCH_MAIL", 0xB4 },                        // Start Mail key
        { "LAUNCH_MEDIA_SELECT", 0xB5 },                // Select Media key
        { "LAUNCH_APP1", 0xB6 },                        // Start Application 1 key
        { "LAUNCH_APP2", 0xB7 },                        // Start Application 2 key

        { "OEM_1", 0xBA },                              // Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the ';:' key
        { "OEM_PLUS", 0xBB },                           // For any country/region, the '+' key
        { "OEM_COMMA", 0xBC },                          // For any country/region, the ',' key
        { "OEM_MINUS", 0xBD },                          // For any country/region, the '-' key
        { "OEM_PERIOD", 0xBE },                         // For any country/region, the '.' key
        { "OEM_2", 0xBF },                              // Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the '/?' key
        { "OEM_3", 0xC0 },                              // Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the '`~' key
        { "OEM_4", 0xDB },                              // Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the '[{' key
        { "OEM_5", 0xDC },                              // Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the '\|' key
        { "OEM_6", 0xDD },                              // Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the ']}' key
        { "OEM_7", 0xDE },                              // Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the 'single-quote/double-quote' key
        { "OEM_8", 0xDF },                              // Used for miscellaneous characters; it can vary by keyboard.
        { "OEM_102", 0xE2 },                            // Either the angle bracket key or the backslash key on the RT 102-key keyboard

        { "PROCESSKEY", 0xE5 },                         // IME PROCESS key
        { "PACKET", 0xE7 },                             // Used to pass Unicode characters as if they were keystrokes.
        { "ATTN", 0xF6 },                               // Attn key
        { "CRSEL", 0xF7 },                              // CrSel key
        { "EXSEL", 0xF8 },                              // ExSel key
        { "EREOF", 0xF9 },                              // Erase EOF key
        { "PLAY", 0xFA },                               // Play key
        { "ZOOM", 0xFB },                               // Zoom key
        { "NONAME", 0xFC },                             // Reserved
        { "PA1", 0xFD },                                // PA1 key
        { "OEM_CLEAR", 0xFE }                           // Clear key
    };

    public static readonly Dictionary<string, uint> WindowStyles = new()
    {
        { "WS_BORDER", 0x00800000 },                    // The window has a thin-line border.
        //{ "WS_CAPTION", 0x00C00000 },                   // The window has a title bar (includes the WS_BORDER style).
        { "WS_CHILD", 0x40000000 },                     // The window is a child window. A window with this style cannot have a menu bar. This style cannot be used with the WS_POPUP style.
        { "WS_CHILDWINDOW", 0x40000000 },               // Same as the WS_CHILD style.
        { "WS_CLIPCHILDREN", 0x02000000 },              // Excludes the area occupied by child windows when drawing occurs within the parent window. This style is used when creating the parent window.
        { "WS_CLIPSIBLINGS", 0x04000000 },              // Clips child windows relative to each other; that is, when a particular child window receives a WM_PAINT message, the WS_CLIPSIBLINGS style clips all other overlapping child windows out of the region of the child window to be updated. If WS_CLIPSIBLINGS is not specified and child windows overlap, it is possible, when drawing within the client area of a child window, to draw within the client area of a neighboring child window.
        { "WS_DISABLED", 0x08000000 },                  // The window is initially disabled. A disabled window cannot receive input from the user. To change this after a window has been created, use the EnableWindow function.
        { "WS_DLGFRAME", 0x00400000 },                  // The window has a border of a style typically used with dialog boxes. A window with this style cannot have a title bar.
        { "WS_GROUP", 0x00020000 },                     // The window is the first control of a group of controls. The group consists of this first control and all controls defined after it, up to the next control with the WS_GROUP style. The first control in each group usually has the WS_TABSTOP style so that the user can move from group to group. The user can subsequently change the keyboard focus from one control in the group to the next control in the group by using the direction keys. You can turn this style on and off to change dialog box navigation. To change this style after a window has been created, use the SetWindowLong function.
        { "WS_HSCROLL", 0x00100000 },                   // The window has a horizontal scroll bar.
        { "WS_ICONIC", 0x20000000 },                    // The window is initially minimized. Same as the WS_MINIMIZE style.
        { "WS_MAXIMIZE", 0x01000000 },                  // The window is initially maximized.
        { "WS_MAXIMIZEBOX", 0x00010000 },               // The window has a maximize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.
        { "WS_MINIMIZE", 0x20000000 },                  // The window is initially minimized. Same as the WS_ICONIC style.
        { "WS_MINIMIZEBOX", 0x00020000 },               // The window has a minimize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.
        { "WS_OVERLAPPED", 0x00000000 },                // The window is an overlapped window. An overlapped window has a title bar and a border. Same as the WS_TILED style.
        //{ "WS_OVERLAPPEDWINDOW", 0x00CF0000 },          // The window is an overlapped window. Same as the WS_TILEDWINDOW style.
        { "WS_POPUP", 0x80000000 },                     // The window is a pop-up window. This style cannot be used with the WS_CHILD style.
        //{ "WS_POPUPWINDOW", 0x80880000 },               // The window is a pop-up window. The WS_CAPTION and WS_POPUPWINDOW styles must be combined to make the window menu visible.
        { "WS_SIZEBOX", 0x00040000 },                   // The window has a sizing border. Same as the WS_THICKFRAME style.
        { "WS_SYSMENU", 0x00080000 },                   // The window has a window menu on its title bar. The WS_CAPTION style must also be specified.
        { "WS_TABSTOP", 0x00010000 },                   // The window is a control that can receive the keyboard focus when the user presses the TAB key. Pressing the TAB key changes the keyboard focus to the next control with the WS_TABSTOP style. You can turn this style on and off to change dialog box navigation. To change this style after a window has been created, use the SetWindowLong function. For user-created windows and modeless dialogs to work with tab stops, alter the message loop to call the IsDialogMessage function.
        { "WS_THICKFRAME", 0x00040000 },                // The window has a sizing border. Same as the WS_SIZEBOX style.
        { "WS_TILED", 0x00000000 },                     // The window is an overlapped window. An overlapped window has a title bar and a border. Same as the WS_OVERLAPPED style.
        //{ "WS_TILEDWINDOW", 0x00CF0000 },               // The window is an overlapped window. Same as the WS_OVERLAPPEDWINDOW style.
        { "WS_VISIBLE", 0x10000000 },                   // The window is initially visible. This style can be turned on and off by using the ShowWindow or SetWindowPos function.
        { "WS_VSCROLL", 0x00200000 }                    // The window has a vertical scroll bar.
    };

    public static readonly Dictionary<string, uint> ExtendedWindowStyles = new()
    {
        { "WS_EX_ACCEPTFILES", 0x00000010 },            // The window accepts drag-drop files.
        { "WS_EX_APPWINDOW", 0x00040000 },              // Forces a top-level window onto the taskbar when the window is visible.
        { "WS_EX_CLIENTEDGE", 0x00000200 },             // The window has a border with a sunken edge.
        { "WS_EX_COMPOSITED", 0x02000000 },             // Paints all descendants of a window in bottom-to-top painting order using double-buffering. Bottom-to-top painting order allows a descendent window to have translucency (alpha) and transparency (color-key) effects, but only if the descendent window also has the WS_EX_TRANSPARENT bit set. Double-buffering allows the window and its descendents to be painted without flicker. This cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC. Windows 2000: This style is not supported.
        { "WS_EX_CONTEXTHELP", 0x00000400 },            // The title bar of the window includes a question mark. When the user clicks the question mark, the cursor changes to a question mark with a pointer. If the user then clicks a child window, the child receives a WM_HELP message. The child window should pass the message to the parent window procedure, which should call the WinHelp function using the HELP_WM_HELP command. The Help application displays a pop-up window that typically contains help for the child window. WS_EX_CONTEXTHELP cannot be used with the WS_MAXIMIZEBOX or WS_MINIMIZEBOX styles.
        { "WS_EX_CONTROLPARENT", 0x00010000 },          // The window itself contains child windows that should take part in dialog box navigation. If this style is specified, the dialog manager recurses into children of this window when performing navigation operations such as handling the TAB key, an arrow key, or a keyboard mnemonic.
        { "WS_EX_DLGMODALFRAME", 0x00000001 },          // The window has a double border; the window can, optionally, be created with a title bar by specifying the WS_CAPTION style in the dwStyle parameter.
        { "WS_EX_LAYERED", 0x00080000 },                // The window is a layered window. This style cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC. Windows 8: The WS_EX_LAYERED style is supported for top-level windows and child windows. Previous Windows versions support WS_EX_LAYERED only for top-level windows.
        { "WS_EX_LAYOUTRTL", 0x00400000 },              // If the shell language is Hebrew, Arabic, or another language that supports reading order alignment, the horizontal origin of the window is on the right edge. Increasing horizontal values advance to the left.
        { "WS_EX_LEFT", 0x00000000 },                   // The window has generic left-aligned properties. This is the default.
        { "WS_EX_LEFTSCROLLBAR", 0x00004000 },          // If the shell language is Hebrew, Arabic, or another language that supports reading order alignment, the vertical scroll bar (if present) is to the left of the client area. For other languages, the style is ignored.
        { "WS_EX_LTRREADING", 0x00004000 },             // The window text is displayed using left-to-right reading-order properties. This is the default.
        { "WS_EX_MDICHILD", 0x00000040 },               // The window is a MDI child window.
        { "WS_EX_NOACTIVATE", 0x08000000 },             // A top-level window created with this style does not become the foreground window when the user clicks it. The system does not bring this window to the foreground when the user minimizes or closes the foreground window. The window should not be activated through programmatic access or via keyboard navigation by accessible technology, such as Narrator. To activate the window, use the SetActiveWindow or SetForegroundWindow function. The window does not appear on the taskbar by default. To force the window to appear on the taskbar, use the WS_EX_APPWINDOW style.
        { "WS_EX_NOINHERITLAYOUT", 0x00100000 },        // The window does not pass its window layout to its child windows.
        { "WS_EX_NOPARENTNOTIFY", 0x00000004 },         // The child window created with this style does not send the WM_PARENTNOTIFY message to its parent window when it is created or destroyed.
        { "WS_EX_NOREDIRECTIONBITMAP", 0x00200000 },    // The window does not render to a redirection surface. This is for windows that do not have visible content or that use mechanisms other than surfaces to provide their visual.
        //{ "WS_EX_OVERLAPPEDWINDOW", 0x00000300 },       // The window is an overlapped window.
        //{ "WS_EX_PALETTEWINDOW", 0x00000188 },          // The window is palette window, which is a modeless dialog box that presents an array of commands.
        { "WS_EX_RIGHT", 0x00001000 },                  // The window has generic "right-aligned" properties. This depends on the window class. This style has an effect only if the shell language is Hebrew, Arabic, or another language that supports reading-order alignment; otherwise, the style is ignored. Using the WS_EX_RIGHT style for static or edit controls has the same effect as using the SS_RIGHT or ES_RIGHT style, respectively.Using this style with button controls has the same effect as using BS_RIGHT and BS_RIGHTBUTTON styles.
        { "WS_EX_RIGHTSCROLLBAR", 0x00000000 },         // The vertical scroll bar (if present) is to the right of the client area. This is the default.
        { "WS_EX_RTLREADING", 0x00002000 },             // If the shell language is Hebrew, Arabic, or another language that supports reading-order alignment, the window text is displayed using right-to-left reading-order properties. For other languages, the style is ignored.
        { "WS_EX_STATICEDGE", 0x00020000 },             // The window has a three-dimensional border style intended to be used for items that do not accept user input.
        { "WS_EX_TOOLWINDOW", 0x00000080 },             // The window is intended to be used as a floating toolbar. A tool window has a title bar that is shorter than a normal title bar, and the window title is drawn using a smaller font. A tool window does not appear in the taskbar or in the dialog that appears when the user presses ALT+TAB. If a tool window has a system menu, its icon is not displayed on the title bar. However, you can display the system menu by right-clicking or by typing ALT+SPACE.
        { "WS_EX_TOPMOST", 0x00000008 },                // The window should be placed above all non-topmost windows and should stay above them, even when the window is deactivated. To add or remove this style, use the SetWindowPos function.
        { "WS_EX_TRANSPARENT", 0x00000020 },            // The window should not be painted until siblings beneath the window (that were created by the same thread) have been painted. The window appears transparent because the bits of underlying sibling windows have already been painted. To achieve transparency without these restrictions, use the SetWindowRgn function.
        { "WS_EX_WINDOWEDGE", 0x00000100 }              // The window has a border with a raised edge.
    };
}