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
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Configuration;
using System.Data;
using System.Reflection;

namespace PH1_QTCSDL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OracleConnection conn = null;
        public MainWindow()
        {
            this.setConnection();
            InitializeComponent();
        }

        private void loadCombobox()
        {
            try
            {
                int len = 0;

                // Load CBB VAITRO
                OracleCommand cmd1 = conn.CreateCommand();
                DataTable dt1 = new DataTable();
                OracleDataReader dr1;
                List<DataRow> dtr1;
                List<Combobox> ls1 = new List<Combobox>();

                cmd1.CommandType = CommandType.Text;
                cmd1.CommandText = "SELECT VAITRO, MAVAITRO FROM VAITRO ORDER BY MAVAITRO DESC";

                dr1 = cmd1.ExecuteReader();
                dt1.Load(dr1);
                dtr1 = dt1.AsEnumerable().ToList();

                len = dtr1.Count();
                while(len > 0)
                {
                    ls1.Add(new Combobox() { DisplayName = dtr1[len-1].ItemArray[0].ToString(), SelectedValue = dtr1[len - 1].ItemArray[1].ToString() });
                    len--;
                }

                cbbPositions.ItemsSource = ls1;

                // Load CBB DONVI
                OracleCommand cmd2 = conn.CreateCommand();
                DataTable dt2 = new DataTable();
                OracleDataReader dr2;
                List<DataRow> dtr2;
                List<Combobox> ls2 = new List<Combobox>();

                cmd2.CommandType = CommandType.Text;
                cmd2.CommandText = "SELECT TENDV, MADV FROM DONVI ORDER BY MADV DESC";

                dr2 = cmd2.ExecuteReader();
                dt2.Load(dr2);
                dtr2 = dt2.AsEnumerable().ToList();

                len = dtr2.Count();
                while (len > 0)
                {
                    ls2.Add(new Combobox() { DisplayName = dtr2[len - 1].ItemArray[0].ToString(), SelectedValue = dtr2[len - 1].ItemArray[1].ToString() });
                    len--;
                }

                cbbDepartments.ItemsSource = ls2;

                // Close DataReader
                dr1.Close();
                dr2.Close();
            }
            catch(Exception exp)
            {
                throw new Exception("Failed to load combobox!");
            }
        }

        public class Combobox
        {
            public string DisplayName { get; set; }
            public string SelectedValue { get; set; }
        }

        private void updateDataGrid()
        {
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT MANV, HOTEN, LUONG, VAITRO, DONVI FROM NHANVIEN ORDER BY MANV DESC";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            myDataGrid.ItemsSource = dt.DefaultView;
            dr.Close();
        }

        private void setConnection()
        {
            string connectionString = "TNS_ADMIN = C:\\Users\\Qouc Tahi\\Oracle\\network\\admin; USER ID = nqt; PASSWORD = nqt; DATA SOURCE = localhost:1521/ORCLCDB.localdomain; PERSIST SECURITY INFO = True";
            this.conn = new OracleConnection();
            this.conn.ConnectionString = connectionString;

            try
            {
                this.conn.Open();
            }
            catch (Exception exp)
            {
                throw new Exception("Failed to connect to OracleDB!");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.updateDataGrid();
            this.loadCombobox();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            conn.Close();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            String sql = "INSERT INTO NHANVIEN(MANV, HOTEN, LUONG, VAITRO, DONVI) VALUES (:MANV, :HOTEN, :LUONG, :VAITRO, :DONVI)";
            this.AUD(sql, 0);
            btnAdd.IsEnabled = false;
            btnUpdate.IsEnabled = true;
            btnDelete.IsEnabled = true;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            txtbxMaNV.Text = "";
            txtbxName.Text = "";
            txtbxSalary.Text = "";
            cbbDepartments.SelectedIndex = 0;
            cbbPositions.SelectedIndex = 0;

            btnAdd.IsEnabled = true;
            btnUpdate.IsEnabled = false;
            btnDelete.IsEnabled = false;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            String sql = "DELETE FROM NHANVIEN WHERE MANV=:MANV";
            this.AUD(sql, 2);
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            String sql = "UPDATE NHANVIEN SET HOTEN=:HOTEN, LUONG=:LUONG, DONVI=:DONVI, VAITRO=:VAITRO WHERE MANV=:MANV";
            this.AUD(sql, 1);
        }

        private void AUD(String sql_stmt, int state)
        {
            String msg = "";
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql_stmt;
            cmd.CommandType = CommandType.Text;

            switch(state)
            {
                case 0:
                    {
                        msg = "Inserted succecssfully!";

                        cmd.Parameters.Add("MANV", OracleDbType.Varchar2, 8).Value = txtbxMaNV.Text;
                        cmd.Parameters.Add("HOTEN", OracleDbType.NVarchar2, 30).Value = txtbxName.Text;
                        cmd.Parameters.Add("LUONG", OracleDbType.Int32, 6).Value = Int32.Parse(txtbxSalary.Text);
                        cmd.Parameters.Add("VAITRO", OracleDbType.NVarchar2, 20).Value = cbbPositions.SelectedValue;
                        cmd.Parameters.Add("DONVI", OracleDbType.NVarchar2, 20).Value = cbbDepartments.SelectedValue;

                        break;
                    }
                case 1:
                    {
                        msg = "Updated succecssfully!";

                        cmd.Parameters.Add("HOTEN", OracleDbType.NVarchar2, 30).Value = txtbxName.Text;
                        cmd.Parameters.Add("LUONG", OracleDbType.Int32, 6).Value = Int32.Parse(txtbxSalary.Text);
                        cmd.Parameters.Add("VAITRO", OracleDbType.NVarchar2, 20).Value = cbbPositions.SelectedValue;
                        cmd.Parameters.Add("DONVI", OracleDbType.NVarchar2, 20).Value = cbbDepartments.SelectedValue;

                        cmd.Parameters.Add("MANV", OracleDbType.Varchar2, 8).Value = txtbxMaNV.Text;

                        break;
                    }
                case 2:
                    {
                        msg = "Deleted succecssfully!";

                        cmd.Parameters.Add("MANV", OracleDbType.Varchar2, 8).Value = txtbxMaNV.Text;

                        break;
                    }
            }

            try
            {
                int n = cmd.ExecuteNonQuery();
                if (n > 0)
                {
                    MessageBox.Show(msg);
                    this.updateDataGrid();
                }
            }catch(Exception exp)
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
                txtbxMaNV.Text = dr["MANV"].ToString();
                txtbxName.Text = dr["HOTEN"].ToString();
                txtbxSalary.Text = dr["LUONG"].ToString();

                // Read DB
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT DV.MADV, DV.TENDV, VT.MAVAITRO, VT.VAITRO FROM NHANVIEN NV, DONVI DV, VAITRO VT WHERE NV.DONVI=DV.MADV AND NV.VAITRO=VT.MAVAITRO";
                cmd.CommandType = CommandType.Text;
                OracleDataReader odr = cmd.ExecuteReader();

                DataTable dt = new DataTable();
                List <DataRow> dtr = new List<DataRow>();
                
                dt.Load(odr);
                dtr = dt.AsEnumerable().ToList();

                odr.Close();

                // Bug here
                cbbPositions.SelectedIndex = 1;
                cbbDepartments.SelectedIndex = 1;

                btnAdd.IsEnabled = false;
                btnUpdate.IsEnabled = true;
                btnDelete.IsEnabled = true;
            }
        }
    }
}
