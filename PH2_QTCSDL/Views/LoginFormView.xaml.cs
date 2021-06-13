using PH2_QTCSDL.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
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
            //string connStr = "TNS_ADMIN=C: \\Users\\Qouc Tahi\\Oracle\\network\\admin;USER ID=" + txtUsername.Text + ";PASSWORD=" + txtPassword.Password +";DATA SOURCE=localhost:1521/orclpdb1.localdomain;PERSIST SECURITY INFO=True";

            if (txtUsername.Text == "sys" || txtUsername.Text == "SYS")
            {
                connStr = "DBA PRIVILEGE = SYSDBA;" + connStr;
            }

            OracleDatabase.connStr = connStr;
            OracleDatabase instance = OracleDatabase.Instance;
            try
            {   if (txtUsername.Text == "sys" || txtUsername.Text == "SYS")
                {
                    main.SetWindownActive(main.View_Sys);
                } else
                {
                    DataTable dt = instance.Query("select get_user_role from dual");
                    string role = dt.Rows[0]["GET_USER_ROLE"].ToString();
                    if (role == "BACSI")
                        main.SetWindownActive(main.View_BacSi);
                    else if (role == "QLTAINGUYENNHANSU")
                    {
                        //main.SetWindownActive(main.View_TaiNguyen_NhanSu);
                    }

                    else if (role == "TIEPTAN")
                        main.SetWindownActive(main.View_TiepTan);
                    else if (role == "TAIVU")
                        main.SetWindownActive(main.View_QuanLyTaiVu);
                    else
                    {
                        MessageBox.Show("Bạn không có quyền vào hệ thống");
                        instance = OracleDatabase.ResetInstance;
                    }
                }
                txtUsername.Clear();
                txtPassword.Clear();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                instance = OracleDatabase.ResetInstance;
            }
        }
    }
}
