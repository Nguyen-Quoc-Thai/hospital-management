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

namespace PH2_QTCSDL.Views
{
    /// <summary>
    /// Interaction logic for TaiNguyen_NhanSu_View.xaml
    /// </summary>
    public partial class TaiNguyen_NhanSu_View : UserControl
    {
        public TaiNguyen_NhanSu_View()
        {
            InitializeComponent();
            ShowUserControl(Home);
        }

        private void ShowListStaff(object sender, RoutedEventArgs e)
        {
            ShowUserControl(ListStaff);

        }


        public void ShowUserControl(UserControl uc)
        {
            Home.Visibility = Visibility.Collapsed;
            ListStaff.Visibility = Visibility.Collapsed;

            uc.Visibility = Visibility.Visible;
        }
    }
}
