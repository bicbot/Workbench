namespace SchoolsApp.Infrastructure
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    [Serializable]
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        public virtual void Dispose()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Child classes can override this method to perform
        ///     clean-up logic, such as removing event handlers.
        /// </summary>
        protected virtual void OnDispose()
        {
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
#if (DEBUG)
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                var message = $"PropertyName not found [{propertyName}]";
            }

#endif

            var changed = this.PropertyChanged;
            changed?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}