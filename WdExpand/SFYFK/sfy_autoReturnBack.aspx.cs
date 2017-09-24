using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FineUI;
using System.Data;
using IETCsoft.sql;

namespace WdExpand.SFYFK
{
    public partial class sfy_autoReturnBack : System.Web.UI.Page
    {               
        //初始化接口
        net.sogal.mlily.WebService sfyWebSrv = new net.sogal.mlily.WebService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                if (getClientIp() != "112.124.48.116")
                {
                    btn_autoReturnBack.Enabled = false;
                    btn_close.Enabled = false;
                }
            }
        }

        protected void execReturn()
        {
            DataTable dt1 = new DataTable();
            //string sqlcmd="select [TradeStatus],[TradeNO],[ShopID],(select [Name] FROM [mbh_wdgj31].[dbo].[G_Cfg_LogisticList] where [G_Trade_TradeList].LogisticID= [G_Cfg_LogisticList].LogisticID) as [LogisticName]"
            //    +" ,[PostID],[TradeNO2] from [mbh_wdgj31].[dbo].[G_Trade_TradeList] "
            //    +"   where [ShopID]='1029' and [TradeStatus]=11 and [TradeNO2] like '%-%' "
            //    + " and [TradeNO]>(select top 1  max([TradeNO]) from WDApiFH ) and TradeFrom='蜘蛛'";

            //取已同步的最大发货时间前1天所有数据去重并同步
            string sqlcmd = "select [TradeStatus],[TradeNO],[ShopID],[sndtime],(select [Name] FROM [mbh_wdgj31].[dbo].[G_Cfg_LogisticList] where [G_Trade_TradeList].LogisticID= [G_Cfg_LogisticList].LogisticID) as [LogisticName]"
                + " ,[PostID],[TradeNO2] from [mbh_wdgj31].[dbo].[G_Trade_TradeList] "
                + "   where [ShopID]='1029' and [TradeStatus]=11 and ([TradeNO2] like '%-%' or [TradeNO2] like '%SFY%') "
                + " and sndTime>dateadd(day,-1,(select max(sendtime) from WDApiFH)) and TradeFrom='蜘蛛'";


            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlcmd);
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string LogisticName = dt.Rows[i]["LogisticName"].ToString();
                    string PostID = dt.Rows[i]["PostID"].ToString();
                    string orderid = dt.Rows[i]["TradeNO2"].ToString();
                    string tradeNo = dt.Rows[i]["TradeNO"].ToString();
                    string sendTime = Convert.ToDateTime(dt.Rows[i]["sndTime"]).ToString("yyyy-MM-dd HH:mm:ss.fff");
                    mSndSFY(orderid, LogisticName, PostID, tradeNo, sendTime);
                }

                string sqlCmd1 = "select max(rqtid) from wdapifh";
                int curId = Convert.ToInt32(SqlSel.GetSqlScale(sqlCmd1));

                //写入索菲亚接口-批量回传物流信息
                DataTable newdt = new DataTable();
                string sqlCmd = "select * from WDApiFH where FHSat=0 ";
                SqlSel.GetSqlSel(ref newdt, sqlCmd);
                for (int ii = 0; ii < newdt.Rows.Count; ii++)
                {
                    string orderid = newdt.Rows[ii]["rqtid"].ToString();
                    //ServiceReference1.WebServiceSoapClient wbc = new ServiceReference1.WebServiceSoapClient();
                    if (sfyWebSrv.AddPoRequisition(Int32.Parse(newdt.Rows[ii]["Header_ID"].ToString()), Int32.Parse(newdt.Rows[ii]["Line_ID"].ToString()), 0, newdt.Rows[ii]["LOG_INFO"].ToString()))//传值
                    {
                        string sqlstr = "update WDApiFH set [FHSat]=1 where [rqtid]='" + orderid + "' ";
                        SqlSel.ExeSql(sqlstr);
                    }
                }
                //记录自动执行日志
                sqlCmd1 = "insert into t_sfy_autoExecLog (execTime,execCount) values ('" + DateTime.Now + "'," + curId + ")";
                SqlSel.ExeSql(sqlCmd1);
                Label_status.Text =string.Format("{0},同步物流信息完成。",DateTime.Now);
                //Alert.ShowInTop(string.Format("同步物流信息完成！共反馈{0}条信息。",dt1.Rows.Count));
            }
            catch (Exception ex)
            {
                Label_status.Text = ex.Message;
                Timer1.Enabled = false;
            }


            //dt1.DataSet.Clear();
        }

        /// <summary>
        /// 发送反馈
        /// </summary>
        /// <param name="OrderNO">订单号</param>
        /// <param name="SndStyle">物流方式</param>
        /// <param name="BillID">物流单号</param>
        /// <returns></returns>
        public void mSndSFY(string OrderNO, string SndStyle, string BillID, string tradeNo, string sendTime)
        {
            try
            {

                int i = OrderNO.Split(',').Length - 1;
                string IPAddress = IPHelp.ClientIP;
                DataTable dt2 = new DataTable();
                foreach (var id in OrderNO.Split(','))
                {
                    if (id.Contains("-"))
                    {
                        string sqlstr = "select * from WDApiFH where OrderId='" + id + "' and LOG_INFO='" + string.Format("发货类型:{0},快递单号:{1} ", SndStyle, BillID) + "'";
                        SqlSel.GetSqlSel(ref dt2, sqlstr);
                        if (dt2.Rows.Count > 0)
                        {
                            continue;
                        }
                        else
                        {
                            string dtbln = id.Split('-')[0];
                            string dline = id.Substring(dtbln.Length + 1);
                            string qunty = "0";
                            string sqlCmd2 = "insert into WDApiFH ([Header_ID],[Line_ID],[OrderId],[TQuantity],[LOG_INFO],[FHSat],[Create_Date],[TradeNO],[sendTime]) values ('" + dtbln + "','" + dline + "','" + id + "','" + qunty + "','" + string.Format("发货类型:{0},快递单号:{1} ", SndStyle, BillID) + "','0','" + DateTime.Now + "','" + tradeNo + "','" + sendTime + "')";
                            //执行发货同步
                            int execounts2 = SqlSel.ExeSql(sqlCmd2);
                            if (execounts2 == 0)
                            {
                                label_mark.Text = string.Format("执行发货同步失败，SQL语句：{0}", sqlCmd2);
                                return;
                            }
                        }
                    }
                    if (id.Contains("SFY"))
                    {
                        string sqlCmdStr = "select parentid from SFYOrderTab where OrderId='" + id + "'";
                        string orgId = SqlSel_Pro.GetSqlScale(sqlCmdStr).ToString();
                        sqlCmdStr = "select * from WDApiFH where OrderId='" + orgId + "' and LOG_INFO='" + string.Format("发货类型:{0},快递单号:{1} ", SndStyle, BillID) + "'";
                        SqlSel.GetSqlSel(ref dt2, sqlCmdStr);
                        if (dt2.Rows.Count > 0)
                        {
                            continue;
                        }
                        else
                        {
                            string dtbln = orgId.Split('-')[0];
                            string dline = orgId.Substring(dtbln.Length + 1);
                            string qunty = "0";
                            string sqlCmd2 = "insert into WDApiFH ([Header_ID],[Line_ID],[OrderId],[TQuantity],[LOG_INFO],[FHSat],[Create_Date],[TradeNO],[sendTime]) values ('" + dtbln + "','" + dline + "','" + orgId + "','" + qunty + "','" + string.Format("发货类型:{0},快递单号:{1} ", SndStyle, BillID) + "','0','" + DateTime.Now + "','" + tradeNo + "','" + sendTime + "')";
                            //执行发货同步
                            int execounts2 = SqlSel.ExeSql(sqlCmd2);
                            if (execounts2 == 1)
                            {

                            }
                        }

                    }
                    else
                    {
                        continue;
                    }
                }


                //string sqlCmdt = "insert into WDApi_logs ([PageUrl],[AddedTime],[UserName],[IPAddress],[Privilege],[Description]) values ('','" + DateTime.Now + "','wdgj_api','" + IPAddress + "','发货同步','" + string.Format("【wdgj_发货同步】订单号:{0}，同步时间{1}，发货类型:{2},快递单号:{3} ", OrderNO.Replace(',', '，'), DateTime.Now, SndStyle, BillID) + "')";
                //执行插入日志
                //int execountst = SqlSel_Pro.ExeSql(sqlCmdt);
            }
            catch (Exception ex) 
            {
                Label_status.Text = ex.Message;
                Timer1.Enabled = false;
            }

        }


        //定时器：定时执行回传
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            Label_status.Text = "正在同步...";
            execReturn();
        }
        //开启定时器
        protected void btn_autoReturnBack_Click(object sender, EventArgs e)
        {
            Timer1.Enabled = true;
            btn_autoReturnBack.Enabled = false;
            label_mark.Text = "（自动同步已开启，手动操作前请先关闭自动同步！）";
        }
        //关闭定时器
        protected void btn_close_Click(object sender, EventArgs e)
        {
            Timer1.Enabled = false;
            btn_autoReturnBack.Enabled = true;
            label_mark.Text = "（自动同步已关闭）";
            Label_status.Text = "";
        }

        //获取执行客户端IP
        protected string getClientIp()
        {
            HttpRequest request = HttpContext.Current.Request;
            string result = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(result))
            {
                result = request.UserHostAddress;
            }
            if (string.IsNullOrEmpty(result))
            {
                result = "0.0.0.0";
            }

            return result;
        }

        protected void btn_post_Click(object sender, EventArgs e)
        {
            execReturn();
        }

    }
}