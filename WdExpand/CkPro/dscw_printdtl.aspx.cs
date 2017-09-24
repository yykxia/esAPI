using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IETCsoft.sql;
using System.Data;

namespace WdExpand.CkPro
{
    public partial class dscw_printdtl : System.Web.UI.Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = new DataTable();
                string ddls = HttpContext.Current.Request.QueryString["ddls"];
                txt_ddls.Value = ddls.ToString();
                //string sqlCmd = "select c.TradeNo,b.GoodsName,b.GoodsSpec,b.GoodsNO,a.SellCount,d.NickName,";
                //sqlCmd += " c.TradeTime,c.SndTo,c.Province from G_Trade_Goodslist a ";
                //sqlCmd += " left join G_Goods_GoodsList b on a.GoodsID=b.GoodsID";
                //sqlCmd += " left join G_Trade_Tradelist c on c.TradeID=a.TradeID";
                //sqlCmd += " left join G_Customer_CustomerList d on c.customerid=d.customerid";
                //sqlCmd += " where a.RecID="+ddls;
                //SqlSel.GetSqlSel(ref dt, sqlCmd);
                //string poid = dt.Rows[0]["NickName"].ToString();
                //string goodsname = dt.Rows[0]["GoodsName"].ToString() + dt.Rows[0]["GoodsSpec"].ToString();
                //string goodscount = dt.Rows[0]["Province"].ToString();
                //CreateTable(poid, goodsname, goodscount);
                sendDtl(ddls);
            }         

        }

        protected void sendDtl(string ddlsStr) 
        {
            //表头
            DataTable dt = new DataTable();
            dt.Columns.Add("NickName", typeof(string));
            dt.Columns.Add("GoodsName", typeof(string));
            dt.Columns.Add("SellCount", typeof(string));
            string sqlCmd = "select c.TradeNo,b.GoodsName,b.GoodsSpec,b.GoodsNO,CONVERT(float,a.SellCount) as SellCount,d.NickName,";
            sqlCmd += " c.TradeTime,c.SndTo,c.Province from G_Trade_Goodslist a ";
            sqlCmd += " left join G_Goods_GoodsList b on a.GoodsID=b.GoodsID";
            sqlCmd += " left join G_Trade_Tradelist c on c.TradeID=a.TradeID";
            sqlCmd += " left join G_Customer_CustomerList d on c.customerid=d.customerid";
            sqlCmd += " where a.RecID=";
            newTable.Attributes.Add("border", "1px");
            //newTable.Attributes.Add("BorderStyle", "Solid");
            TableRow newTableRow1 = new TableRow();
            TableCell newTableCell = new TableCell();
            newTableCell.Attributes.Add("style", "text-align: center;font-weight: 700;");
            newTableCell.Width = 200;
            newTableCell.Text = "订单号";
            newTableRow1.Controls.Add(newTableCell);
            TableCell newTableCel3 = new TableCell();
            newTableCel3.Width = 100;
            newTableCel3.Attributes.Add("style", "text-align: center;font-weight: 700;");
            newTableCel3.Text = "数量";
            newTableRow1.Controls.Add(newTableCel3);
            TableCell newTableCel2 = new TableCell();
            newTableCel2.Attributes.Add("style", "text-align: center;font-weight: 700;");
            newTableCel2.Text = "品名/规格";
            newTableRow1.Controls.Add(newTableCel2);
            newTable.Controls.Add(newTableRow1);
            //newTab.Controls.Add(newTableRow1);
            //解析货品明细Id
            String[] str = ddlsStr.Split(';');
            //int testID = Convert.ToInt32(str[0]);
            //sqlCmd += testID;
            //txt_ddls.Value = sqlCmd;
            //临时datatable
            DataTable tmpDt = new DataTable();
            //string find = "";
            foreach (string it in str)
            {
                if (it == "")
                {
                    break;
                }
                //获取订单流水
                int recId = Convert.ToInt32(it);
                //得到订单明细
                string sqlStr = sqlCmd + recId;
                SqlSel.GetSqlSel(ref tmpDt, sqlStr);
                //查询结果添加至主表dt
                if (tmpDt.Rows.Count > 0)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = tmpDt.Rows[0]["NickName"].ToString();
                    dr[1] = tmpDt.Rows[0]["GoodsName"].ToString() + "/" + tmpDt.Rows[0]["GoodsSpec"].ToString();
                    dr[2] = tmpDt.Rows[0]["SellCount"].ToString();
                    dt.Rows.Add(dr);
                }
                else 
                {
                    //find += it;
                }
            }
            //txt_ddls.Value = find;
            //生成明细项
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TableRow newTableRow = new TableRow();
                    TableCell table_cell1 = new TableCell();
                    table_cell1.Attributes.Add("style", "text-align: center");
                    table_cell1.Width = 200;
                    table_cell1.Text = dt.Rows[i]["NickName"].ToString();
                    newTableRow.Controls.Add(table_cell1);
                    TableCell table_cell3 = new TableCell();
                    table_cell3.Width = 100;
                    table_cell3.Attributes.Add("style", "text-align: center");
                    table_cell3.Text = dt.Rows[i]["SellCount"].ToString();
                    newTableRow.Controls.Add(table_cell3);
                    TableCell table_cell2 = new TableCell();
                    table_cell2.Text = dt.Rows[i]["GoodsName"].ToString();
                    newTableRow.Controls.Add(table_cell2);
                    newTable.Controls.Add(newTableRow);
                }
            }
            //将结果表格在div1中显示
            //div1.Attributes.Add("style", "text-align: center");
            //div1.Controls.Add(newTab);
        }

        //

        public void CreateTable(string pno, string goodsname, string province)
        {

            Table newTable = new Table();
            newTable.Width = 230;
            newTable.Height = 160;
            newTable.Attributes.Add("style", "border:solid #add9c0; border-width:1px 0px 0px 1px;");
            TableRow newTableRow = new TableRow();
            //newTableRow.Height = 16;
            TableCell newTableCell = new TableCell();
            newTableCell.Attributes.Add("style", "border:solid #add9c0; border-width:0px 1px 1px 0px; padding:10px 0px;Height:32px;font-size:13px;font-weight:bold;");
            newTableCell.Width = 230;
            newTableCell.Text = "定制家·索菲亚";
            //TableCell newTableCell1 = new TableCell();
            //newTableCell1.Attributes.Add("style", "border:solid #add9c0; border-width:0px 1px 1px 0px; padding:10px 0px;");

            //newTableCell1.Width =230 ;
            //newTableCell1.Text = "定制家·索菲亚";

            newTableRow.Controls.Add(newTableCell);
            newTable.Controls.Add(newTableRow);
            //newTableRow.Controls.Add(newTableCell1);
            //newTable.Controls.Add(newTableRow);


            TableRow newTableRow1 = new TableRow();
            //newTableRow1.Height = 16;
            TableCell newTableCell2 = new TableCell();
            newTableCell2.Attributes.Add("style", "border:solid #add9c0; border-width:0px 1px 1px 0px; padding:10px 0px;Height:32px;font-size:13px;font-weight:bold;");

            newTableCell2.Width = 230;
            newTableCell2.Text = "送货地址：" + province;
            //TableCell newTableCell3 = new TableCell();
            //newTableCell3.Attributes.Add("style", "border:solid #add9c0; border-width:0px 1px 1px 0px; padding:10px 0px;");

            //newTableCell3.Width = 90;
            //newTableCell3.Text = province;

            newTableRow1.Controls.Add(newTableCell2);
            newTable.Controls.Add(newTableRow1);
            //newTableRow1.Controls.Add(newTableCell3);
            //newTable.Controls.Add(newTableRow1);

            TableRow newTableRow2 = new TableRow();
            //newTableRow2.Height = 24;
            TableCell newTableCell4 = new TableCell();
            newTableCell4.Attributes.Add("style", "border:solid #add9c0; border-width:0px 1px 1px 0px; padding:10px 0px;Height:56px;vertical-align:top;font-size:12px;font-weight:bold;");

            newTableCell4.Width = 230;
            newTableCell4.Text ="品名："+ goodsname;
            //TableCell newTableCell5 = new TableCell();
            //newTableCell5.Attributes.Add("style", "border:solid #add9c0; border-width:0px 1px 1px 0px; padding:10px 0px;");

            ////newTableCell5.Width = 90;
            //newTableCell5.Text = goodsname;

            newTableRow2.Controls.Add(newTableCell4);
            newTable.Controls.Add(newTableRow2);
            //newTableRow2.Controls.Add(newTableCell5);
            //newTable.Controls.Add(newTableRow2);

            TableRow newTableRow3 = new TableRow();
            //newTableRow3.Height = 16;
            TableCell newTableCell6 = new TableCell();
            newTableCell6.Attributes.Add("style", "border:solid #add9c0; border-width:0px 1px 1px 0px; padding:10px 0px;Height:32px;font-size:15px;font-weight:bold;");

            newTableCell6.Width = 230;
            newTableCell6.Text = "订单号：" + pno;
            //TableCell newTableCell7 = new TableCell();
            //newTableCell7.Attributes.Add("style", "border:solid #add9c0; border-width:0px 1px 1px 0px; padding:10px 0px;");

            ////newTableCell7.Width = 90;
            //newTableCell7.Text = pno;

            newTableRow3.Controls.Add(newTableCell6);
            newTable.Controls.Add(newTableRow3);
            //newTableRow3.Controls.Add(newTableCell7);
            //newTable.Controls.Add(newTableRow3);

            div1.Controls.Add(newTable);


        }

        protected void btn1_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string ddlsStr = txt_ddls.Value;
            String[] str = ddlsStr.Split(';');

            foreach (string it in str)
            {
                if (it == "")
                {
                    break;
                }
                //获取订单流水
                int recId = Convert.ToInt32(it);
                //
                string sqlCmd = "select * from T_Print_dtl where RecID=" + recId;
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                if (dt.Rows.Count > 0)
                {
                    continue;
                }
                else
                {
                    sqlCmd = "insert into T_Print_dtl (IsPrint,PrintTime,RecID) values (1,'" + DateTime.Now + "'," + recId + ")";
                    SqlSel.ExeSql(sqlCmd);                    
                }

            }


        }

    }
}