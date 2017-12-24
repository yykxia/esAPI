using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IETCsoft.sql;
using System.Data;
using FineUI;

namespace WdExpand.CkPro
{
    public partial class SellBack_Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DatePicker1.SelectedDate = DateTime.Now;
                DatePicker2.SelectedDate = DateTime.Now.AddDays(1);

                BindGrid1(DatePicker1.Text, DatePicker2.Text);
            }

        }

        private void BindGrid1(string StartDate,string EndDate)
        {
            try
            {
                string sqlCmd = "select t2.TradeNo,t1.regSum,t3.shopName,t4.backSum,t2.TradeId,t2.GoodsList,t2.sndTo,t2.adr,t2.rcvPostId," +
                    "(case when (cast(t4.backSum as int)-t1.regSum)=0 then 1 else 0 end) as closeStaus " +
                    "from (select BillId, sum(regCount) as regSum from ES_WH_SellBackRegister " +
                    "where regTime between '" + StartDate + "' and '" + EndDate + "' group by BillId) t1 " +
                    "left join G_SellBack_List t2 on t1.BillId = t2.TradeId " +
                    "left join g_cfg_shoplist t3 on t3.shopId = t2.shopId " +
                    "left join(select tradeId, sum(sellCount) as backSum from G_SellBack_GoodsList group by tradeId) t4 " +
                    "on t4.tradeId = t2.tradeId";
                DataTable dt = new DataTable();
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                Grid1.DataSource = dt;
                Grid1.DataBind();

            } catch(Exception ex)
            {
                Alert.ShowInTop(ex.Message);
            }
        }

        private void BindGrid2(string TradeId)
        {
            string sqlCmd = "select t1.recId,cast(t1.sellCount as int) as sellCount,t2.GoodsNo,t2.GoodsName from G_SellBack_GoodsList t1 " +
            "left join g_goods_goodsList t2 on cast(t1.goodsid as int)=t2.goodsid where tradeId='" + TradeId + "'";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            Grid2.DataSource = dt;
            Grid2.DataBind();
        }

        private void BindGrid3(string TradeId)
        {
            string sqlCmd = "select t1.recId,t3.GoodsNo,t3.GoodsName,t1.regCount,t1.dealwith,t1.regTime from ES_WH_SellBackRegister t1 " +
                "left join G_SellBack_GoodsList t2 on t1.recId=t2.recId " +
                "left join g_goods_goodsList t3 on cast(t2.goodsid as int)= t3.goodsid where billId = '" + TradeId + "' order by regTime";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            Grid3.DataSource = dt;
            Grid3.DataBind();
        }

        protected void Grid1_RowSelect(object sender, GridRowSelectEventArgs e)
        {
            if (Grid1.SelectedRowIndex < 0)
            {
                return;
            }

            string TradeId = Grid1.DataKeys[Grid1.SelectedRowIndex][0].ToString();

            BindGrid2(TradeId);
            BindGrid3(TradeId);
        }

        protected void btn_query_Click(object sender, EventArgs e)
        {
            BindGrid1(DatePicker1.Text, DatePicker2.Text);
        }

        protected void btn_export_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(Window1.GetShowReference("ExportAsExcel.aspx?startDate=" 
                + DatePicker1.Text + "&endDate=" + DatePicker2.Text));
        }
    }
}