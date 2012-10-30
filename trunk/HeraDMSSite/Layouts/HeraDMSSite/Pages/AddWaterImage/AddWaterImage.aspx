<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddWaterImage.aspx.cs" Inherits="HeraDMS.Layouts.HeraDMSSite.Pages.AddWaterImage.AddWaterImage" DynamicMasterPageFile="~masterurl/default.master" %>

<%@ Register Assembly="HeraDMS.ServerControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=1df39b57a33dabfd" Namespace="HeraDMS.ServerControls" TagPrefix="cc1" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script src="../../JS/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../JS/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../../JS/iColorPicker.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
    function Check_FileType() {
        var str = document.getElementById('<%=FileUpload1.ClientID%>').value;
        if (str == null || str == '') {
            alert("请选择.jpg,.gif,.png类型的图片上传！");
            return false;
        }
        var pos = str.lastIndexOf(".");
        var lastname = str.substring(pos, str.length);
        if (lastname.toLowerCase() != ".jpg" && lastname.toLowerCase() != ".gif"
            && lastname.toLowerCase() != ".png") {
            alert("您上传的文件类型为" + lastname + "，图片必须为.jpg,.gif,.png类型！");
            return false;
        }
        else {
            return true;
        }
    } 
    </script> 
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">



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
                                    <asp:Image ID="ImgPicture" runat="server" Height="150px" Width="200px" />
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
                                        onclick="btnUpLoad_Click" OnClientClick="return Check_FileType()"  />
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
                                <td >
                                    <div style="height:150px; width:200px; background-color:White;text-align:center; vertical-align:middle">
                                    <asp:Label ID="ImgWord" runat="server" Height="150px" Width="200px" >  </asp:Label>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="tbWord" runat="server" AutoPostBack="true" OnTextChanged="tbWord_TextChanged"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td rowspan="3" style="width: 25%; text-align: right; vertical-align:middle; font-family:" >
                                    字体设置：
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlFamily" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFamily_SelectedIndexChanged">
                                    <asp:ListItem Selected="True" Text="宋体" />
                                    <asp:ListItem Text="黑体" />
                                    <asp:ListItem Text="楷体" />
                                    <asp:ListItem Text="仿宋" />
                                    <asp:ListItem Text="隶书" />
                                    <asp:ListItem Text="幼圆" />
                                    <asp:ListItem Text="Arial" />
                                    <asp:ListItem Text="Arial Black" />
                                    <asp:ListItem Text="Arial Narrow" />
                                    <asp:ListItem Text="Brush Script MT" />
                                    <asp:ListItem Text="Century Gothic" />
                                    <asp:ListItem Text="Comic Sans MS" />
                                    <asp:ListItem Text="Courier" />
                                    <asp:ListItem Text="Courier New" />
                                    <asp:ListItem Text="MS Sans Serif" />
                                    <asp:ListItem Text="Script" />
                                    <asp:ListItem Text="System" />
                                    <asp:ListItem Text="Times New Roman" />
                                    <asp:ListItem Text="Verdana" />
                                    <asp:ListItem Text="Wide Latin" />
                                    <asp:ListItem Text="Wingdings" />
                                    </asp:DropDownList>
                                     <asp:DropDownList ID="ddlSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSize_SelectedIndexChanged">
                                     <asp:ListItem Text="8" />
                                     <asp:ListItem Text="10" />
                                     <asp:ListItem Text="12" />
                                     <asp:ListItem Text="14" />
                                     <asp:ListItem Selected="True" Text="18" />
                                     <asp:ListItem Text="24" />
                                     <asp:ListItem Text="36" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="cblFont" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="cblFont_SelectedIndexChanged">
                                    <asp:ListItem Text="粗体" Value="B"></asp:ListItem>
                                    <asp:ListItem Text="斜体" Value="I"></asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <cc1:ColorPicker ID="ColorPicker1" runat="server" OnColorChanged="ColorPicker1_OnColorChanged" AutoPostBack="true"/>
                                    <%--<asp:TextBox ID="tbColor" runat="server" BackColor="Black" Enabled="false" 
                                        Width="50px" Text="#000000" CssClass="iColorPicker"></asp:TextBox>--%>
                                       

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
                            <input type="button" id="btnCancel" value="取消" onclick="javascript:window.frameElement.commonModalDialogClose(1, 1);" />
                        <%--<asp:Button ID="btnCancel" runat="server" Text="取消"  OnClientClick="SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.cancel);" />--%>&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
添加水印图片
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
My Application Page
</asp:Content>
