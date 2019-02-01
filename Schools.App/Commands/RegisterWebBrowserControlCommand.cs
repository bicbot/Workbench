using System.Windows.Controls;
using SchoolsApp.Infrastructure;
using SchoolsApp.ViewModels;

namespace SchoolsApp.Commands
{
    public class RegisterWebBrowserControlCommand : CommandBase
    {
        private readonly ShellViewModel shellViewModel;

        public RegisterWebBrowserControlCommand(ShellViewModel shellViewModel)
        {
            this.shellViewModel = shellViewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            this.shellViewModel.Browser = (WebBrowser)parameter;
        }
    }
}
