using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace HeraDMS.ServerControls
{
    public class AspNetPagerActionList:DesignerActionList
    {
        private DesignerActionItemCollection _daic;
        private AspNetPagerDesigner _designer;
        public AspNetPagerActionList(AspNetPagerDesigner designer)
            : base(designer.Component)
        {
            _designer = designer;
        }
        #region 属性
        public bool Visible
        {
            get
            {
                AspNetPager pager = (AspNetPager)_designer.Component;
                return pager.Visible;
            }
            set
            {
                PropertyDescriptor prop = TypeDescriptor.GetProperties(_designer.Component)["Visible"];
                prop.SetValue(_designer.Component, value);
            }
        }
        public string ButtonText
        {
            get
            {
                AspNetPager pager = (AspNetPager)_designer.Component;
                return pager.ButtonText;
            }
            set
            {
                PropertyDescriptor prop = TypeDescriptor.GetProperties(_designer.Component)["ButtonText"];
                prop.SetValue(_designer.Component, value);
            }
        }
        public string FirstPageText
        {
            get
            {
                AspNetPager pager = (AspNetPager)_designer.Component;
                return pager.FirstPageText;
            }
            set
            {
                PropertyDescriptor prop = TypeDescriptor.GetProperties(_designer.Component)["FirstPageText"];
                prop.SetValue(_designer.Component, value);
            }
        }
        public string PrePageText
        {
            get
            {
                AspNetPager pager = (AspNetPager)_designer.Component;
                return pager.PrePageText;
            }
            set
            {
                PropertyDescriptor prop = TypeDescriptor.GetProperties(_designer.Component)["PrePageText"];
                prop.SetValue(_designer.Component, value);
            }
        }
        public string NextPageText
        {
            get
            {
                AspNetPager pager = (AspNetPager)_designer.Component;
                return pager.NextPageText;
            }
            set
            {
                PropertyDescriptor prop = TypeDescriptor.GetProperties(_designer.Component)["NextPageText"];
                prop.SetValue(_designer.Component, value);
            }
        }
        public string EndPageText
        {
            get
            {
                AspNetPager pager = (AspNetPager)_designer.Component;
                return pager.EndPageText;
            }
            set
            {
                PropertyDescriptor prop = TypeDescriptor.GetProperties(_designer.Component)["EndPageText"];
                prop.SetValue(_designer.Component, value);
            }
        }
        public int PageSize
        {
            get
            {
                AspNetPager pager = (AspNetPager)_designer.Component;
                return pager.PageSize;
            }
            set
            {
                PropertyDescriptor prop = TypeDescriptor.GetProperties(_designer.Component)["PageSize"];
                prop.SetValue(_designer.Component, value);
            }
        }
        public Unit Width
        {
            get
            {
                AspNetPager pager = (AspNetPager)_designer.Component;
                return pager.Width;
            }
            set
            {
                PropertyDescriptor prop = TypeDescriptor.GetProperties(_designer.Component)["Width"];
                prop.SetValue(_designer.Component, value);
            }
        }
        public Unit Height
        {
            get
            {
                AspNetPager pager = (AspNetPager)_designer.Component;
                return pager.Height;
            }
            set
            {
                PropertyDescriptor prop = TypeDescriptor.GetProperties(_designer.Component)["Height"];
                prop.SetValue(_designer.Component, value);
            }
        }
        public Unit BorderWidth
        {
            get
            {
                AspNetPager pager = (AspNetPager)_designer.Component;
                return pager.BorderWidth;
            }
            set
            {
                PropertyDescriptor prop = TypeDescriptor.GetProperties(_designer.Component)["BorderWidth"];
                prop.SetValue(_designer.Component, value);
            }
        }
        public BorderStyle BorderStyle
        {
            get
            {
                AspNetPager pager = (AspNetPager)_designer.Component;
                return pager.BorderStyle;
            }
            set
            {
                PropertyDescriptor prop = TypeDescriptor.GetProperties(_designer.Component)["BorderStyle"];
                prop.SetValue(_designer.Component, value);
            }
        }
        public Color BorderColor
        {
            get
            {
                AspNetPager pager = (AspNetPager)_designer.Component;
                return pager.BorderColor;
            }
            set
            {
                PropertyDescriptor prop = TypeDescriptor.GetProperties(_designer.Component)["BorderColor"];
                prop.SetValue(_designer.Component, value);
            }
        }
        public Color BackColor
        {
            get
            {
                AspNetPager pager = (AspNetPager)_designer.Component;
                return pager.BackColor;
            }
            set
            {
                PropertyDescriptor prop = TypeDescriptor.GetProperties(_designer.Component)["BackColor"];
                prop.SetValue(_designer.Component, value);
            }
        }
        
        #endregion
        public override DesignerActionItemCollection GetSortedActionItems()
        {
            if (_daic == null)
            {
                _daic = new DesignerActionItemCollection();
                _daic.Add(new DesignerActionHeaderItem("设计时面板设置"));
                _daic.Add(new DesignerActionPropertyItem("Visible","是否显示"));
                _daic.Add(new DesignerActionPropertyItem("ButtonText", "转到按钮文字"));
                _daic.Add(new DesignerActionPropertyItem("FirstPageText", "首页文本"));
                _daic.Add(new DesignerActionPropertyItem("PrePageText", "上一页文本"));
                _daic.Add(new DesignerActionPropertyItem("NextPageText", "下一页文本"));
                _daic.Add(new DesignerActionPropertyItem("EndPageText", "末页文本"));
                _daic.Add(new DesignerActionPropertyItem("PageSize", "每页记录数"));
                _daic.Add(new DesignerActionPropertyItem("Width","宽度"));
                _daic.Add(new DesignerActionPropertyItem("Height", "高度"));
                _daic.Add(new DesignerActionPropertyItem("BackColor","背景色"));
                _daic.Add(new DesignerActionPropertyItem("BorderStyle", "边框类型"));
                _daic.Add(new DesignerActionPropertyItem("BorderColor", "边框颜色"));
                _daic.Add(new DesignerActionPropertyItem("BorderWidth", "边框宽度"));
 
            }
            return _daic;
        }
    }
}
