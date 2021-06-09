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

namespace PH2_QTCSDL.Views.BacSi_Component
{
    /// <summary>
    /// Interaction logic for HSBA.xaml
    /// </summary>
    public partial class HSBA : UserControl
    {
        OracleDatabase db;
        public HSBA()
        {
            InitializeComponent();
            btnInsert.IsEnabled = false;
            btnUpdate.IsEnabled = false;
            btnReset.IsEnabled = false;
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateDataGrid();
        }

        private void UpdateDataGrid()
        {
            db = OracleDatabase.Instance;
            OracleCommand cmd = db.CreateCommand("ALTER SESSION SET \"_ORACLE_SCRIPT\" = TRUE");
            try
            {
                DataTable dt = db.Query("SELECT * FROM SYS2.HSBA");
                HSBA_table.ItemsSource = dt.DefaultView;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void HSBA_table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;

            if (dr != null)
            {
                HSBA_MAKB.Text = dr["MAKB"].ToString();
                HSBA_MABN.Text = dr["MABN"].ToString();
                HSBA_TTBD.Text = dr["TINHTRANGBANDAU"].ToString();
                HSBA_KLBS.Text = dr["KETLUANBS"].ToString();
            }
            btnInsert.IsEnabled = true;
            btnUpdate.IsEnabled = true;
            btnReset.IsEnabled = true;
        }

        private void HSBA_textChange(object sender, TextChangedEventArgs e)
        {
            btnInsert.IsEnabled = true;
            btnUpdate.IsEnabled = true;
            btnReset.IsEnabled = true;
        }

        private void HSBA_update(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql = "UPDATE SYS2.HSBA SET TINHTRANGBANDAU='" + HSBA_TTBD.Text + "', KETLUANBS='" + HSBA_KLBS.Text + "' WHERE MAKB='" + HSBA_MAKB.Text + "'";
                db = OracleDatabase.Instance;
                db.Query(sql);
                MessageBox.Show("Update thành công");
                this.UpdateDataGrid();
            }
            catch
            {
                MessageBox.Show("Không thể chỉnh sửa bệnh án");
            }
        }

        private void HSBA_insert(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql = "INSERT INTO SYS2.HSBA(MAKB, MABN, MABS, TINHTRANGBANDAU, KETLUANBS) VALUES ('"
                    + HSBA_MAKB.Text + "', '" + HSBA_MABN.Text + "', SYS_CONTEXT('userenv', 'SESSION_USER'), '" + HSBA_TTBD.Text + "', '" + HSBA_KLBS.Text + "')";
                db = OracleDatabase.Instance;
                db.Query(sql);
                MessageBox.Show("Thêm thành công");
                this.UpdateDataGrid();
            }
            catch
            {
                MessageBox.Show("Thêm không thành công");
            }
        }

        private void HSBA_reset(object sender, RoutedEventArgs e)
        {
            HSBA_MAKB.IsReadOnly = false;
            HSBA_MABN.IsReadOnly = false;
            HSBA_MAKB.Clear();
            HSBA_MABN.Clear();
            HSBA_TTBD.Clear();
            HSBA_KLBS.Clear();
            btnInsert.IsEnabled = false;
            btnUpdate.IsEnabled = false;
        }
    }
}
