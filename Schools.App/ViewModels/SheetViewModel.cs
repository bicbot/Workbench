using Schools.Services.Models;
using SchoolsApp.Infrastructure;

namespace SchoolsApp.ViewModels
{
    public class SheetViewModel : ObservableObject
    {
        private readonly Sheet model;

        public SheetViewModel(Sheet model)
        {
            this.model = model;
        }

        public string Name
        {
            get => this.model.Name;
            set
            {
                this.model.Name = value;
                this.OnPropertyChanged();
            }
        }

        public int Index
        {
            get => this.model.Index;
            set
            {
                this.model.Index = value;
                this.OnPropertyChanged();
            }
        }
    }
}
