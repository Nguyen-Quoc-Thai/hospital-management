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
    /// Interaction logic for Sys.xaml
    /// </summary>
    public partial class Sys : UserControl
    {
        MainWindow main = (MainWindow)Application.Current.MainWindow;
        public Sys()
        {
            InitializeComponent();
        }

        public void Logout_Click(object sender, RoutedEventArgs e)
        {
            OracleDatabase instance = OracleDatabase.ResetInstance;
            main.SetWindownActive(main.View_Login);
        }

        public void ShowUserControl(UserControl uc)
        {
            Audit.Visibility = Visibility.Collapsed;

            uc.Visibility = Visibility.Visible;
        }

        private void TabSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = sender as TabControl;
            var selected = item.SelectedItem as TabItem;
            if (selected.Header.ToString() == "Audit logs")
                ShowUserControl(Audit);
        }
    }
}
