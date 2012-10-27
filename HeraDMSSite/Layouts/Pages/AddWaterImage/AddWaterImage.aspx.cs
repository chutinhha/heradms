using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI;
using HeraDMS.Layouts.Helper;
using System.IO;
using System.Drawing;
using HeraDMS.Core;

namespace HeraDMS.Layouts.Pages.AddWaterImage
{
    public partial class AddWaterImage : BasePage
    {
        #region 参数初始化
        /// <summary>
        /// 当前文档库的ID
        /// </summary>
        private string ListId
        {
            get
            {
                return ViewState["AuditLogList_ListId"] + "";
            }
            set
            {
                ViewState["AuditLogList_ListId"] = value;
            }
        }
        /// <summary>
        /// 需要添加水印图片在列表中的项的集合
        /// </summary>
        private List<int> ImageItems
        {
            get
            {
                if (ViewState["ImageItems"] != null)
                {
                    return (List<int>)ViewState["ImageItems"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                ViewState["ImageItems"] = value;
            }
        }
        /// <summary>
        /// 没有添加水印图片项的集合
        /// </summary>
        private List<int> SkipItems
        {
            get
            {
                if (ViewState["SkipItems"] != null)
                {
                    return (List<int>)ViewState["SkipItems"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                ViewState["SkipItems"] = value;
            }
        }
        /// <summary>
        /// 添加水印图片失败项的集合
        /// </summary>
        private List<int> LoseItems
        {
            get
            {
                if (ViewState["LoseItems"] != null)
                {
                    return (List<int>)ViewState["LoseItems"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                ViewState["LoseItems"] = value;
            }
        }
        
        WaterImageManage WaterImageManage = new WaterImageManage();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //图片所在列表
                ListId = (Request.QueryString["listID"] != null) ? Request.QueryString["listID"].ToString().Trim().ToLower() : "";
                //列表中选择的项的集合 | 拼接而成
                string selectedItems = (Request.QueryString["selectedItemIds"] != null) ? Request.QueryString["selectedItemIds"].ToString().Trim().ToLower() : "";
                //当有传递参数的时候
                if (!String.IsNullOrEmpty(ListId) && !String.IsNullOrEmpty(selectedItems))
                {
                    //分割
                    string[] items = selectedItems.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    InitData();
                    SkipItems = new List<int>();
                    GetImageItems(items);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Type), System.Guid.NewGuid().ToString(), "alert('参数不正确，无法添加水印！');window.frameElement.commonModalDialogClose(1, 1);", true);
                }

            }
        }
        /// <summary>
        /// 初始化页面的数据
        /// </summary>
        private void InitData()
        {
            this.ddlPosition.Items.Clear();
            this.ddlPosition.Items.Insert(0, new ListItem("左上", ImagePosition.LeftTop.ToString()));
            this.ddlPosition.Items.Insert(1, new ListItem("左下", ImagePosition.LeftBottom.ToString()));
            this.ddlPosition.Items.Insert(2, new ListItem("右上", ImagePosition.RightTop.ToString()));
            this.ddlPosition.Items.Insert(3, new ListItem("右下", ImagePosition.RigthBottom.ToString()));
            this.ddlPosition.Items.Insert(4, new ListItem("顶部居中", ImagePosition.TopMiddle.ToString()));
            this.ddlPosition.Items.Insert(5, new ListItem("底部居中", ImagePosition.BottomMiddle.ToString()));
            this.ddlPosition.Items.Insert(6, new ListItem("中心", ImagePosition.Center.ToString()));

        }

        /// <summary>
        /// 从列表中获取选择项，并且取得图片的项
        /// </summary>
        /// <param name="items"></param>
        private void GetImageItems(string[] selectedItems)
        {
            List<int> items = new List<int>();
            if (selectedItems != null)
            {
                SPSite siteColl = SPContext.Current.Site;
                SPWeb site = SPContext.Current.Web;
                //提高权限
                SPSecurity.RunWithElevatedPrivileges(
                delegate()
                {
                    using (SPSite ElevatedSiteCollection = new SPSite(siteColl.ID))
                    {
                        using (SPWeb ElevatedSite = ElevatedSiteCollection.OpenWeb(site.ID))
                        {

                            //取得列表
                            SPList list = ElevatedSite.Lists[new Guid(ListId)];
                            foreach (string item in selectedItems)
                            {
                                //取得列表项 
                                SPListItem listItem = list.GetItemById(Convert.ToInt32(item));
                                
                                //当是文件是图片的时候
                                if (listItem != null && listItem.FileSystemObjectType == SPFileSystemObjectType.File)
                                {
                                    //取得后缀名
                                    string extension = Path.GetExtension(listItem.Url);
                                    if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".gif") || extension.ToLower().Equals(".png"))
                                    {
                                        items.Add(listItem.ID);
                                        //把第一张图片放到预览窗口
                                        if (String.IsNullOrEmpty(ImgPreview.ImageUrl))
                                        {
                                            this.ImgPreview.ImageUrl = listItem.File.ServerRelativeUrl;
                                            this.ImgPreview.Attributes.Add("ItemId", listItem.ID.ToString());

                                        }
                                    }
                                    else
                                    {
                                        SkipItems.Add(listItem.ID);
                                    }
                                }
                                else
                                {
                                    SkipItems.Add(listItem.ID);
                                }

                            }
                        }
                    }
                });
            }
            //判断是否存在图片
            if (items != null && items.Count > 0)
            {
                ImageItems = items;
            }
            else
            {
                //base.AlertMessage("", "请选择.jpg,.gif,.png类型的图片添加水印！");
                ScriptManager.RegisterStartupScript(this, typeof(Type), System.Guid.NewGuid().ToString(), "alert('请选择.jpg,.gif,.png类型的图片添加水印！');window.frameElement.commonModalDialogClose(1, 1);", true);
            }
        }
        #region 按钮事件
        /// <summary>
        /// 预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPreview_Click(object sender, EventArgs e)
        {
            //当选择水印图片的时候
            if (rblType.SelectedValue.Equals("Y"))
            {
                string imageID = ImgPreview.Attributes["ItemId"] + "";
                string waterImageUrl = ImgPicture.Attributes["WaterPath"];
                if (String.IsNullOrEmpty(imageID) || String.IsNullOrEmpty(waterImageUrl))
                {
                    base.AlertMessage("", "无法预览！");
                    return;
                }
                //ImgPicture.
                byte[] image = null;

                SPSite siteColl = SPContext.Current.Site;
                SPWeb site = SPContext.Current.Web;
                //提高权限
                SPSecurity.RunWithElevatedPrivileges(
                delegate()
                {
                    using (SPSite ElevatedSiteCollection = new SPSite(siteColl.ID))
                    {
                        using (SPWeb ElevatedSite = ElevatedSiteCollection.OpenWeb(site.ID))
                        {

                            //取得列表
                            SPList list = ElevatedSite.Lists[new Guid(ListId)];
                            //取得列表项 
                            SPListItem listItem = list.GetItemById(Convert.ToInt32(imageID));
                            image = listItem.File.OpenBinary();
                        }
                    }
                });

                if (image != null)
                {
                    Stream str = new MemoryStream(image);
                    //创建存放文件的目录
                    Guid gd = Guid.NewGuid();
                    string filePath = Server.MapPath("~/") + "UploadFile" + "\\" + gd.ToString();
                    if (Directory.Exists(filePath))
                    {
                        Directory.Delete(filePath, true);
                    }
                    Directory.CreateDirectory(filePath);
                    filePath = filePath + "\\" + Path.GetFileName(ImgPreview.ImageUrl);
                    if (WaterImageManage.DrawImage(str, ImgPicture.ImageUrl, float.Parse(ddlAlpha.SelectedValue), float.Parse(ddlImgSize.SelectedValue), WaterImageManage.GetPosition(ddlPosition.SelectedValue), filePath))
                    {
                        this.ImgPreview.ImageUrl = "/UploadFile/" + gd.ToString() + "/" + Path.GetFileName(ImgPreview.ImageUrl);
                    }
                    else

                    {
                        base.AlertMessage("", "无法预览！");
                    }
                }
                else
                {
                    base.AlertMessage("", "无法预览！");
                }
            }
            else
            {
                string words = tbWord.Text.Trim();
                string imageID = ImgPreview.Attributes["ItemId"] + "";
                if (String.IsNullOrEmpty(imageID) || String.IsNullOrEmpty(words))
                {
                    base.AlertMessage("", "无法预览！");
                    return;
                }
                //ImgPicture.
                byte[] image = null;

                SPSite siteColl = SPContext.Current.Site;
                SPWeb site = SPContext.Current.Web;
                //提高权限
                SPSecurity.RunWithElevatedPrivileges(
                delegate()
                {
                    using (SPSite ElevatedSiteCollection = new SPSite(siteColl.ID))
                    {
                        using (SPWeb ElevatedSite = ElevatedSiteCollection.OpenWeb(site.ID))
                        {

                            //取得列表
                            SPList list = ElevatedSite.Lists[new Guid(ListId)];
                            //取得列表项 
                            SPListItem listItem = list.GetItemById(Convert.ToInt32(imageID));
                            image = listItem.File.OpenBinary();
                        }
                    }
                });

                if (image != null)
                {
                    Stream str = new MemoryStream(image);
                    //创建存放文件的目录
                    Guid gd = Guid.NewGuid();
                    string filePath = Server.MapPath("~/") + "UploadFile" + "\\" + gd.ToString();
                    if (Directory.Exists(filePath))
                    {
                        Directory.Delete(filePath, true);
                    }
                    Directory.CreateDirectory(filePath);
                    filePath = filePath + "\\" + Path.GetFileName(ImgPreview.ImageUrl);
                    Font f = GetWordFont();
                    Color col = System.Drawing.ColorTranslator.FromHtml(ColorPicker1.Color);
                    if (WaterImageManage.DrawWords(str, words, float.Parse(ddlAlpha.SelectedValue), f, col, WaterImageManage.GetPosition(ddlPosition.SelectedValue), filePath))
                    {
                        this.ImgPreview.ImageUrl = "/UploadFile/" + gd.ToString() + "/" + Path.GetFileName(ImgPreview.ImageUrl);
                    }
                    else
                    {
                        base.AlertMessage("", "无法预览！");
                    }
                }
                else
                {
                    base.AlertMessage("", "无法预览！");
                }
            }
        }

        /// <summary>
        /// 确认事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            
            //有需要添加水印图片的时候
            if (ImageItems != null && ImageItems.Count > 0)
            {
                //当选择图片水印的时候
                if (rblType.SelectedValue.Equals("Y"))
                {
                    string waterImageUrl = ImgPicture.ImageUrl;
                    if (String.IsNullOrEmpty(waterImageUrl))
                    {
                        base.AlertMessage("", "无法添加水印，缺少水印图片！");
                        return;
                    }
                }
                else
                {
                    string words = tbWord.Text.Trim();
                    if (String.IsNullOrEmpty(words))
                    {
                        base.AlertMessage("", "无法添加水印，缺少水印文字！");
                        return;
                    }
                }

                AddWater();
                base.AlertMessage("", "水印添加成功！");
                ScriptManager.RegisterStartupScript(this, typeof(Type), System.Guid.NewGuid().ToString(), "window.frameElement.commonModalDialogClose(1, 1);", true);
            }
        }

        /// <summary>
        /// 给图片添加水印
        /// </summary>
        private void AddWater()
        {
            SPSite siteColl = SPContext.Current.Site;
            SPWeb site = SPContext.Current.Web;
            List<WaterImageEntity> SuccessItems = new List<WaterImageEntity>();
            //提高权限
            SPSecurity.RunWithElevatedPrivileges(
            delegate()
            {
                using (SPSite ElevatedSiteCollection = new SPSite(siteColl.ID))
                {
                    using (SPWeb ElevatedSite = ElevatedSiteCollection.OpenWeb(site.ID))
                    {

                        ElevatedSiteCollection.AllowUnsafeUpdates = true;
                        ElevatedSite.AllowUnsafeUpdates = true;
                        //取得列表
                        SPList list = ElevatedSite.Lists[new Guid(ListId)];
                        
                        //ImgPicture.
                        byte[] image = null;
                        //循环能添加水印的图片
                        foreach (int i in ImageItems)
                        {
                            //取得列表项 
                            SPListItem listItem = list.GetItemById(i);
                            image = listItem.File.OpenBinary();
                            //当图片存在的时候
                            if (image != null && image.Length > 0)
                            {
                                #region 创建服务器暂存图片文件路径
                                Stream str = new MemoryStream(image);
                                //创建存放文件的目录
                                Guid gd = Guid.NewGuid();
                                string filePath = Server.MapPath("~/") + "UploadFile" + "\\" + gd.ToString();
                                if (Directory.Exists(filePath))
                                {
                                    Directory.Delete(filePath, true);
                                }
                                Directory.CreateDirectory(filePath);
                                //文档全路径地址
                                filePath = filePath + "\\" + listItem.Name;
                                #endregion

                                //当选择水印图片的时候
                                if (rblType.SelectedValue.Equals("Y"))
                                {
                                    //调用给图片添加水印图片方法
                                    if (WaterImageManage.DrawImage(str, ImgPicture.ImageUrl, float.Parse(ddlAlpha.SelectedValue), float.Parse(ddlImgSize.SelectedValue), WaterImageManage.GetPosition(ddlPosition.SelectedValue), filePath))
                                    {
                                        WaterImageEntity entity = new WaterImageEntity();
                                        entity.ItemId = i;
                                        entity.ItemName = listItem.Name;
                                        entity.ItemUrl = listItem.File.ServerRelativeUrl;
                                        SuccessItems.Add(entity);
                                    }
                                    //当失败的时候
                                    else
                                    {
                                        LoseItems.Add(i);
                                    }
                                }
                                //当选择水印文字的时候
                                else
                                {
                                    //需要添加的水印文字
                                    string words = tbWord.Text.Trim();
                                    //文字样式
                                    Font f = GetWordFont();
                                    //文字颜色
                                    Color col = System.Drawing.ColorTranslator.FromHtml(ColorPicker1.Color);
                                    //调用添加文字水印
                                    if (WaterImageManage.DrawWords(str, words, float.Parse(ddlAlpha.SelectedValue), f, col, WaterImageManage.GetPosition(ddlPosition.SelectedValue), filePath))
                                    {
                                        WaterImageEntity entity = new WaterImageEntity();
                                        entity.ItemId = i;
                                        entity.ItemName = listItem.Name;
                                        entity.ItemUrl = listItem.File.ServerRelativeUrl;
                                        SuccessItems.Add(entity);
                                    }
                                    else
                                    {
                                        LoseItems.Add(i);
                                    }
                                }
                                //给文档库更新文档
                                if (File.Exists(filePath))
                                {
                                    listItem.File.SaveBinary(File.ReadAllBytes(filePath));
                                }
                            }
                            list.Update();
                        }
                    }
                }
            });
        }
        /// <summary>
        /// 上传水印图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileUpload1.PostedFile.FileName.Trim() == "")
                {
                    base.AlertMessage("","请选择.jpg,.gif,.png类型的图片上传！");
                    return;
                }
                //文件名称
                string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);

                Guid gd = Guid.NewGuid();
                //创建存放文件的目录
                string filePath = Server.MapPath("~/") + "\\" + "UploadFile" + "\\" + gd.ToString();
                if (Directory.Exists(filePath))
                {
                    Directory.Delete(filePath, true);
                }
                Directory.CreateDirectory(filePath);
                string filefullpath = filePath + "\\" + fileName;

                //拷贝到服务器目录
                FileUpload1.SaveAs(filefullpath);

                //img.
                this.ImgPicture.ImageUrl = "/UploadFile/" + gd.ToString() + "/" + fileName;
                this.ImgPicture.Attributes.Add("WaterPath", filefullpath);
            }
            catch (Exception ex)
            {
                base.AlertMessage("", "上传错误！");
            }
        }

        /// <summary>
        /// 水印类型选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //当选择图片水印
            if (rblType.SelectedValue.Equals("Y"))
            {
                this.Panel1.Visible = true;
                this.Panel2.Visible = false;

            }
            //当选择文字水印
            else
            {
                this.Panel1.Visible = false;
                this.Panel2.Visible = true;
            }
        }
        /// <summary>
        /// 输入文字样式预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tbWord_TextChanged(object sender, EventArgs e)
        {
            WordPreview();
        }
        /// <summary>
        /// 输入文字样式预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlFamily_SelectedIndexChanged(object sender, EventArgs e)
        {
            WordPreview();
        }
        /// <summary>
        /// 输入文字样式预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            WordPreview();
        }
        /// <summary>
        /// 输入文字样式预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cblFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            WordPreview();
        }

        /// <summary>
        /// 输入文字样式预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ColorPicker1_OnColorChanged(object sender, EventArgs e)
        {
            WordPreview();
        }
        /// <summary>
        /// 字体预览
        /// </summary>
        private void WordPreview()
        {
            if (!String.IsNullOrEmpty(tbWord.Text.Trim()))
            {

                string family = ddlFamily.SelectedItem.Text;
                string size = ddlSize.SelectedItem.Text;
                Color col = System.Drawing.ColorTranslator.FromHtml(ColorPicker1.Color);

                this.ImgWord.Text = tbWord.Text.Trim();

                ImgWord.ForeColor = col;
                ImgWord.Font.Size = FontUnit.Parse(size);
                ImgWord.Font.Name = family;
                //设置字体粗细
                foreach (ListItem item in cblFont.Items)
                {
                    if (item.Selected)
                    {
                        if (item.Value.Equals("B"))
                        {
                            ImgWord.Font.Bold = true;
                        }
                        else
                        {
                            ImgWord.Font.Italic = true;
                        }

                    }
                }

            }
        }

        /// <summary>
        /// 设置字体样式
        /// </summary>
        /// <returns></returns>
        private Font GetWordFont()
        {
            string family = ddlFamily.SelectedItem.Text;
            string size = ddlSize.SelectedItem.Text;

            Font f = null;
            bool isB = false;
            bool isI = false;
            //设置字体粗细
            foreach (ListItem item in cblFont.Items)
            {
                if (item.Selected)
                {
                    if (item.Value.Equals("B"))
                    {
                        isB = true;
                    }
                    else if(item.Value.Equals("I"))
                    {
                        isI = true;
                    }

                }
            }
            if (isB)
            {
                if (isI)
                {
                    f = new Font(family, Convert.ToInt32(size), FontStyle.Bold | FontStyle.Italic);
                }
                else
                {
                    f = new Font(family, Convert.ToInt32(size), FontStyle.Bold);
                }
            }
            else
            {
                if (isI)
                {
                    f = new Font(family, Convert.ToInt32(size), FontStyle.Italic);
                }
                else
                {
                    f = new Font(family, Convert.ToInt32(size));
                }
            }

            return f;
        }
        #endregion
    }
}
