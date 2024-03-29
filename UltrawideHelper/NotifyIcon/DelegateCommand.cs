﻿using System;
using System.Windows.Input;

namespace UltrawideHelper.NotifyIcon;

/// <summary>
/// Simplistic delegate command for the demo.
/// </summary>
public class DelegateCommand : ICommand
{
    public Action CommandAction { get; init; }
    public Func<bool> CanExecuteFunc { get; init; }

    public void Execute(object parameter)
    {
        CommandAction();
    }

    public bool CanExecute(object parameter)
    {
        return CanExecuteFunc == null || CanExecuteFunc();
    }

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}