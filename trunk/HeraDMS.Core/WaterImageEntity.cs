using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeraDMS.Core
{
    /// <summary>
    /// 文档库图片类
    /// </summary>
    public class WaterImageEntity
    {
        /// <summary>
        /// 文档ID
        /// </summary>
        private int _itemId;

        public int ItemId
        {
            get { return _itemId; }
            set { _itemId = value; }
        }

        /// <summary>
        /// 文档名称
        /// </summary>
        private string _itemName;

        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; }
        }

        /// <summary>
        /// 文档路径
        /// </summary>
        private string _itemUrl;

        public string ItemUrl
        {
            get { return _itemUrl; }
            set { _itemUrl = value; }
        }
    }
}
