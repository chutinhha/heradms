using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace HeraDMS.SPHelpler
{
    /// <summary>
    /// 用于向Microsoft.SharePoint对象添加一些扩展方法。
    /// </summary>
    public static class SPExtensions
    {
        /// <summary>
        /// 判断SPListItem实例是否是一个文件夹
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool IsFolder(this SPListItem item)
        {
            return (item.Folder != null);
        }

        /// <summary>
        /// 判断SPList类是否仅仅是一个文档
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsDocumentLibrary(this SPList list)
        {
            return (list.BaseType == SPBaseType.DocumentLibrary);
        }
    }
}
