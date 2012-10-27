using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HeraDMS.DAL.Common;
using HeraDMS.Entity;
using System.Data;

namespace HeraDMS.DAL
{
    public class CommentDao : AbstractLinqDao<DM_Comment, Guid>, IDao<DM_Comment, Guid>
    {
        #region 保存评论
        /// <summary>
        /// 保存评论
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="bFlag"></param>
        public void SaveComment(DM_Comment entity, bool bFlag)
        {
            HeraDMSEntityClassDataContext TDC = new HeraDMSEntityClassDataContext();
            if (bFlag)
            {
                TDC.DM_Comments.InsertOnSubmit(entity);
            }
            else if (bFlag == false)
            {
                DM_Comment dmc = TDC.DM_Comments.Single(a => a.ID == entity.ID);
                dmc.ItemId = entity.ItemId;
                dmc.ListId = entity.ListId;
                dmc.Modifier = entity.Modifier;
                dmc.ModifierName = entity.ModifierName;
                dmc.ModifyTime = entity.ModifyTime;
                dmc.SiteId = entity.SiteId;
                dmc.Comment = entity.Comment;
                dmc.CreateTime = entity.CreateTime;
                dmc.Creator = entity.Creator;
                dmc.CreatorName = entity.CreatorName;
                dmc.Description = entity.Description;    
            }
            TDC.SubmitChanges();
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
            HeraDMSEntityClassDataContext TDC = new HeraDMSEntityClassDataContext();

            DM_Comment dmc = TDC.DM_Comments.SingleOrDefault(a => a.ID == Id);

            return dmc;
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
            HeraDMSEntityClassDataContext TDC = new HeraDMSEntityClassDataContext();
            var query = from c in TDC.DM_Comments
                        where c.SiteId.ToString()==siteId
                        where c.ListId.ToString()==listId
                        where c.ItemId.ToString() == itemId
                        orderby c.CreateTime ascending
                        select new
                        {
                            ID=c.ID,
                            UserId=c.Creator,
                            Comment=c.Comment,
                            ModifyTime=c.ModifyTime,
                            ModifierName=c.ModifierName,
                            IsShowButton = c.Creator.Value == currentUserId?true:false
                        };
            pageCount = query.Count();
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
        #endregion

        #region 删除评论
        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteComment(DM_Comment entity)
        {
            HeraDMSEntityClassDataContext TDC = new HeraDMSEntityClassDataContext();
            DM_Comment dmc = TDC.DM_Comments.SingleOrDefault(a => a.ID == entity.ID);
            if (dmc != null)
            {
                TDC.DM_Comments.DeleteOnSubmit(dmc);
                TDC.SubmitChanges();
            }

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
            HeraDMSEntityClassDataContext TDC = new HeraDMSEntityClassDataContext();
            var query = (from a in TDC.DM_Comments
                                group a by new {a.SiteId,a.ListId,a.ItemId } into g
                                select new
                                {
                                    ItemId=g.Key.ItemId,
                                    ListId=g.Key.ListId,
                                    SiteId=g.Key.SiteId,
                                    Seq = g.Count()
                                }).OrderByDescending(a => a.Seq).Take(topCount);
            return query;
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
            HeraDMSEntityClassDataContext TDC = new HeraDMSEntityClassDataContext();
            DM_Comment entity = (from a in TDC.DM_Comments
                        where a.ItemId == ItemId
                        where a.ListId == ListId
                        where a.SiteId == SiteId
                         select a).FirstOrDefault();
            return entity;
        }
        #endregion

        #region 当对应的文档被删除的时候，删除评论和我的收藏表中的数据
        /// <summary>
        /// 当对应的文档被删除的时候，删除评论和我的收藏表中的数据
        /// </summary>
        /// <param name="ItemId"></param>
        /// <param name="ListId"></param>
        /// <param name="SiteId"></param>
        public void DeleteCommentAndMyFavorites(int ItemId,Guid ListId, Guid SiteId)
        { 
            HeraDMSEntityClassDataContext TDC = new HeraDMSEntityClassDataContext();

            //删除评论
            var commentQuery = from a in TDC.DM_Comments
                               where a.ItemId == ItemId
                               where a.ListId == ListId
                               where a.SiteId == SiteId
                               select a;
            if (commentQuery != null && commentQuery.Count() > 0)
            {
                TDC.DM_Comments.DeleteAllOnSubmit(commentQuery);
            }

            //删除我的收藏
            var myFavoritesQuery = from a in TDC.DM_MyFavorites
                               where a.ItemId == ItemId
                               where a.ListId == ListId
                               where a.SiteId == SiteId
                               select a;
            if (myFavoritesQuery != null && myFavoritesQuery.Count() > 0)
            {
                TDC.DM_MyFavorites.DeleteAllOnSubmit(myFavoritesQuery);
            }

            TDC.SubmitChanges();
        }
        #endregion
    }
}
