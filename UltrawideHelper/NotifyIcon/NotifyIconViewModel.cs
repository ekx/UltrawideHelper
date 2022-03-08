using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using UltrawideHelper.Configuration;

namespace UltrawideHelper.NotifyIcon;

/// <summary>
/// Provides bindable properties and commands for the NotifyIcon. In this sample, the
/// view model is assigned to the NotifyIcon in XAML. Alternatively, the startup routing
/// in App.xaml.cs could have created this view model, and assigned it to the NotifyIcon.
/// </summary>
public class NotifyIconViewModel
{
    public ICommand ShowConfigurationFile
    {
        get
        {
            return new DelegateCommand 
            { 
                CanExecuteFunc = () => true,
                CommandAction = () =>
                {
                    new Process
                    {
                        StartInfo = new ProcessStartInfo(ConfigurationManager.FilePath)
                        {
                            UseShellExecute = true
                        }
                    }.Start();
                } 
            };
        }
    }

    /// <summary>
    /// Shuts down the application.
    /// </summary>
    public ICommand ExitApplicationCommand
    {
        get
        {
            return new DelegateCommand
            {
                CanExecuteFunc = () => true,
                CommandAction = () => Application.Current.Shutdown()
            };
        }
    }
}