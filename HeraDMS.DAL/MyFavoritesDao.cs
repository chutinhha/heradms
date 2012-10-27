using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HeraDMS.DAL.Common;
using HeraDMS.Entity;

namespace HeraDMS.DAL
{
    public class MyFavoritesDao : AbstractLinqDao<DM_MyFavorite, Guid>, IDao<DM_MyFavorite, Guid>
    {
        #region 保存我的收藏
        /// <summary>
        /// 保存我的收藏
        /// </summary>
        /// <param name="entitys"></param>
        public void InsertMyFavorites(List<DM_MyFavorite> entitys)
        {
            HeraDMSEntityClassDataContext TDC = new HeraDMSEntityClassDataContext();
            foreach (DM_MyFavorite entity in entitys)
            {
                DM_MyFavorite dm = TDC.DM_MyFavorites.SingleOrDefault(c => c.SiteId == entity.SiteId && c.ListId == entity.ListId && c.ItemId == entity.ItemId&&c.Creator==entity.Creator);
                if (dm != null)
                {
                    dm.DocTitle = entity.DocTitle;
                    dm.DocUrl = entity.DocUrl;
                    dm.CreateTime = DateTime.Now;
                    dm.ModifyTime = DateTime.Now;
                }
                else
                {
                    TDC.DM_MyFavorites.InsertOnSubmit(entity);
                }
            }
            
            TDC.SubmitChanges();
        }
        #endregion

        #region 根据当前用户查询我的收藏
        /// <summary>
        /// 根据当前用户查询我的收藏
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public IQueryable LoadMyFavoritesByUserId(int currentUserId, int pageSize, int pageIndex, out int pageCount)
        {
            HeraDMSEntityClassDataContext TDC = new HeraDMSEntityClassDataContext();
            var query = from c in TDC.DM_MyFavorites
                        where c.Creator == currentUserId
                        orderby c.CreateTime ascending
                        select c;
            pageCount = query.Count();
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
        #endregion

        #region 删除我的收藏
        /// <summary>
        /// 删除我的收藏
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteMyFavoritesById(DM_MyFavorite entity)
        {
            HeraDMSEntityClassDataContext TDC = new HeraDMSEntityClassDataContext();
            DM_MyFavorite dmc = TDC.DM_MyFavorites.SingleOrDefault(a => a.ID == entity.ID);
            if (dmc != null)
            {
                TDC.DM_MyFavorites.DeleteOnSubmit(dmc);
                TDC.SubmitChanges();
            }

        }
        #endregion

        #region 取得收藏排行榜
        /// <summary>
        /// 取得收藏排行榜
        /// </summary>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public IQueryable LoadFavoritesRanking(int topCount)
        {
            HeraDMSEntityClassDataContext TDC = new HeraDMSEntityClassDataContext();
            var query = (from a in TDC.DM_MyFavorites
                         group a by new { a.SiteId, a.ListId, a.ItemId } into g
                         select new
                         {
                             ItemId = g.Key.ItemId,
                             ListId = g.Key.ListId,
                             SiteId = g.Key.SiteId,
                             Seq = g.Count()
                         }).OrderByDescending(a => a.Seq).Take(topCount);
            return query;
        }
        #endregion
    }
}
