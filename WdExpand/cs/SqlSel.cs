using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;

namespace IETCsoft.sql
{
    public class SqlSel
    {
        static string getconn()
        {
            string ConnectionString = "server=jconn2n8exx5b.sqlserver.rds.aliyuncs.com,3433;uid=jusrj2y5fv9t;pwd=mbh_2014;database=mbh_wdgj31;";
            return ConnectionString;
        }
        public static bool GetSqlSel(ref DataTable ODT, string SQL)
        {
            string ConnectionString = getconn();
            SqlConnection _SqlConnection1 = new SqlConnection();
            SqlCommand sc = new SqlCommand();
            try
            {
                if (_SqlConnection1.State != ConnectionState.Open)
                {
                    _SqlConnection1.ConnectionString = ConnectionString;
                    _SqlConnection1.Open();
                }
                //开始填充
                string sqlCmd = SQL;
                sc.Connection = _SqlConnection1;
                sc.CommandText = sqlCmd;
                SqlDataAdapter sda = new SqlDataAdapter(sc);
                ODT = new DataTable();
                sda.Fill(ODT);
                if (ODT.Rows.Count == 0)
                {
                    return false;
                }
                return true;
            }
            catch 
            {
                return false;
            }
            finally
            {
                _SqlConnection1.Close();
            }
        }

        public static object GetSqlScale(string SQL)
        {
            string ConnectionString = getconn();
            SqlConnection _SqlConnection1 = new SqlConnection();
            SqlCommand sc = new SqlCommand();
            try
            {
                if (_SqlConnection1.State != ConnectionState.Open)
                {
                    _SqlConnection1.ConnectionString = ConnectionString;
                    _SqlConnection1.Open();
                }
                //开始填充
                string sqlCmd = SQL;
                sc.Connection = _SqlConnection1;
                sc.CommandText = sqlCmd;
                return sc.ExecuteScalar();
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                _SqlConnection1.Close();
            }
        }

        public static int ExeSql(string SQL)
        {
            string ConnectionString = getconn();
            SqlConnection _SqlConnection1 = new SqlConnection();
            SqlCommand sc = new SqlCommand();
            try
            {
                if (_SqlConnection1.State != ConnectionState.Open)
                {
                    _SqlConnection1.ConnectionString = ConnectionString;
                    _SqlConnection1.Open();
                }
                //开始执行
                string sqlCmd = SQL;
                sc.Connection = _SqlConnection1;
                sc.CommandText = sqlCmd;
                return sc.ExecuteNonQuery();
            }
            catch
            {
                return 0;
            }
            finally
            {
                _SqlConnection1.Close();
            }
        }
    }
}
