<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SFY_orderCompare.aspx.cs" Inherits="WdExpand.SFYFK.SFY_orderCompare" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Grid1" />
        <f:Grid ID="Grid1" runat="server" Title="索菲亚订单状态">
            <Toolbars>
                <f:Toolbar runat="server" ID="tlb1">
                    <Items>
                        <f:DatePicker ID="DatePicker1" runat="server" Label="下载日期"></f:DatePicker>
                        <f:ToolbarSeparator runat="server" ID="tlbsp1"></f:ToolbarSeparator>
                        <f:TriggerBox ID="trgb_custNo" runat="server" EmptyText="输入PO号查询"></f:TriggerBox>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Columns>
                <f:BoundField DataField="isAPI" HeaderText="" Width="100px"></f:BoundField>
                <f:BoundField DataField="CUST_PO_NUM" HeaderText="PO号" Width="100px"></f:BoundField>
                <f:BoundField DataField="ITEM_NUMBER" HeaderText="货品编码" Width="100px"></f:BoundField>
                <f:BoundField DataField="ITEM_DESC" HeaderText="品名" Width="100px"></f:BoundField>
                <f:BoundField DataField="NEED_BY_DATE" HeaderText="需求日期" Width="100px"></f:BoundField>
                <f:BoundField DataField="CREATION_DATE" HeaderText="下单时间" Width="100px"></f:BoundField>
                <f:BoundField DataField="QUANTITY" HeaderText="数量" Width="100px"></f:BoundField>
                <f:BoundField DataField="CONTACT_NAME" HeaderText="联系人" Width="100px"></f:BoundField>
                <f:BoundField DataField="PHONE_NUMBER" HeaderText="联系电话" Width="100px"></f:BoundField>
                <f:BoundField DataField="ADDRESS" HeaderText="地址" Width="100px"></f:BoundField>
            </Columns>
        </f:Grid>
    </form>
</body>
</html>
