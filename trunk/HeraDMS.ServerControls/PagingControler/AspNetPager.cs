using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace HeraDMS.ServerControls
{
    [Designer(typeof(AspNetPagerDesigner))]
    public class AspNetPager : WebControl, INamingContainer
    {
        public AspNetPager()
        {
            PageIndex = 1;
            PageSize = 20;
            FirstPageText = "首页";
            PrePageText = "上一页";
            NextPageText = "下一页";
            EndPageText = "末页";
            ButtonText = "转到";
        }

        #region 属性块
        private object baseState = null;
        private object buttonStyleState = null;
        private object textBoxStyleState = null;
        private object labelStyleState = null;
        private object linkButtonStyleState = null;
        private LinkButton _lnkbtnFrist;
        private LinkButton _lnkbtnPre;
        private LinkButton _lnkbtnNext;
        private LinkButton _lnkbtnLast;
        private Label _lblCurrentPage;
        private Label _lblRecodeCount;
        private Label _lblPageCount;
        private Label _lblPageSize;
        private TextBox _txtPageIndex;
        private Button _btnChangePage;
        private LinkButton _page_0;
        private LinkButton _page_1;
        private LinkButton _page_2;
        private LinkButton _page_3;
        private LinkButton _page_4;
        private LinkButton _page_5;
        private LinkButton _page_6;
        private LinkButton _page_7;
        private LinkButton _page_8;
        private LinkButton _page_9;
        private static readonly object EventPageChange = new object();
        [Category("Pagination"), Description("是否显示记录数"),
        DefaultValue(true)]
        public virtual bool ShowSize
        {
            get
            {
                EnsureChildControls();
                return ViewState["ShowSize"] != null ? (bool)ViewState["ShowSize"] : true;
            }
            set
            {
                EnsureChildControls();
                ViewState["ShowSize"] = value;
                _lblCurrentPage.Visible = _lblRecodeCount.Visible = _lblPageCount.Visible = _lblPageSize.Visible = value;
            }
        }
        [Category("Pagination"), Description("每页显示的记录数"),
        DefaultValue(20)]
        public virtual int PageSize
        {
            get
            {
                EnsureChildControls();
                return _lblPageSize.Text.Trim() != "" ? int.Parse(_lblPageSize.Text.Trim()) : 20;
            }
            set
            {
                EnsureChildControls();
                _lblPageSize.Text = value.ToString();
            }
        }
        [Category("Pagination"), Description("总记录数"),
        DefaultValue(0), Bindable(true)]
        public virtual int RecordCount
        {
            get
            {
                EnsureChildControls();
                return _lblRecodeCount.Text.Trim() != "" ? int.Parse(_lblRecodeCount.Text.Trim()) : 0;
            }
            set
            {
                EnsureChildControls();
                if (value > 0)
                {
                    int recodeCount = value;
                    _lblPageCount.Text = (value % PageSize == 0 ? value / PageSize : value / PageSize + 1).ToString();//计算总页数
                }
                _lblRecodeCount.Text = value.ToString();
            }
        }
        [Category("Pagination"), Description("当前页码"),
        DefaultValue(1), Bindable(true)]
        public virtual int PageIndex
        {
            get
            {
                EnsureChildControls();
                return _lblCurrentPage.Text.Trim() != "" ? int.Parse(_lblCurrentPage.Text.Trim()) : 1;
            }
            set
            {
                EnsureChildControls();
                _lblCurrentPage.Text = value.ToString();
            }
        }
        [Category("Appearance"), Description("设置第一页的文本"),
        DefaultValue("首页"), Bindable(true)]
        public virtual string FirstPageText
        {
            get
            {
                EnsureChildControls();
                return _lnkbtnFrist.Text.Trim() != "" ? _lnkbtnFrist.Text.Trim() : "首页";
            }
            set
            {
                EnsureChildControls();
                _lnkbtnFrist.Text = value;
            }
        }
        [Category("Appearance"), Description("设置上一页的文本"),
        DefaultValue("上一页"), Bindable(true)]
        public virtual string PrePageText
        {
            get
            {
                EnsureChildControls();
                return _lnkbtnPre.Text.Trim() != "" ? _lnkbtnPre.Text.Trim() : "上一页";
            }
            set
            {
                EnsureChildControls();
                _lnkbtnPre.Text = value;
            }
        }
        [Category("Appearance"), Description("设置下一页的文本"),
        DefaultValue("下一页"), Bindable(true)]
        public virtual string NextPageText
        {
            get
            {
                EnsureChildControls();
                return _lnkbtnNext.Text.Trim() != "" ? _lnkbtnNext.Text.Trim() : "下一页";
            }
            set
            {
                EnsureChildControls();
                _lnkbtnNext.Text = value;
            }
        }
        [Category("Appearance"), Description("设置末页的文本"),
        DefaultValue("末页"), Bindable(true)]
        public virtual string EndPageText
        {
            get
            {
                EnsureChildControls();
                return _lnkbtnLast.Text.Trim() != "" ? _lnkbtnLast.Text.Trim() : "末页";
            }
            set
            {
                EnsureChildControls();
                _lnkbtnLast.Text = value;
            }
        }
        [Category("Appearance"), Description("是否显示跳转"),
        DefaultValue(true)]
        public virtual bool ShowGo
        {
            get
            {
                EnsureChildControls();
                return ViewState["ShowGo"] != null ? (bool)ViewState["ShowGo"] : true;
            }
            set
            {
                EnsureChildControls();
                ViewState["ShowGo"] = value;
                _txtPageIndex.Visible = _btnChangePage.Visible = value;
            }
        }
        [Category("Appearance"), Description("设置跳转按钮的文本"),
        DefaultValue("转到"), Bindable(true)]
        public virtual string ButtonText
        {
            get
            {
                EnsureChildControls();
                return _btnChangePage.Text.Trim() != "" ? _btnChangePage.Text.Trim() : "转到";
            }
            set
            {
                EnsureChildControls();
                _btnChangePage.Text = value;
            }
        }
        [Category("Behavior"), Description("设置分页模式"),
        DefaultValue(AspNetPageMode.Comprehensive)]
        public virtual AspNetPageMode PageMode
        {
            get
            {
                return ViewState["PageMode"] != null ? (AspNetPageMode)ViewState["PageMode"] : AspNetPageMode.Comprehensive;
            }
            set
            {
                ViewState["PageMode"] = value;
            }
        }
        [Category("Behavior"), Description("设置对齐方式"),
        DefaultValue("left")]
        public virtual string Align
        {
            get
            {
                return ViewState["Align"] != null ? (string)ViewState["Align"] : "left";
            }
            set
            {
                ViewState["Align"] = value;
            }
        }
        #endregion

        #region 分页事件相关
        public event EventHandler PageChanged
        {
            add
            {
                Events.AddHandler(EventPageChange, value);
            }
            remove
            {
                Events.RemoveHandler(EventPageChange, value);
            }

        }
        protected void OnPageChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler)Events[EventPageChange];
            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion

        #region 样式属性
        private Style _buttonStyle;
        private Style _textBoxStyle;
        private Style _linkButtonStyle;
        private Style _labelStyle;
        [
        Category("Styles"),
        DefaultValue(null),
        DesignerSerializationVisibility(
            DesignerSerializationVisibility.Content),
        PersistenceMode(PersistenceMode.InnerProperty),
        Description(
            "应用于按钮的样式")
        ]
        public virtual Style ButtonStyle
        {
            get
            {
                if (_buttonStyle == null)
                {
                    _buttonStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_buttonStyle).TrackViewState();
                    }
                }
                return _buttonStyle;
            }
        }
        [
        Category("Styles"),
        DefaultValue(null),
        DesignerSerializationVisibility(
           DesignerSerializationVisibility.Content),
        PersistenceMode(PersistenceMode.InnerProperty),
        Description(
           "应用于链接按钮的样式")
        ]
        public virtual Style LinkButtonStyle
        {
            get
            {
                if (_linkButtonStyle == null)
                {
                    _linkButtonStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_linkButtonStyle).TrackViewState();
                    }
                }
                return _linkButtonStyle;
            }
        }
        [
        Category("Styles"),
        DefaultValue(null),
        DesignerSerializationVisibility(
            DesignerSerializationVisibility.Content),
        PersistenceMode(PersistenceMode.InnerProperty),
        Description(
            "应用于文本框的样式")
        ]
        public virtual Style TextBoxStyle
        {
            get
            {
                if (_textBoxStyle == null)
                {
                    _textBoxStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_textBoxStyle).TrackViewState();
                    }
                }
                return _textBoxStyle;
            }
        }
        [
        Category("Styles"),
        DefaultValue(null),
        DesignerSerializationVisibility(
            DesignerSerializationVisibility.Content),
        PersistenceMode(PersistenceMode.InnerProperty),
        Description(
            "应用于标签的样式")
        ]
        public virtual Style LabelStyle
        {
            get
            {
                if (_labelStyle == null)
                {
                    _labelStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_labelStyle).TrackViewState();
                    }
                }
                return _labelStyle;
            }
        }
        #endregion

        #region 自定义视图状态
        protected override void LoadViewState(object savedState)
        {
            if (savedState == null)
            {
                base.LoadViewState(null);
                return;
            }
            else
            {
                Triplet t = savedState as Triplet;

                if (t != null)
                {
                    base.LoadViewState(baseState);

                    if ((t.Second) != null)
                    {
                        ((IStateManager)ButtonStyle).LoadViewState(buttonStyleState);
                    }

                    if ((t.Third) != null)
                    {
                        ((IStateManager)TextBoxStyle).LoadViewState(textBoxStyleState);
                    }
                    if (labelStyleState != null)
                    {
                        ((IStateManager)(_labelStyle)).LoadViewState(labelStyleState);
                    }
                    if (linkButtonStyleState != null)
                    {
                        ((IStateManager)(_linkButtonStyle)).LoadViewState(linkButtonStyleState);
                    }
                }
                else
                {
                    throw new ArgumentException("Invalid view state.");
                }
            }
        }

        protected override object SaveViewState()
        {
            baseState = base.SaveViewState();
            buttonStyleState = null;
            textBoxStyleState = null;
            labelStyleState = null;
            linkButtonStyleState = null;
            if (_buttonStyle != null)
            {
                buttonStyleState =
                    ((IStateManager)_buttonStyle).SaveViewState();
            }

            if (_textBoxStyle != null)
            {
                textBoxStyleState =
                    ((IStateManager)_textBoxStyle).SaveViewState();
            }
            if (_labelStyle != null)
            {
                labelStyleState = ((IStateManager)_labelStyle).SaveViewState();
            }
            if (_linkButtonStyle != null)
            {
                linkButtonStyleState = ((IStateManager)_linkButtonStyle).SaveViewState();
            }
            return new Triplet(baseState,
                buttonStyleState, textBoxStyleState);

        }

        protected override void TrackViewState()
        {
            base.TrackViewState();
            if (_buttonStyle != null)
            {
                ((IStateManager)_buttonStyle).TrackViewState();
            }
            if (_textBoxStyle != null)
            {
                ((IStateManager)_textBoxStyle).TrackViewState();
            }
            if (_labelStyle != null)
            {
                ((IStateManager)_labelStyle).TrackViewState();
            }
            if (_linkButtonStyle != null)
            {
                ((IStateManager)_linkButtonStyle).TrackViewState();
            }
        }
        #endregion

        #region 生成控件
        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            _btnChangePage = new Button();
            _btnChangePage.ID = "btnChangePage";
            _btnChangePage.Click += new EventHandler(BtnChangePage_Click);
            _lblCurrentPage = new Label();
            _lblCurrentPage.ID = "lblCurrentPage";
            _lblPageCount = new Label();
            _lblPageCount.ID = "lblPageCount";
            _lblRecodeCount = new Label();
            _lblRecodeCount.ID = "lblRecodeCount";
            _lnkbtnFrist = new LinkButton();
            _lnkbtnFrist.ID = "lnkbtnFrist";
            _lnkbtnFrist.Click += new EventHandler(lnkbtnFrist_Click);
            _lnkbtnLast = new LinkButton();
            _lnkbtnLast.ID = "lnkbtnLast";
            _lnkbtnLast.Click += new EventHandler(lnkbtnLast_Click);
            _lnkbtnNext = new LinkButton();
            _lnkbtnNext.ID = "lnkbtnNext";
            _lnkbtnNext.Click += new EventHandler(lnkbtnNext_Click);
            _lnkbtnPre = new LinkButton();
            _lnkbtnPre.ID = "lnkbtnPre";
            _lnkbtnPre.Click += new EventHandler(lnkbtnPre_Click);
            _page_0 = new LinkButton();
            _page_0.ID = "_page_0";
            _page_0.Click += new EventHandler(CurrentPage_Click);
            _page_1 = new LinkButton();
            _page_1.ID = "_page_1";
            _page_1.Click += new EventHandler(CurrentPage_Click);
            _page_2 = new LinkButton();
            _page_2.ID = "_page_2";
            _page_2.Click += new EventHandler(CurrentPage_Click);
            _page_3 = new LinkButton();
            _page_3.ID = "_page_3";
            _page_3.Click += new EventHandler(CurrentPage_Click);
            _page_4 = new LinkButton();
            _page_4.ID = "_page_4";
            _page_4.Click += new EventHandler(CurrentPage_Click);
            _page_5 = new LinkButton();
            _page_5.ID = "_page_5";
            _page_5.Click += new EventHandler(CurrentPage_Click);
            _page_6 = new LinkButton();
            _page_6.ID = "_page_6";
            _page_6.Click += new EventHandler(CurrentPage_Click);
            _page_7 = new LinkButton();
            _page_7.ID = "_page_7";
            _page_7.Click += new EventHandler(CurrentPage_Click);
            _page_8 = new LinkButton();
            _page_8.ID = "_page_8";
            _page_8.Click += new EventHandler(CurrentPage_Click);
            _page_9 = new LinkButton();
            _page_9.ID = "_page_9";
            _page_9.Click += new EventHandler(CurrentPage_Click);
            _txtPageIndex = new TextBox();
            _txtPageIndex.ID = "txtPageIndex";
            _lblPageSize = new Label();
            _lblPageSize.ID = "lblPageSize";
            this.Controls.Add(_page_0);
            this.Controls.Add(_page_1);
            this.Controls.Add(_page_2);
            this.Controls.Add(_page_3);
            this.Controls.Add(_page_4);
            this.Controls.Add(_page_5);
            this.Controls.Add(_page_6);
            this.Controls.Add(_page_7);
            this.Controls.Add(_page_8);
            this.Controls.Add(_page_9);
            this.Controls.Add(_btnChangePage);
            this.Controls.Add(_lblCurrentPage);
            this.Controls.Add(_lblPageCount);
            this.Controls.Add(_lblRecodeCount);
            this.Controls.Add(_lnkbtnFrist);
            this.Controls.Add(_lnkbtnLast);
            this.Controls.Add(_lnkbtnNext);
            this.Controls.Add(_lnkbtnPre);
            this.Controls.Add(_txtPageIndex);
            base.CreateChildControls();
        }
        #endregion

        #region 按钮点击事件
        #region 翻页相关的事件
        /// <summary>
        /// 处理翻页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkbtnFrist_Click(object sender, EventArgs e) //第一页
        {
            _lblCurrentPage.Text = "1";
            OnPageChanged(EventArgs.Empty);
        }
        protected void lnkbtnPre_Click(object sender, EventArgs e) //上一页
        {
            int pageIndex = int.Parse(_lblCurrentPage.Text);
            if (pageIndex > 0)
            {
                pageIndex--;
                _lblCurrentPage.Text = pageIndex.ToString();
                OnPageChanged(EventArgs.Empty);
            }
        }
        protected void lnkbtnNext_Click(object sender, EventArgs e)//下一页
        {
            int pageIndex = int.Parse(_lblCurrentPage.Text);
            int pageCount = int.Parse(_lblPageCount.Text);
            if (pageIndex < pageCount)
            {
                pageIndex++;
                _lblCurrentPage.Text = pageIndex.ToString();
            }
            OnPageChanged(EventArgs.Empty);
        }
        protected void lnkbtnLast_Click(object sender, EventArgs e)//末页
        {
            _lblCurrentPage.Text = _lblPageCount.Text;
            OnPageChanged(EventArgs.Empty);
        }
        protected void CurrentPage_Click(object sender, EventArgs e)
        {
            string strID = ((LinkButton)sender).ID;
            if (strID != "")
            {
                int pageIndex = 0;
                int pageNum = (PageIndex - 1) / 10;
                try
                {
                    int pageNum2 = int.Parse(strID.Split('_')[2]);
                    if (pageNum2 == 0)
                        pageNum2 = 10;
                    pageIndex = pageNum2 + pageNum * 10;
                }
                catch
                {
                    System.Web.HttpContext.Current.Response.Write("<Script>alert('非法的页码!');</script>");
                    return;
                }
                _lblCurrentPage.Text = pageIndex.ToString();
                OnPageChanged(EventArgs.Empty);
            }
        }
        #endregion
        protected void BtnChangePage_Click(object sender, EventArgs e)//跳转到指定页
        {
            int pageIndex = 0;
            try
            {
                pageIndex = int.Parse(_txtPageIndex.Text);
            }
            catch
            {
                System.Web.HttpContext.Current.Response.Write("<Script>alert('请输入正确的页数!');</script>");
                return;
            }
            int pageCount = int.Parse(_lblPageCount.Text);
            if (pageIndex == 0)//如果为0，则提示错误
            {
                System.Web.HttpContext.Current.Response.Write("<Script>alert('请输入正确的页数!');</script>");
                return;
            }
            if (pageIndex > pageCount)//如果大于总页数则提示错误
            {
                System.Web.HttpContext.Current.Response.Write("<Script>alert('请输入正确的页数!');</script>");
                return;
            }
            _lblCurrentPage.Text = pageIndex.ToString();
            OnPageChanged(EventArgs.Empty);
        }
        #endregion

        #region 重写TagKey
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Table;
            }
        }
        #endregion

        #region 绘制控件
        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (ButtonStyle != null)
            {
                _btnChangePage.ApplyStyle(ButtonStyle);
            }
            if (TextBoxStyle != null)
            {
                _txtPageIndex.ApplyStyle(TextBoxStyle);
            }
            if (LabelStyle != null)
            {
                _lblCurrentPage.ApplyStyle(LabelStyle);
                _lblPageCount.ApplyStyle(LabelStyle);
                _lblRecodeCount.ApplyStyle(LabelStyle);
                _lblPageSize.ApplyStyle(LabelStyle);
            }
            //AddAttributesToRender(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);
            writer.RenderBeginTag(HtmlTextWriterTag.Tr); // <tr>
            writer.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);
            writer.AddAttribute(HtmlTextWriterAttribute.Align, Align);
            writer.RenderBeginTag(HtmlTextWriterTag.Td); // <td>
            if (ShowSize)
            {
                writer.Write("当前第");
                if (_lblCurrentPage != null)
                    _lblCurrentPage.RenderControl(writer);
                writer.Write("页,每页");
                if (_lblPageSize != null)
                {
                    _lblPageSize.RenderControl(writer);
                }
                writer.Write("条记录,总共");
                if (_lblRecodeCount != null)
                    _lblRecodeCount.RenderControl(writer);
                writer.Write("条记录,共");
                if (_lblPageCount != null)
                    _lblPageCount.RenderControl(writer);
                writer.Write("页&nbsp;&nbsp;[&nbsp;&nbsp;");
            }
            else if (ShowGo)
            {
                writer.Write("[&nbsp;&nbsp;");
            }
            if (_lnkbtnFrist != null)
            {
                if (PageIndex == 1) //如果是第一页，则第一页灰显，作用是避免不必要的点击造成没必要的数据传输
                {
                    _lnkbtnFrist.Enabled = false;
                }
                else
                {
                    _lnkbtnFrist.Enabled = true;
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);
                _lnkbtnFrist.RenderControl(writer);
            }
            writer.Write("&nbsp;");
            if (_lnkbtnPre != null)
            {
                if (PageIndex > 1) //如果当前页大于1，则上一页显示，否则灰显
                {
                    _lnkbtnPre.Enabled = true;
                }
                else
                {
                    _lnkbtnPre.Enabled = false;
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);
                _lnkbtnPre.RenderControl(writer);
            }
            writer.Write("&nbsp;");
            if (PageMode == AspNetPageMode.Comprehensive)
            {
                if (_lblPageCount.Text == "")
                {
                    for (int i = 0; i < 10; i++)
                    {
                        LinkButton lb = (LinkButton)this.FindControl("_page_" + i.ToString());
                        lb.Enabled = false;
                        lb.Text = "[" + (i + 1).ToString() + "]";
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);
                        lb.RenderControl(writer);
                        writer.Write("&nbsp;");
                    }
                }
                else
                {
                    int pageCount = int.Parse(_lblPageCount.Text);
                    int quot = (PageIndex - 1) / 10;
                    int rema = PageIndex % 10;
                    //if (pageCount < 10 || pageCount - PageIndex < 10)
                    if (pageCount < 10 || pageCount - quot * 10 < 10)
                    {
                        for (int i = quot * 10 + 1; i <= pageCount; i++)
                        {
                            LinkButton lb2 = (LinkButton)this.FindControl("_page_" + (i % 10).ToString());
                            if (i == PageIndex)
                            {
                                lb2.Text = i.ToString();
                                lb2.Enabled = false;
                                lb2.Font.Bold = true;
                                lb2.ForeColor = Color.Red;
                            }
                            else
                            {
                                lb2.Text = "[" + i.ToString() + "]";
                                lb2.Enabled = true;
                            }
                            writer.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);
                            lb2.RenderControl(writer);
                            writer.Write("&nbsp;");
                        }
                    }
                    else
                    {
                        for (int i = quot * 10 + 1; i <= quot * 10 + 10; i++)
                        {
                            LinkButton lb2 = (LinkButton)this.FindControl("_page_" + (i % 10).ToString());
                            if (i == PageIndex)
                            {
                                lb2.Text = i.ToString();
                                lb2.Enabled = false;
                                lb2.Font.Bold = true;
                                lb2.ForeColor = Color.Red;
                            }
                            else
                            {
                                lb2.Text = "[" + i.ToString() + "]";
                                lb2.Enabled = true;
                            }
                            writer.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);
                            lb2.RenderControl(writer);
                            writer.Write("&nbsp;");
                        }
                    }
                }
            }
            if (_lnkbtnNext != null)
            {
                if (_lblPageCount.Text == "")
                {
                    _lnkbtnNext.Enabled = false;
                }
                else
                {
                    int pageCount = int.Parse(_lblPageCount.Text); //获取总页数
                    if (PageIndex < pageCount)//如果当前页小于总页数，则下一页显示，否则灰显
                    {
                        _lnkbtnNext.Enabled = true;
                    }
                    else
                    {
                        _lnkbtnNext.Enabled = false;
                    }
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);
                _lnkbtnNext.RenderControl(writer);
            }
            writer.Write("&nbsp;");
            if (_lnkbtnLast != null)
            {
                if (_lblPageCount.Text == "")
                {
                    _lnkbtnLast.Enabled = false;
                }
                else
                {
                    int pageCount = int.Parse(_lblPageCount.Text); //获取总页数
                    if (PageIndex == pageCount)//如果当前页为最后一页，则末页灰显
                    {
                        _lnkbtnLast.Enabled = false;
                    }
                    else
                    {
                        _lnkbtnLast.Enabled = true;
                    }
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);
                _lnkbtnLast.RenderControl(writer);
            }
            if (ShowGo)
            {
                writer.Write("&nbsp;&nbsp;]&nbsp;&nbsp;跳转到第");
                if (_txtPageIndex != null)
                    _txtPageIndex.RenderControl(writer);
                writer.Write("页");
                if (_btnChangePage != null)
                    _btnChangePage.RenderControl(writer);
            }
            else if (ShowSize)
            {
                writer.Write("&nbsp;&nbsp;]");
            }
            writer.RenderEndTag(); // </td>
            writer.RenderEndTag(); // </tr>
            //base.RenderContents(writer);
        }
        #endregion
    }
}
