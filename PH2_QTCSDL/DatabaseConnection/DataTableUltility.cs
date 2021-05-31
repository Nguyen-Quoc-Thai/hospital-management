using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PH2_QTCSDL
{
    static class DataTableUltility
    {
        /// <summary>
        /// Get List String at Rows
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="atCol"></param>
        /// <returns>List string at column</returns>
        public static List<string> GetRowsName(this DataTable dt , int atCol)
        {
            List<string> result = new List<string>();
            List<DataRow> dataRow = new List<DataRow>();
            dataRow = dt.AsEnumerable().ToList();

            foreach(DataRow row in dataRow)
            {
                result.Add(row.ItemArray[atCol].ToString());
            }
            return result;
        }

        /// <summary>
        /// Get Rows from DataTabase
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>List DataRow</returns>
        public static List<DataRow> GetRows(this DataTable dt,bool sort = false)
        {

            return dt.AsEnumerable().ToList();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtr"></param>
        /// <param name="atRow"></param>
        /// <returns>Value as string type</returns>
        public static string GetData(this DataRow dtr, int atRow)
        {
            return dtr.ItemArray[atRow].ToString();
        }
    }
}
