using System;

namespace SchoolsApp.Infrastructure
{
    [Serializable]
    public class ObservableObject : ViewModelBase, ISelectable
    {
        private bool isChecked;
        private bool isExpanded;
        private bool isSelected;

        public bool IsChecked
        {
            get => this.isChecked;
            set
            {
                if (this.isChecked != value)
                {
                    this.isChecked = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public bool IsExpanded
        {
            get => this.isExpanded;
            set
            {
                if (this.isExpanded != value)
                {
                    this.isExpanded = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public virtual bool IsSelected
        {
            get => this.isSelected;
            set
            {
                if (this.isSelected != value)
                {
                    this.isSelected = value;
                    this.OnPropertyChanged();
                }
            }
        }
    }
}
