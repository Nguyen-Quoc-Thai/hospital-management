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
    /// Interaction logic for HoaDon.xaml
    /// </summary>
    public partial class HoaDon : UserControl
    {
        OracleDatabase db;
        public HoaDon()
        {
            InitializeComponent();
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
                DataTable dt = db.Query("SELECT * FROM QT.HOADON");
                HOADON_table.ItemsSource = dt.DefaultView;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
    }
}
