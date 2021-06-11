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

namespace PH2_QTCSDL.Views.TiepTan_Component
{
    /// <summary>
    /// Interaction logic for BenhNhan.xaml
    /// </summary>
    public partial class BenhNhan : UserControl
    {
        OracleDatabase db;
        public BenhNhan()
        {
            InitializeComponent();
            btnInsert.IsEnabled = false;
            btnUpdate.IsEnabled = false;
            btnReset.IsEnabled = false;
            btnDelete.IsEnabled = false;
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
                DataTable dt = db.Query("SELECT * FROM qt.BENHNHAN");
                BenhNhan_table.ItemsSource = dt.DefaultView;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void BenhNhan_table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;

            if (dr != null)
            {
                BN_MABN.Text = dr["MABN"].ToString();
                BN_HOTEN.Text = dr["HOTEN"].ToString();
                BN_NGAYSINH.Text = dr["NGAYSINH"].ToString();
                BN_DIACHI.Text = dr["DIACHI"].ToString();
                BN_SDT.Text = dr["SDT"].ToString();
            }
            btnInsert.IsEnabled = true;
            btnUpdate.IsEnabled = true;
            btnDelete.IsEnabled = true;
            btnReset.IsEnabled = true;
        }

        private void BenhNhan_update(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql = "UPDATE QT.BENHNHAN SET HOTEN='" + BN_HOTEN.Text + "', NGAYSINH=TO_DATE('"
                    + BN_NGAYSINH.Text + "', 'dd/mm/yyyy'), DIACHI ='" + BN_DIACHI.Text + "', SDT='" + BN_SDT.Text + "' WHERE MABN='" + BN_MABN.Text + "'";
                db = OracleDatabase.Instance;
                db.Query(sql);
                db.Query("COMMIT");
                MessageBox.Show("Cập nhật thành công");
                this.UpdateDataGrid();
            }
            catch
            {
                MessageBox.Show("Không thể chỉnh sửa thông tin bệnh nhân");
            }
        }

        private void BenhNhan_textChange(object sender, TextChangedEventArgs e)
        {
            btnInsert.IsEnabled = true;
            btnUpdate.IsEnabled = true;
            btnReset.IsEnabled = true;
        }

        private void BenhNhan_insert(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql = "INSERT INTO QT.BENHNHAN(MABN, HOTEN, NGAYSINH, DIACHI, SDT) VALUES ('"
                    + BN_MABN.Text + "', '" + BN_HOTEN.Text + "', TO_DATE('" + BN_NGAYSINH.Text + "', 'dd/mm/yyyy'), '" + BN_DIACHI.Text + "', '" + BN_SDT.Text + "')";
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

        private void BenhNhan_delete(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql = "DELETE FROM QT.BENHNHAN WHERE MABN='" + BN_MABN.Text + "'";
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


        private void BenhNhan_reset(object sender, RoutedEventArgs e)
        {
            BN_MABN.Clear();
            BN_HOTEN.Clear();
            BN_NGAYSINH.SelectedDate = null;
            BN_DIACHI.Clear();
            BN_SDT.Clear();
            btnInsert.IsEnabled = false;
            btnUpdate.IsEnabled = false;
            btnDelete.IsEnabled = false;
        }
    }
}
