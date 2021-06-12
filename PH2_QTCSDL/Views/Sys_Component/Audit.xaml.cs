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
using Oracle.ManagedDataAccess.Client;

namespace PH2_QTCSDL.Views.Sys_Component
{
    /// <summary>
    /// Interaction logic for Audit.xaml
    /// </summary>
    public partial class Audit : UserControl
    {
        OracleDatabase db;
        public Audit()
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
                DataTable dt = db.Query("SELECT TIMESTAMP,db_user,object_schema,object_name,policy_name,sql_text FROM DBA_FGA_AUDIT_TRAIL ORDER BY TIMESTAMP DESC");
                Audit_table.ItemsSource = dt.DefaultView;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
    }
}
