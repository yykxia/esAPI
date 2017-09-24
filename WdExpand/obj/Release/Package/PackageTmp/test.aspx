<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="WdExpand.test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" />
        <f:TextBox ID="txb1" runat="server"></f:TextBox><f:TextBox ID="TextBox1" runat="server"></f:TextBox>
        <f:Button ID="btn1" Text="查询" runat="server" OnClick="btn1_Click"></f:Button>
    </form>
</body>
</html>
