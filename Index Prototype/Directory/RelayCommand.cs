

using System;
using System.Windows.Input;


namespace Index_Prototype
{
    internal class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged = (sender, e) => { };
        private Action mAction;
        public bool CanExecute(object parameter) => true;
        public RelayCommand(Action action)
        {
            mAction = action;
        }

        public void Execute(object parameter)
        {
            mAction();
        }
    }
}
