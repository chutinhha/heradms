<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShowLoginPartUserControl.ascx.cs" Inherits="HeraDMS.CustomWebPart.ShowLoginPart.ShowLoginPartUserControl" %>
<object id="activeDemo" classid="clsid:FF422FEC-C082-4CF6-9F76-D1D030A8B98E" codebase="/_layouts/Lib/ActiveDemo.cab"
    height="200px" width="300px">
    </object>
    <a href="../../../_layouts/Lib/LocalCSSetup.msi">海运系统程序下载</a>
    <a href="http://172.29.129.79:10000/_admin/sssvc/ManageSSSvcApplication.aspx?AppId=58f00eeb-6a4a-4862-a49f-f8c323cbea13" target="_blank">单点登录(SSO)账户维护</a>
<asp:HiddenField ID="hfUserName" runat="server" />
<asp:HiddenField ID="hfPsd" runat="server" />
<br />
<a href="../../../_layouts/AuditLog/AuditLogList.aspx">审计日志</a>
<script defer language="javascript">

    SetActiveDemo();
    function SetActiveDemo() {
        var activeId = document.getElementById("activeDemo");
        var name = document.getElementById('<%=hfUserName.ClientID %>').value;
        var psd = document.getElementById('<%=hfPsd.ClientID %>').value;
        //alert(name+"|"+psd);
        activeId.SetUserAccount(name, psd);
    }

</script>
