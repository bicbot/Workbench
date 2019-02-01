using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;

namespace SchoolsApp.Infrastructure
{
    public class EntityObservableCollection<T> : ObservableCollection<T>, IObservableCollection<T>
        where T : class, ISelectable
    {
        private bool isDirty;
        private bool isEditing;
        private bool isLoading;
        private T selectedItem;

        [Obfuscation]
        public bool IsDirty
        {
            get => this.isDirty;
            set
            {
                if (!this.IsLoading)
                {
                    this.isDirty = value;
                    this.OnPropertyChanged("IsDirty");
                }
            }
        }

        [Obfuscation]
        public bool IsEditing
        {
            get => this.isEditing;
            set
            {
                this.isEditing = value;
                this.OnPropertyChanged("IsEditing");
            }
        }

        [Obfuscation]
        public bool IsLoading
        {
            get => this.isLoading;
            set
            {
                this.isLoading = value;
                this.OnPropertyChanged("IsLoading");
            }
        }

        [Obfuscation]
        public virtual T SelectedItem
        {
            get
            {
                this.EnsureSelected();

                return this.selectedItem;
            }
            set
            {
                if (this.selectedItem != null)
                {
                    this.selectedItem.IsSelected = false;
                }

                this.selectedItem = value;

                if (this.selectedItem != null)
                {
                    this.selectedItem.IsSelected = true;
                }

                this.OnPropertyChanged(new PropertyChangedEventArgs("SelectedItem"));
                this.OnSelectedItemChanged();
            }
        }

        public void BeginEdit()
        {
            this.IsEditing = true;
        }

        public void BeginLoading()
        {
            this.IsLoading = true;
        }

        public void EndEdit()
        {
            this.IsEditing = false;
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void EndLoading()
        {
            this.IsLoading = false;
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            this.IsDirty = true;
            if (this.IsEditing || this.IsLoading)
            {
                return;
            }

            base.OnCollectionChanged(e);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        private void OnSelectedItemChanged()
        {
            var handler = this.SelectedItemChanged;
            if (handler != null)
            {
                handler(this, null);
            }
        }

        public void RefreshSelection()
        {
            base.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected void SetSelectedItem(T item)
        {
            this.selectedItem = item;
            this.selectedItem.IsSelected = true;
        }

        public event EventHandler SelectedItemChanged;

        public void EnsureSelected()
        {
            if (this.selectedItem == null && this.Items.Count > 0)
            {
                this.SelectedItem = this.Items[0];
            }
        }
    }
}
