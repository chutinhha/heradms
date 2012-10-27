<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CommentList.aspx.cs" Inherits="HeraDMS.Layouts.Pages.Comment.CommentList"
    DynamicMasterPageFile="~masterurl/default.master" %>
<%@ Register Assembly="HeraDMS.ServerControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=1df39b57a33dabfd" Namespace="HeraDMS.ServerControls" TagPrefix="cc1" %>
<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div style="text-align: left">
        &nbsp;评论</div>
    <div style="width: 100%">
        <table width="100%">
            <tr>
                <td>
                    <asp:TextBox ID="tbComment" runat="server" TextMode="MultiLine" Rows="5" Width="100%" MaxLength="200"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Button ID="btnPublish" runat="server" Text="发布" OnClick="btnPublish_Click" />
                </td>
            </tr>
            <tr>
                <td>
                     <cc1:AspNetPager ID="AspNetPager1" runat="server" ShowGo="false" ShowSize="true" OnPageChanged="AspNetPager1_PageChanged" >
</cc1:AspNetPager>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="gvComment" runat="server" AutoGenerateColumns="False" 
        Width="100%" onrowediting="gvComment_RowEditing" 
        onrowcancelingedit="gvComment_RowCancelingEdit" 
        onrowdeleting="gvComment_RowDeleting" onrowupdating="gvComment_RowUpdating"
         OnRowDataBound="gvComment_RowDataBound" ShowHeader="false" EmptyDataText="尚未发表任何评论。">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <table width="100%">
                        <tr style="height:10px">
                            <td rowspan="2" valign="top" style="width:8%">
                                <asp:Image ID="ImPicture" runat="server" /></td>
                            <td align="left" valign="top" style="height:10px"><%#DataBinder.Eval(Container.DataItem, "ModifierName")%>&nbsp;&nbsp;
                            <%#DataBinder.Eval(Container.DataItem, "ModifyTime")%></td>
                            <td align="right" valign="top" style="height:10px">
                                <asp:HiddenField ID="hfUserId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "UserId")%>' />
                                <asp:LinkButton ID="lbEdit" runat="server" Visible='<%#DataBinder.Eval(Container.DataItem, "IsShowButton")%>' CommandName="Edit" CausesValidation="false"
>编辑</asp:LinkButton>
                                <asp:LinkButton ID="lbDelete" runat="server" OnClientClick="javascript:return confirm('是否确认要删除此项？')" CommandName="Delete" CausesValidation="false" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID")%>' Visible='<%#DataBinder.Eval(Container.DataItem, "IsShowButton")%>'>删除</asp:LinkButton></td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top"><%#DataBinder.Eval(Container.DataItem, "Comment")%></td>
                        </tr>
                    </table>
                </ItemTemplate>
                <EditItemTemplate>
                    <table width="100%">
                    <tr>
                        <td>
                             <asp:TextBox ID="tbUpdateComment" runat="server" TextMode="MultiLine" Rows="5" Width="100%" Text='<%#DataBinder.Eval(Container.DataItem, "Comment")%>' MaxLength="200"></asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <td align="right">
                            <asp:Button ID="btnUpdate" runat="server" Text="发布" CommandName="Update" CausesValidation="false" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID")%>'
/>
                            <asp:Button ID="btnCancle" runat="server" Text="取消" CommandName="Cancel" CausesValidation="false"/>
                        </td>
                    </tr>
                    </table>
                </EditItemTemplate>
            </asp:TemplateField>
        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                 <cc1:AspNetPager ID="AspNetPager2" runat="server" ShowGo="false" ShowSize="true" OnPageChanged="AspNetPager1_PageChanged" >
</cc1:AspNetPager>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    评论
</asp:Content>
<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea"
    runat="server">
    My Application Page
</asp:Content>
