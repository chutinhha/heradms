using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using HeraDMS.Entity;
using System.Collections.Generic;
using HeraDMS.BLL;

namespace HeraDMS.Layouts.HeraDMSSite.Pages.MyFavorites
{
    public partial class MyFavoritesOperation : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string ListId = (Request.QueryString["listid"] != null) ? Request.QueryString["listid"].ToString().Trim().ToLower() : "";
            string selectedItems = (Request.QueryString["selectedItems"] != null) ? Request.QueryString["selectedItems"].ToString().Trim().ToLower() : "";

            if (!String.IsNullOrEmpty(ListId) && !String.IsNullOrEmpty(selectedItems))
            {
                string[] items = selectedItems.Split(new char[]{'|'},StringSplitOptions.RemoveEmptyEntries);
                AddMyFavorites(ListId, items);
                Response.Write("OK");
                Response.End();
            }
        }

        /// <summary>
        /// 批量添加收藏
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="items"></param>
        private void AddMyFavorites(string listId, string[] items)
        {
            SPSite siteColl = SPContext.Current.Site;
            SPWeb web = SPContext.Current.Web;
            //取得列表
            SPList list = web.Lists[new Guid(listId)];
            List<DM_MyFavorite> entitys = new List<DM_MyFavorite>();
            foreach (string itemid in items)
            {
                SPListItem item = list.Items.GetItemById(Convert.ToInt32(itemid));
                //当是文件
                if (item != null && item.FileSystemObjectType == SPFileSystemObjectType.File)
                {
                    DM_MyFavorite entity = new DM_MyFavorite();
                    int userId = SPContext.Current.Web.CurrentUser.ID;
                    string userName = String.IsNullOrEmpty(SPContext.Current.Web.CurrentUser.Name) ? SPContext.Current.Web.CurrentUser.LoginName : SPContext.Current.Web.CurrentUser.Name;
                    entity.ID = Guid.NewGuid();
                    entity.ItemId = item.ID;
                    entity.ListId = new Guid(listId);
                    entity.Modifier = userId;
                    entity.ModifierName = userName;
                    entity.ModifyTime = DateTime.Now;
                    entity.SiteId = siteColl.ID;
                    entity.CreateTime = DateTime.Now;
                    entity.Creator = userId;
                    entity.CreatorName = userName;
                    entity.DocTitle = item.Name;
                    entity.DocUrl = item.File.ServerRelativeUrl;

                    entitys.Add(entity);
                }
                
            }
            if (entitys != null && entitys.Count > 0)
            {
                MyFavoritesBLL MyFavoritesBLL = new MyFavoritesBLL();
                MyFavoritesBLL.InsertMyFavorites(entitys);
            }
        }
    }
}
