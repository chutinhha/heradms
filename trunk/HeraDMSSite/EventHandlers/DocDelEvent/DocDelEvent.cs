using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using HeraDMS.BLL;

namespace HeraDMS.EventHandlers.DocDelEvent
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class DocDelEvent : SPItemEventReceiver
    {
       /// <summary>
       /// An item is being deleted.
       /// </summary>
       public override void ItemDeleting(SPItemEventProperties properties)
       {
           base.ItemDeleting(properties);
       }

       /// <summary>
       /// An item was deleted.
       /// </summary>
       public override void ItemDeleted(SPItemEventProperties properties)
       {
           base.ItemDeleted(properties);
           CommentBLL CommentBLL = new CommentBLL();
           CommentBLL.DeleteCommentAndMyFavorites(properties.ListItemId, properties.ListId, properties.SiteId);
       }


    }
}
