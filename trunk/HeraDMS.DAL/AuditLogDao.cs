using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using HeraDMS.Core;

namespace HeraDMS.DAL
{
    public class AuditLogDao
    {
        /// <summary>
        /// 获取审核日志（包括全部或者翻页）
        /// </summary>
        /// <param name="flag">Y:表示翻页查询；N表示全部查询</param>
        /// <param name="siteId">网站ID</param>
        /// <param name="listId">列表ID</param>
        /// <param name="itemId">列表项ID</param>
        /// <param name="dtS">开始时间</param>
        /// <param name="dtE">结束时间</param>
        /// <param name="eventType">事件类型（如 1,2,3   的格式）</param>
        /// <param name="userId">用户ID</param>
        /// <param name="pageSize">每页多少项</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageCount">返回参数：一共多少项</param>
        /// <returns></returns>
        private static DataTable GetAuditLogData(char flag, string siteId, string listId, string itemId, DateTime dtS,
                                        DateTime dtE, string eventType, int userId, int pageSize, int pageIndex,
                                        out int pageCount)
        {
            pageCount = 0;
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@Flag", flag));
            paramList.Add(new SqlParameter("@SiteId", siteId));
            paramList.Add(new SqlParameter("@ListId", listId));
            if (!String.IsNullOrEmpty(itemId))
            {
                paramList.Add(new SqlParameter("@ItemId", itemId));
            }
            else
            {
                paramList.Add(new SqlParameter("@ItemId", DBNull.Value));
            }
            if (dtS != null && DateTime.MinValue != dtS)
            {
                paramList.Add(new SqlParameter("@StartTime", dtS));
            }
            else
            {
                paramList.Add(new SqlParameter("@StartTime", DBNull.Value));
            }
            if (dtE != null && DateTime.MaxValue != dtE)
            {
                paramList.Add(new SqlParameter("@EndTime", dtE));
            }
            else
            {
                paramList.Add(new SqlParameter("@EndTime", DBNull.Value));
            }
            paramList.Add(new SqlParameter("@EventCSV", eventType));
            if (userId != 0)
            {
                paramList.Add(new SqlParameter("@UserId", userId));
            }
            else
            {
                paramList.Add(new SqlParameter("@UserId", DBNull.Value));
            }
            paramList.Add(new SqlParameter("@PageSize", pageSize));
            paramList.Add(new SqlParameter("@PageIndex", pageIndex));
            SqlParameter donePageCountParam = new SqlParameter("@PageCount", pageCount);
            donePageCountParam.Direction = ParameterDirection.Output;
            paramList.Add(donePageCountParam);
            DataTable dt = SqlHelper.ExecuteDataTable(SqlHelper.ContentDBString, CommandType.StoredProcedure, "SP_GetAuditData", paramList.ToArray());
            pageCount = Convert.ToInt32(paramList.ToArray()[paramList.Count - 1].Value);
            return dt;
        }

        /// <summary>
        /// 根据查询条件获取日志（适用翻页）
        /// </summary>
        /// <param name="siteId">网站ID</param>
        /// <param name="listId">列表ID</param>
        /// <param name="itemId">列表项ID</param>
        /// <param name="dtS">开始时间</param>
        /// <param name="dtE">结束时间</param>
        /// <param name="eventType">事件类型（如 1,2,3   的格式）</param>
        /// <param name="userId">用户ID</param>
        /// <param name="pageSize">每页多少项</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageCount">返回参数：一共多少项</param>
        /// <returns></returns>
        public static DataTable LoadAuditLog(string siteId, string listId, string itemId, DateTime dtS,
                                        DateTime dtE, string eventType, int userId, int pageSize, int pageIndex,
                                        out int pageCount)
        {
            char c = 'Y';
            return GetAuditLogData(c, siteId, listId, itemId, dtS, dtE, eventType, userId, pageSize, pageIndex, out pageCount);
        }

        /// <summary>
        /// 根据查询条件获取所有日志
        /// </summary>
        /// <param name="siteId">网站ID</param>
        /// <param name="listId">列表ID</param>
        /// <param name="itemId">列表项ID</param>
        /// <param name="dtS">开始时间</param>
        /// <param name="dtE">结束时间</param>
        /// <param name="eventType">事件类型（如 1,2,3   的格式）</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public static DataTable LoadAuditLog(string siteId, string listId, string itemId, DateTime dtS,
                                        DateTime dtE, string eventType, int userId)
        {
            char c = 'N';
            int pageCount = 0;
            return GetAuditLogData(c, siteId, listId, itemId, dtS, dtE, eventType, userId, 0, 0, out pageCount);
        }
    }
}
