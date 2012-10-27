using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HeraDMS.DAL;
using HeraDMS.Entity;

namespace HeraDMS.BLL
{
    public class MyFavoritesBLL
    {
        MyFavoritesDao MyFavoritesDao = new MyFavoritesDao();

        #region 保存我的收藏
        /// <summary>
        /// 保存我的收藏
        /// </summary>
        /// <param name="entitys"></param>
        public void InsertMyFavorites(List<DM_MyFavorite> entitys)
        {
            MyFavoritesDao.InsertMyFavorites(entitys);
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
            return MyFavoritesDao.LoadMyFavoritesByUserId(currentUserId, pageSize, pageIndex, out pageCount);
        }
        #endregion

        #region 删除我的收藏
        /// <summary>
        /// 删除我的收藏
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteMyFavoritesById(DM_MyFavorite entity)
        {
            MyFavoritesDao.DeleteMyFavoritesById(entity);

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
            return MyFavoritesDao.LoadFavoritesRanking(topCount);
        }
        #endregion
    }
}
