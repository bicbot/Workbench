using Schools.Services.Providers;
using SchoolsApp.Infrastructure;
using SchoolsApp.ViewModels;

namespace SchoolsApp.Commands
{
    public class GetSchemaCommand : CommandBase
    {
        private readonly ShellViewModel shellViewModel;

        public GetSchemaCommand(ShellViewModel shellViewModel)
        {
            this.shellViewModel = shellViewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(this.shellViewModel.MasterFilePath);
        }

        public override void Execute(object parameter)
        {
            var provider = new ExcelProvider();
            var sheets = provider.ReadSchema(this.shellViewModel.MasterFilePath);

            this.shellViewModel.Sheets.Clear();

            foreach (var sheet in sheets)
            {
                this.shellViewModel.Sheets.Add(new SheetViewModel(sheet));
            }

            if (!string.IsNullOrEmpty(this.shellViewModel.SelectedSheetName))
            {
                foreach (var sheet in this.shellViewModel.Sheets)
                {
                    if (sheet.Name == this.shellViewModel.SelectedSheetName)
                    {
                        this.shellViewModel.Sheets.SelectedItem = sheet;
                    }
                }
            }

            this.shellViewModel.Sheets.EnsureSelected();
        }
    }
}
