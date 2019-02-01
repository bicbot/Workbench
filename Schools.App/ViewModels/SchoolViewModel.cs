using Schools.Services.Models;
using SchoolsApp.Infrastructure;

namespace SchoolsApp.ViewModels
{
    public class SchoolViewModel : ObservableObject, ISelectable
    {
        public SchoolViewModel(School model)
        {
            this.Model = model;
        }

        public string Uid
        {
            get => this.Model.Uid;
            set
            {
                this.Model.Uid = value;
                this.OnPropertyChanged();
            }
        }

        public string SchoolFullName
        {
            get => this.Model.SchoolFullName;
            set
            {
                this.Model.SchoolFullName = value;
                this.OnPropertyChanged();
            }
        }

        public School Model { get; }
    }
}
