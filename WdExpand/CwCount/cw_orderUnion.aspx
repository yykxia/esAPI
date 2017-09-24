<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cw_orderUnion.aspx.cs" Inherits="WdExpand.CwCount.cw_orderUnion" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>订单参照</title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" />
        <f:Form ID="SimpleForm1" BodyPadding="5px" Width="600px" LabelAlign="Left" LabelWidth="80px"
                    Title="数据设定" runat="server">
                <Rows>
                    <f:FormRow ID="FormRow5">
                        <Items>
                            <f:DropDownList ID="ddl_shopList" runat="server" Label="店铺" Required="true"
                                 EmptyText="选取相应店铺" AutoSelectFirstItem="false"></f:DropDownList>
                        </Items>
                    </f:FormRow>
                    <f:FormRow ID="FormRow6" runat="server">
                        <Items>
                            <f:RadioButtonList ID="RadioButtonList1" runat="server" Label="类型" Required="true"></f:RadioButtonList>
                        </Items>
                    </f:FormRow>
                    <f:FormRow ID="FormRow1" runat="server">
                        <Items>
                        <f:FileUpload runat="server" ID="fileData" ButtonText="选取文件" AutoPostBack="true" Label="操作"
                             OnFileSelected="filePhoto_FileSelected">
                        </f:FileUpload>
                            <f:Button ID="btn_loadData" CssClass="btn-in-form" runat="server" ValidateForms="SimpleForm1"
                                 OnClick="btn_loadData_Click" Text="导入数据">
                            </f:Button>
                        </Items>
                    </f:FormRow>
                </Rows>
        </f:Form>
        <f:Grid ID="Grid1" runat="server" Title="导入明细">
            <Columns>
                <f:RowNumberField></f:RowNumberField>
                <f:BoundField DataField="shopName" TextAlign="Center" HeaderText="店铺" Width="100px"></f:BoundField>
                <f:BoundField DataField="custNickName" TextAlign="Center" HeaderText="客户昵称" Width="100px"></f:BoundField>
                <f:BoundField DataField="tradeNo" TextAlign="Center" HeaderText="原始单号" Width="100px"></f:BoundField>
                <f:BoundField DataField="cashGo" TextAlign="Center" HeaderText="资金转入" Width="100px"></f:BoundField>
                <f:BoundField DataField="goodsCount" TextAlign="Center" HeaderText="数量" Width="100px"></f:BoundField>
                <f:BoundField DataField="goodsName" TextAlign="Center" HeaderText="品名" Width="100px"></f:BoundField>
                <f:BoundField DataField="payCount" TextAlign="Center" HeaderText="订单金额" Width="100px"></f:BoundField>
                <f:BoundField DataField="payDate" TextAlign="Center" HeaderText="交易时间" Width="100px"></f:BoundField>
                <f:BoundField DataField="tradeName" TextAlign="Center" HeaderText="交易类型" Width="100px"></f:BoundField>
                <f:BoundField DataField="customerName" TextAlign="Center" HeaderText="收款人" Width="100px"></f:BoundField>
                <f:BoundField DataField="addDesc" TextAlign="Center" HeaderText="补充说明" ExpandUnusedSpace="true"></f:BoundField>
            </Columns>
        </f:Grid>
    </form>
</body>
</html>
