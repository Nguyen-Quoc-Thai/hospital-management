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
using PH2_QTCSDL.ViewModels;

namespace PH2_QTCSDL.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        private static OracleDatabase instance = null;

        public LoginView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //new OracleDatabase("")
            string connStr = "TNS_ADMIN=C: \\Users\\Qouc Tahi\\Oracle\\network\\admin;USER ID=" + usernameBox.Text+ ";PASSWORD=" + passwordBox.Password + ";DATA SOURCE=localhost:1521/ORCLCDB.localdomain;PERSIST SECURITY INFO=True";

            try
            {
                OracleDatabase.connStr = connStr;
                OracleDatabase instance =  OracleDatabase.Instance;
                MessageBox.Show(connStr);

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
    }
}
