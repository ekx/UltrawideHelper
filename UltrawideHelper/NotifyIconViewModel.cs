using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using UltrawideHelper.Configuration;

namespace UltrawideHelper
{
    /// <summary>
    /// Provides bindable properties and commands for the NotifyIcon. In this sample, the
    /// view model is assigned to the NotifyIcon in XAML. Alternatively, the startup routing
    /// in App.xaml.cs could have created this view model, and assigned it to the NotifyIcon.
    /// </summary>
    public class NotifyIconViewModel
    {
        /// <summary>
        /// Shows a window, if none is already open.
        /// </summary>
        public ICommand ToggleWindowCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CanExecuteFunc = () => Application.Current.MainWindow != null,
                    CommandAction = () =>
                    {
                        if (Application.Current.MainWindow == null)
                        {
                            Application.Current.MainWindow = new MainWindow();
                        }

                        Application.Current.MainWindow.Visibility = Application.Current.MainWindow.IsVisible ? Visibility.Hidden : Visibility.Visible;
                    }
                };
            }
        }

        public ICommand ShowConfigurationFile
        {
            get
            {
                return new DelegateCommand 
                { 
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
                return new DelegateCommand { CommandAction = () => Application.Current.Shutdown() };
            }
        }
    }
}
