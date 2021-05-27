using System;
using System.Windows.Input;

namespace NHotPhrase.Wpf.Demo
{
    public class DelegateCommand : ICommand
    {
        public readonly Action _execute;
        public readonly Func<bool> _canExecute;

        public DelegateCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}