using System;
using System.Collections.Generic;
using FineUI;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IETCsoft.sql;
using System.Data;
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;

namespace WdExpand.CkPro
{
    public partial class dscw_dddy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                //DataTable dt = new DataTable();
                //string sqlCmd = "select top 1 * from wddata";
                //SqlSel.GetSqlSel(ref dt, sqlCmd);
                //Grid1.DataSource = dt;
                //Grid1.DataBind();
                //String dt1 = Str_Dtep.Text;
                //String dt2 = End_Dtep.Text;
                //int printStat = Convert.ToInt32(RadioButtonList1.SelectedValue);
                //Button1.OnClientClick = Window1.GetShowReference("dscw_printdtl.aspx?dt1=" + Str_Dtep.Text + "&dt2=" + End_Dtep.Text + "&printstat=" + RadioButtonList1.SelectedValue + "", "打印列表");

                //Button1.OnClientClick = Window1.GetHidePostBackReference("dscw_printdtl.aspx?dt1=" + Str_Dtep.Text + "&dt2=" + End_Dtep.Text + "&printstat=" + RadioButtonList1.SelectedValue + "");
            }
        }

        protected void btn_confirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Str_Dtep.SelectedDate.ToString()) || string.IsNullOrEmpty(End_Dtep.SelectedDate.ToString()))
            {
                Alert.ShowInTop("日期不可为空！");
                return;
            }
            else
            {
                if (string.IsNullOrEmpty(txb_no.Text))
                {
                    Alert.ShowInTop("请输入收件人！");
                    return;
                }
                int printStat =Convert.ToInt32(RadioButtonList1.SelectedValue);
                String dt1 = Str_Dtep.Text;
                String dt2 = End_Dtep.Text;
                string sendto = txb_no.Text;
                BindGird(dt1, dt2, sendto, printStat);
            }
        }

        protected void BindGird(string dt1,string dt2,string sendto,int printStat) 
        {
            DataTable dt = new DataTable();
            if (printStat == 0)
            {
                string sqlCmd = "select c.TradeNo,b.GoodsName,b.GoodsNO,Convert(float,a.sellCount) as sellCount,d.NickName,c.confirmTime,c.SndTo,c.Province,a.RecID,('未打印') as printstat,e.name as postName  from G_Trade_GoodsList a";
                sqlCmd += " left join G_Goods_GoodsList b on a.GoodsID=b.GoodsID";
                sqlCmd += " left join G_Trade_TradeList c on a.TradeID=c.TradeID";
                sqlCmd += " left join G_Customer_CustomerList d on c.CustomerID=d.CustomerID";
                sqlCmd += " left join G_Cfg_LogisticList e on c.LogisticID=e.LogisticID";
                sqlCmd += " where CONVERT(varchar(100), c.ConfirmTime, 23)<='" + dt2 + "' ";
                sqlCmd += " and CONVERT(varchar(100), c.ConfirmTime, 23)>='" + dt1 + "' and c.SndTo like '%" + sendto + "%' and c.ShopID=1029";
                sqlCmd += " and a.RecID not in (select RecID from T_Print_dtl) order by c.ConfirmTime desc";
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                Grid1.DataSource = dt;
                Grid1.DataBind();

            }
            if (printStat == 1)
            {
                string sqlCmd = "select c.TradeNo,b.GoodsName,b.GoodsNO,Convert(float,a.sellCount) as sellCount,d.NickName,c.confirmTime,c.SndTo,c.Province,a.RecID,('已打印') as printstat,e.name as postName from G_Trade_GoodsList a";
                sqlCmd += " left join G_Goods_GoodsList b on a.GoodsID=b.GoodsID";
                sqlCmd += " left join G_Trade_TradeList c on a.TradeID=c.TradeID";
                sqlCmd += " left join G_Customer_CustomerList d on c.CustomerID=d.CustomerID";
                sqlCmd += " left join G_Cfg_LogisticList e on c.LogisticID=e.LogisticID";
                sqlCmd += " where CONVERT(varchar(100), c.ConfirmTime, 23)<='" + dt2 + "' ";
                sqlCmd += " and CONVERT(varchar(100), c.ConfirmTime, 23)>='" + dt1 + "' and c.SndTo like '%" + sendto + "%' and c.ShopID=1029";
                sqlCmd += " and a.RecID in (select RecID from T_Print_dtl) order by c.ConfirmTime desc";
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                Grid1.DataSource = dt;
                Grid1.DataBind();

            }
            if (printStat == 2)
            {
                string sqlCmd = "select c.TradeNo,b.GoodsName,b.GoodsNO,Convert(float,a.sellCount) as sellCount,d.NickName,c.confirmTime,c.SndTo,c.Province,a.RecID,";
                sqlCmd += " (case when f.IsPrint=0 then '未打印'";
                sqlCmd += " when f.IsPrint is null then '未打印'";
                sqlCmd += " when f.IsPrint=1 then '已打印' end ) as printstat,e.name as postName";
                sqlCmd += " from G_Trade_GoodsList a";
                sqlCmd += " left join G_Goods_GoodsList b on a.GoodsID=b.GoodsID";
                sqlCmd += " left join G_Trade_TradeList c on a.TradeID=c.TradeID";
                sqlCmd += " left join G_Customer_CustomerList d on c.CustomerID=d.CustomerID";
                sqlCmd += " left join T_Print_dtl f on a.RecID=f.RecID";
                sqlCmd += " left join G_Cfg_LogisticList e on c.LogisticID=e.LogisticID";
                sqlCmd += " where CONVERT(varchar(100), c.ConfirmTime, 23)<='" + dt2 + "' ";
                sqlCmd += " and CONVERT(varchar(100), c.ConfirmTime, 23)>='" + dt1 + "' and c.SndTo like '%" + sendto + "%' and c.ShopID=1029 order by c.ConfirmTime desc";
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                Grid1.DataSource = dt;
                Grid1.DataBind();
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            //int printStat = Convert.ToInt32(RadioButtonList1.SelectedValue);
            //String dt1 = Str_Dtep.Text;
            //String dt2 = End_Dtep.Text;
            //string sendto = txb_no.Text;
            //BindGird(dt1, dt2, sendto, printStat);
            Window1.Hidden = true;


        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            int[] selections = Grid1.SelectedRowIndexArray;
            foreach (int rowIndex in selections)
            {
                sb.AppendFormat("{0};", Grid1.DataKeys[rowIndex][0]);
            }
            PageContext.RegisterStartupScript(Window1.GetShowReference("dscw_printdtl.aspx?ddls=" + sb + ""));
        }
        //protected void Window1_Close(object sender, WindowCloseEventArgs e)
        //{
        //    int printStat = Convert.ToInt32(RadioButtonList1.SelectedValue);
        //    String dt1 = Str_Dtep.Text;
        //    String dt2 = End_Dtep.Text;
        //    string sendto = txb_no.Text;
        //    BindGird(dt1, dt2, sendto, printStat);
        //}
}
}