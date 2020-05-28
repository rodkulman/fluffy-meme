using System;
using System.Windows.Input;

namespace BrowserChoice
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action executeAction;
        private Func<bool> canExecuteAction;

        public RelayCommand(Action execute) : this(execute, null) { }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }            

            this.executeAction = execute;
            this.canExecuteAction = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecuteAction?.Invoke() ?? true;
        }

        public void Execute(object parameter)
        {
            executeAction();
        }
    }

    public class RelayCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action<T> executeAction;
        private Func<T, bool> canExecuteAction;

        public RelayCommand(Action<T> execute) : this(execute, null) { }

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }            

            this.executeAction = execute;
            this.canExecuteAction = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecuteAction?.Invoke((T)parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            executeAction((T)parameter);
        }
    }
}