using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FineUI;
using System.Data;
using IETCsoft.sql;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Text;

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
                    mSndSFY(orderid, LogisticName, PostID, tradeNo, sendTime,"1029");
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
                sqlCmd1 = "insert into t_sfy_autoExecLog (execTime,execCount,shopId) values ('" + DateTime.Now + "'," + curId + ",'1029')";
                SqlSel.ExeSql(sqlCmd1);
                Label_status.Text =string.Format("{0},【1029】同步物流信息完成。",DateTime.Now);

                //同步记录前台更新
                reloadLog();
            }
            catch (Exception ex)
            {
                Label_status.Text = ex.Message;
                Timer1.Enabled = false;
            }


            //dt1.DataSet.Clear();
        }


        protected void autoLogistics(string shopId)
        {
            //DataTable dt1 = new DataTable();
            //string sqlcmd="select [TradeStatus],[TradeNO],[ShopID],(select [Name] FROM [mbh_wdgj31].[dbo].[G_Cfg_LogisticList] where [G_Trade_TradeList].LogisticID= [G_Cfg_LogisticList].LogisticID) as [LogisticName]"
            //    +" ,[PostID],[TradeNO2] from [mbh_wdgj31].[dbo].[G_Trade_TradeList] "
            //    +"   where [ShopID]='1029' and [TradeStatus]=11 and [TradeNO2] like '%-%' "
            //    + " and [TradeNO]>(select top 1  max([TradeNO]) from WDApiFH ) and TradeFrom='蜘蛛'";

            //取已同步的最大发货时间前1天所有数据去重并同步
            string sqlcmd = "select [TradeStatus],[TradeNO],[ShopID],[sndtime],(select [Name] FROM"
                + " [mbh_wdgj31].[dbo].[G_Cfg_LogisticList] where [G_Trade_TradeList].LogisticID= [G_Cfg_LogisticList].LogisticID) as [LogisticName]"
                + " ,[PostID],[TradeNO2],(select nickName from g_customer_customerlist where G_Trade_TradeList.customerId=g_customer_customerlist.customerId) as nickName"
                + " from [mbh_wdgj31].[dbo].[G_Trade_TradeList] where [ShopID]='" + shopId+"' and [TradeStatus]=11"
                + " and sndTime>dateadd(day,-1,ISNULL((select max(sendtime) from WDApiFH where cipher='"+shopId+"'),'2017-09-01'))"
                + " and TradeFrom='蜘蛛'";


            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlcmd);
            StringBuilder sb = new StringBuilder();
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string LogisticName = dt.Rows[i]["LogisticName"].ToString();
                    string PostID = dt.Rows[i]["PostID"].ToString();
                    string orderid = dt.Rows[i]["TradeNO2"].ToString();
                    string tradeNo = dt.Rows[i]["TradeNO"].ToString();
                    string sendTime = Convert.ToDateTime(dt.Rows[i]["sndTime"]).ToString("yyyy-MM-dd HH:mm:ss.fff");
                    string nickName = dt.Rows[i]["nickName"].ToString();
                    sb.Append(mSndGoods(orderid, LogisticName, PostID, nickName, tradeNo, sendTime, shopId));
                }

                Label1.Text = sb.ToString();

                sqlcmd = "select isnull(max(rqtid),0) from wdapifh where cipher='" + shopId + "' ";
                int curId = Convert.ToInt32(SqlSel.GetSqlScale(sqlcmd));

                //记录自动执行日志
                sqlcmd = "insert into t_sfy_autoExecLog (execTime,execCount,shopId) values ('" + DateTime.Now + "'," + curId + ",'" + shopId + "')";
                SqlSel.ExeSql(sqlcmd);
                Label_status.Text = string.Format("{0},【1039】同步物流信息完成。", DateTime.Now);
                //同步记录前台更新
                reloadLog();
            }
            catch (Exception ex)
            {
                Label_status.Text = ex.Message;
                Timer2.Enabled = false;
            }


            //dt1.DataSet.Clear();
        }

        private string reloadLog()
        {
            string sqlcmd = "select top 20 * from t_sfy_autoExecLog order by execTime desc";
            DataTable data = new DataTable();
            SqlSel.GetSqlSel(ref data, sqlcmd);
            Grid1.DataSource = data;
            Grid1.DataBind();
            return sqlcmd;
        }

        protected string mSndGoods(string OrderNO, string SndStyle, string BillID,
                                string CustomerID, string OrderID, string SndDate,string shopId)
        {
            StringBuilder sb = new StringBuilder();
            //是否已回传物流信息
            string sqlCmd = "select isnull(count(*),0) from wdapifh where TradeNo='" + OrderID + "'";
            if (Convert.ToInt32(SqlSel.GetSqlScale(sqlCmd)) > 0)
            {
                return "";
            }
            else
            {
                string jsonStr = string.Empty;
                string ucodeShopAction = "ucodeShopAction";
                string OrderNotice = "orderNotice";
                string url = System.Configuration.ConfigurationManager.
                    AppSettings["mSndGoods_youjiagou"];
                string uCode = System.Configuration.ConfigurationManager.AppSettings["uCode"];
                string code = string.Empty;
                string errorMsg = string.Empty;
                int count = 0;
                foreach (var id in OrderNO.Split(','))
                {
                    string responseUrl = string.Empty;
                    //设置订单物流相关参数信息
                    OrderNotice notice = new OrderNotice();
                    notice.action = ucodeShopAction;
                    notice.method = OrderNotice;
                    Param param = new Param()
                    {
                        uCode = uCode,
                        informSn = id,
                        operateUser = "Mlily_API",
                        operateUserId = CustomerID,
                        deliveryTime = SndDate,
                        waybillSn = BillID,
                        serviceName = SndStyle
                    };
                    notice.param = param;
                    //实体类序列化成json字符串
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    jsonStr = jss.Serialize(notice);
                    //推送相关物流信息
                    //URL编码
                    string encodeStr = HttpUtility.UrlEncode(jsonStr);
                    //发起请求
                    sb.AppendFormat("[{0}]",id);
                    string retString = HttpPost(url, encodeStr);
                    JObject jo = (JObject)JsonConvert.DeserializeObject(retString);
                    code = jo["code"].ToString().Replace("\"", "");

                    if (code == "0")
                    {
                        //插入同步信息
                        sqlCmd = "insert into WDApiFH ([OrderId],[TQuantity],[LOG_INFO],[FHSat],[Create_Date],[TradeNO],[sendTime],cipher)";
                        sqlCmd += " values ('" + id + "','0','" + string.Format("发货类型:{0},快递单号:{1} ", SndStyle, BillID) + "',";
                        sqlCmd += "'1','" + DateTime.Now + "','" + OrderID + "','" + SndDate + "','" + shopId + "')";
                        if (SqlSel.ExeSql(sqlCmd) > 0)
                        {
                            sb.Append(":ok;");
                        }
                    }
                    else if (code == "-1")
                    {
                        errorMsg = jo["description"].ToString().Replace("\"", "");
                        sb.AppendFormat(":{0};",errorMsg);
                    }
                }
                return sb.ToString();
            }

        }

        /// <summary>
        /// 发送反馈
        /// </summary>
        /// <param name="OrderNO">订单号</param>
        /// <param name="SndStyle">物流方式</param>
        /// <param name="BillID">物流单号</param>
        /// <returns></returns>
        public void mSndSFY(string OrderNO, string SndStyle, string BillID, string tradeNo, string sendTime, string shopId)
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
                            string sqlCmd2 = "insert into WDApiFH ([Header_ID],[Line_ID],[OrderId],[TQuantity],[LOG_INFO],[FHSat],[Create_Date],[TradeNO],[sendTime],cipher) values ('" + dtbln + "','" + dline + "','" + id + "','" + qunty + "','" + string.Format("发货类型:{0},快递单号:{1} ", SndStyle, BillID) + "','0','" + DateTime.Now + "','" + tradeNo + "','" + sendTime + "','" + shopId + "')";
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
            Timer2.Enabled = true;
            btn_autoReturnBack.Enabled = false;
            label_mark.Text = "（自动同步已开启，手动操作前请先关闭自动同步！）";
        }
        //关闭定时器
        protected void btn_close_Click(object sender, EventArgs e)
        {
            Timer1.Enabled = false;
            Timer2.Enabled = false;
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
            //索菲亚
            execReturn();
            //优家购
            autoLogistics("1039");
        }

        /// <summary>  
        /// POST请求与获取结果  
        /// </summary>  
        public static string HttpPost(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postDataStr.Length;
            StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
            writer.Write(postDataStr);
            writer.Flush();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string encoding = response.ContentEncoding;
            if (encoding == null || encoding.Length < 1)
            {
                encoding = "UTF-8"; //默认编码  
            }
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
            string retString = reader.ReadToEnd();
            return retString;
        }

        protected void Timer2_Tick(object sender, EventArgs e)
        {
            autoLogistics("1039");
        }
    }
}