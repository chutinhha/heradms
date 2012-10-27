using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using HeraDMS.BLL;
using Microsoft.SharePoint;

namespace HeraDMS.CustomWebPart.FavritesRankingWebPart
{
    public partial class FavritesRankingWebPartUserControl : UserControl
    {
        MyFavoritesBLL MyFavoritesBLL = new MyFavoritesBLL();
        const int topCount = 10;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.gvFavoritesRanking.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");
            if (!IsPostBack)
            {
                BindData();
            }
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindData()
        {
            this.gvFavoritesRanking.DataSource = MyFavoritesBLL.LoadFavoritesRanking(topCount);
            this.gvFavoritesRanking.DataBind();
        }

        /// <summary>
        /// 行绑定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFavoritesRanking_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex >= 0)
            {
                HiddenField hfItemId = e.Row.FindControl("hfItemId") as HiddenField;
                HiddenField hfListId = e.Row.FindControl("hfListId") as HiddenField;
                HiddenField hfSiteId = e.Row.FindControl("hfSiteId") as HiddenField;
                HyperLink hlTitle = e.Row.FindControl("hlTitle") as HyperLink;
                if (String.IsNullOrEmpty(hfItemId.Value) || String.IsNullOrEmpty(hfListId.Value) || String.IsNullOrEmpty(hfSiteId.Value))
                {
                    return;
                }

                SPSecurity.RunWithElevatedPrivileges(
                delegate()
                {
                    using (SPSite ElevatedSiteCollection = new SPSite(new Guid(hfSiteId.Value)))
                    {
                        using (SPWeb ElevatedSite = ElevatedSiteCollection.OpenWeb())
                        {
                            //取得列表
                            SPList list = ElevatedSite.Lists[new Guid(hfListId.Value)];
                            if (list != null)
                            {
                                SPListItem item = list.Items.GetItemById(Convert.ToInt32(hfItemId.Value));
                                if (item != null)
                                {
                                    hlTitle.Text = item.Name;
                                    hlTitle.NavigateUrl = SPContext.Current.Web.Url + "//" + item.Url;
                                }
                            }
                        }
                    }
                });

            }
        }
    }
}
