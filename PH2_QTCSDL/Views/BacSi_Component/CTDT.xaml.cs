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
using Oracle.ManagedDataAccess.Client;
using System.Text.RegularExpressions;

namespace PH2_QTCSDL.Views.BacSi_Component
{
    /// <summary>
    /// Interaction logic for CTDT.xaml
    /// </summary>
    public partial class CTDT : UserControl
    {
        OracleDatabase db;
        public CTDT()
        {
            InitializeComponent();
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid();
        }

        private void UpdateDataGrid()
        {
            db = OracleDatabase.Instance;
            OracleCommand cmd = db.CreateCommand("ALTER SESSION SET \"_ORACLE_SCRIPT\" = TRUE");
            try
            {
                DataTable dt = db.Query("SELECT * FROM BS_VIEW_CHITIETDONTHUOC");
                CTDT_table.ItemsSource = dt.DefaultView;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void CTDT_SOLUONG_NUM(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void CTDT_table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;

            if (dr != null)
            {
                CTDT_MAKB.Text = dr["MAKB"].ToString();
                CTDT_MATHUOC.Text = dr["MATHUOC"].ToString();
                CTDT_SOLUONG.Text = dr["SOLUONG"].ToString();
                CTDT_LIEUDUNG.Text = dr["LIEUDUNG"].ToString();
                CTDT_MOTA.Text = dr["MOTA"].ToString();
            }
        }

        private void CTDT_update(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show("Đì vé lợp pin");
            }
            catch
            {
                MessageBox.Show("Không thể chỉnh sửa bệnh án");
            }
        }
    }
}
