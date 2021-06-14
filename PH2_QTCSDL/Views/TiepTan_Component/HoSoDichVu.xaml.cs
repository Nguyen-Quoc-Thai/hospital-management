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
    /// Interaction logic for HoSoDichVu.xaml
    /// </summary>
    public partial class HoSoDichVu : UserControl
    {
        OracleDatabase db;
        DataRowView dr = null;
        public HoSoDichVu()
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
                DataTable dt = db.Query("SELECT * FROM QT.HOSO_DICHVU");
                HSDV_table.ItemsSource = dt.DefaultView;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void HSDV_table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            dr = dg.SelectedItem as DataRowView;

            if (dr != null)
            {
                HSDV_MAKB.Text = dr["MAKB"].ToString();
                HSDV_MADV.Text = dr["MADV"].ToString();
                HSDV_NGAY.Text = dr["NGAY"].ToString();
                HSDV_MANV.Text = dr["NGUOITHUCHIEN"].ToString();
                HSDV_KL.Text = dr["KETLUAN"].ToString();
            }
            btnInsert.IsEnabled = true;
            btnUpdate.IsEnabled = true;
            btnDelete.IsEnabled = true;
            btnReset.IsEnabled = true;
        }

        private void HSDV_update(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql = "";

                if(dr != null && dr["NGUOITHUCHIEN"].ToString() == HSDV_MANV.Text)
                {
                    sql = $"UPDATE QT.HOSO_DICHVU SET MADV='{HSDV_MADV.Text}', NGAY=TO_DATE('{HSDV_NGAY.Text}','dd/mm/yyyy')" +
                        $", KETLUAN='{HSDV_KL.Text}' WHERE MAKB='{HSDV_MAKB.Text}'";
                } 
                else
                {
                    sql = $"UPDATE QT.HOSO_DICHVU SET MADV='{HSDV_MADV.Text}', NGAY=TO_DATE('{HSDV_NGAY.Text}','dd/mm/yyyy')" +
                        $", NGUOITHUCHIEN='{HSDV_MANV.Text}', KETLUAN='{HSDV_KL.Text}' WHERE MAKB='{HSDV_MAKB.Text}'";
                }

                db = OracleDatabase.Instance;
                db.Query(sql);
                db.Query("COMMIT");
                MessageBox.Show("Cập nhật thành công");
                this.UpdateDataGrid();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Không thể chỉnh sửa hồ sơ dịch vụ");
            }
        }

        private void HSDV_textChange(object sender, TextChangedEventArgs e)
        {
            btnInsert.IsEnabled = true;
            btnUpdate.IsEnabled = true;
            btnReset.IsEnabled = true;
        }

        private void HSDV_insert(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql = "INSERT INTO QT.HOSO_DICHVU(MAKB, MADV, NGAY, NGUOITHUCHIEN, KETLUAN) VALUES ('"
                    + HSDV_MAKB.Text + "', '" + HSDV_MADV.Text + "', TO_DATE('" + HSDV_NGAY.Text + "', 'dd/mm/yyyy'), '" + HSDV_MANV.Text + "', '" + HSDV_KL.Text + "')";
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

        private void HSDV_delete(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql = "DELETE FROM QT.HOSO_DICHVU WHERE MAKB='" + HSDV_MAKB.Text + "'";
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


        private void HSDV_reset(object sender, RoutedEventArgs e)
        {
            HSDV_MAKB.Clear();
            HSDV_MADV.Clear();
            HSDV_NGAY.SelectedDate = null;
            HSDV_MANV.Clear();
            HSDV_KL.Clear();
            btnInsert.IsEnabled = false;
            btnUpdate.IsEnabled = false;
            btnDelete.IsEnabled = false;
        }
    }
}
