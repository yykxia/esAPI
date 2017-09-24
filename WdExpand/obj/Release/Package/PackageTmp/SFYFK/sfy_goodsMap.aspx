<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sfy_goodsMap.aspx.cs" Inherits="WdExpand.SFYFK.sfy_goodsMap" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>商品编码映射</title>
    <style type="text/css">
        .label_hid {
            display:none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Grid1" />
        <f:Grid ID="Grid1" runat="server" Title="索菲亚商品编码映射明细" OnRowCommand="Grid1_RowCommand"
            DataKeyNames="id" AllowCellEditing="true" ClicksToEdit="1">
            <Toolbars>
                <f:Toolbar runat="server">
                    <Items>
                        <f:Button ID="btn_addNew" runat="server" Text="新增"></f:Button>
                        <f:Button ID="btn_save" runat="server" Text="保存编辑" OnClick="btn_save_Click"></f:Button>
                        <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server"></f:ToolbarSeparator>
                        <f:TriggerBox ID="trgbx_NO" runat="server" Label="索菲亚编码" TriggerIcon="Search" OnTriggerClick="trgbx_NO_TriggerClick"></f:TriggerBox>
                        <f:ToolbarFill ID="ToolbarFill1" runat="server">
                        </f:ToolbarFill>
                        <f:Button ID="btn_reLoad" runat="server" Text="刷新全部" OnClick="Button1_Click"></f:Button>
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
                <f:RenderField DataField="goodsSpec" ColumnID="goodsSpec" HeaderText="物料说明" Width="400px">
                    <Editor>
                        <f:TextBox ID="tbxEditorGoodsSpec" Required="true" runat="server">
                        </f:TextBox>
                    </Editor>                
                </f:RenderField>
                <f:RenderField DataField="targetNo" ColumnID="targetNo" TextAlign="Center" HeaderText="索菲亚编码" Width="150px">
                    <Editor>
                        <f:TextBox ID="tbxEditorTargetNo" Required="true" runat="server">
                        </f:TextBox>
                    </Editor>                
                </f:RenderField>
                <f:RenderCheckField Width="100px" ColumnID="isMinUnitOrNo" DataField="isMinUnitOrNo" HeaderText="不是最小单位" />            
                <f:RenderField DataField="UnitTimes" ColumnID="UnitTimes" TextAlign="Center" HeaderText="计量数量" Width="100px">
                    <Editor>
                        <f:NumberBox ID="tbxEditorUnit" NoDecimal="true" NoNegative="true" runat="server">
                        </f:NumberBox>                    
                    </Editor>                
                </f:RenderField>
                <f:RenderCheckField Width="80px" ColumnID="isOrder" DataField="isOrder" HeaderText="定制款" />            
                <f:LinkButtonField HeaderText="&nbsp;" Width="80px" ConfirmText="删除选中行？" ConfirmTarget="Top"
                    CommandName="Delete" Icon="Delete" ColumnID="Grid1_ctl15" />            

            </Columns>
        </f:Grid>
        <f:Label ID="label_hidden" runat="server" CssClass="label_hid"></f:Label>
    </form>
</body>
</html>
