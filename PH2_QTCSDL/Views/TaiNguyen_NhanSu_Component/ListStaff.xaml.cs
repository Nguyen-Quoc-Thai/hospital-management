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
    /// Interaction logic for ListStaff.xaml
    /// </summary>
    public partial class ListStaff : UserControl
    {

        OracleDatabase db;
        DataTable dt;
        public static string currentMaNV;
        public ListStaff()
        {
            InitializeComponent();
        }


        private void Grid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            db = OracleDatabase.Instance;
            dt = db.Query("select * from qt.NHANVIEN");


            myDataGrid.ItemsSource = dt.DefaultView;
        }

        private void myDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataGrid dg = sender as DataGrid;
                DataRowView dr = dg.SelectedItem as DataRowView;
                currentMaNV = dr.Row.ItemArray[0].ToString();

                txtbxUsername.Text = dr.Row.ItemArray[0].ToString();
                txtbxPassword.Text = dr.Row.ItemArray[1].ToString();
                txtLuong.Text = dr.Row.ItemArray[2].ToString();
                txtVaiTro.Text = dr.Row.ItemArray[3].ToString();
                txtDonvi.Text = dr.Row.ItemArray[4].ToString();
            }
            catch
            {

            }


        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string sql = $@"INSERT INTO qt.NHANVIEN ( manv, hoten, luong, vaitro, donvi) VALUES ('{txtbxUsername.Text}','{txtbxPassword.Text}',{Int32.Parse(txtLuong.Text)}, '{txtVaiTro.Text}','{txtDonvi.Text}')";
            //string sql = @"INSERT INTO qt.NHANVIEN ( manv, hoten, luong, vaitro, donvi) VALUES ('NV200','Nguyen Van A', 200, 'BS','DV1')";

            try
            {
                db.Query(sql);
                MessageBox.Show("Create thành công");
                UpdateDataGrid();
            }
            catch
            {
                MessageBox.Show("Thông tin nhập không hợp lệ");
            }
        }

        private void UpdateDataGrid()
        {
            DataTable dt = db.Query("select * from qt.NHANVIEN");
            myDataGrid.ItemsSource = dt.DefaultView;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string sql = $@"UPDATE qt.NHANVIEN set MANV = '{txtbxUsername.Text}', HOTEN = '{txtbxPassword.Text}', LUONG = {Int32.Parse(txtLuong.Text)}, VAITRO = '{txtVaiTro.Text}',DONVI = '{txtDonvi.Text}' WHERE MANV = '{currentMaNV}'";
            //string sql = @"INSERT INTO qt.NHANVIEN ( manv, hoten, luong, vaitro, donvi) VALUES ('NV200','Nguyen Van A', 200, 'BS','DV1')";

            try
            {
                db.Query(sql);
                MessageBox.Show("Update thành công");
                UpdateDataGrid();
            }
            catch
            {
                MessageBox.Show("Thông tin nhập không hợp lệ");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string sql = $@"DELETE FROM qt.NHANVIEN WHERE MANV = '{currentMaNV}'";
            //string sql = @"INSERT INTO qt.NHANVIEN ( manv, hoten, luong, vaitro, donvi) VALUES ('NV200','Nguyen Van A', 200, 'BS','DV1')";

            try
            {
                db.Query(sql);
                MessageBox.Show("Delete thành công");
                UpdateDataGrid();
            }
            catch
            {
                MessageBox.Show("Delete không thành công");
            }
        }
    }




}
