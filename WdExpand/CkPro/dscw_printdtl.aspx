<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dscw_printdtl.aspx.cs" Inherits="WdExpand.CkPro.dscw_printdtl" %>

<!DOCTYPE html>

<html>
<head id="head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type="text/javascript" src="../js/LodopFuncs.js"></script>

    <style type="text/css">
        table{
            width: 100%;
            border: 1px solid;
            border-collapse: collapse;
        }

        tr td {
            border: 1px solid;
        }
    </style>
</head>
<body>
    <script type="text/javascript">
        var LODOP = getLodop(document.getElementById('LODOP_OB'), document.getElementById('LODOP_EM'));
    </script>
    <form id="form1" runat="server">
        <asp:Button ID="btn1" runat="server" Text="打印全部" OnClientClick="prn_preview1()" OnClick="btn1_Click" />
    <div id="div1" runat="server">
        <asp:Table ID="newTable" runat="server" Caption="【定制家·索菲亚】发货清单" CellPadding="0" CellSpacing="0" BorderColor="#000000"></asp:Table>
        <%--<asp:Label ID="Label_title" runat="server" Text="索菲亚发货清单"></asp:Label>--%>
<%--    <input id="tableTitle" type="text" value="索菲亚发货清单" style="text-align:center;font-weight:700" />--%>
    </div>
        <input id="txt_ddls" type="text" runat="server" visible="false" />
    </form>
    <script type="text/javascript">
        function prn_preview1() {
            LODOP.PRINT_INIT("物流单打印");
            var strheadStyle = "<head>" + document.getElementById("head1").innerHTML + "</head>";
            var strFormHtml = strheadStyle + document.getElementById("div1").innerHTML;
            LODOP.ADD_PRINT_HTM(0, 10, "98%", "98%", strFormHtml);
            LODOP.SET_PRINT_PAGESIZE(1, 0, 0, "A4");
            LODOP.SET_SHOW_MODE("NP_NO_RESULT", true);
            LODOP.PREVIEW();
        };
    </script>    
</body>
</html>
