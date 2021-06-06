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
    /// Interaction logic for QuanLyTaiVu_View.xaml
    /// </summary>
    public partial class QuanLyTaiVu_View : UserControl
    {
        public QuanLyTaiVu_View()
        {
            InitializeComponent();
            ShowUserControl(Home);
        }

        private void ShowListStaff(object sender, RoutedEventArgs e)
        {
            ShowUserControl(ListInfomation);

        }

        private void ShowHomePage(Object sender, RoutedEventArgs e)
        {
            ShowUserControl(Home);
        }


        public void ShowUserControl(UserControl uc)
        {
            Home.Visibility = Visibility.Collapsed;
            ListInfomation.Visibility = Visibility.Collapsed;

            uc.Visibility = Visibility.Visible;
        }
    }
}
