using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace HeraDMS.Layouts.Helper
{
    public partial class BaseUserControl : UserControl
    {
        /// <summary>
        /// 弹出提示消息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        public void AlertMessage(string title, string msg)
        {
            msg = msg.Replace("'", "''");
            msg = msg.Replace("\\", "\\\\");
            msg = msg.Replace("\r\n", "\\n");
            msg = msg.Replace("\n", "\\n");
            msg = msg.Replace("\"", "\\\"");
            string script = "";
            if (title == null)
            {
                script = "<script type='text/javascript'>window.alert('" + this.GetHTMLSafeString(msg) + "');</script>";
                //script = "<script>Ext.onReady(function(){Ext.MessageBox.alert('" + msg + "');})</script>";
            }
            else
            {
                script = "<script type='text/javascript'>window.alert('" + this.GetHTMLSafeString(msg) + "');</script>";
                //script = "<script>Ext.onReady(function(){Ext.MessageBox.alert('" + title + "', '" + msg + "');})</script>";
            }
            ScriptManager.RegisterStartupScript(this, typeof(Type), System.Guid.NewGuid().ToString(), script, false);
        }

        /// <summary>
        /// 返回HTML安全的字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string GetHTMLSafeString(string s)
        {
            s = s.Trim();
            s = s.Replace("<", "&lt;");
            s = s.Replace(">", "&gt;");
            s = s.Replace("'", "''");
            s = s.Replace("\n", "<br />");
            s = s.Replace("\r", "<br />");
            s = s.Replace("\r\n", "<br />");
            return s;
        }
    }
}
