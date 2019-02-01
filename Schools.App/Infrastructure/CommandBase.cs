namespace SchoolsApp.Infrastructure
{
    using System;
    using System.Windows.Input;

    [Serializable]
    public abstract class CommandBase : ICommand
    {
        public abstract bool CanExecute(object parameter);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public abstract void Execute(object parameter);
    }
}