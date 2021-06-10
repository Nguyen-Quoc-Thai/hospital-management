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

        public ListStaff()
        {
            InitializeComponent();
            //Initialize();
            //LoadStaff();
        }


        public void Initialize()
        {
            db = OracleDatabase.Instance;
            OracleCommand cmd = db.CreateCommand("ALTER SESSION SET \"_ORACLE_SCRIPT\" = TRUE");
            cmd.ExecuteNonQuery();
        }

        public void LoadStaff()
        {
            DataTable dt = db.Query("select nv.MANV, nv.HOTEN, nv.LUONG, nv.VAITRO,  vt.MAVAITRO , nv.NGAYSINH, nv.DONVI from NHANVIEN nv, VAITRO vt WHERE nv.VAITRO = vt.MAVAITRO");
            myDataGrid.ItemsSource = dt.DefaultView;
        }

    }




}
