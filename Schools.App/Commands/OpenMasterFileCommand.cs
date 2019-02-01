using System.Windows;
using Microsoft.Win32;
using SchoolsApp.Infrastructure;
using SchoolsApp.Properties;
using SchoolsApp.ViewModels;

namespace SchoolsApp.Commands
{
    public class OpenMasterFileCommand : CommandBase
    {
        private readonly ShellViewModel shellViewModel;

        public OpenMasterFileCommand(ShellViewModel shellViewModel)
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
                DefaultExt = "xlsx",
                Filter = "Excel|*.xlsx|All Files|*.*",
                FileName = Settings.Default.MasterFilePath
            };

            var result = dialog.ShowDialog(Application.Current.MainWindow);

            if (!result.Value)
            {
                return;
            }

            using (new WaitCursor())
            {
                var fileName = dialog.FileName;
                this.shellViewModel.MasterFilePath = fileName;
                Settings.Default.MasterFilePath = fileName;
                Settings.Default.Save();


                if (this.shellViewModel.GetSchema.CanExecute(null))
                {
                    this.shellViewModel.GetSchema.Execute(null);
                }
                
                Settings.Default.MasterFilePath = fileName;
                Settings.Default.Save();
            }
        }
    }
}
