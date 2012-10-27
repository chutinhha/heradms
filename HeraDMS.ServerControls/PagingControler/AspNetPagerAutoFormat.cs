using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web.UI;
using System.Web;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;
using System.Web.UI.WebControls;
using System.Drawing;

namespace HeraDMS.ServerControls
{
    public class AspNetPagerAutoFormat : DesignerAutoFormat
    {
        public AspNetPagerAutoFormat(string name) : base(name) { }
        public override void Apply(Control control)
        {
            if (control is AspNetPager)
            {
                AspNetPager aspNetPager = (AspNetPager)control;
                if (this.Name == "英文样式")
                {
                    aspNetPager.ButtonText = "Go";
                    aspNetPager.FirstPageText = "First";
                    aspNetPager.PrePageText = "Prev";
                    aspNetPager.NextPageText = "Next";
                    aspNetPager.EndPageText = "End";
                    aspNetPager.LabelStyle.ForeColor = Color.Blue;
                    aspNetPager.LabelStyle.Font.Bold = true;
                    aspNetPager.TextBoxStyle.CssClass = "blue_rounded";
                    aspNetPager.TextBoxStyle.Width = Unit.Parse("50px");
                    aspNetPager.PageSize = 20;
                    aspNetPager.RecordCount = 0;
                }
                else if (this.Name == "符号样式")
                {
                    aspNetPager.ButtonText = "转到";
                    aspNetPager.FirstPageText = "<font face=webdings color=\"red\">9</font>";
                    aspNetPager.PrePageText = "<font face=webdings color=\"red\">7</font>";
                    aspNetPager.NextPageText = "<font face=webdings color=\"red\">8</font>";
                    aspNetPager.EndPageText = "<font face=webdings color=\"red\">:</font>";
                    aspNetPager.LabelStyle.ForeColor = Color.Red;
                    aspNetPager.LabelStyle.Font.Bold = true;
                    aspNetPager.TextBoxStyle.CssClass = "blue_rounded";
                    aspNetPager.TextBoxStyle.Width = Unit.Parse("40px");
                    aspNetPager.PageSize = 30;
                }
                else if (this.Name == "默认样式")
                {
                    aspNetPager.ButtonText = "转到";
                    aspNetPager.FirstPageText = "首页";
                    aspNetPager.PrePageText = "上一页";
                    aspNetPager.NextPageText = "下一页";
                    aspNetPager.EndPageText = "末页";
                    aspNetPager.LabelStyle.ForeColor = Color.Blue;
                    aspNetPager.LabelStyle.Font.Bold = true;
                    aspNetPager.TextBoxStyle.CssClass = "blue_rounded";
                    aspNetPager.TextBoxStyle.Width = Unit.Parse("40px");
                    aspNetPager.PageSize = 20;
                    aspNetPager.RecordCount = 0;
                }

            }
            else
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }
    }
}
