using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeraDMS.DAL.Common
{
    /// <summary>
    /// 通用DAO接口
    /// </summary>
    /// <typeparam name="T">实体</typeparam>
    /// <typeparam name="IdT">主键类型</typeparam>
    public interface IDao<T, IdT>
    {
        //T GetById(IdT id, bool shouldLock);
        //List<T> GetAll();
        //List<T> GetByExample(T exampleInstance, params string[] propertiesToExclude);
        //T GetUniqueByExample(T exampleInstance, params string[] propertiesToExclude);
        //T Save(T entity);
        //T SaveOrUpdate(T entity);
        //void Delete(T entity);
        //void CommitChanges();
    }
}
