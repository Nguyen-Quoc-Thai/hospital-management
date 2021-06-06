using PH2_QTCSDL.ViewModels;
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
    /// Interaction logic for LoginFormView.xaml
    /// </summary>
    public partial class LoginFormView : UserControl
    {
        MainWindow main = (MainWindow)Application.Current.MainWindow;
        public LoginFormView()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string connStr = "TNS_ADMIN=C:\\Users\\HP\\Oracle\\network\\admin;USER ID=" + txtUsername.Text + ";PASSWORD=" + txtPassword.Password + ";DATA SOURCE=localhost:1521/orcl;PERSIST SECURITY INFO=True";

            try
            {
                OracleDatabase.connStr = connStr;
                OracleDatabase instance = OracleDatabase.Instance;
                //MessageBox.Show("Login successfully!");

                //goi oracle redirect
                main.SetWindownActive(main.View_BacSi);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
    }
}
