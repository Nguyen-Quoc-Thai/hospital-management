using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Controls;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
namespace PH1_QTCSDL
{
    class OracleDatabase
    {

        //Singleton
        private static OracleDatabase instance = new OracleDatabase(@"serverconfig.txt");
        public static OracleDatabase Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new OracleDatabase(@"serverconfig.txt");
                }
                return instance;
            }
        }


        private OracleConnection _conn = null;
        private OracleDataReader _dr = null;
        public OracleDatabase(string path)
        {
            string connectConfig = System.IO.File.ReadAllText(path);
            _conn = new OracleConnection(connectConfig);

            try
            {
                _conn.Open();
            }
            catch
            {
                throw new ArithmeticException("Check lai link database Config di dkm");
            }
            
        }

        /// <summary>
        /// Query by string
        /// </summary>
        /// <param name="querystr"></param>
        /// <returns></returns>
        public DataTable Query(string querystr)
        {
            OracleCommand cmd = _conn.CreateCommand();
            cmd.CommandText = querystr;
            cmd.CommandType = CommandType.Text;
            _dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(_dr);
            _dr.Close();
            return dt;
        }

    }
}
