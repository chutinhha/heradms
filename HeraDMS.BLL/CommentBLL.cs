using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HeraDMS.DAL;
using HeraDMS.Entity;

namespace HeraDMS.BLL
{
    public class CommentBLL
    {
        CommentDao CommentDao = new CommentDao();

        #region 保存评论
        /// <summary>
        /// 保存评论
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="bFlag"></param>
        public void SaveComment(DM_Comment entity, bool bFlag)
        {
            CommentDao.SaveComment(entity, bFlag);
        }
        #endregion

        #region 根据ID找出评论对象
        /// <summary>
        /// 根据ID找出评论对象
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="bFlag"></param>
        public DM_Comment LoadCommentEntity(Guid Id)
        {
            return CommentDao.LoadCommentEntity(Id);
        }
        #endregion

        #region 根据列表项查询评论
        /// <summary>
        /// 查询出当前列表项的评论
        /// </summary>
        /// <param name="siteId">网站ID</param>
        /// <param name="listId">列表ID</param>
        /// <param name="itemId">列表项ID</param>
        /// <param name="currentUserId">当前登录用ID</param>
        public IQueryable LoadCommentsByListItemId(string siteId, string listId, string itemId, int currentUserId, int pageSize, int pageIndex, out int pageCount)
        {

            return CommentDao.LoadCommentsByListItemId(siteId, listId, itemId, currentUserId, pageSize, pageIndex, out pageCount);
        }
        #endregion

        #region 删除评论
        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteComment(DM_Comment entity)
        {
            CommentDao.DeleteComment(entity);

        }
        #endregion

        #region 取得评论排行榜
        /// <summary>
        /// 取得评论排行榜
        /// </summary>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public IQueryable LoadCommentRanking(int topCount)
        {
            return CommentDao.LoadCommentRanking(topCount);
        }
        #endregion

        #region 取得评论
        /// <summary>
        /// 取得评论
        /// </summary>
        /// <param name="ItemId"></param>
        /// <param name="ListId"></param>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        public DM_Comment GetCommentByListId(int ItemId, Guid ListId, Guid SiteId)
        {
            return CommentDao.GetCommentByListId(ItemId, ListId, SiteId);
        }
        #endregion

        #region 当对应的文档被删除的时候，删除评论和我的收藏表中的数据
        /// <summary>
        /// 当对应的文档被删除的时候，删除评论和我的收藏表中的数据
        /// </summary>
        /// <param name="ItemId"></param>
        /// <param name="ListId"></param>
        /// <param name="SiteId"></param>
        public void DeleteCommentAndMyFavorites(int ItemId, Guid ListId, Guid SiteId)
        {
            CommentDao.DeleteCommentAndMyFavorites(ItemId, ListId, SiteId);
        }
        #endregion
    }
}
