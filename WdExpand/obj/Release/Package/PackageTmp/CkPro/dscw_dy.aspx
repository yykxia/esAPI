<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dscw_dy.aspx.cs" Inherits="WdExpand.CkPro.dscw_dy" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>物流单打印-原始版</title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
        <f:Panel ID="Panel1" Layout="Fit" runat="server" ShowBorder="false" ShowHeader="false">
            <Items>
        <f:Grid ID="Grid1" ShowBorder="true" ShowHeader="true" Title="数据查询"
             EnableCollapse="true" runat="server" DataKeyNames="TradeID">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:DatePicker Label="选择时间段" ID="Str_Dtep" DateFormatString="yyyy-MM-dd" Width="210px" runat="server">
                        </f:DatePicker>
                        <f:DatePicker Label="至" DateFormatString="yyyy-MM-dd" ID="End_Dtep" Width="210px" runat="server">
                        </f:DatePicker>
                        <f:TextBox ID="txb_no" runat="server" Label="收件人" Width="200px" EmptyText="输入收件人"></f:TextBox>
                        <f:RadioButtonList ID="RadioButtonList1" runat="server" Width="200px">
                            <f:RadioItem Text="全部" Value="2" />
                            <f:RadioItem Text="已打印" Value="1" />
                            <f:RadioItem Text="未打印" Value="0" Selected="true" />                            
                        </f:RadioButtonList>
                        <f:Button ID="btn_confirm" Text="查询" Icon="SystemSearch" runat="server"
                             OnClick="btn_confirm_Click"></f:Button>
<%--                        <f:Button ID="Button1" Text="生成" runat="server" OnClick="Button1_Click"></f:Button>--%>
<%--                        <f:Button ID="btn_Print" Text="打印" runat="server" OnClientClick="javascript:prn_preview()"></f:Button>--%>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Columns>
                <f:TemplateField Width="20px" EnableColumnHide="false" EnableHeaderMenu="false">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                    </ItemTemplate>
                </f:TemplateField>
                <f:BoundField DataField="TradeNO" Width="120px" TextAlign="Center" HeaderText="订单号" />
                <f:BoundField DataField="confirmTime" Width="140px" TextAlign="Center" DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="审核时间" />
                <f:BoundField DataField="NickName" Width="150px" TextAlign="Center" HeaderText="索菲亚PO号" />
                <f:BoundField DataField="GoodsName" ExpandUnusedSpace="true" HeaderText="品名" />
                <f:BoundField DataField="SndTo" Width="200px" TextAlign="Center" HeaderText="收件人" />
                <f:BoundField DataField="SellCount" Width="60px" TextAlign="Center" HeaderText="数量" />
                <f:BoundField DataField="postName" Width="100px" TextAlign="Center" HeaderText="物流" />
                <f:BoundField DataField="printstat" Width="80px" TextAlign="Center" HeaderText="打印状态" />
                <f:WindowField ColumnID="PrintWindow" TextAlign="Center" Icon="Printer" ToolTip="打印预览" HeaderText="打印预览"
                     WindowID="Window1" DataIFrameUrlFields="RecID" DataIFrameUrlFormatString="dscw_print_orig.aspx?ddls={0}" Width="80px" />                               
            </Columns>
        </f:Grid>
                </Items>
            </f:Panel>
    <f:Window ID="Window1" Title="物流单打印预览" runat="server" Hidden="true" EnableResize="true"
        EnableMaximize="true" EnableIFrame="true" Width="600px" Height="600px">
            <Toolbars>
                <f:Toolbar ID="Toolbar2" runat="server" Position="Top" CssStyle="text-align:right">
                    <Items>
                        <f:Button ID="btnClose" Text="关闭" Icon="SystemClose" EnablePostBack="true" runat="server" OnClick="btnClose_Click">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
    </f:Window>
    </form>
</body>
</html>
