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
    public partial class ListUserView : UserControl
    {
        OracleConnection conn = null;
        OracleDatabase db;
        public ListUserView()
        {
            //this.setConnection();
            InitializeComponent();
            Window_Loaded();
            //DataContext = new MainWindow();
        }

        private void Window_Loaded()
        {
            db = OracleDatabase.Instance;

            this.UpdateDataGrid();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            conn.Close();
        }

        private void UpdateDataGrid()
        {
            DataTable dt = db.Query("SELECT * FROM all_users");
            myDataGrid.ItemsSource = dt.DefaultView;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var sql = "CREATE USER " + txtbxUsername;
            if (txtbxPassword.Text.Trim() != "")
                sql += " IDENTIFIED BY " + txtbxPassword.Text.Trim();

            try
            {
                DataTable dt = db.Query(sql);
                MessageBox.Show(dt.ToString());
            }
            catch
            {
                MessageBox.Show("Không thể tạo user");
            }

            btnUpdate.IsEnabled = true;
            btnUpdate.IsEnabled = false;
            btnDelete.IsEnabled = false;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            txtbxUsername.Text = "";
            txtbxPassword.Text = "";
            txtbxPassword.IsReadOnly = false;

            btnAdd.IsEnabled = true;
            btnUpdate.IsEnabled = false;
            btnDelete.IsEnabled = false;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var sql = "DROP USER " + txtbxUsername.Text;
            try
            {
                MessageBox.Show(sql);
                db.Query(sql);
            }
            catch
            {
                MessageBox.Show("Không thể xóa user");
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string sql = "";
            if (txtbxPassword.Text.Trim() != "")
            {
                sql = "ALTER USER " + txtbxUsername.Text + " IDENTIFIED BY " + txtbxPassword.Text;
            }    
            try
            {
                MessageBox.Show(sql);
                db.Query(sql);
            }
            catch
            {
                MessageBox.Show("Không thể chỉnh sửa user");
            }
        }

        private void AUD(String sql_stmt, int state)
        {
            String msg = "";
            OracleCommand cmd = db.CreateCommand(sql_stmt);

            switch (state)
            {
                case 2:
                    {
                        msg = "Deleted succecssfully!";

                        break;
                    }
            }

            try
            {
                int n = cmd.ExecuteNonQuery();
                if (n > 0)
                {
                    MessageBox.Show(msg);
                    this.UpdateDataGrid();
                }
            }
            catch (Exception exp)
            {
                throw new Exception("Failed to exec task!");
            }
        }

        private void myDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;

            if (dr != null)
            {
                txtbxUsername.Text = dr["USERNAME"].ToString();

                btnAdd.IsEnabled = false;
                btnUpdate.IsEnabled = true;
                btnDelete.IsEnabled = true;
            }
        }
    }
}
