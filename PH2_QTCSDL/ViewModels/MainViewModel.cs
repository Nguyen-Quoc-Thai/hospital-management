using PH2_QTCSDL.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PH2_QTCSDL.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        private BaseViewModel _currentViewModel = null;
        public ICommand UpdateViewCommand { get; set; }

        public MainViewModel(BaseViewModel viewModel)
        {
            _currentViewModel = viewModel;
            UpdateViewCommand = new UpdateViewCommand(this);
        }


        public BaseViewModel CurrentViewModel
        {

            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }
    }
}

