using System.Windows;
using SchoolsApp.ViewModels;

namespace SchoolsApp
{
    public partial class Shell : Window
    {
        private readonly ShellViewModel shellViewModel;

        public Shell()
        {
            this.InitializeComponent();

            this.shellViewModel = new ShellViewModel();
            this.DataContext = this.shellViewModel;
        }

        private void Shell_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.shellViewModel.Refresh();
        }
    }
}
