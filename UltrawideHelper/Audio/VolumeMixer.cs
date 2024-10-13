using System;
using System.Runtime.InteropServices;

namespace UltrawideHelper.Audio;

internal static class VolumeMixer
{
    public static void SetApplicationMute(uint pid, bool mute)
    {
        var volume = GetVolumeObject(pid);
        if (volume == null) return;

        var guid = Guid.Empty;
        volume.SetMute(mute, ref guid);
        
        Marshal.ReleaseComObject(volume);
    }

    public static void ToggleApplicationMute(uint pid)
    {
        var volume = GetVolumeObject(pid);
        if (volume == null) return;
        
        var guid = Guid.Empty;
        volume.GetMute(out var mute);
        volume.SetMute(!mute, ref guid);
        
        Marshal.ReleaseComObject(volume);
    }

    private static ISimpleAudioVolume GetVolumeObject(uint targetPid)
    {
        var deviceEnumerator = (IMmDeviceEnumerator) new MMDeviceEnumerator();
        deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.ERender, ERole.EMultimedia, out var speakers);

        var audioSessionManagerGuid = typeof(IAudioSessionManager2).GUID;
        speakers.Activate(ref audioSessionManagerGuid, 0, IntPtr.Zero, out var o);
        var audioSessionManager = (IAudioSessionManager2) o;

        audioSessionManager.GetSessionEnumerator(out var sessionEnumerator);
        sessionEnumerator.GetCount(out var sessionCount);

        ISimpleAudioVolume volumeControl = null;
        for (var i = 0; i < sessionCount; i++)
        {
            sessionEnumerator.GetSession(i, out var audioSessionControl);
            audioSessionControl.GetProcessId(out var currentPid);

            if (currentPid == targetPid)
            {
                volumeControl = audioSessionControl as ISimpleAudioVolume;
                break;
            }

            Marshal.ReleaseComObject(audioSessionControl);
        }

        Marshal.ReleaseComObject(sessionEnumerator);
        Marshal.ReleaseComObject(audioSessionManager);
        Marshal.ReleaseComObject(speakers);
        Marshal.ReleaseComObject(deviceEnumerator);
        return volumeControl;
    }
}

[ComImport]
[Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
internal class MMDeviceEnumerator
{
}

internal enum EDataFlow
{
    ERender,
    ECapture,
    EAll,
    EDataFlowEnumCount
}

internal enum ERole
{
    EConsole,
    EMultimedia,
    ECommunications,
    ERoleEnumCount
}

[Guid("A95664D2-9614-4F35-A746-DE8DB63617E6")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IMmDeviceEnumerator
{
    int NotImpl1();

    [PreserveSig]
    int GetDefaultAudioEndpoint(EDataFlow dataFlow, ERole role, out IMmDevice ppDevice);

    // the rest is not implemented
}

[Guid("D666063F-1587-4E43-81F1-B948E807363F")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IMmDevice
{
    [PreserveSig]
    int Activate(ref Guid iid, int dwClsCtx, IntPtr pActivationParams,
        [MarshalAs(UnmanagedType.IUnknown)] out object ppInterface);

    // the rest is not implemented
}

[Guid("77AA99A0-1BD6-484F-8BC7-2C654C9A9B6F")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IAudioSessionManager2
{
    int NotImpl1();
    int NotImpl2();

    [PreserveSig]
    int GetSessionEnumerator(out IAudioSessionEnumerator sessionEnum);

    // the rest is not implemented
}

[Guid("E2F5BB11-0570-40CA-ACDD-3AA01277DEE8")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IAudioSessionEnumerator
{
    [PreserveSig]
    int GetCount(out int sessionCount);

    [PreserveSig]
    int GetSession(int sessionCount, out IAudioSessionControl2 session);
}

[Guid("87CE5498-68D6-44E5-9215-6DA47EF883D8")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface ISimpleAudioVolume
{
    [PreserveSig]
    int SetMasterVolume(float fLevel, ref Guid eventContext);

    [PreserveSig]
    int GetMasterVolume(out float pfLevel);

    [PreserveSig]
    int SetMute(bool bMute, ref Guid eventContext);

    [PreserveSig]
    int GetMute(out bool pbMute);
}

[Guid("bfb7ff88-7239-4fc9-8fa2-07c950be9c6d")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IAudioSessionControl2
{
    // IAudioSessionControl
    [PreserveSig]
    int NotImpl0();

    [PreserveSig]
    int GetDisplayName([MarshalAs(UnmanagedType.LPWStr)] out string pRetVal);

    [PreserveSig]
    int SetDisplayName([MarshalAs(UnmanagedType.LPWStr)] string value,
        [MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

    [PreserveSig]
    int GetIconPath([MarshalAs(UnmanagedType.LPWStr)] out string pRetVal);

    [PreserveSig]
    int SetIconPath([MarshalAs(UnmanagedType.LPWStr)] string value,
        [MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

    [PreserveSig]
    int GetGroupingParam(out Guid pRetVal);

    [PreserveSig]
    int SetGroupingParam([MarshalAs(UnmanagedType.LPStruct)] Guid Override,
        [MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

    [PreserveSig]
    int NotImpl1();

    [PreserveSig]
    int NotImpl2();

    // IAudioSessionControl2
    [PreserveSig]
    int GetSessionIdentifier([MarshalAs(UnmanagedType.LPWStr)] out string pRetVal);

    [PreserveSig]
    int GetSessionInstanceIdentifier([MarshalAs(UnmanagedType.LPWStr)] out string pRetVal);

    [PreserveSig]
    int GetProcessId(out int pRetVal);

    [PreserveSig]
    int IsSystemSoundsSession();

    [PreserveSig]
    int SetDuckingPreference(bool optOut);
}