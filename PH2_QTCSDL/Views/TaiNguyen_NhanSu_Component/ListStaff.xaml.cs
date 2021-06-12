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
        public ListStaff()
        {
            InitializeComponent();
        }


        private void Grid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            db = OracleDatabase.Instance;
            dt = db.Query("select get_user_role from dual");
            string role = dt.Rows[0]["GET_USER_ROLE"].ToString();
            
            if(role == "BACSI")
            {
                dt = db.Query("select * from qt.NHANVIEN");

            }
            else if(role == "CLGT")
            {

            }

            myDataGrid.ItemsSource = dt.DefaultView;
            UpdateContentLabel();

        }

        void UpdateContentLabel()
        {
            content1.Content = dt.Columns[0].ColumnName;
            content2.Content = dt.Columns[1].ColumnName;
            content3.Content = dt.Columns[2].ColumnName;
            content4.Content = dt.Columns[3].ColumnName;
            content5.Content = dt.Columns[4].ColumnName;

        }
    }




}
