using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web;
using HeraDMS.Layouts.Helper;

namespace HeraDMS.Layouts.Common
{
    public partial class DownloadFile : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //文件路径，包括文件名：D:\Products\SRM\C-Procurement\Development\SRM\DataFolder\FileExport\WOItemSearchReport2005041415155.XLS
                string fullPathName = "";
                //文件名：WOItemSearchReport2005041415155.XLS
                string fileName = "";

                if (Page.Request["FilePath"] != null && Page.Request["FilePath"].ToString().Length > 0)
                {
                    fullPathName = HttpUtility.UrlDecode(Page.Request["FilePath"].ToString());
                    fileName = HttpUtility.UrlDecode(Page.Request["FileName"].ToString());
                }
                else
                {
                    Response.Write("<script>window.close();</script>");
                    return;
                }
                //System.IO.FileInfo fleInfo = new System.IO.FileInfo(fullPathName);
                //Response.Clear();
                //Response.ClearHeaders();
                //Response.Buffer = true;
                //Response.AddHeader("content-disposition", "attachment; filename=" + Server.UrlEncode(fileName.Trim()));
                ////inline(在线打开)，attachment（下载）
                //Response.AddHeader("Content-Length", fleInfo.Length.ToString());
                //Response.ContentType = "application/x-msexcel";
                //Response.WriteFile(fullPathName);
                //Response.Flush();
                //Response.End();

                string fileserverpath = Server.MapPath(fullPathName);
                System.IO.FileInfo fi = new System.IO.FileInfo(fileserverpath);
                fi.Attributes = System.IO.FileAttributes.Normal;
                System.IO.FileStream filestream = new System.IO.FileStream(fileserverpath, System.IO.FileMode.Open);
                long filesize = filestream.Length;
                int i = Convert.ToInt32(filesize);

                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
                Response.AddHeader("Content-Length", filesize.ToString());
                byte[] fileBuffer = new byte[i];
                filestream.Read(fileBuffer, 0, i);
                filestream.Close();
                Response.BinaryWrite(fileBuffer);
                Response.Flush();
                Response.End();

            }
            catch (Exception Ex)
            {
                Response.Write("<script>alert('" + Ex.Message.Replace("'", "\\'") + "');window.close();</script>");
                Response.End();
            }
        }
    }
}
