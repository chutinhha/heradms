<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InitializeDocMenuWebPartUserControl.ascx.cs" Inherits="HeraDMS.CustomWebPart.InitializeDocMenuWebPart.InitializeDocMenuWebPartUserControl" %>
<script type="text/javascript">
    //文档右键菜单加载
    //c和a自己随便取名称
    function Custom_AddDocLibMenuItems(c, a) {
        //审核日志菜单
        //下拉菜单名称
        strDisplayText = "操作历史记录";

        //需要执行的脚本，可以自定义
        //a.HttpRoot当前的web站点地址
        //currentItemID当前的ItemId
        //a.listName当前列表名称
        strAction = "ShowItemLogDialog('" + currentItemID + "','" + a.listName + "')";

        //下拉菜单显示的图片
        strImagePath = a.imagesPath + "oisweb.gif";

        //执行脚本
        menuOption = CAMOpt(c, strDisplayText, strAction, strImagePath, null, 260);

        //菜单ID
        menuOption.id = "ID_MySubMenu";


        //评论菜单
        //给文档库的项的下拉菜单增加选项
        //下拉菜单名称
        strDisplayText1 = "评论";

        //需要执行的脚本，可以自定义
        //a.HttpRoot当前的web站点地址
        //currentItemID当前的ItemId
        //a.listName当前列表名称
        //STSNavigate是MOSS执行跳转的函数
        //strAction = "STSNavigate('"+a.HttpRoot+"/_layouts/AuditLog/AuditLogList.aspx?itemID="+currentItemID+"&listID="+ a.listName +"')";
        strAction1 = "ShowCommentDialog('" + currentItemID + "','" + a.listName + "')";

        //下拉菜单显示的图片
        strImagePath1 = a.imagesPath + "oisweb.gif";

        //执行脚本
        menuOption1 = CAMOpt(c, strDisplayText1, strAction1, strImagePath1, null, 260);

        //菜单ID
        menuOption1.id = "ID_MyCommentMenu";


        //我的收藏菜单
        //给文档库的项的下拉菜单增加选项
        //下拉菜单名称
        strDisplayText1 = "收藏";

        //需要执行的脚本，可以自定义
        strAction1 = "DoMyFavorites('" + currentItemID + "','" + a.listName + "')";

        //下拉菜单显示的图片
        strImagePath1 = a.imagesPath + "oisweb.gif";

        //执行脚本
        menuOption1 = CAMOpt(c, strDisplayText1, strAction1, strImagePath1, null, 260);

        //菜单ID
        menuOption1.id = "ID_MyFavoritesMenu";
        //return
        return false;
    }
    //我的收藏操作
    function DoMyFavorites(itemID, listID) {
        document.getElementById("<%=hfItemId.ClientID%>").value = itemID;
        document.getElementById("<%=hfListId.ClientID%>").value = listID;
        document.getElementById("<%=btnAddMyFavorites.ClientID%>").click();
    
    }
    //显示窗体
    function ShowItemLogDialog(itemID, listID) {
        var options = SP.UI.$create_DialogOptions();

        options.title = "审计日志";
        options.width = 800;
        options.height = 450;
        options.url = "/_layouts/Pages/AuditLog/AuditLogList.aspx?itemID=" + currentItemID + "&listID=" + listID;
        SP.UI.ModalDialog.showModalDialog(options);
    }

    //显示窗体
    function ShowCommentDialog(itemID, listID) {
        var options = SP.UI.$create_DialogOptions();

        options.title = "评论";
        options.width = 800;
        options.height = 450;
        options.url = "/_layouts/Pages/Comment/CommentList.aspx?itemID=" + currentItemID + "&listID=" + listID;
        SP.UI.ModalDialog.showModalDialog(options);
    }

	</script>
<asp:Button ID="btnAddMyFavorites" runat="server" Text="" style="display:none" 
    onclick="btnAddMyFavorites_Click" />
<asp:HiddenField ID="hfItemId" runat="server" />
<asp:HiddenField ID="hfListId" runat="server" />
