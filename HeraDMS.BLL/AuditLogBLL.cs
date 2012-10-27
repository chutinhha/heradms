using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using HeraDMS.DAL;

namespace HeraDMS.BLL
{
    public class AuditLogBLL
    {

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
            return AuditLogDao.LoadAuditLog(siteId, listId, itemId, dtS, dtE, eventType, userId, pageSize, pageIndex, out pageCount);
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
            return AuditLogDao.LoadAuditLog(siteId, listId, itemId, dtS, dtE, eventType, userId);
        }
    }
}
