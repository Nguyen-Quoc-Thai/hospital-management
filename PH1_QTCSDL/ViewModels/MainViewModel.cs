using PH1_QTCSDL.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PH1_QTCSDL.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        private BaseViewModel _currentViewModel = new HomeWindowModel();
        public ICommand UpdateViewCommand { get; set; }

        public MainViewModel()
        {
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

