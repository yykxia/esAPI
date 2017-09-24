<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dscw_print_orig.aspx.cs" Inherits="WdExpand.CkPro.dscw_print_orig" %>

<!DOCTYPE html>

<html>
<head id="head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type="text/javascript" src="../js/LodopFuncs.js"></script>

</head>
<body>
    <script type="text/javascript">
        var LODOP = getLodop(document.getElementById('LODOP_OB'), document.getElementById('LODOP_EM'));
    </script>
    <form id="form1" runat="server">
        <asp:Button ID="btn1" runat="server" Text="打印全部" OnClientClick="prn_preview1()" OnClick="btn1_Click" /><br />
        <asp:Label ID="Label2" runat="server" Text="订单数量："></asp:Label>
        <asp:Label ID="Label_count" runat="server"></asp:Label>
        <asp:Label ID="Label1" runat="server" Text="单箱容量："></asp:Label>
        <input id="boxSize" runat="server" />
        <asp:Button ID="btn_separate" runat="server" Text="生成" OnClick="btn_separate_Click" /><br />

    <div id="div1" runat="server">
    
    </div>
        <input id="txt_ddls" type="text" runat="server" visible="false" />
    </form>
    <script type="text/javascript">
        function prn_preview1() {
            LODOP.PRINT_INIT("物流单打印-原始版");
            var strheadStyle = "<head>" + document.getElementById("head1").innerHTML + "</head>";
            var strFormHtml = strheadStyle + "<body>" + document.getElementById("div1").innerHTML + "</body>";
            LODOP.ADD_PRINT_HTM("1mm", 0, "70mm", "50mm", strFormHtml);
            LODOP.SET_PRINT_PAGESIZE(1, "70mm", "50mm", "");
            LODOP.SELECT_PRINTER();
            LODOP.PRINT();

        };
    </script>    
</body>
</html>
