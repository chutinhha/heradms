using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Configuration;

namespace HeraDMS.Core
{
    /// <summary>
    /// Summary description for SqlHelper
    /// </summary>
    public sealed class SqlHelper
    {
        /// <summary>
        /// 连接内容数据库字符串
        /// </summary>
        public static readonly string ContentDBString = ConfigurationManager.ConnectionStrings["ContentDB"].ConnectionString;
        // Hashtable缓存SQL参数
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());
        /// <summary>
        /// 执行一个Sql语句(insert、update、delete)，该方法没有返回数据集(select语句有返回的数据集)
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个数据库连接字符串</param>
        /// <param name="commandType">执行命令的类型CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">执行的存储过程的名字或者T-SQL语句</param>
        /// <param name="commandParameters">Sql参数数组,用于执行命令</param>
        /// <returns>返回受影响的行的行数</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {

            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// 执行一个Sql语句(insert、update、delete)，该方法没有返回数据集(select语句有返回的数据集)
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">一个SqlConnection对象,代表一个数据库的连接</param>
        /// <param name="commandType">执行命令的类型CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">执行的存储过程的名字或者T-SQL语句</param>
        /// <param name="commandParameters">Sql参数数组,用于执行命令</param>
        /// <returns>返回受影响的行的行数</returns>
        public static int ExecuteNonQuery(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {

            SqlCommand cmd = new SqlCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// 使用一个SQL事务,执行一个SqlCommand(包含了命令的信息)
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="trans">一个SQL事务对象</param>
        /// <param name="commandType">执行命令的类型CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">执行的存储过程的名字或者T-SQL语句</param>
        /// <param name="commandParameters">Sql参数数组,用于执行命令</param>
        /// <returns>返回受影响的行的行数</returns>
        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// 执行一个SqlCommand(包含要执行命令信息),返回一个数据集(SqlDataReader)
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个数据库连接字符串</param>
        /// <param name="commandType">执行命令的类型CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">执行的存储过程的名字或者T-SQL语句</param>
        /// <param name="commandParameters">Sql参数数组,用于执行命令</param>
        /// <returns>返回一个SqlDataReader对象包含查询的结果集</returns>
        public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }
        /// <summary>
        /// 断线模式执行一个SqlCommand(包含要执行命令信息),返回一个数据集(DataTable)
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个数据库连接字符串</param>
        /// <param name="commandType">执行命令的类型CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">执行的存储过程的名字或者T-SQL语句</param>
        /// <param name="commandParameters">Sql参数数组,用于执行命令</param>
        /// <returns>返回一个DataTable对象包含查询的结果集</returns>
        public static DataTable ExecuteDataTable(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);
            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        /// <summary>
        /// 执行一个SqlCommand(包含要执行命令信息),返回第一行第一列的数据
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个数据库连接字符串</param>
        /// <param name="commandType">执行命令的类型CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">执行的存储过程的名字或者T-SQL语句</param>
        /// <param name="commandParameters">Sql参数数组,用于执行命令</param>
        /// <returns>返回一个Object对象，该对象可以被转换为预先设定好的类型</returns>
        public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// 执行一个SqlCommand(包含要执行命令信息),返回第一行第一列的数据
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">一个SqlConnection对象,代表一个数据库的连接</param>
        /// <param name="commandType">执行命令的类型CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">执行的存储过程的名字或者T-SQL语句</param>
        /// <param name="commandParameters">Sql参数数组,用于执行命令</param>
        /// <returns>返回一个Object对象，该对象可以被转换为预先设定好的类型</returns>
        public static object ExecuteScalar(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {

            SqlCommand cmd = new SqlCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }



        /// <summary>
        /// add parameter array to the cache
        /// </summary>
        /// <param name="cacheKey">Key to the parameter cache</param>
        /// <param name="cmdParms">an array of SqlParamters to be cached</param>
        public static void CacheParameters(string cacheKey, params SqlParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }

        /// <summary>
        /// Retrieve cached parameters
        /// </summary>
        /// <param name="cacheKey">key used to lookup parameters</param>
        /// <returns>Cached SqlParamters array</returns>
        public static SqlParameter[] GetCachedParameters(string cacheKey)
        {
            SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }

        /// <summary>
        /// Prepare a command for execution
        /// </summary>
        /// <param name="cmd">SqlCommand object</param>
        /// <param name="conn">SqlConnection object</param>
        /// <param name="trans">SqlTransaction object</param>
        /// <param name="cmdType">Cmd type e.g. stored procedure or text</param>
        /// <param name="cmdText">Command text, e.g. Select * from Products</param>
        /// <param name="cmdParms">SqlParameters to use in the command</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }


        //Run SQL
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="strSql"></param>
        public void ExecuteSQL(string strSql)
        {
            SqlConnection conn = new SqlConnection(ContentDBString);
            SqlCommand cmd = new SqlCommand(strSql, conn);
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        public static DataSet GetDataSet(string strSQL)
        {
            SqlConnection sqlConn = new SqlConnection(ContentDBString);
            SqlDataAdapter sdr = new SqlDataAdapter(strSQL, sqlConn);
            try
            {
                sqlConn.Open();
                DataSet ds = new DataSet();
                sdr.Fill(ds);
                sqlConn.Close();
                return ds;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
