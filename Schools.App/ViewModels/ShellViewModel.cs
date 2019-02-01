using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using SchoolsApp.Commands;
using SchoolsApp.Infrastructure;
using SchoolsApp.Infrastructure.Extensions;
using SchoolsApp.Properties;

namespace SchoolsApp.ViewModels
{
    public class ShellViewModel : ObservableObject
    {
        private string detailFilePath;
        private string masterFilePath;
        private string searchTerm;

        public ShellViewModel()
        {
            // Commands
            this.OpenMasterFile = new OpenMasterFileCommand(this);
            this.OpenDetailFile = new OpenDetailFileCommand(this);
            this.RegisterWebBrowserControl = new RegisterWebBrowserControlCommand(this);
            this.Load = new LoadCommand(this);
            this.GetSchema = new GetSchemaCommand(this);
            this.ShowSummary  = new ShowSummaryCommand(this);

            // ViewModels
            this.Schools = new SchoolCollection();
            this.FilteredSchools = new SchoolCollection();
            this.Sheets = new SheetCollection();

            // Settings
            this.MasterFilePath = Settings.Default.MasterFilePath;
            this.DetailFilePath = Settings.Default.DetailFilePath;
            this.SelectedSheetName = Settings.Default.SelectedSheetName;

            // Events
            this.Sheets.SelectedItemChanged += this.Sheets_SelectedItemChanged;
            this.FilteredSchools.SelectedItemChanged += this.FilteredSchools_SelectedItemChanged;
        }

        public ShowSummaryCommand ShowSummary { get; }

        private void FilteredSchools_SelectedItemChanged(object sender, EventArgs e)
        {
            this.ShowSummary.Execute(null);
        }

        public string SelectedSheetName { get; set; }
        public GetSchemaCommand GetSchema { get; }
        public LoadCommand Load { get; }
        public OpenDetailFileCommand OpenDetailFile { get; }
        public OpenMasterFileCommand OpenMasterFile { get; }
        public ICommand RegisterWebBrowserControl { get; }

        public string MasterFilePath
        {
            get => this.masterFilePath;
            set
            {
                this.masterFilePath = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged("MasterFilePathExists");
            }
        }

        public string DetailFilePath
        {
            get => this.detailFilePath;
            set
            {
                this.detailFilePath = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged("DetailFilePathExists");
            }
        }

        public bool MasterFilePathExists => File.Exists(this.MasterFilePath);
        public bool DetailFilePathExists => File.Exists(this.DetailFilePath);
        public SchoolCollection Schools { get; }
        public SheetCollection Sheets { get; }
        public SchoolCollection FilteredSchools { get; }

        public string SearchTerm
        {
            get => this.searchTerm;
            set
            {
                this.searchTerm = value;
                this.ApplyFilter();
                this.OnPropertyChanged();
            }
        }

        public string SummaryPath => new FileInfo(@"Resources\SummaryTemplate.html").FullName;
        public WebBrowser Browser { get; set; }

        private void Sheets_SelectedItemChanged(object sender, EventArgs e)
        {
            this.SelectedSheetName = this.Sheets.SelectedItem?.Name;
            Settings.Default.SelectedSheetName = this.SelectedSheetName;
            Settings.Default.Save();
        }

        public void ApplyFilter()
        {
            this.FilteredSchools.Clear();
            if (string.IsNullOrEmpty(this.SearchTerm))
            {
                foreach (var school in this.Schools)
                {
                    this.FilteredSchools.Add(school);
                }
                return;
            }

            foreach (var school in this.Schools)
            {
                if (school.SchoolFullName.Contains(this.SearchTerm, StringComparison.InvariantCultureIgnoreCase)
                || school.Uid.Contains(this.SearchTerm, StringComparison.InvariantCultureIgnoreCase))
                {
                    this.FilteredSchools.Add(school);
                }
            }
        }

        public void Refresh()
        {
            if (this.GetSchema.CanExecute(null))
            {
                this.GetSchema.Execute(null);
            }
        }
    }
}
