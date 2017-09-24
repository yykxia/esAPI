<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sfy_wlfk.aspx.cs" Inherits="WdExpand.SFYFK.sfy_wlfk" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>索菲亚物流信息反馈（重要）</title>
    <style>
        .label_style {
            color:red;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
      <f:PageManager ID="PageManager1" runat="server" AjaxTimeout="60000" />
<%--        <f:Timer ID="Timer1" Interval="300" Enabled="false" OnTick="Timer1_Tick" EnableAjaxLoading="false" runat="server">
        </f:Timer>--%>
        <f:Grid ID="Grid1" runat="server" ShowBorder="true" Title="已同步物流信息查询-索菲亚" ShowHeader="true" EnableTextSelection="true"
             AllowPaging="true" PageSize="50" Height="600px" EnableCollapse="true"  OnPageIndexChange="Grid1_PageIndexChange">
            <Toolbars>
                <f:Toolbar runat="server">
                    <Items>
                        <f:DatePicker ID="DatePicker1" Label="按发货日期" LabelWidth="80px" runat="server">
                        </f:DatePicker>
                        <f:DatePicker ID="DatePicker2" Label="至" CompareControl="DatePicker1"
                             CompareOperator="GreaterThanEqual" CompareMessage="结束日期应大于开始时间！" LabelWidth="30px" runat="server">
                        </f:DatePicker>
                        <f:Button ID="Button1" Text="确认"  runat="server"
                             OnClick="Button1_Click"></f:Button>
                        <f:ToolbarSeparator ID="tlbsp1" runat="server" ></f:ToolbarSeparator>
                        <f:TextBox ID="txb_order" runat="server" Label="订单号" MinLength="10" LabelWidth="60px" EmptyText="输入PO号查询"></f:TextBox>
                        <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server" ></f:ToolbarSeparator>
                        <f:Button ID="btn_confirm" Text="查询"  runat="server"
                             OnClick="btn_confirm_Click"></f:Button>
<%--                        <f:Label ID="label_mark" CssClass="label_style" Text="（自动同步已关闭）" runat="server"></f:Label>--%>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Columns>
                <f:BoundField HeaderText="PO号" DataField="NickName" TextAlign="Center" Width="150px" />
                <f:BoundField HeaderText="原始单号" DataField="orderId" TextAlign="Center" Width="150px" />
                <f:BoundField HeaderText="同步时间" DataField="create_date" TextAlign="Center" Width="150px" />
                <f:BoundField HeaderText="物流信息" DataField="LOG_INFO" ExpandUnusedSpace="true" />

            </Columns>
        </f:Grid>
<%--         <f:Button ID="btn_autoReturnBack" Text="开启自动回传" OnClick="btn_autoReturnBack_Click" runat="server"></f:Button>
         <f:Button ID="btn_close" Text="关闭自动回传" OnClick="btn_close_Click" runat="server"></f:Button><br />
        <f:Label ID="Label_status" runat="server"></f:Label><br />--%>
    </form>
</body>
</html>
