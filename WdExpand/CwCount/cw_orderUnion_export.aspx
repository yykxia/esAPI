<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cw_orderUnion_export.aspx.cs" Inherits="WdExpand.CwCount.cw_orderUnion_export" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>数据联合</title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Grid1" />
        <f:Grid ID="Grid1" runat="server" Title="数据明细">
            <Toolbars>
                <f:Toolbar ID="tlb1" runat="server">
                    <Items>
                        <f:DropDownList ID="ddl_shopList" runat="server" Label="店铺" Required="true"
                                EmptyText="选取相应店铺" AutoSelectFirstItem="false"></f:DropDownList>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Toolbars>
                <f:Toolbar ID="tlb2" runat="server">
                    <Items>
                        <f:DatePicker ID="DatePicker1" runat="server" Label="登记起始日期" Required="true">
                        </f:DatePicker> 
                        <f:DatePicker ID="DatePicker2" runat="server" Label="至" CompareControl="DatePicker1"
                            CompareOperator="GreaterThanEqual" CompareMessage="结束日期应该大于等于开始日期！" Required="true">
                        </f:DatePicker> 
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Toolbars>
                <f:Toolbar ID="tlb3" runat="server">
                    <Items>
                        <f:Button ID="btn_union" runat="server" Text="查询结果" ValidateForms="Grid1" OnClick="btn_union_Click">
                        </f:Button>
                        <f:ToolbarSeparator runat="server"></f:ToolbarSeparator>
                        <f:Button ID="btn_saveAsExcel" runat="server" Text="页面结果另存为"
                             EnableAjax="false" DisableControlBeforePostBack="false" OnClick="btn_saveAsExcel_Click">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Columns>
                <f:TemplateField ColumnID="tfNumber" Width="60px">
                    <ItemTemplate>
                        <span id="spanNumber" runat="server"><%# Container.DataItemIndex + 1 %></span>
                    </ItemTemplate>
                </f:TemplateField>
                <f:GroupField EnableLock="true" HeaderText="登记数据" TextAlign="Center">
                    <Columns>                
                        <f:BoundField DataField="custNickName" TextAlign="Center" HeaderText="客户昵称" Width="100px"></f:BoundField>
                        <f:BoundField DataField="tradeNo" TextAlign="Center" HeaderText="原始单号" Width="150px"></f:BoundField>
                        <f:BoundField DataField="cashGo" TextAlign="Center" HeaderText="资金转入" Width="100px"></f:BoundField>
                        <f:BoundField DataField="goodsCount" TextAlign="Center" HeaderText="数量" Width="100px"></f:BoundField>
                        <f:BoundField DataField="goodsName" TextAlign="Center" HeaderText="品名" Width="100px"></f:BoundField>
                        <f:BoundField DataField="payCount" TextAlign="Center" HeaderText="订单金额" Width="100px"></f:BoundField>
                        <f:BoundField DataField="payDate" TextAlign="Center" DataFormatString="{0:yyyy-MM-dd}" HeaderText="登记时间" Width="100px"></f:BoundField>
                        <f:BoundField DataField="payType" TextAlign="Center" HeaderText="交易类型" Width="100px"></f:BoundField>
                        <f:BoundField DataField="customerName" TextAlign="Center" HeaderText="收款人" Width="100px"></f:BoundField>
                        <f:BoundField DataField="addDesc" TextAlign="Center" HeaderText="补充说明" Width="100px"></f:BoundField>
                    </Columns>
                </f:GroupField>            
                <f:GroupField EnableLock="true" HeaderText="后台数据" TextAlign="Center">
                    <Columns>                
                        <f:BoundField DataField="nickName_orig" TextAlign="Center" HeaderText="客户昵称" Width="100px"></f:BoundField>
                        <f:BoundField DataField="tradeNo_orig" TextAlign="Center" HeaderText="原始单号" Width="150px"></f:BoundField>
                        <f:BoundField DataField="moneyFrom_orig" TextAlign="Center" HeaderText="来源帐号" Width="100px"></f:BoundField>
                        <f:BoundField DataField="goodsCount_orig" TextAlign="Center" HeaderText="数量" Width="100px"></f:BoundField>
                        <f:BoundField DataField="goodsName_orig" TextAlign="Center" HeaderText="商品描述" Width="100px"></f:BoundField>
                        <f:BoundField DataField="paySum_orig" TextAlign="Center" HeaderText="订单金额" Width="100px"></f:BoundField>
                        <f:BoundField DataField="orderStartTime_orig" TextAlign="Center" HeaderText="交易时间" Width="100px"></f:BoundField>
                        <f:BoundField DataField="orderStatus_orig" TextAlign="Center" HeaderText="交易状态" Width="100px"></f:BoundField>
                        <f:BoundField DataField="tradeDesc_orig" TextAlign="Center" HeaderText="订单补充" Width="100px"></f:BoundField>
                        <f:BoundField DataField="customerDesc_orig" TextAlign="Center" HeaderText="客户备注" Width="100px"></f:BoundField>
                        <f:BoundField DataField="saleDesc_orig" TextAlign="Center" HeaderText="订单备注" Width="100px"></f:BoundField>
                    </Columns>
                </f:GroupField>
                <f:GroupField EnableLock="true" HeaderText="网店管家" TextAlign="Center">
                    <Columns>
                        <f:BoundField DataField="tradeno2" TextAlign="Center" HeaderText="原始单号" Width="150px"></f:BoundField>
                        <f:BoundField DataField="tradestatusext" TextAlign="Center" HeaderText="状态" Width="100px"></f:BoundField>                
                        <f:BoundField DataField="goodslist" TextAlign="Center" HeaderText="货品描述" Width="100px"></f:BoundField>
                        <f:BoundField DataField="remark" TextAlign="Center" HeaderText="备注" Width="100px"></f:BoundField>
                        <f:BoundField DataField="rcvtotal" TextAlign="Center" HeaderText="实际支付" Width="100px"></f:BoundField>

                    </Columns>
                </f:GroupField>            
            </Columns>
        </f:Grid>
    </form>
</body>
</html>
