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
            this.UpdateDataGrid();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            conn.Close();
        }

        private void UpdateDataGrid()
        {
            DataTable dt = db.Query("SELECT * FROM DBA_ROLES");
            myDataGrid.ItemsSource = dt.DefaultView;
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

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            txtbxVaitro.Text = "";
            
            btnAdd.IsEnabled = true;
            btnUpdate.IsEnabled = false;
            btnDelete.IsEnabled = false;
        }
    }
}
