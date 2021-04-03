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
        OracleDatabase db;
        public MainWindow()
        {
            //this.setConnection();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            db = OracleDatabase.Instance;

            this.UpdateDataGrid();
            this.LoadCombobox();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            conn.Close();
        }


        private void LoadCombobox()
        {
            try
            {
                // Load CBB VAITRO

                //Initialize
                DataTable dt1 = db.Query("SELECT VAITRO, MAVAITRO FROM VAITRO ORDER BY MAVAITRO DESC");
                List<DataRow> dataRows_pos = dt1.GetRows();

                List<Combobox> listPosition = new List<Combobox>();
                foreach (DataRow dtr in dataRows_pos)
                {
                    listPosition.Add(new Combobox() {
                        DisplayName = dtr.GetData(0),
                        SelectedValue = dtr.GetData(1)

                    });
                }
                cbbPositions.ItemsSource = listPosition;


                // Load CBB DONVI
                //Initialize
                DataTable dt2 = db.Query("SELECT TENDV, MADV FROM DONVI ORDER BY MADV ASC");
                List<DataRow> dataRows_depart = dt2.GetRows();

                List<Combobox> listDepart = new List<Combobox>();
                foreach (DataRow dtr in dataRows_depart)
                {
                    listDepart.Add(new Combobox()
                    {
                        DisplayName = dtr.GetData(0),
                        SelectedValue = dtr.GetData(1)

                    });
                }
                cbbDepartments.ItemsSource = listDepart;



            }
            catch (Exception exp)
            {
                throw new Exception("Failed to load combobox!");
            }
        }

        //public class Combobox
        //{
        //    public string DisplayName { get; set; }
        //    public string SelectedValue { get; set; }
        //}

        private void UpdateDataGrid()
        {
            DataTable dt =  db.Query("SELECT MANV, HOTEN, LUONG, VAITRO, DONVI FROM NHANVIEN ORDER BY MANV DESC");
            myDataGrid.ItemsSource = dt.DefaultView;
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
                    this.UpdateDataGrid();
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
