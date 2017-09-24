using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using IETCsoft.sql;
using System.Web.UI;
using System.Web.UI.WebControls;
using FineUI;

namespace WdExpand.SFYFK
{
    public partial class sfy_wlfk : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }
        protected void btn_confirm_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string sqlCmd = "select '" + txb_order.Text + "' as NickName,* from wdapifh where wdapifh.tradeno in ( ";
            sqlCmd += "select g_trade_tradelist.tradeno from  g_trade_tradelist ";
            sqlCmd += "inner join g_customer_customerlist on g_customer_customerlist.CustomerID=g_trade_tradelist.CustomerID ";
            sqlCmd += "where g_customer_customerlist.NickName = '" + txb_order.Text + "')";
            if (!string.IsNullOrEmpty(txb_order.Text))
            {
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                Grid1.DataSource = dt;
                Grid1.DataBind();
            }
            else 
            {
                Alert.ShowInTop("请输入有效订单号！");
            }
            //dt1 = new DataTable();
            //dt1.Columns.Add("TradeNO", typeof(string));
            //dt1.Columns.Add("orderId", typeof(string));
            //dt1.Columns.Add("LOG_INFO", typeof(string));

            ////string sqlcmd="select [TradeStatus],[TradeNO],[ShopID],(select [Name] FROM [mbh_wdgj31].[dbo].[G_Cfg_LogisticList] where [G_Trade_TradeList].LogisticID= [G_Cfg_LogisticList].LogisticID) as [LogisticName]"
            ////    +" ,[PostID],[TradeNO2] from [mbh_wdgj31].[dbo].[G_Trade_TradeList] "
            ////    +"   where [ShopID]='1029' and [TradeStatus]=11 and [TradeNO2] like '%-%' "
            ////    + " and [TradeNO]>(select top 1  max([TradeNO]) from WDApiFH ) and TradeFrom='蜘蛛'";

            ////取已同步的最大发货时间前4天所有数据去重并同步
            //string sqlcmd = "select [TradeStatus],[TradeNO],[ShopID],[sndtime],(select [Name] FROM [mbh_wdgj31].[dbo].[G_Cfg_LogisticList] where [G_Trade_TradeList].LogisticID= [G_Cfg_LogisticList].LogisticID) as [LogisticName]"
            //    + " ,[PostID],[TradeNO2] from [mbh_wdgj31].[dbo].[G_Trade_TradeList] "
            //    + "   where [ShopID]='1029' and [TradeStatus]=11 and ([TradeNO2] like '%-%' or [TradeNO2] like '%SFY%') "
            //    + " and sndTime>dateadd(day,-1,(select max(sendtime) from WDApiFH)) and TradeFrom='蜘蛛'";

            
            //DataTable dt = new DataTable();
            //SqlSel.GetSqlSel(ref dt, sqlcmd);
            //string sqlCmd1 = "select max(rqtid) from wdapifh";
            //int curId = Convert.ToInt32(SqlSel.GetSqlScale(sqlCmd1));
            //try
            //{
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        string LogisticName = dt.Rows[i]["LogisticName"].ToString();
            //        string PostID = dt.Rows[i]["PostID"].ToString();
            //        string orderid = dt.Rows[i]["TradeNO2"].ToString();
            //        string tradeNo = dt.Rows[i]["TradeNO"].ToString();
            //        string sendTime = Convert.ToDateTime(dt.Rows[i]["sndTime"]).ToString("yyyy-MM-dd HH:mm:ss.fff");
            //        mSndSFY(orderid, LogisticName, PostID, tradeNo, sendTime);
            //    }
            //    sqlCmd1 = "select * from wdapifh where rqtid>" + curId;
            //    SqlSel.GetSqlSel(ref dt1, sqlCmd1);
            //    Grid1.DataSource = dt1;
            //    Grid1.DataBind();
            //    //Label_status.Text = string.Format("同步物流信息完成！共反馈{0}条信息。", dt1.Rows.Count);
            //    //Alert.ShowInTop(string.Format("同步物流信息完成！共反馈{0}条信息。",dt1.Rows.Count));
            //}
            //catch (Exception) 
            //{
            //    Alert.ShowInTop("出错了，请联系管理员！");
            //    return;
            //}


            //dt1.DataSet.Clear();
        }

        /// <summary>
        /// 发送反馈
        /// </summary>
        /// <param name="OrderNO">订单号</param>
        /// <param name="SndStyle">物流方式</param>
        /// <param name="BillID">物流单号</param>
        /// <returns></returns>
        //public void mSndSFY(string OrderNO, string SndStyle, string BillID, string tradeNo,string sendTime)
        //{
            
          
        //    int i = OrderNO.Split(',').Length - 1;
        //    string IPAddress = IPHelp.ClientIP;
        //    DataTable dt2=new DataTable();
        //    foreach (var id in OrderNO.Split(','))
        //    {
        //        if (id.Contains("-"))
        //        {
        //            string sqlstr = "select * from WDApiFH where OrderId='" + id + "' and LOG_INFO='" + string.Format("发货类型:{0},快递单号:{1} ", SndStyle, BillID) + "'";
        //            SqlSel.GetSqlSel(ref dt2, sqlstr);
        //            if (dt2.Rows.Count > 0)
        //            {
        //                continue;
        //            }
        //            else
        //            {
        //                string dtbln = id.Split('-')[0];
        //                string dline = id.Substring(dtbln.Length + 1);
        //                string qunty = "0";
        //                string sqlCmd2 = "insert into WDApiFH ([Header_ID],[Line_ID],[OrderId],[TQuantity],[LOG_INFO],[FHSat],[Create_Date],[TradeNO],[sendTime]) values ('" + dtbln + "','" + dline + "','" + id + "','" + qunty + "','" + string.Format("发货类型:{0},快递单号:{1} ", SndStyle, BillID) + "','0','" + DateTime.Now + "','" + tradeNo + "','" + sendTime + "')";
        //                //执行发货同步
        //                int execounts2 = SqlSel.ExeSql(sqlCmd2);
        //                if (execounts2 == 0) 
        //                {
        //                    Alert.Show(string.Format("出错了，请联系管理员！{0}", sqlCmd2));
        //                    return;
        //                }
        //            }
        //        }
        //        if (id.Contains("SFY"))
        //        {
        //            string sqlCmdStr = "select parentid from SFYOrderTab where OrderId='" + id + "'";
        //            string orgId = SqlSel_Pro.GetSqlScale(sqlCmdStr).ToString();
        //            sqlCmdStr = "select * from WDApiFH where OrderId='" + orgId + "' and LOG_INFO='" + string.Format("发货类型:{0},快递单号:{1} ", SndStyle, BillID) + "'";
        //            SqlSel.GetSqlSel(ref dt2, sqlCmdStr);
        //            if (dt2.Rows.Count > 0)
        //            {
        //                continue;
        //            }
        //            else
        //            {
        //                string dtbln = orgId.Split('-')[0];
        //                string dline = orgId.Substring(dtbln.Length + 1);
        //                string qunty = "0";
        //                string sqlCmd2 = "insert into WDApiFH ([Header_ID],[Line_ID],[OrderId],[TQuantity],[LOG_INFO],[FHSat],[Create_Date],[TradeNO],[sendTime]) values ('" + dtbln + "','" + dline + "','" + orgId + "','" + qunty + "','" + string.Format("发货类型:{0},快递单号:{1} ", SndStyle, BillID) + "','0','" + DateTime.Now + "','" + tradeNo + "','" + sendTime + "')";
        //                //执行发货同步
        //                int execounts2 = SqlSel.ExeSql(sqlCmd2);
        //                if (execounts2 == 1)
        //                {

        //                }
        //            }

        //        }
        //        else
        //        {
        //            continue;
        //        }
        //    }
           
        //    //写入索菲亚接口

        //    DataTable newdt = new DataTable();
        //    string sqlCmd = "select * from WDApiFH where FHSat=0 ";
        //    SqlSel.GetSqlSel(ref newdt, sqlCmd);
        //    for (int ii = 0; ii < newdt.Rows.Count; ii++)
        //    {

        //        string orderid = newdt.Rows[ii]["OrderId"].ToString();
        //        ServiceReference1.WebServiceSoapClient wbc = new ServiceReference1.WebServiceSoapClient();
        //        wbc.AddPoRequisition(Int32.Parse(newdt.Rows[ii]["Header_ID"].ToString()), Int32.Parse(newdt.Rows[ii]["Line_ID"].ToString()), 0, newdt.Rows[ii]["LOG_INFO"].ToString());//传值
        //        string sqlstr = "update WDApiFH set [FHSat]=1 where [OrderId]='" + orderid + "' ";
        //        int execounts = SqlSel.ExeSql(sqlstr);
        //        if (execounts == 0)
        //        {
        //            break;
        //        }
        //        //DataRow dr = dt1.NewRow();
        //        //dr["TradeNO"] = newdt.Rows[ii]["TradeNO"].ToString();
        //        //dr["orderId"] = orderid;
        //        //dr["LOG_INFO"] = newdt.Rows[ii]["LOG_INFO"].ToString();
        //        //dt1.Rows.Add(dr);

        //    }

        //    string sqlCmdt = "insert into WDApi_logs ([PageUrl],[AddedTime],[UserName],[IPAddress],[Privilege],[Description]) values ('','" + DateTime.Now + "','wdgj_api','" + IPAddress + "','发货同步','" + string.Format("【wdgj_发货同步】订单号:{0}，同步时间{1}，发货类型:{2},快递单号:{3} ", OrderNO.Replace(',', '，'), DateTime.Now, SndStyle, BillID) + "')";
        //    //执行插入日志
        //    int execountst = SqlSel.ExeSql(sqlCmdt);


        //}

        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(DatePicker1.Text) & !string.IsNullOrEmpty(DatePicker2.Text))
            {
                string sqlCmd = "select g_customer_customerlist.NickName,* from wdapifh ";
                sqlCmd += "left join g_trade_tradelist on g_trade_tradelist.tradeNO=wdapifh.tradeNO ";
                sqlCmd += "left join g_customer_customerlist on g_customer_customerlist.CustomerID=g_trade_tradelist.CustomerID ";
                sqlCmd += "where sendTime>='" + DatePicker1.Text + "' and sendTime<='" + DatePicker2.Text + "'";
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                Grid1.DataSource = dt;
                Grid1.DataBind();
            }
            else 
            {
                Alert.ShowInTop("请输入有效的起止日期进行查询！");
            }
        }

        ////定时器：定时执行回传
        //protected void Timer1_Tick(object sender, EventArgs e)
        //{
        //    Label_status.Text = "正在同步...";
        //    btn_confirm_Click(sender, e);
        //}
        ////开启定时器
        //protected void btn_autoReturnBack_Click(object sender, EventArgs e)
        //{
        //    Timer1.Enabled = true;
        //    label_mark.Text = "（自动同步已开启，手动操作前请先关闭自动同步！）";
        //}
        ////关闭定时器
        //protected void btn_close_Click(object sender, EventArgs e)
        //{
        //    Timer1.Enabled = false;
        //    label_mark.Text = "（自动同步已关闭）";
        //    Label_status.Text = "";
        //}



    }
}