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
                case "ListUser":
                    _viewModel.CurrentViewModel = new ListUserViewModel();
                    break;
                case "Home":
                    _viewModel.CurrentViewModel = new HomeWindowModel();
                    break;
                case "Privilges":
                    _viewModel.CurrentViewModel = new PrivilgesViewModel();
                    break;
                case "ListUserGrant":
                    _viewModel.CurrentViewModel = new GrantOnUserViewModel();
                    break;
                case "ListRoleGrant":
                    _viewModel.CurrentViewModel = new GrantOnRoleViewModel();
                    break;
                case "ListRole":
                    _viewModel.CurrentViewModel = new ListRoleViewModel();
                    break;
                case "Login":
                    _viewModel.CurrentViewModel = new ListRoleViewModel();
                    break;
            }
        }
    }
}
