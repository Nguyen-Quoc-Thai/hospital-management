using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Oracle.ManagedDataAccess.Client;

namespace PH1_QTCSDL.Views
{
    /// <summary>
    /// Interaction logic for Grant.xaml
    /// </summary>
    public partial class GrantOnUser : UserControl
    {
        OracleDatabase db;

        private object _currRow = null;

        public GrantOnUser()
        {
            InitializeComponent();
            Window_Loaded();
        }

        private void Window_Loaded()
        {
            db = OracleDatabase.Instance;
            OracleCommand cmd = db.CreateCommand("ALTER SESSION SET \"_ORACLE_SCRIPT\" = TRUE");
            this.UpdateDataGrid();
            this.LoadCombobox();
        }

        private void UpdateDataGrid()
        {
            DataTable dt = db.Query("SELECT * FROM all_users");
            userList.ItemsSource = dt.DefaultView;
        }

        private void LoadCombobox()
        {
            List<Combobox> TableList = new List<Combobox>();

            //DataTable dt1 = db.Query("SELECT table_name FROM user_tables");
            //DataTable dt1 = db.Query("SELECT table_name FROM all_tables WHERE last_analyzed IS NULL AND table_name <> 'PSTUBTBL' MINUS SELECT table_name FROM all_tables WHERE last_analyzed IS NULL AND(REGEXP_LIKE(table_name, '\\$', 'i') OR REGEXP_LIKE(table_name, '\\_.', 'i')) AND table_name <> 'HOSO_DICHVU' ORDER BY table_name ASC");
            DataTable dt1 = db.Query("SELECT table_name FROM all_tables WHERE table_name <> 'PSTUBTBL' MINUS SELECT table_name FROM all_tables WHERE (REGEXP_LIKE(table_name, '\\$', 'i') OR REGEXP_LIKE(table_name, '\\_.', 'i')) AND table_name <> 'HOSO_DICHVU' ORDER BY table_name ASC");
            List<DataRow> dataRows_pos = dt1.GetRows();
            
            try
            {
                foreach (DataRow row in dataRows_pos)
                {
                    TableList.Add(new Combobox()
                    {
                        DisplayName = row.GetData(0),
                        SelectedValue = row.GetData(0)
                    });
                }
                cbbTables.ItemsSource = TableList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbbTables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.LoadTableColumns();
        }

        private void GrantOptions_CheckedChanged(object sender, RoutedEventArgs e)
        {
            bool newVal = (allGrantOption.IsChecked == true);
            grantSelect.IsChecked = newVal;
            grantUpdate.IsChecked = newVal;
            grantInsert.IsChecked = newVal;
            grantDelete.IsChecked = newVal;
        }

        private void grantOption_CheckedChanged(object sender, RoutedEventArgs e)
        {
            allGrantOption.IsChecked = null;
            if ((grantSelect.IsChecked == true) && (grantUpdate.IsChecked == true)
                && (grantInsert.IsChecked == true) && (grantDelete.IsChecked == true))
                allGrantOption.IsChecked = true;
            if ((grantSelect.IsChecked == false) && (grantUpdate.IsChecked == false)
               && (grantInsert.IsChecked == false) && (grantDelete.IsChecked == false))
                allGrantOption.IsChecked = false;
            LoadTableColumns();
        }

        private void LoadTableColumns()
        {
            if (cbbTables.SelectedItem != null)
            {
                tableColumns.Children.Clear();
                string tableName = cbbTables.SelectedValue.ToString();
                string queryString = "SELECT DISTINCT column_name FROM all_tab_columns WHERE table_name = '" + tableName + "'";
                DataTable dt1 = db.Query(queryString);
                List<DataRow> dataRows_pos = dt1.GetRows();

                TextBlock text = new TextBlock();
                text.Text = "Chọn cột để cấp quyền";
                tableColumns.Children.Add(text);

                if ((grantInsert.IsChecked == true) || (grantDelete.IsChecked == true))
                {
                    foreach (DataRow row in dataRows_pos)
                    {
                        CheckBox columnName = new CheckBox();
                        columnName.Content = row.GetData(0);
                        columnName.IsChecked = true;
                        columnName.IsEnabled = false;       // cannot modify if insert or update option is on
                        columnName.Foreground = new SolidColorBrush(Colors.Blue);
                        tableColumns.Children.Add(columnName);
                    }
                }
                else
                {
                    foreach (DataRow row in dataRows_pos)
                    {
                        CheckBox columnName = new CheckBox();
                        columnName.Content = row.GetData(0);
                        columnName.IsChecked = true;
                        columnName.IsEnabled = true;       // cannot modify if insert or update option is on
                        columnName.Foreground = new SolidColorBrush(Colors.Blue);
                        tableColumns.Children.Add(columnName);
                    }
                }
            }
        }

        private int checkSubmit()
        {
            if (cbbTables.SelectedIndex <= -1)
            {
                MessageBox.Show("Hãy chọn một bảng trong cơ sở dữ liệu");
                return -1;
            };
            if ((grantSelect.IsChecked == false) && (grantUpdate.IsChecked == false)
               && (grantInsert.IsChecked == false) && (grantDelete.IsChecked == false))
            {
                MessageBox.Show("Hãy Chọn ít nhất một quyền Select/Insert/Update/Delete");
                return -1;
            }
            return 0;
        }
        
        private string GetCheckedColumns()
        {
            // dieu kien check cac column
            string checkedColumns = "";
            int columnCount = tableColumns.Children.Count;
            for (int i = 0; i < columnCount; i++)
            {
                if (tableColumns.Children[i] is CheckBox)
                {
                    string[] words = tableColumns.Children[i].ToString().Split(' ');
                    string[] lastField = words[words.Length - 1].Split(":");
                    if (lastField[0] == "IsChecked" && lastField[1] == "True")
                        checkedColumns += words[words.Length - 2].Split(":")[1] + ", ";
                }
            }

            checkedColumns = checkedColumns.Trim();
            if (checkedColumns.EndsWith(","))
                checkedColumns = checkedColumns.Remove(checkedColumns.Length - 1, 1);
            return checkedColumns;
        }

        private void grantClick(object sender, RoutedEventArgs e)
        {
            if (this.checkSubmit() == -1)
                return;

            DataRowView dr = userList.SelectedItem as DataRowView;

            if (dr != null)
            {
                // GRANT....
                string privileges = "";
                string privileges2 = "";
                if (grantSelect.IsChecked == true)
                    privileges2 += "SELECT, ";
                if (grantUpdate.IsChecked == true)
                    privileges2 += "UPDATE, ";
                if (grantInsert.IsChecked == true)
                    privileges2 += "INSERT, ";
                if (grantDelete.IsChecked == true)
                    privileges2 += "DELETE ";

                privileges2 = privileges2.Trim();
                if (privileges2.EndsWith(","))
                    privileges2 = privileges2.Remove(privileges2.Length - 1, 1);

                string tb_name = cbbTables.Text;
                string checkedColumns = this.GetCheckedColumns();
                if (checkedColumns != "")
                    privileges += checkedColumns;

                //MessageBox.Show(tb_name);
                try
                {
                    // inseertmax,asc,asc,asc
                    // Create view
                    //tb_name IN STRING, v_name IN STRING, str_select IN STRING
                    String view_name = ("V_" + privileges).Replace(',', '_').Replace(" ", "");
                    OracleCommand cmd2 = new OracleCommand("CREATE_VIEW_HIDE_COLUMNS", db.Conn);
                    cmd2.CommandType = CommandType.StoredProcedure;

                    cmd2.Parameters.Add("tb_name", OracleDbType.Varchar2).Value = tb_name;
                    cmd2.Parameters.Add("v_name", OracleDbType.Varchar2).Value = view_name;
                    cmd2.Parameters.Add("str_select", OracleDbType.Varchar2).Value = privileges;

                    cmd2.ExecuteNonQuery();


                    // Grant view to user
                    char withGrant = withGrantOption.IsChecked == true ? 'T' : 'F';
                    OracleCommand cmd = new OracleCommand("GRANT_PRIVILEGES_TO", db.Conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("tb_name", OracleDbType.Varchar2).Value = view_name;
                    cmd.Parameters.Add("str_priv", OracleDbType.Varchar2).Value = privileges2;
                    cmd.Parameters.Add("USER", OracleDbType.Varchar2).Value = dr[0].ToString();
                    cmd.Parameters.Add("with_grant", OracleDbType.Char).Value = withGrant;

                    cmd.ExecuteNonQuery();

                    // Update view
                    //this.userList_SelectionChanged(userList, null);

                    MessageBox.Show("Grant thành công");
                }
                catch
                {
                    MessageBox.Show("fail to grant");
                }
            }
            else
            {
                MessageBox.Show("Chọn một người dùng!");
            }
        }

        //private void revokeClick(object sender, RoutedEventArgs e)
        //{
        //    string checkedColumns = this.GetCheckedColumns();
        //    MessageBox.Show("Checked Columns: " + checkedColumns);

        //    if (this.checkSubmit() == -1)
        //        return;

        //    DataRowView dr = userList.SelectedItem as DataRowView;

        //    if (dr != null)
        //    {
        //        // GRANT....
        //        string privileges = "";
        //        if (grantSelect.IsChecked == true)
        //            privileges += "SELECT, ";
        //        if (grantUpdate.IsChecked == true)
        //            privileges += "UPDATE, ";
        //        if (grantInsert.IsChecked == true)
        //            privileges += "INSERT, ";
        //        if (grantDelete.IsChecked == true)
        //            privileges += "DELETE ";

        //        privileges = privileges.Trim();
        //        if (privileges.EndsWith(","))
        //            privileges = privileges.Remove(privileges.Length - 1, 1);

        //        try
        //        {
        //            //string withGrant = withGrantOption.IsChecked == true ? "T" : "F";
        //            //string proc = "REVOKE_PRIVILEGES_FROM(" + cbbTables.Text + ", " + "'" + privileges + "'" + ", " + dr["USERNAME"] + ")";
        //            //db.Query(proc);
        //            OracleCommand cmd = new OracleCommand("REVOKE_PRIVILEGES_FROM", db.Conn);
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            cmd.Parameters.Add("tb_name", OracleDbType.Varchar2).Value = cbbTables.Text;
        //            cmd.Parameters.Add("str_priv", OracleDbType.Varchar2).Value = privileges;
        //            cmd.Parameters.Add("user", OracleDbType.Varchar2).Value = dr["USERNAME"];

        //            cmd.ExecuteNonQuery();
        //        }
        //        catch
        //        {
        //            MessageBox.Show("fail to revoke");
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Chọn một người dùng!");
        //    }
        //}

        private void revokeClick(object sender, RoutedEventArgs e)
        {
            DataRowView dr = priviList.SelectedItem as DataRowView;

            if (_currRow == null)
            {
                MessageBox.Show("Chọn một dòng trong table 2 để thu hồi!");
            }
            else
            {
                // Call store proc
                OracleCommand cmd = new OracleCommand("REVOKE_PRIVILEGES_FROM", db.Conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("tb_name", OracleDbType.Varchar2).Value = dr[4].ToString();
                cmd.Parameters.Add("str_priv", OracleDbType.Varchar2).Value = dr[3].ToString();
                cmd.Parameters.Add("user", OracleDbType.Varchar2).Value = dr[0].ToString();

                cmd.ExecuteNonQuery();

                // Update view
                this.userList_SelectionChanged(_currRow, null);

                MessageBox.Show("Revoke privilege success!");
            }
        }

        private void userList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;

            table2_title.Text = "Danh sách quyền của user " + dr[0].ToString();
            var sql = "SELECT GRANTEE, OWNER, TABLE_NAME GRANTOR, PRIVILEGE, TABLE_NAME, GRANTABLE FROM DBA_TAB_PRIVS WHERE GRANTEE='" + dr[0].ToString() + "'";
            
            DataTable dt = db.Query(sql);
            priviList.ItemsSource = dt.DefaultView;
        }

        private void priviList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this._currRow = sender;
        }
    }
}