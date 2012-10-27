using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;
using System.Web.UI;
using System.Web;
using System.Data;

namespace HeraDMS.Layouts.Helper
{
    public class BasePage : LayoutsPageBase
    {
        /// <summary>
        /// 立刻跳转到其他页面byAjax
        /// </summary>
        /// <param name="url">页面地址</param>
        public void JavascriptGoUrlByAjaxNow(string url)
        {
            string script = string.Format("setTimeout(function(){{location.href='{0}';}},0);", url);
            ScriptManager.RegisterClientScriptBlock(this, typeof(UpdatePanel), System.Guid.NewGuid().ToString(), script, true);
        }
        /// <summary>
        /// 该提示运用于弹出消息后，关闭Sharepoint的dailog
        /// </summary>
        /// <param name="msg"></param>
        public void ShowDialogMessage(string msg)
        {
            msg = msg.Replace("'", "''");
            msg = msg.Replace("\\", "\\\\");
            msg = msg.Replace("\r\n", "\\n");
            msg = msg.Replace("\n", "\\n");
            msg = msg.Replace("\"", "\\\"");
            ScriptManager.RegisterStartupScript(this, typeof(Type), System.Guid.NewGuid().ToString(), "alert('" + msg + "');window.frameElement.commonModalDialogClose(1, 1);", true);
        }
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

        //由dataset导出Excel        
        public void CreateExcel(DataTable dt,string FileName)
        {
            HttpResponse resp;
            resp = Page.Response;
            

            ////resp.ContentEncoding = System.Text.Encoding.Default;
            //resp.Charset = "GB2312";
            //resp.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            
            resp.AddHeader("Content-Disposition", String.Format("attachment; filename={0}",
                HttpUtility.UrlEncode(FileName)));
            resp.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");//起的作用
            resp.ContentType = "application/excel";
            resp.Charset = "utf-8";//起的作用
            resp.ClearContent(); 
            //resp.AppendHeader("Content-Disposition", "attachment;filename=" + FileName);
            string colHeaders = "", ls_item = "";
            int i = 0;

            //定义表对象和行对像，同时用DataSet对其值进行初始化 
            DataRow[] myRow = dt.Select("");
            // typeid=="1"时导出为EXCEL格式文档；typeid=="2"时导出为XML格式文档 
            //if (typeid == "1")
            //{
                //取得数据表各列标题，各标题之间以\t分割，最后一个列标题后加回车符 
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    if (i == dt.Columns.Count - 1)
                    {
                        colHeaders += dt.Columns[i].Caption.ToString() + "\n";
                    }
                    else
                    {
                        colHeaders += dt.Columns[i].Caption.ToString() + "\t";
                    }
                }
                //向HTTP输出流中写入取得的数据信息 
                resp.Write(colHeaders);
                //逐行处理数据 
                foreach (DataRow row in myRow)
                {
                    //在当前行中，逐列获得数据，数据之间以\t分割，结束时加回车符\n 
                    for (i = 0; i < dt.Columns.Count; i++)
                    {

                        if (i == dt.Columns.Count - 1)
                        {
                            ls_item += row[i].ToString() + "\n";
                        }
                        else
                        {
                            ls_item += row[i].ToString() + "\t";
                        }
                    }
                    //当前行数据写入HTTP输出流，并且置空ls_item以便下行数据 
                    resp.Write(ls_item);
                    ls_item = "";
                }
            //}
            //else
            //{
            //    if (typeid == "2")
            //    {
            //        //从DataSet中直接导出XML数据并且写到HTTP输出流中 
            //        resp.Write(ds.GetXml());
            //    }
            //}
            //写缓冲区中的数据到HTTP头文档中 
            resp.End();
        } 
    }
}
