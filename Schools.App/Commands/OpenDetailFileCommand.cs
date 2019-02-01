using System.Windows;
using Microsoft.Win32;
using SchoolsApp.Infrastructure;
using SchoolsApp.Properties;
using SchoolsApp.ViewModels;

namespace SchoolsApp.Commands
{
    public class OpenDetailFileCommand : CommandBase
    {
        private readonly ShellViewModel shellViewModel;

        public OpenDetailFileCommand(ShellViewModel shellViewModel)
        {
            this.shellViewModel = shellViewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            var dialog = new OpenFileDialog
            {
                DefaultExt = "csv",
                Filter = "Comma Seperater Values|*.csv|All Files|*.*",
                FileName = Settings.Default.DetailFilePath
            };

            var result = dialog.ShowDialog(Application.Current.MainWindow);

            if (!result.Value)
            {
                return;
            }

            using (new WaitCursor())
            {
                var fileName = dialog.FileName;
                this.shellViewModel.DetailFilePath = fileName;
                Settings.Default.DetailFilePath = fileName;
                Settings.Default.Save();
            }
        }
    }
}
