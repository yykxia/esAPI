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
        <f:Timer ID="Timer2" Interval="1200" Enabled="false" OnTick="Timer2_Tick" EnableAjaxLoading="false" runat="server">
        </f:Timer>
        <f:Label ID="label_mark" CssClass="label_style" Text="（自动同步已关闭）" runat="server"></f:Label><br />
         <f:Button ID="btn_autoReturnBack" Text="开启自动回传" OnClick="btn_autoReturnBack_Click" runat="server"></f:Button>
         <f:Button ID="btn_close" Text="关闭自动回传" OnClick="btn_close_Click" runat="server"></f:Button>
        <f:Button ID="btn_post" Text="手动回传" OnClick="btn_post_Click" runat="server"></f:Button><br /><br />
        <f:Grid ID="Grid1" runat="server" Title="物流同步记录" Height="500px" Width="400px">
            <Columns>
                <f:RowNumberField></f:RowNumberField>
                <f:BoundField DataField="shopId" HeaderText="店铺ID" Width="80px" TextAlign="Center"></f:BoundField>
                <f:BoundField DataField="execCount" HeaderText="同步日志id" Width="80px" TextAlign="Center"></f:BoundField>
                <f:BoundField DataField="execTime" HeaderText="同步结束时间" ExpandUnusedSpace="true"></f:BoundField>
            </Columns>
        </f:Grid>
        <f:Label ID="Label_status" runat="server"></f:Label><br />
        <f:Label ID="Label1" runat="server"></f:Label>
    </form>
</body>
</html>
