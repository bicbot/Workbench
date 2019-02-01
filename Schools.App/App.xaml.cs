using System;
using System.Windows;
using System.Windows.Threading;

namespace SchoolsApp
{
    public partial class App : Application
    {
        public App()
        {
            Current.DispatcherUnhandledException += this.AppDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += this.AppDomainUnhandledException;
        }

        private void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            this.LogException(e.Exception);

            e.Handled = true;
        }

        private void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            this.LogException(ex);
        }

        private void LogException(Exception e)
        {
            // Log both the root exception and the full wrapped exception
            MessageBox.Show($"Exception: {e.GetBaseException()}");
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var shell = new Shell();

            shell.Show();
        }
    }
}
