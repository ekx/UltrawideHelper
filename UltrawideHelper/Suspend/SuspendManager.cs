using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace UltrawideHelper.Suspend;

public class SuspendManager
{
    // ReSharper disable once InconsistentNaming
    private const uint PROCESS_ALL_ACCESS = 0xFFFF;
    
    [DllImport("ntdll.dll", EntryPoint = "NtSuspendProcess", SetLastError = true, ExactSpelling = false)]
    private static extern UIntPtr NtSuspendProcess(UIntPtr processHandle);

    [DllImport("ntdll.dll", EntryPoint = "NtResumeProcess", SetLastError = true, ExactSpelling = false)]
    private static extern UIntPtr NtResumeProcess(UIntPtr processHandle);
    
    [DllImport("kernel32.dll")]
    private static extern UIntPtr OpenProcess(UIntPtr dwDesiredAccess, bool bInheritHandle, UIntPtr dwProcessId);
    
    [DllImport("kernel32.dll")]
    private static extern bool CloseHandle(UIntPtr hObject);

    private readonly Dictionary<UIntPtr, UIntPtr> SuspendedProcesses = new();

    private void SuspendProcess(UIntPtr processId)
    {
        if (SuspendedProcesses.ContainsKey(processId))
            return;
        
        var processHandle = OpenProcess(new UIntPtr(PROCESS_ALL_ACCESS), false, new UIntPtr((uint) processId));
        NtSuspendProcess(processHandle);
        SuspendedProcesses.Add(processId, processHandle);
    }

    private void ResumeProcess(UIntPtr processId)
    {
        if (!SuspendedProcesses.TryGetValue(processId, out var processHandle))
            return;

        NtResumeProcess(processHandle);
        CloseHandle(processHandle);
        SuspendedProcesses.Remove(processId);
    }

    public void ToggleProcessSuspended(UIntPtr processId)
    {
        if (SuspendedProcesses.ContainsKey(processId))
            ResumeProcess(processId);
        else
            SuspendProcess(processId);
    }
}