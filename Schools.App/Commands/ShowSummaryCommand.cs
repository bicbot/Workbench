using System.IO;
using SchoolsApp.Infrastructure;
using SchoolsApp.ViewModels;

namespace SchoolsApp.Commands
{
    public class ShowSummaryCommand : CommandBase
    {
        private readonly ShellViewModel shellViewModel;

        public ShowSummaryCommand(ShellViewModel shellViewModel)
        {
            this.shellViewModel = shellViewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            var filePath = new FileInfo(@"Resources\Summarytemplate.html").FullName;

            this.shellViewModel.Browser?.Navigate(filePath);
        }
    }
}
