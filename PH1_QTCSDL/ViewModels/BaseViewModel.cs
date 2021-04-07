﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PH1_QTCSDL
{
    class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //Need to biding Data
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
