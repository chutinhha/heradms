using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.IO;
using HeraDMS.Core;
using HeraDMS.SPHelpler;

namespace HeraDMS.Layouts.HeraDMSSite.Pages
{
    /// <summary>
    /// 该页面负责处理来自客户端的请求并生成对应的zip文件返回到客户端
    /// </summary>
    /// <remarks>
    /// 它的功能如同已知的在SharePoint 2010 Ribbon菜单中的“下载副本”的功能。来自客户端的POST请求会被发送到我的DownloadZip.aspx页面，
    /// 然后这个页面把一些文档进行打包压缩到一个zip文件然后发送到客户端浏览器。这个页面需要两个参数：
    /// sourceUrl –完整的文档（文件夹，包含子文件夹），请求的来源 
    /// itemIDs – 列表项ID ，用分号隔开的一个SPListItem ID项目集，作为zip文件的一部分。
    /// 注意：一个文件夹也有IDs如果一个文件夹被选中的话，自然其ID值也会被传递。 
    /// 该应用程序页的基本功能是根据传递过来的id检索对应的SharePoint中的文档项，使用ZipBuilder 类把检索到的文档打包成一个zip文件。
    /// 如果是一个文件夹的id，则会创建文件夹（最终保存到zip文件中），并依次检索文件夹下的所有文件
    /// </remarks>
    public partial class DownloadZip : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string fullDocLibSourceUrl = Request.Params["sourceUrl"];
            if (string.IsNullOrEmpty(fullDocLibSourceUrl)) return;

            string docLibUrl = fullDocLibSourceUrl.Replace(SPContext.Current.Site.Url, "");

            SPList list = SPContext.Current.Web.GetList(docLibUrl);
            if (!list.IsDocumentLibrary()) return;

            string pItemIds = Request.Params["itemIDs"];
            if (string.IsNullOrEmpty(pItemIds)) return;

            SPDocumentLibrary library = (SPDocumentLibrary)list;

            string[] sItemIds = pItemIds.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            int[] itemsIDs = new int[sItemIds.Length];
            for (int i = 0; i < sItemIds.Length; i++)
            {
                itemsIDs[i] = Convert.ToInt32(sItemIds[i]);
            }

            if (itemsIDs.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (ZipFileBuilder builder = new ZipFileBuilder(ms))
                    {
                        foreach (int id in itemsIDs)
                        {
                            SPListItem item = library.GetItemById(id);
                            if (item.IsFolder())
                                AddFolder(builder, item.Folder, string.Empty);
                            else
                                AddFile(builder, item.File, string.Empty);
                        }

                        builder.Finish();
                        WriteStreamToResponse(ms);
                    }
                }
            }

        }

        /// <summary>
        /// 制作压缩文件
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="file"></param>
        /// <param name="folder"></param>
        private static void AddFile(ZipFileBuilder builder, SPFile file, string folder)
        {
            using (Stream fileStream = file.OpenBinaryStream())
            {
                builder.Add(folder + "\\" + file.Name, fileStream);
                fileStream.Close();
            }
        }

        /// <summary>
        /// 制作压缩文件夹
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="folder"></param>
        /// <param name="parentFolder"></param>
        private static void AddFolder(ZipFileBuilder builder, SPFolder folder, string parentFolder)
        {
            string folderPath = parentFolder == string.Empty ? folder.Name : parentFolder + "\\" + folder.Name;
            builder.AddDirectory(folderPath);

            foreach (SPFile file in folder.Files)
            {
                AddFile(builder, file, folderPath);
            }

            foreach (SPFolder subFolder in folder.SubFolders)
            {
                AddFolder(builder, subFolder, folderPath);
            }
        }

        private void WriteStreamToResponse(MemoryStream ms)
        {
            if (ms.Length > 0)
            {
                string filename = DateTime.Now.ToFileTime().ToString() + ".zip";
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("Content-Length", ms.Length.ToString());
                Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/octet-stream";

                byte[] buffer = new byte[65536];
                ms.Position = 0;
                int num;
                do
                {
                    num = ms.Read(buffer, 0, buffer.Length);
                    Response.OutputStream.Write(buffer, 0, num);
                }

                while (num > 0);

                Response.Flush();
            }
        } 
    }
}
