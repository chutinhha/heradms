using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace HeraDMS.Layouts.Helper
{
    public class Common
    {
        /// <summary>
        /// 替换中文列名
        /// </summary>
        /// <param name="dtEN"></param>
        /// <param name="CHNcols"></param>
        /// <returns></returns>
        public static DataTable ChangesCH(DataTable dtEN, string[] CHNcols)
        {
            if (null != dtEN && dtEN.Rows.Count > 0)
            {
                for (int i = 0; i < dtEN.Columns.Count; i++)
                {
                    dtEN.Columns[i].ColumnName = CHNcols[i].Trim();
                }
            }
            return dtEN;
        }
    }
}
