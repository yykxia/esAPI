<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="WdExpand.main" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link type="text/css" rel="stylesheet" href="./res/main.css" />
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
        <f:Panel ID="Panel1" Layout="Region" ShowBorder="false" ShowHeader="false" runat="server">
            <Items>
                <f:ContentPanel ID="ContentPanel1" RegionPosition="Top" ShowBorder="false" CssClass="jumbotron" ShowHeader="false" runat="server">
                    <div class="wrap">
                        <div class="logos">
                            恒康电商管理
                        </div>
                        <div class="member">
                            <f:Label CssClass="label_loginInfo" ID="l_loginInfo" runat="server"></f:Label>
                        </div>
                        <div class="exit">
                            <f:Button ID="btn_exit" runat="server" Text="安全退出" Icon="UserRed" OnClick="btn_exit_Click" ConfirmTitle="提示" ConfirmIcon="Information" ConfirmText="是否退出？"></f:Button>
                        </div>
                    </div>
                </f:ContentPanel>
<%--                <f:Panel ID="Region2" RegionPosition="Left" RegionSplit="true" Width="200px"
                    ShowHeader="true" Title="业务菜单" Icon="Outline" Layout="Fit"
                    EnableCollapse="true" IFrameName="leftframe" IFrameUrl="about:blank"
                    runat="server">
                    <Items>--%>
                        <f:Tree RegionPosition="Left" Icon="Outline" RegionSplit="true" ShowHeader="true" ID="tree_menu" Width="200px"  EnableCollapse="true"
                           Title="业务菜单"  runat="server" OnNodeCommand="tree_menu_NodeCommand1">
                        </f:Tree>
<%--                    </Items>
                </f:Panel>--%>
                <f:TabStrip ID="mainTabStrip" RegionPosition="Center" EnableTabCloseMenu="true"  ShowBorder="true" runat="server">
                    <Tabs>
                        <f:Tab ID="Tab1" Title="首页" Layout="Fit" Icon="House" CssClass="maincontent" runat="server">
                            <Items>
                                <f:ContentPanel ID="ContentPanel2" ShowBorder="false" BodyPadding="10px" ShowHeader="false" AutoScroll="true"
                                    runat="server">
                                    首页内容
                                </f:ContentPanel>
                            </Items>
                        </f:Tab>
                    </Tabs>
                </f:TabStrip>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
