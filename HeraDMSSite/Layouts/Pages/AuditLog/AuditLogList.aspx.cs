using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.IO;
using System.Data;
using HeraDMS.Layouts.Helper;
using System.Web;
using Microsoft.SharePoint.Linq;
using System.Data.SqlClient;
using HeraDMS.BLL;

namespace HeraDMS.Layouts.AuditLog
{
    public partial class AuditLogList : BasePage
    {
        #region 参数设置和初始化对象
        /// <summary>
        /// 当前文档库的ID
        /// </summary>
        private string ListId
        {
            get {
                return ViewState["AuditLogList_ListId"]+"";
            }
            set
            {
                ViewState["AuditLogList_ListId"] = value;
            }
        }
        /// <summary>
        /// 当前文档库中被选择的文档的ID
        /// </summary>
        private string ItemId
        {
            get
            {
                return ViewState["AuditLogList_ItemId"] + "";
            }
            set
            {
                ViewState["AuditLogList_ItemId"] = value;
            }
        }
        /// <summary>
        /// 导出文件的名称
        /// </summary>
        private string ExportFileTitle
        {
            get
            {
                return ViewState["ExportFileTitle"] + "";
            }
            set
            {
                ViewState["ExportFileTitle"] = value;
            }
        }
        /// <summary>
        /// 设置每页显示多少行
        /// </summary>
        private Int32 PageSize
        {
            get
            {
                return String.IsNullOrEmpty(ViewState["OPCPageSize"] + "") ? 10 : Convert.ToInt32(ViewState["OPCPageSize"] + "");
            }
            set
            {
                ViewState["OPCPageSize"] = value;
            }
        }
        /// <summary>
        /// 设置当前显示多少页
        /// </summary>
        private Int32 PageIndex
        {
            get
            {
                return String.IsNullOrEmpty(ViewState["OPCPageIndex"] + "") ? -1 : Convert.ToInt32(ViewState["OPCPageIndex"] + "");
            }
            set
            {
                ViewState["OPCPageIndex"] = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.gvData.Attributes.Add("style", "word-break:break-all;word-wrap:break-word"); 
            if (!IsPostBack)
            {

                ListId = (Request.QueryString["listID"] != null) ? Request.QueryString["listID"].ToString().Trim().ToLower() : "";
                ItemId = (Request.QueryString["itemID"] != null) ? Request.QueryString["itemID"].ToString().Trim().ToLower() : "";
                SetTitle();
                PageIndex = 1;
                //this.AspNetPager1.PageIndex = PageIndex;
                BindData();
            }
        }

        /// <summary>
        /// 设置dialog的页面标题
        /// </summary>
        private void SetTitle()
        { 
            if(String.IsNullOrEmpty(ListId))
            {
                return;
            }

            //取得列表
            SPList list = SPContext.Current.Web.Lists[new Guid(ListId)];

            //显示列表的名称
            if (String.IsNullOrEmpty(ItemId))
            {
                ExportFileTitle = "文档库" + list.Title + "的历史记录";
            }
            //显示文档名称
            else
            {
                //获取该文档
                SPListItem item = list.Items.GetItemById(Convert.ToInt32(ItemId));
                ExportFileTitle = "文档" + item.DisplayName + "的历史记录";
            }
            this.Page.Title = ExportFileTitle;
        }
        #region 按钮事件
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            PageIndex = AspNetPager1.PageIndex;
            //this.gvData.PageIndex = AspNetPager1.PageIndex;
            BindData();

        }

        protected void BindData()
        {
            if (String.IsNullOrEmpty(ListId))
            {
                return;
            }
            DateTime dtS = DateTime.MinValue;
            DateTime dtE = DateTime.MaxValue;
            //事件类型
            string eventType = "";
            //获取当前选择用户的ID
            int userId = 0;
            string siteId = "";

            if (!dateStart.IsDateEmpty)
            {
                dtS = dateStart.SelectedDate;
            }
            
            if (!dateEnd.IsDateEmpty)
            {
                dtE = dateEnd.SelectedDate;
            }
            
            if (userPicker.ResolvedEntities.Count > 0)
            {
                PickerEntity pe = (PickerEntity)userPicker.ResolvedEntities[0];
                userId = String.IsNullOrEmpty(pe.EntityData["SPUserID"] + "") ? Convert.ToInt32(pe.EntityData["SPUserID"] + "") : 0;
                //GetUserId(pe.Key);
            }

            SPSite siteColl = SPContext.Current.Site;
            siteId = siteColl.ID.ToString();

            #region 页面事件的查询条件
            //添加选择的判断条件
            foreach (ListItem item in cbHandle.Items)
            {
                if (item.Selected)
                {
                    eventType = eventType + item.Value + ",";
                    
                }
            }
            if (!String.IsNullOrEmpty(eventType))
            {
                eventType = eventType.Substring(0, eventType.Length-1);
            }
            #endregion

            #region 插入用户数据到数据库
            int pageCount=0;
            
            DataTable dt = AuditLogBLL.LoadAuditLog(siteId,ListId,ItemId,dtS,dtE,eventType,userId,PageSize,
                                                    PageIndex,out pageCount);
            #endregion
            if (dt == null || dt.Rows.Count == 0)
            {
                AspNetPager1.Visible = false;
            }
            else
            {
                AspNetPager1.Visible = true;
            }
            this.AspNetPager1.PageIndex = PageIndex;
            AspNetPager1.PageSize = PageSize;
            AspNetPager1.RecordCount = pageCount;
            this.gvData.DataSource = dt;
            this.gvData.DataBind();

        }

        /// <summary>
        /// 查询数据绑定(使用sharepoint的API方法，放弃)
        /// </summary>
        protected void BindGrid()
        {
            if (String.IsNullOrEmpty(ListId))
            {
                return;
            }
            DateTime dtS = DateTime.MinValue;
            if (!dateStart.IsDateEmpty)
            {
                dtS = dateStart.SelectedDate;
            }
            DateTime dtE = DateTime.MaxValue;
            if (!dateEnd.IsDateEmpty)
            {
                dtE = dateEnd.SelectedDate;
            }
            //获取当前选择用户的ID
            int userId=0;
            if (userPicker.ResolvedEntities.Count > 0)
            {
                PickerEntity pe = (PickerEntity)userPicker.ResolvedEntities[0];
                userId = String.IsNullOrEmpty(pe.EntityData["SPUserID"]+"") ? Convert.ToInt32(pe.EntityData["SPUserID"] + "") : 0;
                //GetUserId(pe.Key);
            }
            //foreach (PickerEntity pe in userPicker.ResolvedEntities)
            //{
            //    string principalType = pe.EntityData["PrincipalType"].ToString();

            //    string loginName = pe.Key;
               
            //}
            SPSite siteColl = SPContext.Current.Site;
            SPWeb site = SPContext.Current.Web;

            SPSecurity.RunWithElevatedPrivileges(
          delegate()
          {
              using (SPSite ElevatedSiteCollection = new SPSite(siteColl.ID))
              {
                  using (SPWeb ElevatedSite = ElevatedSiteCollection.OpenWeb(site.ID))
                  {

                      //取得列表
                      SPList list = ElevatedSite.Lists[new Guid(ListId)];

                      SPAuditQuery wssQuery = new SPAuditQuery(ElevatedSiteCollection);

                      SPAuditEntryCollection auditCol;
                      //SPList list = siteCollection.OpenWeb().Lists["公司共享文档"];
                      wssQuery.RestrictToList(list);

                      //当是选择了某个文档库的某个文件的时候
                      if (!String.IsNullOrEmpty(ItemId))
                      {
                          //获取该文档
                          SPListItem item = list.Items.GetItemById(Convert.ToInt32(ItemId));
                          wssQuery.RestrictToListItem(item);
                      }

                      #region 页面事件的查询条件
                      //判断页面的事件查询条件是否选择
                      bool isChecked = false;
                      //添加选择的判断条件
                      foreach (ListItem item in cbHandle.Items)
                      {
                          if (item.Selected)
                          {
                              isChecked = true;
                              if (item.Value.Equals("3"))
                              {
                                  //查看
                                  wssQuery.AddEventRestriction(SPAuditEventType.View);
                              }
                              else if (item.Value.Equals("5"))
                              {
                                  //更新
                                  wssQuery.AddEventRestriction(SPAuditEventType.Update);
                              }
                              else if (item.Value.Equals("4"))
                              {
                                  //删除
                                  wssQuery.AddEventRestriction(SPAuditEventType.Delete);
                              }
                              else if (item.Value.Equals("2"))
                              {
                                  //签入
                                  wssQuery.AddEventRestriction(SPAuditEventType.CheckIn);
                              }
                              else if (item.Value.Equals("1"))
                              {
                                  //签出
                                  wssQuery.AddEventRestriction(SPAuditEventType.CheckOut);
                              }
                              else if (item.Value.Equals("10"))
                              {
                                  //从回收站还原
                                  wssQuery.AddEventRestriction(SPAuditEventType.Undelete);
                              }
                          }
                      }
                      //当没选择事件的时候，默认添加以下条件
                      if (!isChecked)
                      {
                          //查看
                          wssQuery.AddEventRestriction(SPAuditEventType.View);
                          //更新
                          wssQuery.AddEventRestriction(SPAuditEventType.Update);
                          //删除
                          wssQuery.AddEventRestriction(SPAuditEventType.Delete);
                          //签入
                          wssQuery.AddEventRestriction(SPAuditEventType.CheckIn);
                          //签出
                          wssQuery.AddEventRestriction(SPAuditEventType.CheckOut);
                          //从回收站还原
                          wssQuery.AddEventRestriction(SPAuditEventType.Undelete);
                      }
                      #endregion

                      wssQuery.SetRangeStart(dtS);
                      wssQuery.SetRangeEnd(dtE);
                      
                      if (userId != 0)
                      {
                          wssQuery.RestrictToUser(userId);
                      }
                      auditCol = ElevatedSiteCollection.Audit.GetEntries(wssQuery);
                      //List<SPAuditEntry> data = new List<SPAuditEntry>();
                      //foreach (SPAuditEntry item in auditCol)
                      //{
                      //    item.ItemType=SPAuditItemType.Document
                      //    data.Add(item);
                      //}
                    
                      
                      this.AspNetPager1.PageIndex = PageIndex;
                      AspNetPager1.PageSize = PageSize;
                      AspNetPager1.RecordCount = auditCol.Count;
                      this.gvData.DataSource = auditCol;
                      this.gvData.DataBind();
      
                      // enumerate through audit log and read entries
                  }
              }

          });
        }

        /// <summary>
        /// 根据用户ID获取用户名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GetUserId(string id)
        {
            string name = "";
            SPSecurity.RunWithElevatedPrivileges(
          delegate()
          {
              SPWeb web = SPContext.Current.Web;
              SPUser user = web.SiteUsers.GetByID(Convert.ToInt32(id));

              if (user != null)
              {
                  name = user.Name;
              }
          });
            return name;

        }

        /// <summary>
        /// 获取事件类型的中文名称
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        private string GetEventName(string eventType)
        { 
            switch (eventType)
            {
                case "View":
                    return "查看";
                case "Update":
                    return "更新";
                case "Delete":
                    return "删除";
                case "CheckIn":
                    return "签入";
                case "CheckOut":
                    return "签出";
                case "Undelete":
                    return "从回收站还原";
                default:
                    return "未知";
            }
        }

        /// <summary>
        /// grid行绑定事件(废弃)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex >= 0)
            {
                string userId = e.Row.Cells[0].Text;
                e.Row.Cells[0].Text = GetUserId(userId);

                string eventType = e.Row.Cells[1].Text;
                e.Row.Cells[1].Text = GetEventName(eventType);
            }
        }
        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearSelect();
        }

        /// <summary>
        /// 清空查询条件
        /// </summary>
        private void ClearSelect()
        {
            this.gvData.DataSource = null;
            this.gvData.DataBind();

            this.dateEnd.ClearSelection();
            this.dateStart.ClearSelection();
            this.userPicker.CommaSeparatedAccounts = "";

            foreach (ListItem item in cbHandle.Items)
            {
                item.Selected = true;
            }
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (this.gvData!=null&&this.gvData.Rows.Count > 0)
            {
                string[] colNameEn = new string[] { "UserId", "Event", "DocLocation", "Occurred", "EventSource" };
                string[] colNameCh = new string[] { "用户", "事件", "文档路径", "时间", "事件源" };

                string strDownloadFileName = string.Empty;
                string strFileName = string.Empty;
                string strExcelConn = string.Empty;

                //下载文件名必须按照这个规范！！！
                strFileName = ExportFileTitle + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                strDownloadFileName = "/DownloadFile/" + strFileName;

                // 验证文件保存目录
                if (!Directory.Exists(MapPath("/DownloadFile/")))
                {
                    Directory.CreateDirectory(MapPath("/DownloadFile/"));
                }
                DataTable dt = GetExportData(colNameEn);
 
                if (null != dt && dt.Rows.Count > 0)
                {

                    dt = HeraDMS.Layouts.Helper.Common.ChangesCH(dt, colNameCh);

                    if (NPOIHelper.Export(dt, "审计日志", Server.MapPath(strDownloadFileName)))
                    {
                        //必须用这种方式下载，在现有Ajax框架下千万不能用response.redirect跳转
                        base.JavascriptGoUrlByAjaxNow("../Common/DownloadFile.aspx?FilePath=" + HttpUtility.UrlEncode(strDownloadFileName) + "&FileName=" + HttpUtility.UrlEncode(strFileName));
                    }
                    else
                    {
                        base.AlertMessage("错误提示", "导出提示：导出失败");
                    }
                }
                else
                {
                    base.AlertMessage("提示", "没有可以导出的数据！");
                }
            }
            else
            {
                base.AlertMessage("提示", "没有可以导出的数据！");
            }
        }

        /// <summary>
        /// 获取导出的数据
        /// </summary>
        /// <returns></returns>
        private DataTable GetExportData(string[] colNameEn)
        {
            if (String.IsNullOrEmpty(ListId))
            {
                return null;
            }
            DateTime dtS = DateTime.MinValue;
            DateTime dtE = DateTime.MaxValue;
            //事件类型
            string eventType = "";
            //获取当前选择用户的ID
            int userId = 0;
            string siteId = "";

            if (!dateStart.IsDateEmpty)
            {
                dtS = dateStart.SelectedDate;
            }

            if (!dateEnd.IsDateEmpty)
            {
                dtE = dateEnd.SelectedDate;
            }

            if (userPicker.ResolvedEntities.Count > 0)
            {
                PickerEntity pe = (PickerEntity)userPicker.ResolvedEntities[0];
                userId = String.IsNullOrEmpty(pe.EntityData["SPUserID"] + "") ? Convert.ToInt32(pe.EntityData["SPUserID"] + "") : 0;
                //GetUserId(pe.Key);
            }

            SPSite siteColl = SPContext.Current.Site;
            siteId = siteColl.ID.ToString();

            #region 页面事件的查询条件
            //添加选择的判断条件
            foreach (ListItem item in cbHandle.Items)
            {
                if (item.Selected)
                {
                    eventType = eventType + item.Value + ",";

                }
            }
            if (!String.IsNullOrEmpty(eventType))
            {
                eventType = eventType.Substring(0, eventType.Length - 1);
            }
            #endregion

            #region 插入用户数据到数据库

            DataTable dt = AuditLogBLL.LoadAuditLog(siteId, ListId, ItemId, dtS, dtE, eventType, userId);
            #endregion
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.DefaultView.ToTable(false, colNameEn);
            }
            return dt;
        }
        #endregion
    }
}
