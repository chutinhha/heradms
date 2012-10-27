<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WaterImageWebPartUserControl.ascx.cs"
    Inherits="HeraDMS.CustomWebPart.WaterImageWebPart.WaterImageWebPartUserControl" %>
<table style="width: 800px; height: 500px">
    <tr>
        <td style="width: 50%; vertical-align:middle">
            <asp:Image ID="ImgPreview" runat="server" Width="100%" Height="100%" />
        </td>
        <td style="width: 50%; vertical-align: top">
            <table width="100%" style="height: 100%">
                <tr style="height: 10%">
                    <td style="width: 25%; text-align: right">
                        水印类型：
                    </td>
                    <td style="text-align: left">
                        <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" 
                            AutoPostBack="True" onselectedindexchanged="rblType_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Text="图片水印" Value="Y"></asp:ListItem>
                            <asp:ListItem Text="文字水印" Value="N"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr style="height: 300px">
                    <td colspan="2">
                        <asp:Panel ID="Panel1" runat="server">
                        <table width="100%" style="height: 100%">
                            <tr>
                                <td style="width: 25%; text-align: right">
                                    图片：
                                </td>
                                <td valign="top">
                                    <asp:Image ID="ImgPicture" runat="server" Height="100%" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td rowspan="2" style="width: 25%; text-align: right" >
                                    上传图片：
                                </td>
                                <td>
                                    <asp:FileUpload ID="FileUpload1" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnUpLoad" runat="server" Text="上传图片" 
                                        onclick="btnUpLoad_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 25%; text-align: right">
                                    图片大小：
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlImgSize" runat="server">
                                     <asp:ListItem Selected="True" Value="1">100%</asp:ListItem>
                                        <asp:ListItem Value="0.75">75%</asp:ListItem>
                                        <asp:ListItem Value="0.5">50%</asp:ListItem>
                                        <asp:ListItem Value="0.25">25%</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                           
                        </table>
                        </asp:Panel>
                        <asp:Panel ID="Panel2" runat="server" Visible="false">
                        <table width="100%" style="height: 100%">
                            <tr>
                                <td rowspan="2" style="width: 25%; text-align: right; vertical-align:middle">
                                    文字：
                                </td>
                                <td valign="top">
                                    <asp:Image ID="ImgWord" runat="server" Height="100%" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="tbWord" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td rowspan="3" style="width: 25%; text-align: right; vertical-align:middle" >
                                    字体设置：
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlFamily" runat="server">
                                    </asp:DropDownList>
                                     <asp:DropDownList ID="ddlSize" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="cblFont" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="粗体" Value="B"></asp:ListItem>
                                    <asp:ListItem Text="斜体" Value="I"></asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="tbColor" runat="server" BackColor="Black" Enabled="false" 
                                        Width="16px"></asp:TextBox>
                                    <asp:HyperLink ID="hlSelectColor" runat="server">选择颜色...</asp:HyperLink>
                                </td>
                            </tr>
                        </table>
                        </asp:Panel>
                    </td>
                </tr>
                 <tr>
                                <td style="width: 25%; text-align: right">
                                    透明度：
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlAlpha" runat="server">
                                        <asp:ListItem Selected="True" Value="1">100%</asp:ListItem>
                                        <asp:ListItem Value="0.9">90%</asp:ListItem>
                                        <asp:ListItem Value="0.8">80%</asp:ListItem>
                                        <asp:ListItem Value="0.7">70%</asp:ListItem>
                                        <asp:ListItem Value="0.6">60%</asp:ListItem>
                                        <asp:ListItem Value="0.5">50%</asp:ListItem>
                                        <asp:ListItem Value="0.4">40%</asp:ListItem>
                                        <asp:ListItem Value="0.3">30%</asp:ListItem>
                                        <asp:ListItem Value="0.2">20%</asp:ListItem>
                                        <asp:ListItem Value="0.1">10%</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 25%; text-align: right">
                                    位置：
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlPosition" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                <tr style="height: 65px">
                    <td colspan="2" style="text-align: right; vertical-align: bottom">
                        <asp:Button ID="btnPreview" runat="server" Text="预览" 
                            onclick="btnPreview_Click" />&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnConfirm" runat="server" Text="确定" 
                            onclick="btnConfirm_Click" />&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="取消"  OnClientClick="SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.cancel);" />&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
