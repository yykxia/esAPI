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
    public partial class dscw_print_orig : System.Web.UI.Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = new DataTable();
                int ddls = Convert.ToInt32(HttpContext.Current.Request.QueryString["ddls"]);
                txt_ddls.Value = ddls.ToString();
                string sqlCmd = "select c.TradeNo,b.GoodsName,b.GoodsSpec,b.GoodsNO,CONVERT(float,a.SellCount) as SellCount,d.NickName,";
                sqlCmd += " c.TradeTime,c.SndTo,c.Province from G_Trade_Goodslist a ";
                sqlCmd += " left join G_Goods_GoodsList b on a.GoodsID=b.GoodsID";
                sqlCmd += " left join G_Trade_Tradelist c on c.TradeID=a.TradeID";
                sqlCmd += " left join G_Customer_CustomerList d on c.customerid=d.customerid";
                sqlCmd += " where a.RecID=" + ddls;
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                string poid = dt.Rows[0]["NickName"].ToString();
                string goodsname = dt.Rows[0]["GoodsName"].ToString() + dt.Rows[0]["GoodsSpec"].ToString();
                string Province = dt.Rows[0]["Province"].ToString();
                Label_count.Text = dt.Rows[0]["SellCount"].ToString();//数量总数
                CreateTable(poid, goodsname, Province, dt.Rows[0]["SellCount"].ToString());
            }

        }
        public void CreateTable(string pno, string goodsname, string province,string perCount)
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
            newTableCell4.Text = "品名：" + goodsname + " 数量：" + perCount;
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
            int ddls = Convert.ToInt32(txt_ddls.Value);
            string sqlCmd = "select * from T_Print_dtl where RecID=" + ddls;
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            if (dt.Rows.Count > 0)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "", "<script language=javascript>window.opener=null;window.open('','_self');window.close();</script>");
            }
            else
            {
                sqlCmd = "insert into T_Print_dtl (IsPrint,PrintTime,RecID) values (1,'" + DateTime.Now + "'," + ddls + ")";
                SqlSel.ExeSql(sqlCmd);
                ClientScript.RegisterStartupScript(Page.GetType(), "", "<script language=javascript>window.opener=null;window.open('','_self');window.close();</script>");
            }
        }

        //拆分
        protected void btn_separate_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                int ddls = Convert.ToInt32(txt_ddls.Value);
                string sqlCmd = "select c.TradeNo,b.GoodsName,b.GoodsSpec,b.GoodsNO,CONVERT(float,a.SellCount) as SellCount,d.NickName,";
                sqlCmd += " c.TradeTime,c.SndTo,c.Province from G_Trade_Goodslist a ";
                sqlCmd += " left join G_Goods_GoodsList b on a.GoodsID=b.GoodsID";
                sqlCmd += " left join G_Trade_Tradelist c on c.TradeID=a.TradeID";
                sqlCmd += " left join G_Customer_CustomerList d on c.customerid=d.customerid";
                sqlCmd += " where a.RecID=" + ddls;
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                string poid = dt.Rows[0]["NickName"].ToString();
                string goodsname = dt.Rows[0]["GoodsName"].ToString() + dt.Rows[0]["GoodsSpec"].ToString();
                string Province = dt.Rows[0]["Province"].ToString();
                int goodsCount = Convert.ToInt32(dt.Rows[0]["SellCount"]);//总数量
                int perCount = Convert.ToInt32(boxSize.Value);//单箱容积
                int fullCount = (int)Math.Floor(Convert.ToDouble(goodsCount / perCount));
                int lastCount = goodsCount % perCount;//不满一箱的单独打印

                for (int i = 0; i < fullCount; i++) 
                {
                    CreateTable(poid, goodsname, Province, perCount.ToString());
                }
                //剩余装箱件数
                if (lastCount != 0)
                {
                    CreateTable(poid, goodsname, Province, lastCount.ToString());
                }
            }
            catch (Exception ex) 
            {
                Response.Write(ex.ToString());
            }
        }
    }
}