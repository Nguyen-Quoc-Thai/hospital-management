using PH2_QTCSDL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PH2_QTCSDL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //DataContext = new MainViewModel();
            SetWindownActive(View_Login);
        }


        public void SetWindownActive(UserControl uc)
        {
            View_Home.Visibility = Visibility.Collapsed;
            View_BacSi.Visibility = Visibility.Collapsed;
            View_QuanLyTaiVu.Visibility = Visibility.Collapsed;
            View_TaiNguyen_NhanSu.Visibility = Visibility.Collapsed;
            View_TiepTan.Visibility = Visibility.Collapsed;
            View_Login.Visibility = Visibility.Collapsed;
            View_Sys.Visibility = Visibility.Collapsed;

            uc.Visibility = Visibility.Visible;
        }
    }
}
