using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI.Design;
using System.ComponentModel.Design;

namespace HeraDMS.ServerControls
{
    [SupportsPreviewControl(true)]
    public class AspNetPagerDesigner : ControlDesigner
    {
        private DesignerAutoFormatCollection _dafc;
        public override DesignerAutoFormatCollection AutoFormats
        {
            get
            {
                if (_dafc == null)
                {
                    _dafc = new DesignerAutoFormatCollection();
                    _dafc.Add(new AspNetPagerAutoFormat("英文样式"));
                    _dafc.Add(new AspNetPagerAutoFormat("符号样式"));
                    _dafc.Add(new AspNetPagerAutoFormat("默认样式"));
                }
                return _dafc;
            }
        }
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                DesignerActionListCollection actionLists = new DesignerActionListCollection();
                actionLists.Add(new AspNetPagerActionList(this));

                return actionLists;
            }
        }  
    }
}
