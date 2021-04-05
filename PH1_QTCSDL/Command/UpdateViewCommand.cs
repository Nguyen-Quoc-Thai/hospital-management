using PH1_QTCSDL.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PH1_QTCSDL.Command
{
    class UpdateViewCommand : ICommand
    {
        private MainViewModel _viewModel;
        public UpdateViewCommand(MainViewModel viewModel)
        {
            this._viewModel = viewModel;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            switch(parameter)
            {
                case "A":

                    break;
            }
        }
    }
}
