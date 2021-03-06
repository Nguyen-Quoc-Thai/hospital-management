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
            List<Combobox> TableList1 = new List<Combobox>();
            List<Combobox> TableList2 = new List<Combobox>();

            //DataTable dt1 = db.Query("SELECT table_name FROM user_tables");
            //DataTable dt1 = db.Query("SELECT table_name FROM all_tables WHERE last_analyzed IS NULL AND table_name <> 'PSTUBTBL' MINUS SELECT table_name FROM all_tables WHERE last_analyzed IS NULL AND(REGEXP_LIKE(table_name, '\\$', 'i') OR REGEXP_LIKE(table_name, '\\_.', 'i')) AND table_name <> 'HOSO_DICHVU' ORDER BY table_name ASC");
            DataTable dt1 = db.Query("SELECT table_name FROM all_tables WHERE table_name <> 'PSTUBTBL' MINUS SELECT table_name FROM all_tables WHERE (REGEXP_LIKE(table_name, '\\$', 'i') OR REGEXP_LIKE(table_name, '\\_.', 'i')) AND table_name <> 'HOSO_DICHVU' ORDER BY table_name ASC");
            DataTable dt2 = db.Query("SELECT ROLE FROM DBA_ROLES");
            List<DataRow> dataRows_pos1 = dt1.GetRows();
            List<DataRow> dataRows_pos2 = dt2.GetRows();
            
            try
            {
                foreach (DataRow row in dataRows_pos1)
                {
                    TableList1.Add(new Combobox()
                    {
                        DisplayName = row.GetData(0),
                        SelectedValue = row.GetData(0)
                    });
                }
                cbbTables.ItemsSource = TableList1;
                
                foreach (DataRow row in dataRows_pos2)
                {
                    TableList2.Add(new Combobox()
                    {
                        DisplayName = row.GetData(0),
                        SelectedValue = row.GetData(0)
                    });
                }
                cbbRoles.ItemsSource = TableList2;
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
                string privileges = "";
                if (grantSelect.IsChecked == true)
                    privileges += "SELECT, ";
                if (grantUpdate.IsChecked == true)
                    privileges += "UPDATE, ";
                if (grantInsert.IsChecked == true)
                    privileges += "INSERT, ";
                if (grantDelete.IsChecked == true)
                    privileges += "DELETE ";

                privileges = privileges.Trim();
                if (privileges.EndsWith(","))
                    privileges = privileges.Remove(privileges.Length - 1, 1);

                string tb_name = cbbTables.Text;
                string checkedColumns = this.GetCheckedColumns();

                try
                {
                    String view_name = ("V_USER_" + tb_name + "_" + privileges).Replace(',', '_').Replace(" ", "");
                    OracleCommand cmd2 = new OracleCommand("CREATE_VIEW_HIDE_COLUMNS", db.Conn);
                    cmd2.CommandType = CommandType.StoredProcedure;

                    cmd2.Parameters.Add("tb_name", OracleDbType.Varchar2).Value = tb_name;
                    cmd2.Parameters.Add("v_name", OracleDbType.Varchar2).Value = view_name;
                    cmd2.Parameters.Add("str_select", OracleDbType.Varchar2).Value = checkedColumns;

                    cmd2.ExecuteNonQuery();


                    // Grant view to user
                    char withGrant = withGrantOption.IsChecked == true ? 'T' : 'F';
                    OracleCommand cmd = new OracleCommand("GRANT_PRIVILEGES_TO", db.Conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("tb_name", OracleDbType.Varchar2).Value = view_name;
                    cmd.Parameters.Add("str_priv", OracleDbType.Varchar2).Value = privileges;
                    cmd.Parameters.Add("USER", OracleDbType.Varchar2).Value = dr[0].ToString();
                    cmd.Parameters.Add("with_grant", OracleDbType.Char).Value = withGrant;

                    cmd.ExecuteNonQuery();

                    // Update view
                    this.userList_SelectionChanged(userList, null);

                    MessageBox.Show("Grant thành công");
                }
                catch
                {
                    MessageBox.Show("Fail to grant");
                }
            }
            else
            {
                MessageBox.Show("Chọn một người dùng!");
            }
        }

        private void revokeClick(object sender, RoutedEventArgs e)
        {
            DataRowView dr = priviList.SelectedItem as DataRowView;

            try
            {
                if (_currRow == null)
                {
                    MessageBox.Show("Chọn một dòng trong bảng danh sách quyền/role để thu hồi!");
                }
                else
                {
                    // Call store proc
                    OracleCommand cmd = new OracleCommand("REVOKE_PRIVILEGES_FROM", db.Conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("tb_name", OracleDbType.Varchar2).Value = dr[4].ToString();
                    cmd.Parameters.Add("str_priv", OracleDbType.Varchar2).Value = dr[1].ToString();
                    cmd.Parameters.Add("user", OracleDbType.Varchar2).Value = dr[0].ToString();

                    cmd.ExecuteNonQuery();

                    // Update view
                    this.userList_SelectionChanged(_currRow, null);

                    this._currRow = null;

                    MessageBox.Show("Revoke privilege success!");
                }
            }catch(Exception ex)
            {
                MessageBox.Show("Đây là role, không phải privilege");
            }

        }

        private void userList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;

            table2_title.Text = "Danh sách quyền của user " + dr[0].ToString();
            var sql = "SELECT GRANTEE, GRANTED_ROLE AS ROLE_PRIVILEGES, ADMIN_OPTION AS OWNER, DELEGATE_OPTION AS GRANTOR, COMMON AS TABLE_NAME FROM DBA_ROLE_PRIVS WHERE GRANTEE='" + dr[0].ToString() + "' UNION ALL SELECT GRANTEE, PRIVILEGE, OWNER, GRANTOR, TABLE_NAME FROM DBA_TAB_PRIVS WHERE GRANTEE='" + dr[0].ToString() + "'";

            DataTable dt = db.Query(sql);
            priviList.ItemsSource = dt.DefaultView;
        }

        private void priviList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this._currRow = sender;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (cbbRoles.SelectedIndex <= -1)
            {
                MessageBox.Show("Hãy chọn một role");
            };

            DataRowView dr = userList.SelectedItem as DataRowView;

            if (dr != null)
            {
                try
                {
                    // Grant role to user
                    char withGrant = withGrantOption.IsChecked == true ? 'T' : 'F';
                    OracleCommand cmd = new OracleCommand("GRANT_ROLE_TO", db.Conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("role_name", OracleDbType.Varchar2).Value = cbbRoles.Text;
                    cmd.Parameters.Add("USER", OracleDbType.Varchar2).Value = dr[0].ToString();
                    cmd.Parameters.Add("with_grant", OracleDbType.Char).Value = withGrant;

                    cmd.ExecuteNonQuery();

                    // Update view
                    this.userList_SelectionChanged(userList, null);

                    MessageBox.Show("Grant thành công");
                }
                catch
                {
                    MessageBox.Show("Fail to grant");
                }
            }
            else
            {
                MessageBox.Show("Chọn một người dùng!");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DataRowView dr = priviList.SelectedItem as DataRowView;

            try
            {
                if (_currRow == null)
                {
                    MessageBox.Show("Chọn một dòng trong bảng danh sách quyền/role để thu hồi!");
                }
                else
                {
                    // Call store proc
                    OracleCommand cmd = new OracleCommand("REVOKE_ROLE_FROM", db.Conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("role_name", OracleDbType.Varchar2).Value = dr[1].ToString();
                    cmd.Parameters.Add("USER", OracleDbType.Varchar2).Value = dr[0].ToString();

                    cmd.ExecuteNonQuery();

                    // Update view
                    this.userList_SelectionChanged(_currRow, null);

                    this._currRow = null;

                    MessageBox.Show("Revoke role success!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đây là privilege, không phải role");
            }

        }
    }
}