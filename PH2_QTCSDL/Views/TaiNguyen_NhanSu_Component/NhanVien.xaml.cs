using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace PH2_QTCSDL.Views.TaiNguyen_NhanSu_Component
{
    /// <summary>
    /// Interaction logic for NhanVien.xaml
    /// </summary>
    public partial class NhanVien : UserControl
    {
        OracleDatabase db;
        DataRowView dr = null;
        public NhanVien()
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
                DataTable dt = db.Query("SELECT * FROM qt.NHANVIEN");
                NhanVien_table.ItemsSource = dt.DefaultView;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void NhanVien_table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            dr = dg.SelectedItem as DataRowView;

            if (dr != null)
            {
                NV_MANV.Text = dr["MANV"].ToString();
                NV_HOTEN.Text = dr["HOTEN"].ToString();
                NV_NGAYSINH.Text = dr["NGAYSINH"].ToString();
                NV_LUONG.Text = dr["LUONG"].ToString();
                NV_DV.Text = dr["DONVI"].ToString();
                NV_VT.Text = dr["VAITRO"].ToString();
            }

            btnInsert.IsEnabled = true;
            btnUpdate.IsEnabled = true;
            btnDelete.IsEnabled = true;
            btnReset.IsEnabled = true;
        }

        private void NhanVien_update(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql = "";
                if (dr != null && dr["LUONG"].ToString() == NV_LUONG.Text)
                {
                    sql = "UPDATE QT.NHANVIEN SET HOTEN='" + NV_HOTEN.Text + "', NGAYSINH=TO_DATE('"
                    + NV_NGAYSINH.Text + "', 'mm/dd/yyyy'), VAITRO ='" + NV_VT.Text + "', DONVI='" + NV_DV.Text  + "' WHERE MANV='" + NV_MANV.Text + "'";
                }
                else
                {
                    sql = "UPDATE QT.NHANVIEN SET HOTEN='" + NV_HOTEN.Text + "', NGAYSINH=TO_DATE('"
                    + NV_NGAYSINH.Text + "', 'mm/dd/yyyy'), VAITRO ='" + NV_VT.Text + "', DONVI='" + NV_DV.Text + "', LUONG=" + Int32.Parse(NV_LUONG.Text) + " WHERE MANV='" + NV_MANV.Text + "'";
                }

                db = OracleDatabase.Instance;
                db.Query(sql);
                db.Query("COMMIT");
                MessageBox.Show("Cập nhật thành công");
                this.UpdateDataGrid();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Không thể chỉnh sửa thông tin nhân viên");
            }
        }

        private void NhanVien_textChange(object sender, TextChangedEventArgs e)
        {
            btnInsert.IsEnabled = true;
            btnUpdate.IsEnabled = true;
            btnReset.IsEnabled = true;
        }

        private void NhanVien_insert(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql = "INSERT INTO QT.NHANVIEN(MANV, HOTEN, LUONG, VAITRO, DONVI, NGAYSINH) VALUES ('" + NV_MANV.Text + "', '" + NV_HOTEN.Text + "', " + Int32.Parse(NV_LUONG.Text) + ", '" + NV_VT.Text + "', '" + NV_DV.Text + "', TO_DATE('" + NV_NGAYSINH.Text + "', 'mm/dd/yyyy'))";

                db = OracleDatabase.Instance;
                db.Query(sql);
                db.Query("COMMIT");
                MessageBox.Show("Thêm thành công");
                this.UpdateDataGrid();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Thêm không thành công");
            }
        }

        private void NhanVien_delete(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql = "DELETE FROM QT.NHANVIEN WHERE MANV='" + NV_MANV.Text + "'";

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


        private void NhanVien_reset(object sender, RoutedEventArgs e)
        {
            NV_MANV.Clear();
            NV_HOTEN.Clear();
            NV_NGAYSINH.SelectedDate = null;
            NV_LUONG.Clear();
            NV_VT.Clear();
            NV_DV.Clear();

            btnInsert.IsEnabled = false;
            btnUpdate.IsEnabled = false;
            btnDelete.IsEnabled = false;
        }
    }
}
