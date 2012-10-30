using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;
using HeraDMS.Layouts.Helper;
using HeraDMS.BLL;
using System.Linq;
using HeraDMS.Entity;
using System.Net;

namespace HeraDMS.Layouts.HeraDMSSite.Pages.Comment
{
    public partial class CommentList : BasePage
    {
        #region 参数设置和初始化对象
        CommentBLL CommentBLL = new CommentBLL();
        /// <summary>
        /// 当前文档库的ID
        /// </summary>
        private string ListId
        {
            get
            {
                return ViewState["CommentList_ListId"] + "";
            }
            set
            {
                ViewState["CommentList_ListId"] = value;
            }
        }
        /// <summary>
        /// 当前文档库中被选择的文档的ID
        /// </summary>
        private string ItemId
        {
            get
            {
                return ViewState["CommentList_ItemId"] + "";
            }
            set
            {
                ViewState["CommentList_ItemId"] = value;
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
            if (!IsPostBack)
            {
                ListId = (Request.QueryString["listID"] != null) ? Request.QueryString["listID"].ToString().Trim().ToLower() : "";
                ItemId = (Request.QueryString["itemID"] != null) ? Request.QueryString["itemID"].ToString().Trim().ToLower() : "";
                PageIndex = 1;
                if (!CheckFileType())
                {
                    return;
                }
                BindGrid();
            }
        }

        /// <summary>
        /// 判断当传递的参数缺失或者不是文件的时候，不能评论
        /// </summary>
        /// <returns></returns>
        private bool CheckFileType()
        {
            if (String.IsNullOrEmpty(ListId) || String.IsNullOrEmpty(ItemId))
            {
                base.ShowDialogMessage("参数不正确！");
                return   false;
            }

            bool result = true;
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
                        //取得列表项 
                        SPListItem listItem = list.GetItemById(Convert.ToInt32(ItemId));
                        //if (listItem == null || listItem.FileSystemObjectType == SPFileSystemObjectType.File)
                        if (listItem == null)
                        {
                            base.ShowDialogMessage("参数不正确！");
                            result= false;
                        }
                    }
                }
            });
            return result;
        }
        /// <summary>
        /// 绑定评论
        /// </summary>
        private void BindGrid()
        {
           
            #region 查询条件
            //站点ID
            SPSite site = SPContext.Current.Site;
            string siteId = site.ID.ToString();
            //当前登录用户
            int currentUserId = SPContext.Current.Web.CurrentUser.ID;
            #endregion

            int pageCount=0;
            IQueryable query=CommentBLL.LoadCommentsByListItemId(siteId, (new Guid(ListId)).ToString(), ItemId, currentUserId, PageSize, PageIndex, out pageCount);

            this.AspNetPager1.PageIndex = PageIndex;
            AspNetPager1.PageSize = PageSize;
            AspNetPager1.RecordCount = pageCount;
            this.AspNetPager2.PageIndex = PageIndex;
            AspNetPager2.PageSize = PageSize;
            AspNetPager2.RecordCount = pageCount;
            this.gvComment.DataSource = query;
            this.gvComment.DataBind();
            if (gvComment == null || gvComment.Rows.Count <= 0)
            {
                AspNetPager1.Visible = false;
                AspNetPager2.Visible = false;
            }
            else
            {
                AspNetPager1.Visible = true;
                AspNetPager2.Visible = true;
            }


        }

        #region gridview事件
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvComment_RowEditing(object sender, GridViewEditEventArgs e)
        {
            this.gvComment.EditIndex = e.NewEditIndex;
            //在这里重新绑定数据。
            BindGrid();
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvComment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.gvComment.EditIndex = -1;
            //在这里重新绑定数据。
            BindGrid();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvComment_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = e.RowIndex;
            GridViewRow gvr = this.gvComment.Rows[index];
            LinkButton lbDelete = gvr.FindControl("lbDelete") as LinkButton;
            string Id = lbDelete.CommandArgument;
            DM_Comment entity = new DM_Comment();
            entity.ID = new Guid(Id);
            CommentBLL.DeleteComment(entity);
            this.gvComment.EditIndex = -1;
            BindGrid();
            base.AlertMessage("提示", "删除成功！");

        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvComment_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int index = this.gvComment.EditIndex;
            GridViewRow gvr = this.gvComment.Rows[index];
            Button btnUpdate = gvr.FindControl("btnUpdate") as Button;
            TextBox tbUpdateComment = gvr.FindControl("tbUpdateComment") as TextBox;
            string value = tbUpdateComment.Text.Trim();
            if (String.IsNullOrEmpty(value))
            {
                base.AlertMessage("提示", "评论不能为空！");
                return;
            }
            string id = btnUpdate.CommandArgument;
            DM_Comment entity = CommentBLL.LoadCommentEntity(new Guid(id));
            if (entity == null)
            {
                base.AlertMessage("提示", "数据不存在！");
            }
            else
            {
                entity.Comment = value;
                entity.ModifyTime = DateTime.Now;
                CommentBLL.SaveComment(entity,false);
                base.AlertMessage("提示", "发布成功！");
            }
            this.gvComment.EditIndex = -1;
            BindGrid();
            
        }

        protected void gvComment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex >= 0)
            {
                Image ImPicture = e.Row.FindControl("ImPicture") as Image;
                if (ImPicture == null)
                {
                    return;
                }
                HiddenField hfUserId = e.Row.FindControl("hfUserId") as HiddenField;

                ImPicture.ImageUrl = GetUserPicture(hfUserId.Value);
            }
        }

        /// <summary>
        /// 根据用户ID获取用户头像
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GetUserPicture(string id)
        {
            string picturePath = "";
            SPSite siteColl = SPContext.Current.Site;
            SPSecurity.RunWithElevatedPrivileges(
          delegate()
          {
              using (SPSite site = new SPSite(siteColl.ID))
              {
                  using (SPWeb web = site.RootWeb)
                  {
                      Microsoft.SharePoint.SPList list = web.SiteUserInfoList;
                      SPListItem it = list.Items.GetItemById(Convert.ToInt32(id));
                      if (it != null)
                      {
                          picturePath = it["Picture"] + "";
                          
                      }
                  }
              }

          });

            if (String.IsNullOrEmpty(picturePath))
            {
                picturePath = "/_layouts/images/person.gif";
            }
            else
            {
                try
                {
                    HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(picturePath);
                    HttpWebResponse myRes = (HttpWebResponse)myReq.GetResponse();
                    //当网络图片失效后，或者不存在的时候
                    if (myRes.ContentLength <= 0)
                    {
                        picturePath = "/_layouts/images/person.gif";
                    }
                }
                catch
                {
                    picturePath = "/_layouts/images/person.gif";
                }
            }
            return picturePath;

        }

        #endregion

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPublish_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbComment.Text.Trim()))
            {
                base.AlertMessage("提示", "评论不能为空！");
                return;
            }
            int userId=SPContext.Current.Web.CurrentUser.ID;
            string userName = String.IsNullOrEmpty(SPContext.Current.Web.CurrentUser.Name) ? SPContext.Current.Web.CurrentUser.LoginName : SPContext.Current.Web.CurrentUser.Name;
            DM_Comment entity = new DM_Comment();
            entity.ID = Guid.NewGuid();
            entity.Comment = this.tbComment.Text;
            entity.CreateTime = DateTime.Now;
            entity.Creator = userId;
            entity.CreatorName = userName;
            entity.ItemId = Convert.ToInt32(ItemId);
            entity.ListId = new Guid(ListId);
            entity.Modifier = userId;
            entity.ModifierName = userName;
            entity.ModifyTime = DateTime.Now;
            entity.SiteId = SPContext.Current.Site.ID;
            CommentBLL.SaveComment(entity,true);
            this.tbComment.Text = "";
            BindGrid();
            base.AlertMessage("提示", "发布成功！");
        }

        /// <summary>
        /// 翻页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            PageIndex = AspNetPager1.PageIndex;
            PageIndex = AspNetPager2.PageIndex;
            //this.gvData.PageIndex = AspNetPager1.PageIndex;
            BindGrid();

        }
    }
}
