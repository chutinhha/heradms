<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuditLogList.aspx.cs" Inherits="HeraDMS.Layouts.AuditLog.AuditLogList" DynamicMasterPageFile="~masterurl/default.master" %>

<%@ Register Assembly="HeraDMS.ServerControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=1df39b57a33dabfd" Namespace="HeraDMS.ServerControls" TagPrefix="cc1" %>


<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<table width="100%">
    <tr>
        <td align="right" style="width:80px;">时间范围：</td>
        <td>
        <SharePoint:DateTimeControl ID="dateStart" runat="server"  DateOnly="true" />
        </td>
        <td>
-
        </td>
        <td>
        <SharePoint:DateTimeControl ID="dateEnd" runat="server" DateOnly="true"/>
        </td>
        <td  align="right" style="width:50px;">用户：</td>
        <td>
        <SharePoint:PeopleEditor ID="userPicker" runat="server" selectionset="User" MultiSelect="false" Width="250px" /></td>
    </tr>
    <tr>
        <td  align="right">事件：</td>
        
        <td colspan="3">
            <asp:CheckBoxList ID="cbHandle" runat="server" RepeatDirection="Horizontal">
            <asp:ListItem Text="查看" Value="3" Selected="True" ></asp:ListItem>
             <asp:ListItem Text="更新" Value="5" Selected="True"></asp:ListItem>
              <asp:ListItem Text="删除" Value="4" Selected="True"></asp:ListItem>
               <asp:ListItem Text="签入" Value="2" Selected="True"></asp:ListItem>
                <asp:ListItem Text="签出" Value="1" Selected="True"></asp:ListItem>
                 <asp:ListItem Text="从回收站还原" Value="10" Selected="True"></asp:ListItem>
            </asp:CheckBoxList>
        </td>
        <td colspan="2">
            <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click"/>
            <asp:Button ID="btnClear" runat="server" Text="重置" OnClick="btnClear_Click"/>
            <asp:Button ID="btnExport" runat="server" Text="导出" OnClick="btnExport_Click"/>
            </td>
    </tr>
</table>
<asp:GridView ID="gvData" runat="server" Width="100%"  AutoGenerateColumns="false" CellPadding="4"  ForeColor="#333333" 
        GridLines="None" style="margin-right: 0px" EmptyDataText="无数据或未加载">
        <AlternatingRowStyle BackColor="White" />
<Columns>
<asp:BoundField HeaderText="用户" DataField="UserId" ItemStyle-Width="8%">
                <ItemStyle Width="10%" HorizontalAlign="Left" ></ItemStyle>
                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
            </asp:BoundField>
            <asp:BoundField HeaderText="事件" DataField="Event" ItemStyle-Width="8%">
                <ItemStyle Width="8%" HorizontalAlign="Left" ></ItemStyle>
                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
            </asp:BoundField>
            <asp:TemplateField HeaderText="文档路径" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <%#DataBinder.Eval(Container.DataItem, "DocLocation")%>
                </ItemTemplate>
                <ItemStyle Width="38%" HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="时间" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <%#DataBinder.Eval(Container.DataItem, "Occurred")%>
                </ItemTemplate>
                <ItemStyle Width="18%" HorizontalAlign="Left" />
            </asp:TemplateField>
             <asp:TemplateField HeaderText="事件源" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <%#DataBinder.Eval(Container.DataItem, "EventSource")%>
                </ItemTemplate>
                <ItemStyle Width="18%" HorizontalAlign="Left" />
            </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
       <%-- <SortedAscendingCellStyle BackColor="#F5F7FB" />
        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
        <SortedDescendingCellStyle BackColor="#E9EBEF" />
        <SortedDescendingHeaderStyle BackColor="#4870BE" />--%>
</asp:GridView>
   <cc1:AspNetPager ID="AspNetPager1" runat="server" ShowGo="false" ShowSize="true" OnPageChanged="AspNetPager1_PageChanged" >
</cc1:AspNetPager>
</asp:Content>


<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
   日志历史记录
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
My Application Page
</asp:Content>
