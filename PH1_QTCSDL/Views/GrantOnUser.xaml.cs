﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PH1_QTCSDL.Views
{
    /// <summary>
    /// Interaction logic for Grant.xaml
    /// </summary>
    public partial class GrantOnUser : UserControl
    {
        OracleDatabase db;

        public GrantOnUser()
        {
            InitializeComponent();
            Window_Loaded();
        }

        private void Window_Loaded()
        {
            db = OracleDatabase.Instance;

            this.UpdateDataGrid();
            this.LoadCombobox();
        }

        private void UpdateDataGrid()
        {
            DataTable dt = db.Query("SELECT MANV, HOTEN, LUONG, VAITRO, DONVI FROM NHANVIEN ORDER BY MANV DESC");
            userList.ItemsSource = dt.DefaultView;
        }

        private void LoadCombobox()
        {
            List<Combobox> TableList = new List<Combobox>();
            //TableList.Add(new Combobox()
            //{
            //    DisplayName = "Nhân Viên",
            //    SelectedValue = "NHANVIEN"
            //});
            //TableList.Add(new Combobox()
            //{
            //    DisplayName = "Chấm Công",
            //    SelectedValue = "CHAMCONG"
            //});
            // ......

            DataTable dt1 = db.Query("SELECT table_name FROM user_tables");
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
                string queryString = "SELECT column_name FROM all_tab_columns WHERE table_name = '" + tableName + "'";
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
            string checkedColumns = this.GetCheckedColumns();
            MessageBox.Show("Checked Columns: " + checkedColumns);

            if (this.checkSubmit() == -1)
                return;

            DataRowView dr = userList.SelectedItem as DataRowView;

            if (dr != null)
            {
                // GRANT....
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
                MessageBox.Show("GRANT " + privileges + " ON " + cbbTables.Text + " TO " + dr["HOTEN"]);

                try
                {
                    string withGrant = withGrantOption.IsChecked == true ? "T" : "F";
                    string proc = "GRANT_PRIVILEGES_TO(" + cbbTables.Text + ", " + "str_priv = " + privileges + ", " + dr["HOTEN"] + ", " + withGrant + ")";
                    db.Query(proc);
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

        private void revokeClick(object sender, RoutedEventArgs e)
        {
            string checkedColumns = this.GetCheckedColumns();
            MessageBox.Show("Checked Columns: " + checkedColumns);

            if (this.checkSubmit() == -1)
                return;

            DataRowView dr = userList.SelectedItem as DataRowView;

            if (dr != null)
            {
                // GRANT....
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
                MessageBox.Show("REVOKE " + privileges + " ON " + cbbTables.Text + " FROM " + dr["HOTEN"]);

                try
                {
                    string withGrant = withGrantOption.IsChecked == true ? "T" : "F";
                    string proc = "REVOKE_PRIVILEGES_FROM(" + cbbTables.Text + ", " + "str_priv = " + privileges + ", " + dr["HOTEN"] + ")";
                    db.Query(proc);
                }
                catch
                {
                    MessageBox.Show("fail to revoke");
                }
            }
            else
            {
                MessageBox.Show("Chọn một người dùng!");
            }
        }

        private void userList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
        }
    }
}
