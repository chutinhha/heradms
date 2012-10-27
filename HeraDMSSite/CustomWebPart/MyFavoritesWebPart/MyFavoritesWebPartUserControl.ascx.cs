using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using HeraDMS.BLL;
using HeraDMS.Entity;
using HeraDMS.Layouts.Helper;
using Microsoft.SharePoint;
using System.Linq;

namespace HeraDMS.CustomWebPart.MyFavoritesWebPart
{
    public partial class MyFavoritesWebPartUserControl : BaseUserControl
    {
        MyFavoritesBLL MyFavoritesBLL = new MyFavoritesBLL();
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
        protected void Page_Load(object sender, EventArgs e)
        {
            this.gvMyFavorites.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");
            if (!IsPostBack)
            {
                PageIndex = 1;
                BindGrid();
            }
        }

        /// <summary>
        /// 绑定数据源
        /// </summary>
        private void BindGrid()
        {
            //当前登录用户
            int currentUserId = SPContext.Current.Web.CurrentUser.ID;
            int pageCount = 0;
            IQueryable query = MyFavoritesBLL.LoadMyFavoritesByUserId(currentUserId, PageSize, PageIndex, out pageCount);

            this.AspNetPager1.PageIndex = PageIndex;
            AspNetPager1.PageSize = PageSize;
            AspNetPager1.RecordCount = pageCount;

            this.gvMyFavorites.DataSource = query;
            this.gvMyFavorites.DataBind();
            if (gvMyFavorites == null || gvMyFavorites.Rows.Count <= 0)
            {
                AspNetPager1.Visible = false;
            }
            else
            {
                AspNetPager1.Visible = true;
            }
        }
        /// <summary>
        /// 翻页控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            PageIndex = AspNetPager1.PageIndex;
            //this.gvData.PageIndex = AspNetPager1.PageIndex;
            BindGrid();

        }
        /// <summary>
        /// 取消收藏事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbDelete_Click(object sender, EventArgs e)
        {
            LinkButton lbDelete = (LinkButton)sender;
            string id = lbDelete.CommandArgument;
            DM_MyFavorite entity=new DM_MyFavorite();
            entity.ID=new Guid(id);
            MyFavoritesBLL.DeleteMyFavoritesById(entity);
            base.AlertMessage("提示", "取消收藏成功！");
            BindGrid();
        }
    }
}
