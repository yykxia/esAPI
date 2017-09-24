<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sfy_orderMgmt.aspx.cs" Inherits="WdExpand.SFYFK.sfy_orderMgmt" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>订单维护</title>
    <style>
        .x-grid-row .x-grid-cell-inner
        {
            white-space: normal;
            word-break: break-all;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Grid1" />
        <f:Grid ID="Grid1" runat="server" ShowHeader="false" EnableTextSelection="true"
            DataKeyNames="SOId" AllowCellEditing="true" ClicksToEdit="1" AllowPaging="true"
             IsDatabasePaging="true" PageSize="20" OnPageIndexChange="Grid1_PageIndexChange">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btn_save" runat="server" Text="保存编辑" OnClick="btn_save_Click" ValidateForms="Grid1"></f:Button>
                        <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server"></f:ToolbarSeparator>
                        <f:DatePicker ID="DatePicker1" runat="server" Label="下载日期"></f:DatePicker>
                        <f:Button ID="btn_search" runat="server" Text="查询" OnClick="btn_search_Click"></f:Button>
                        <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server"></f:ToolbarSeparator>
                        <f:TriggerBox ID="tgb_PO" LabelWidth="60px" runat="server" Label="PO号" OnTriggerClick="tgb_PO_TriggerClick" TriggerIcon="Search"></f:TriggerBox>
                        <f:ToolbarFill runat="server"></f:ToolbarFill>
                        <f:Button ID="btn_rePush" Text="重新推送" runat="server" OnClick="btn_rePush_Click"
                             ConfirmText="15分钟内会重新推送到网店管家，是否继续？"></f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Columns>
                <f:BoundField  DataField="CUST_PO_NUM" Width="150px" TextAlign="Center" HeaderText="PO号"></f:BoundField>
                <f:BoundField  DataField="ITEM_NUMBER" Width="150px" TextAlign="Center" HeaderText="商品编码"></f:BoundField>
                <f:BoundField  DataField="ITEM_DESC" Width="350px" HeaderText="商品描述"></f:BoundField>
                <f:BoundField  DataField="NEED_BY_DATE" DataFormatString="{0:yyyy-MM-dd}" Width="100px" TextAlign="Center" HeaderText="需求日期"></f:BoundField>
                <f:RenderField DataField="CONTACT_NAME" ColumnID="CONTACT_NAME" TextAlign="Center" HeaderText="联系人" Width="150px">
                    <Editor>
                        <f:TextBox ID="tbxEditor_CONTACT_NAME" Required="true" runat="server">
                        </f:TextBox>
                    </Editor>                
                </f:RenderField>
                <f:RenderField DataField="PHONE_NUMBER" ColumnID="PHONE_NUMBER" HeaderText="联系手机" TextAlign="Center" Width="100px">
                    <Editor>
                        <f:TextBox ID="tbxEditor_PHONE_NUMBER" Required="true" runat="server">
                        </f:TextBox>
                    </Editor>                
                </f:RenderField>
                <f:RenderField DataField="ADDRESS" ColumnID="ADDRESS" TextAlign="Center" HeaderText="联系地址" ExpandUnusedSpace="true">
                    <Editor>
                        <f:TextArea ID="TextArea_address" Required="true" runat="server"></f:TextArea>
                    </Editor>                
                </f:RenderField>
                <f:BoundField  DataField="isAPI" Width="60px" TextAlign="Center" HeaderText="API"></f:BoundField>
                <f:CheckBoxField TextAlign="Center" ColumnID="CheckBoxField1" DataField="APIStatus" HeaderText="选中"
                    RenderAsStaticField="false" Width="50px" />
<%--                <f:CheckBoxField HeaderText="选中" ColumnID="CheckBoxField1" RenderAsStaticField="false"></f:CheckBoxField>--%>
            </Columns>
        </f:Grid>
    </form>
</body>
</html>
