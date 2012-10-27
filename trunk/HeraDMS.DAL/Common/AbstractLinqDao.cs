using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeraDMS.DAL.Common
{
    /// <summary>
    /// 根据Linq抽象出来的DAO类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="IdT"></typeparam>
    public abstract class AbstractLinqDao<T, IdT> : IDao<T, IdT>
    {

    }
}
