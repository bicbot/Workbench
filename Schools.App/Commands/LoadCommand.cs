using System.Linq;
using Schools.Services.Infrastructure;
using Schools.Services.Providers;
using SchoolsApp.Infrastructure;
using SchoolsApp.ViewModels;

namespace SchoolsApp.Commands
{
    public class LoadCommand : CommandBase
    {
        private readonly ShellViewModel shellViewModel;

        public LoadCommand(ShellViewModel shellViewModel)
        {
            this.shellViewModel = shellViewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(this.shellViewModel.MasterFilePath)
                   && !string.IsNullOrEmpty(this.shellViewModel.DetailFilePath);
        }

        public override void Execute(object parameter)
        {
            using (new WaitCursor())
            {
                // Load Master
                this.LoadMasterFile();

                // Load details
                this.LoadDetailFile();
            }
        }

        private void LoadDetailFile()
        {
            var provider = new CsvProvider();
            var schools = provider.Read(this.shellViewModel.DetailFilePath);

            //var distinct = (from s in schools
            //                select s).DistinctBy(s => s.SchoolFullName);

            //this.shellViewModel.Schools.Clear();

            //foreach (var school in distinct)
            //{
            //    this.shellViewModel.Schools.Add(new SchoolViewModel(school));
            //}
        }

        private void LoadMasterFile()
        {
            var provider = new ExcelProvider();
            var sheet = this.shellViewModel.Sheets.SelectedItem;

            if (sheet != null)
            {
                var schools = provider.Read(this.shellViewModel.MasterFilePath, sheet.Name);

                var distinct = (from s in schools
                                orderby s.SchoolFullName
                                select s).DistinctBy(s => s.SchoolFullName);

                this.shellViewModel.Schools.Clear();
                this.shellViewModel.FilteredSchools.Clear();

                foreach (var school in distinct)
                {
                    var viewModel = new SchoolViewModel(school);
                    this.shellViewModel.Schools.Add(viewModel);
                    this.shellViewModel.FilteredSchools.Add(viewModel);
                }
            }

            this.shellViewModel.ApplyFilter();
        }
    }
}
