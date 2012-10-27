<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FavritesRankingWebPartUserControl.ascx.cs" Inherits="HeraDMS.CustomWebPart.FavritesRankingWebPart.FavritesRankingWebPartUserControl" %>
<div style="width:100%">
    <asp:GridView ID="gvFavoritesRanking" runat="server" Width="100%" 
        CellPadding="4"  ForeColor="#333333" 
        GridLines="None" style="margin-right: 0px" EmptyDataText="无数据或未加载" 
        AutoGenerateColumns="False" EnableModelValidation="True" 
        ShowHeader="False" onrowdatabound="gvFavoritesRanking_RowDataBound" >
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"  ItemStyle-Width="10%">
            <ItemTemplate>
                <%#Container.DataItemIndex+1 %>
            </ItemTemplate>
           
        </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="标题" ItemStyle-Width="80%">
            <ItemTemplate>
                <asp:HiddenField ID="hfItemId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "ItemId")%>'/>
                <asp:HiddenField ID="hfListId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "ListId")%>'/>
                <asp:HiddenField ID="hfSiteId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "SiteId")%>'/>
                <asp:HyperLink ID="hlTitle" runat="server" ></asp:HyperLink>
            </ItemTemplate>
           
        </asp:TemplateField>
        <asp:BoundField DataField="Seq" 
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" 
            ItemStyle-Width="10%">
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>

<ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
        </asp:BoundField>
        </Columns>
          <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
    </asp:GridView>
    
</div>
