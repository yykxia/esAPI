<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sfy_salePlan.aspx.cs" Inherits="WdExpand.SFYFK.sfy_salePlan" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>赠品计划</title>
    <style type="text/css">
        .label_hid {
            display:none;
        }
    </style></head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Grid1" />
        <f:Grid ID="Grid1" runat="server" Title="销售计划-赠品类" ShowHeader="false"
            DataKeyNames="id" AllowCellEditing="true" ClicksToEdit="2">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btn_addNew" runat="server" Text="新增"></f:Button>
                        <f:Button ID="btn_save" runat="server" Text="保存编辑" OnClick="btn_save_Click"></f:Button>
                        <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server"></f:ToolbarSeparator>
                        <f:TriggerBox ID="trgbx_NO" runat="server" Label="商品编码" TriggerIcon="Search" OnTriggerClick="trgbx_NO_TriggerClick"></f:TriggerBox>
                        <f:ToolbarFill ID="ToolbarFill1" runat="server">
                        </f:ToolbarFill>
                        <f:Button ID="btn_reLoad" runat="server" Text="重新绑定数据" OnClick="btn_reLoad_Click"></f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Columns>
                <f:RenderField DataField="goodsNo" ColumnID="goodsNo" TextAlign="Center" HeaderText="商品编码" Width="150px">
                    <Editor>
                        <f:TextBox ID="tbxEditorGoodsNo" Required="true" runat="server">
                        </f:TextBox>
                    </Editor>                
                </f:RenderField>
                <f:RenderField DataField="giftNo" ColumnID="giftNo" TextAlign="Center" HeaderText="赠品编码" Width="150px">
                    <Editor>
                        <f:TextBox ID="tbxEditorgiftNo" Required="true" runat="server">
                        </f:TextBox>
                    </Editor>                
                </f:RenderField>
                <f:RenderCheckField Width="100px" ColumnID="enabled" DataField="enabled" HeaderText="启用" />            
                <f:RenderField DataField="dstartDte" ColumnID="startDte" TextAlign="Center" FieldType="Date" Renderer="Date"
                     RendererArgument="yyyy-MM-dd" HeaderText="起始日期" Width="120px">
                    <Editor>
                        <f:DatePicker ID="DatePicker1" Required="true" runat="server">
                        </f:DatePicker>                    
                    </Editor>                
                </f:RenderField>
                <f:RenderField DataField="dendDte" ColumnID="endDte" TextAlign="Center" FieldType="Date" Renderer="Date"
                     RendererArgument="yyyy-MM-dd" HeaderText="结束日期" Width="120px">
                    <Editor>
                        <f:DatePicker ID="DatePicker2" Required="true" runat="server">
                        </f:DatePicker>                    
                    </Editor>                
                </f:RenderField>
            </Columns>
        </f:Grid>
        <f:Label ID="label_hidden" runat="server" CssClass="label_hid"></f:Label>
    </form>
</body>
</html>
