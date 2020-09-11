namespace SpaceLogistic.WpfView.Utility
{
    using System;
    using System.Windows.Input;

    public sealed class DelegateCommand : DelegateCommand<object>
    {
        public DelegateCommand(Action execute)
            : base(_ => execute(), _ => true)
        {
        }

        public DelegateCommand(Action execute, Func<bool> canExecute)
            : base(_ => execute(), _ => canExecute())
        {
        }
    }

    public class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> execute;

        private readonly Func<T, bool> canExecute;

        public DelegateCommand(Action<T> execute)
            : this(execute, _ => true)
        {
        }

        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return this.canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            this.execute((T)parameter);
        }
    }
}
