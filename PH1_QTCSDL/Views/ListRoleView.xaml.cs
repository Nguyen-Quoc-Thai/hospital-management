using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

namespace PH1_QTCSDL.Views
{
    /// <summary>
    /// Interaction logic for ListUserView.xaml
    /// </summary>
    public partial class ListRoleView : UserControl
    {
        OracleConnection conn = null;
        OracleDatabase db;
        public ListRoleView()
        {
            //this.setConnection();
            InitializeComponent();
            Window_Loaded();
            //DataContext = new MainWindow();
        }

        private void Window_Loaded()
        {
            db = OracleDatabase.Instance;
            OracleCommand cmd = db.CreateCommand("ALTER SESSION SET \"_ORACLE_SCRIPT\" = TRUE");
            cmd.ExecuteNonQuery();
            this.UpdateDataGrid();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            conn.Close();
        }

        private void UpdateDataGrid()
        {
            DataTable dt = db.Query("SELECT * FROM DBA_ROLES");
            roleList.ItemsSource = dt.DefaultView;
        }

        private void myDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;

            if (dr != null)
            {
                txtbxVaitro.Text = dr["ROLE"].ToString();

                // Read DB
                //OracleCommand cmd = conn.CreateCommand();
                //cmd.CommandText = "SELECT DV.MADV, DV.TENDV, VT.MAVAITRO, VT.VAITRO FROM NHANVIEN NV, DONVI DV, VAITRO VT WHERE NV.DONVI=DV.MADV AND NV.VAITRO=VT.MAVAITRO";
                //cmd.CommandType = CommandType.Text;
                //OracleDataReader odr = cmd.ExecuteReader();

                btnAdd.IsEnabled = false;
                btnUpdate.IsEnabled = true;
                btnDelete.IsEnabled = true;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var sql = "CREATE ROLE " + txtbxVaitro.Text;
            if (txtbxPassword.Text.Trim() != "")
                sql += " IDENTIFIED BY " + txtbxPassword.Text.Trim();

            try
            {
                db.Query(sql);
                MessageBox.Show("Thêm thành công");
                this.UpdateDataGrid();
            }
            catch
            {
                MessageBox.Show("Không thể tạo role");
            }

            btnUpdate.IsEnabled = true;
            btnUpdate.IsEnabled = false;
            btnDelete.IsEnabled = false;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string sql = "";
            if (txtbxPassword.Text.Trim() != "")
            {
                sql = "ALTER ROLE " + txtbxVaitro.Text + " IDENTIFIED BY " + txtbxPassword.Text;
            }
            try
            {
                db.Query(sql);
                MessageBox.Show("Update thành công");
                this.UpdateDataGrid();
            }
            catch
            {
                MessageBox.Show("Không thể chỉnh sửa user");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dr = roleList.SelectedItem as DataRowView;

            if (dr != null)
            {
                try
                {


                    string sql = "DROP ROLE " + dr["ROLE"];
                    MessageBox.Show("Xoá thành công");

                    db.Query(sql);

                    this.UpdateDataGrid();
                }
                catch
                {
                    MessageBox.Show("Không thể xóa role");
                }
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            txtbxVaitro.Text = "";
            txtbxPassword.Text = "";
            
            btnAdd.IsEnabled = true;
            btnUpdate.IsEnabled = false;
            btnDelete.IsEnabled = false;
        }

        private void txtbxVaitro_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.EnableClickBtn();
        }

        private void txtbxPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.EnableClickBtn();
        }
        private void EnableClickBtn()
        {
            btnAdd.IsEnabled = true;
            btnUpdate.IsEnabled = true;
            btnDelete.IsEnabled = true;
            btnReset.IsEnabled = true;
        }
    }
}
