

using System;
using System.Windows.Input;


namespace Index_Prototype
{
    internal class RelayCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged = (sender, e) => { };
        private Action<T> mAction;
        public bool CanExecute(object parameter) => true;
        public RelayCommand(Action<T> action)
        {
            mAction = action ;
        }

        public void Execute(object parameter)
        {
            mAction((T)parameter);
        }
    }
}
