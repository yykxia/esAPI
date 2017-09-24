<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sys_User.aspx.cs" Inherits="WdExpand.sys_User" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Grid1" />
        <f:Panel ID="Panel1" Layout="VBox" runat="server">
            <Items>
                <f:Form ID="form2" runat="server" BoxFlex="1" ShowBorder="false" ShowHeader="false">
                    <Rows>
                        <f:FormRow ID="FormRow1" runat="server">
                            <Items>
                                <f:TriggerBox ID="tgb_user" runat="server" EmptyText="输入用户名查询" OnTriggerClick="tgb_user_TriggerClick"></f:TriggerBox>                 
                            </Items>

                        </f:FormRow>
                    </Rows>
                </f:Form>

                <f:Grid ID="Grid1" runat="server">
                    <Toolbars>
                        <f:Toolbar runat="server">
                            <Items>
                                <f:Button ID="btnDeleteSelected" Icon="Delete" runat="server" Text="删除选中记录" OnClick="btnDeleteSelected_Click">
                                </f:Button>
                                <f:ToolbarSeparator runat="server">
                                </f:ToolbarSeparator>
                                <f:Button ID="btnChangeEnableUsers" Icon="GroupEdit" EnablePostBack="false" runat="server"
                                    Text="设置启用状态">
                                    <Menu id="Menu1" runat="server">
                                        <f:MenuButton ID="btnEnableUsers" OnClick="btnEnableUsers_Click" runat="server" Text="启用选中记录">
                                        </f:MenuButton>
                                        <f:MenuButton ID="btnDisableUsers" OnClick="btnDisableUsers_Click" runat="server"
                                            Text="禁用选中记录">
                                        </f:MenuButton>
                                    </Menu>
                                </f:Button>
                                <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                </f:ToolbarFill>
                                <f:Button ID="btnNew" runat="server" Icon="Add" EnablePostBack="false" Text="新增用户">
                                </f:Button>                            

                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                </f:Grid>
            </Items>

        </f:Panel>

    </form>
</body>
</html>
