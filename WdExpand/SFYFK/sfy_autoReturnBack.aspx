<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sfy_autoReturnBack.aspx.cs" Inherits="WdExpand.SFYFK.sfy_autoReturnBack" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>物流自动同步</title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" />
        <f:Timer ID="Timer1" Interval="21600" Enabled="false" OnTick="Timer1_Tick" EnableAjaxLoading="false" runat="server">
        </f:Timer>
        <f:Label ID="label_mark" CssClass="label_style" Text="（自动同步已关闭）" runat="server"></f:Label><br />
         <f:Button ID="btn_autoReturnBack" Text="开启自动回传" OnClick="btn_autoReturnBack_Click" runat="server"></f:Button>
         <f:Button ID="btn_close" Text="关闭自动回传" OnClick="btn_close_Click" runat="server"></f:Button>
        <f:Button ID="btn_post" Text="手动回传" OnClick="btn_post_Click" runat="server"></f:Button><br />
        <f:Label ID="Label_status" runat="server"></f:Label>
    </form>
</body>
</html>
