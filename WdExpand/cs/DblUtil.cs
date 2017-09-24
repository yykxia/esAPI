
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace IETCsoft.DBL
{
    class DblUtil
    {
        static SqlConnection _conn;
        /// <summary>
        /// 获取连接
        /// </summary>
        /// <returns></returns>
        public static SqlConnection getConn()
        {
            return new SqlConnection(DblSetting.ConnStr);
            //if (_conn == null)
            //{
            //    if (DblSetting.ConnStr == string.Empty) throw new Exception("请设置数据库连接串 DblSetting.ConnStr");
            //    _conn = new SqlConnection(DblSetting.ConnStr);
            //}
            //return _conn;
        }
    }

    public class DblSetting
    {
        public static readonly string ConnStr = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

    }
}