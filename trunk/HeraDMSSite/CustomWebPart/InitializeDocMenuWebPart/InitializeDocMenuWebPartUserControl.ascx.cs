using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using HeraDMS.Entity;
using HeraDMS.BLL;
using System.Collections.Generic;
using HeraDMS.Layouts.Helper;

namespace HeraDMS.CustomWebPart.InitializeDocMenuWebPart
{
    public partial class InitializeDocMenuWebPartUserControl : BaseUserControl
    {
        MyFavoritesBLL MyFavoritesBLL = new MyFavoritesBLL();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddMyFavorites_Click(object sender, EventArgs e)
        {
            string itemId = hfItemId.Value;
            string listId = hfListId.Value;
            //string siteId="";
            if (String.IsNullOrEmpty(itemId) || String.IsNullOrEmpty(listId))
            {
                return;
            }

            SPSite siteColl = SPContext.Current.Site;
            SPWeb web = SPContext.Current.Web;
            SPSecurity.RunWithElevatedPrivileges(
          delegate()
          {
              using (SPSite ElevatedSiteCollection = new SPSite(siteColl.ID))
              {
                  using (SPWeb ElevatedSite = ElevatedSiteCollection.OpenWeb(web.ID))
                  {

                      //取得列表
                      SPList list = ElevatedSite.Lists[new Guid(listId)];
                      SPListItem item = list.Items.GetItemById(Convert.ToInt32(itemId));
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
                          List<DM_MyFavorite> entitys = new List<DM_MyFavorite>();
                          entitys.Add(entity);
                          MyFavoritesBLL.InsertMyFavorites(entitys);
                          base.AlertMessage("提示", "收藏成功！");
                      }
                  }
              }
          });
        }
    }
}
