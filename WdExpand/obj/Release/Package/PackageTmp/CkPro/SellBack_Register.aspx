<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SellBack_Register.aspx.cs" Inherits="WdExpand.CkPro.SellBack_Register" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
        <f:Panel ID="Panel1" runat="server" Title="退换货收货登记" Layout="VBox">
            <Items>
                <f:Grid ID="Grid1" Height="400px" runat="server" EnableTextSelection="true"
                    DataKeyNames="TradeId" EnableRowSelectEvent="true" OnRowSelect="Grid1_RowSelect" >
                    <Toolbars>
                        <f:Toolbar runat="server" ID="tlb1">
                            <Items>
                                <f:DatePicker Label="收货日期" ID="DatePicker1" runat="server"></f:DatePicker>
                                <f:DatePicker Label="至" ID="DatePicker2" runat="server"></f:DatePicker>
                                <f:Button ID="btn_query" Text="查询" runat="server" OnClick="btn_query_Click"></f:Button>
                                <f:Button ID="btn_export" Text="导出" runat="server" Icon="PageExcel" OnClick="btn_export_Click"></f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Columns>
                        <f:BoundField DataField="TradeNo" HeaderText="单号" TextAlign="Center"
                            runat="server" Width="100px"></f:BoundField>
                        <f:BoundField DataField="rcvPostId" HeaderText="快递单号"
                            runat="server" Width="150px"></f:BoundField>
                        <f:BoundField DataField="shopName" HeaderText="所在店铺"
                            runat="server" Width="100px"></f:BoundField>
                        <f:BoundField DataField="GoodsList" HeaderText="货品概述"
                            runat="server" Width="150px"></f:BoundField>
                        <f:CheckBoxField DataField="closeStaus" HeaderText="退货完成" TextAlign="Center"
                            runat="server" Width="80px"></f:CheckBoxField>
                        <f:BoundField DataField="sndTo" HeaderText="客户姓名" TextAlign="Center"
                            runat="server" Width="80px"></f:BoundField>
                        <f:BoundField DataField="adr" HeaderText="地址"
                            runat="server" ExpandUnusedSpace="true"></f:BoundField>
                    </Columns>
                </f:Grid>
                <f:Panel ID="Panel2" runat="server" Layout="HBox" BoxFlex="1">
                    <Items>
                        <f:Grid ID="Grid2" runat="server" Title="退货登记明细" Width="365px">
                            <Columns>
                                <f:BoundField DataField="GoodsNo" HeaderText="编号"
                                    runat="server" Width="150px"></f:BoundField>
                                <f:BoundField DataField="GoodsName" HeaderText="品名"
                                    runat="server" Width="150px"></f:BoundField>
                                <f:BoundField DataField="sellCount" HeaderText="数量" TextAlign="Center"
                                    runat="server" Width="60px"></f:BoundField>
                            </Columns>
                        </f:Grid>
                        <f:Grid ID="Grid3" runat="server" Title="退货收货明细" BoxFlex="1">
                            <Columns>
                                <f:BoundField DataField="GoodsNo" HeaderText="编号"
                                    runat="server" Width="150px"></f:BoundField>
                                <f:BoundField DataField="GoodsName" HeaderText="品名"
                                    runat="server" Width="150px"></f:BoundField>
                                <f:BoundField DataField="regCount" HeaderText="数量" TextAlign="Center"
                                    runat="server" Width="60px"></f:BoundField>
                                <f:BoundField DataField="regTime" HeaderText="收货时间" TextAlign="Center"
                                    runat="server" DataFormatString="{0:yyyy-MM-dd HH:mm}" Width="120px"></f:BoundField>
                                <f:BoundField DataField="dealwith" HeaderText="收货意见" TextAlign="Center"
                                    runat="server" Width="100px"></f:BoundField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Panel>
            </Items>
        </f:Panel>
    </form>
    <f:Window ID="Window1" Title="Excel导出" Hidden="true" EnableIFrame="true" runat="server"
        CloseAction="HidePostBack" EnableMaximize="true" EnableResize="true" Target="Top"
        IsModal="true" Width="600px" Height="450px">
    </f:Window>
</body>
</html>
