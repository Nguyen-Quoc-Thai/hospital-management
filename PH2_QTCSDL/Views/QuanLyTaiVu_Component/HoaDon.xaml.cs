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

namespace PH2_QTCSDL.Views.QuanLyTaiVu_Component
{
    public partial class HoaDon : UserControl
    {
        OracleDatabase db;
        public HoaDon()
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
                DataTable dt = db.Query("SELECT * FROM QT.HOADON");
                HOADON_table.ItemsSource = dt.DefaultView;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void HOADON_TONGTIEN_NUM(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void HOADON_table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;

            if (dr != null)
            {
                HOADON_SOHD.Text = dr["SOHD"].ToString();
                HOADON_MAKB.Text = dr["MAKB"].ToString();
                HOADON_NGAY.Text = dr["NGAY"].ToString();
                HOADON_NGUOIBANTHUOC.Text = dr["NGUOIBANTHUOC"].ToString();
                HOADON_TONGTIEN.Text = dr["TONGTIEN"].ToString();
            }
            btnInsert.IsEnabled = true;
            btnUpdate.IsEnabled = true;
            btnReset.IsEnabled = true;
        }

        private void HOADON_update(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql = $"UPDATE QT.HOADON SET MAKB='{HOADON_MAKB.Text}', NGAY=TO_DATE('{HOADON_NGAY.Text}', 'dd/mm/yyyy')" +
                    $", NGUOIBANTHUOC='{HOADON_NGUOIBANTHUOC.Text}', TONGTIEN='{HOADON_TONGTIEN.Text}' WHERE SOHD='{HOADON_SOHD.Text}'";
                db = OracleDatabase.Instance;
                db.Query(sql);
                db.Query("COMMIT");
                MessageBox.Show("Cập nhật thành công");
                this.UpdateDataGrid();
            }
            catch
            {
                MessageBox.Show("Không thể chỉnh sửa hóa đơn");
            }
        }

        private void HOADON_textChange(object sender, TextChangedEventArgs e)
        {
            btnInsert.IsEnabled = true;
            btnUpdate.IsEnabled = true;
            btnReset.IsEnabled = true;
        }

        private void HOADON_insert(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql = $"INSERT INTO QT.HOADON(SOHD, MAKB, NGAY, NGUOIBANTHUOC, TONGTIEN) VALUES(" +
                    $"'{HOADON_SOHD.Text}', '{HOADON_MAKB.Text}', TO_DATE('{HOADON_NGAY.Text}', 'dd/mm/yyyy'), '{HOADON_NGUOIBANTHUOC.Text}', '{HOADON_TONGTIEN.Text}')";
                db = OracleDatabase.Instance;
                db.Query(sql);
                db.Query("COMMIT");
                MessageBox.Show("Thêm thành công");
                this.UpdateDataGrid();
            }
            catch
            {
                MessageBox.Show("Thêm không thành công");
            }
        }

        private void HOADON_delete(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql = $"DELETE FROM QT.HOADON WHERE SOHD='{HOADON_SOHD.Text}'";
                db = OracleDatabase.Instance;
                db.Query(sql);
                db.Query("COMMIT");
                MessageBox.Show("Xóa thành công");
                this.UpdateDataGrid();
            }
            catch
            {
                MessageBox.Show("Xóa không thành công");
            }
        }

        private void HOADON_reset(object sender, RoutedEventArgs e)
        {
            HOADON_SOHD.Clear();
            HOADON_MAKB.Clear();
            HOADON_NGAY.SelectedDate = null;
            HOADON_NGUOIBANTHUOC.Clear();
            HOADON_TONGTIEN.Clear();
            btnInsert.IsEnabled = false;
            btnUpdate.IsEnabled = false;
        }
    }
}
