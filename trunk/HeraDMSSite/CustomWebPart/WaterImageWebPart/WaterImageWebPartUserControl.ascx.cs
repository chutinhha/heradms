using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using HeraDMS.Layouts.Helper;
using System.Drawing;
using System.Collections.Generic;
using Microsoft.SharePoint;

namespace HeraDMS.CustomWebPart.WaterImageWebPart
{
    public partial class WaterImageWebPartUserControl : UserControl
    {
        #region 参数初始化
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
        /// 图片在列表中的项的集合
        /// </summary>
        private List<string> ImageItems
        {
            get {
                if(ViewState["ImageItems"]!=null)
                {
                    return (List<string>)ViewState["ImageItems"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                ViewState["ImageItems"] = value;
            }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //图片所在列表
                ListId = (Request.QueryString["listID"] != null) ? Request.QueryString["listID"].ToString().Trim().ToLower() : "";
                //列表中选择的项的集合 | 拼接而成
                string selectedItems = (Request.QueryString["selectedItemIds"] != null) ? Request.QueryString["selectedItemIds"].ToString().Trim().ToLower() : "";
                //当有传递参数的时候
                if (!String.IsNullOrEmpty(ListId) && !String.IsNullOrEmpty(selectedItems))
                {
                    //分割
                    string[] items = selectedItems.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    InitData();
                    GetImageItems(items);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Type), System.Guid.NewGuid().ToString(), "alert('参数不正确，无法添加水印！');SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.cancel);", true);
                }
                
            }
        }
        /// <summary>
        /// 初始化页面的数据
        /// </summary>
        private void InitData()
        {
            this.ddlPosition.Items.Clear();
            this.ddlPosition.Items.Insert(0, new ListItem("左上", ImagePosition.LeftTop.ToString()));
            this.ddlPosition.Items.Insert(0, new ListItem("左下", ImagePosition.LeftBottom.ToString()));
            this.ddlPosition.Items.Insert(0, new ListItem("右上", ImagePosition.RightTop.ToString()));
            this.ddlPosition.Items.Insert(0, new ListItem("右下", ImagePosition.RigthBottom.ToString()));
            this.ddlPosition.Items.Insert(0, new ListItem("顶部居中", ImagePosition.TopMiddle.ToString()));
            this.ddlPosition.Items.Insert(0, new ListItem("底部居中", ImagePosition.BottomMiddle.ToString()));
            this.ddlPosition.Items.Insert(0, new ListItem("中心", ImagePosition.Center.ToString()));

        }

        /// <summary>
        /// 从列表中获取选择项，并且取得图片的项
        /// </summary>
        /// <param name="items"></param>
        private void GetImageItems(string[] selectedItems)
        {
            List<string> items = new List<string>();
            if (selectedItems != null)
            {
                SPSite siteColl = SPContext.Current.Site;
                SPWeb site = SPContext.Current.Web;
                //提高权限
                SPSecurity.RunWithElevatedPrivileges(
                delegate()
                  {
                      using (SPSite ElevatedSiteCollection = new SPSite(siteColl.ID))
                      {
                          using (SPWeb ElevatedSite = ElevatedSiteCollection.OpenWeb(site.ID))
                          {

                              //取得列表
                              SPList list = ElevatedSite.Lists[new Guid(ListId)];
                              foreach (string item in selectedItems)
                              {
                                  //取得列表项
                                  SPListItem listItem = list.GetItemById(Convert.ToInt32(item));
                                  
                              }
                          }
                      }
                  });
            }
        }
        #region 按钮事件
        protected void btnPreview_Click(object sender, EventArgs e)
        {

        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 上传水印图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpLoad_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 水印类型选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //当选择图片水印
            if (rblType.SelectedValue.Equals("Y"))
            {
                this.Panel1.Visible = true;
                this.Panel2.Visible = false;

            }
            //当选择文字水印
            else
            {
                this.Panel1.Visible = false;
                this.Panel2.Visible = true;
            }
        }

        #endregion
    }
}
