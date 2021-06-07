using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Oracle.ManagedDataAccess.Client;
using PH2_QTCSDL;
using PH2_QTCSDL.Command;
using PH2_QTCSDL.ViewModels;

namespace PH2_QTCSDL.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        //private static OracleDatabase instance = null;
        MainWindow main = (MainWindow)Application.Current.MainWindow;

        public LoginView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            main.SetWindownActive(main.View_TaiNguyen_NhanSu);
            //main.SetWindownActive(main.View_QuanLyTaiVu);
        }
    }
}
