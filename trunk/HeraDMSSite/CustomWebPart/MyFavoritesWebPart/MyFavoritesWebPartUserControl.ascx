<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyFavoritesWebPartUserControl.ascx.cs" Inherits="HeraDMS.CustomWebPart.MyFavoritesWebPart.MyFavoritesWebPartUserControl" %>
<%@ Register Assembly="HeraDMS.ServerControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=1df39b57a33dabfd" Namespace="HeraDMS.ServerControls" TagPrefix="cc1" %>
<div style="width:100%">
<asp:GridView ID="gvMyFavorites" runat="server" AutoGenerateColumns="False" 
        EnableModelValidation="True" Width="100%" CellPadding="4"  ForeColor="#333333" 
        GridLines="None" style="margin-right: 0px" EmptyDataText="无数据或未加载" >
        <AlternatingRowStyle BackColor="White" />
    <Columns>
         <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="标题" ItemStyle-Width="70%">
            <ItemTemplate>
                <asp:HyperLink ID="hlTitle" runat="server" NavigateUrl='<%#DataBinder.Eval(Container.DataItem, "DocUrl")%>'><%#DataBinder.Eval(Container.DataItem, "DocTitle")%></asp:HyperLink>
            </ItemTemplate>
           
        </asp:TemplateField>
        <asp:BoundField DataField="CreateTime" HeaderText="收藏时间" 
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" 
            ItemStyle-Width="20%">
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>

<ItemStyle HorizontalAlign="Left" Width="20%"></ItemStyle>
        </asp:BoundField>
        <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="操作" ItemStyle-Width="10%">
            <ItemTemplate>
                <asp:LinkButton ID="lbDelete" runat="server" 
                    CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID")%>' 
                    onclick="lbDelete_Click">取消收藏</asp:LinkButton>
            </ItemTemplate>
           
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>

<ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
           
        </asp:TemplateField>
    </Columns>
     <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
</asp:GridView>
</div>
<div style="width:100%">
   <cc1:AspNetPager ID="AspNetPager1" runat="server" ShowGo="false" ShowSize="true" OnPageChanged="AspNetPager1_PageChanged" >
</cc1:AspNetPager>
</div>
