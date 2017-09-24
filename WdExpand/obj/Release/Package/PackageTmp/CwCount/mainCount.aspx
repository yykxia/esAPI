<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mainCount.aspx.cs" Inherits="WdExpand.mainCount" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>网店订单明细</title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"></f:PageManager>
        <f:Grid ID="Grid1" ShowBorder="true" ShowHeader="true" Title="网店数据查询"
            Width="1000px" EnableCollapse="true" runat="server" DataKeyNames="TradeNO">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:DatePicker Label="选择时间段：" ID="Str_Dtep" DateFormatString="yyyy-MM-dd" runat="server">
                        </f:DatePicker>
                        <f:DatePicker Label="至：" DateFormatString="yyyy-MM-dd" ID="End_Dtep" runat="server">
                        </f:DatePicker>
                        <f:Button ID="btn_confirm" Text="确定" Icon="SystemSearch" runat="server"
                             OnClick="btn_confirm_Click"></f:Button>
                        <f:Button ID="btn_export" Text="导出为Excel" EnableAjax="false" DisableControlBeforePostBack="false"
                             runat="server" OnClick="btn_export_Click"></f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Columns>
                <f:BoundField DataField="TradeNO" HeaderText="订单号" />
                <f:BoundField DataField="TradeTime" Width="120px" HeaderText="交易日期" />
                <f:BoundField DataField="SndTime" Width="120px" HeaderText="发货日期" />
                <f:BoundField DataField="NickName" HeaderText="客户名称" />
                <f:BoundField DataField="ShopName" HeaderText="售出店铺" />
                <f:BoundField DataField="GoodsTotal" HeaderText="货品总计" />
                <f:BoundField DataField="FavourableTotal" HeaderText="优惠金额" />
                <f:BoundField DataField="AllTotal" HeaderText="应收总计" />
                <f:BoundField DataField="RcvTotal" HeaderText="实收金额" />
                <f:BoundField DataField="TradeStatus" HeaderText="订单状态" />
            </Columns>
        </f:Grid>
    </form>
</body>
</html>
