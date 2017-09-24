<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sfy_LogisticsEntry.aspx.cs" Inherits="WdExpand.SFYFK.sfy_LogisticsEntry" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Grid1" />
    <f:Grid ID="Grid1" runat="server">
        <Toolbars>
            <f:Toolbar runat="server" ID="tlb1">
                <Items>
                    <f:TriggerBox ID="trgb_logNo" Label="物流单号" EmptyText="支持至少8位以上查询条件"
                         runat="server" Width="300px" TriggerIcon="Search" OnTriggerClick="trgb_logNo_TriggerClick">
                    </f:TriggerBox>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Columns>
            <f:BoundField DataField="NickName" HeaderText="客户ID" Width="150px"></f:BoundField>
            <f:BoundField DataField="sndto" HeaderText="联系人信息" Width="150px"></f:BoundField>
            <f:BoundField DataField="tel" HeaderText="联系电话" Width="150px" TextAlign="Center"></f:BoundField>
            <f:BoundField DataField="postId" HeaderText="单号" Width="150px" TextAlign="Center"></f:BoundField>
            <f:BoundField DataField="adr" HeaderText="联系地址" ExpandUnusedSpace="true"></f:BoundField>
        </Columns>
    </f:Grid>
    </form>
</body>
</html>
