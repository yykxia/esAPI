<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportAsExcel.aspx.cs" Inherits="WdExpand.CkPro.ExportAsExcel" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Excel导出</title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Grid1" />
        <f:Grid ID="Grid1" runat="server">
            <Toolbars>
                <f:Toolbar runat="server" ID="tlb1">
                    <Items>
                        <f:Button ID="btn_confirm" Text="导出" runat="server"
                             EnableAjax="false" DisableControlBeforePostBack="false"
                            OnClick="btn_confirm_Click"></f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Columns>
                <f:BoundField DataField="GoodsNo" HeaderText="货品编号" Width="120px"></f:BoundField>
                <f:BoundField DataField="GoodsName" HeaderText="品名" Width="120px"></f:BoundField>
                <f:BoundField DataField="regCount" HeaderText="收货数量" TextAlign="Center" Width="80px"></f:BoundField>
                <f:BoundField DataField="dealwith" HeaderText="收货意见" Width="100px"></f:BoundField>
                <f:BoundField DataField="shopName" HeaderText="所在店铺" Width="120px"></f:BoundField>
                <f:BoundField DataField="sndto" HeaderText="客户名称" Width="100px"></f:BoundField>
                <f:BoundField DataField="rcvpostId" HeaderText="快递信息" Width="120px"></f:BoundField>
                <f:BoundField DataField="adr" HeaderText="客户地址" Width="200px"></f:BoundField>
            </Columns>
        </f:Grid>
    </form>
</body>
</html>
