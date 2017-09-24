using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using IETCsoft.sql;
using Newtonsoft.Json;

namespace WdExpand
{
    /// <summary>
    /// wdgjWebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class wdgjWebService : System.Web.Services.WebService
    {
        /// <summary>
        /// 获取相关订单数据
        /// </summary>
        /// <param name="sqlStr">SQL语句</param>
        /// <param name="tableName">返回table名</param>
        /// <returns></returns>
        //[WebMethod]
        //public DataTable OrderTable(string sqlStr,string tableName)
        //{
        //    DataTable dt = new DataTable();
        //    if (SqlSel.GetSqlSel(ref dt, sqlStr))
        //    {
        //        dt.TableName = tableName;
        //        return dt;
        //    }
        //    else 
        //    {
        //        return null;
        //    }
        //}

        /// <summary>
        /// 查询时间段内的定制订单
        /// </summary>
        /// <param name="startDate">起始日期</param>
        /// <param name="endDate">截至日期</param>
        /// <returns></returns>
        [WebMethod]
        public DataTable GetCustOrders(string startDate, string endDate)
        {
            try
            {
                DataTable dt = new DataTable();
                //string dt1 = startDate.ToShortDateString();
                //string dt2 = endDate.ToShortDateString();
                string sqlStr = "select A.RecId,B.tradeTime,D.goodsName,D.goodsNO,A.sellCount,B.CustomerRemark,aa.NickName,B.Remark,";
                sqlStr += " D.goodsSpec,B.SndTo,B.Adr,B.Tel,'WDGJ' as sourceSys,d.classId,b.warehouseId,b.tradeFrom";
                sqlStr += " from g_trade_goodslist A left join g_trade_tradeList B on A.tradeId=B.tradeId";
                sqlStr += " left join G_Goods_GoodsList D on D.goodsId=A.GoodsId";
                sqlStr += " left join g_customer_customerlist aa on aa.customerId=B.customerId";
                sqlStr += " where A.goodsId in (select C.goodsId from g_goods_priceCost C where C.warehouseId in ('1037','1038','1039','1040','1041'))";
                sqlStr += " and B.tradeStatus='5' and D.goodsNO <> '送货入户并安装' and Convert(nvarchar(50),B.tradeTime,23)>='" + startDate + "'";
                sqlStr += " and Convert(nvarchar(50),B.tradeTime,23)<='" + endDate + "'";
                if (SqlSel.GetSqlSel(ref dt, sqlStr))
                {
                    dt.Columns.Add("productShape", typeof(string));//产品形状
                    dt.Columns.Add("productLength", typeof(decimal));//长度
                    dt.Columns.Add("productWidth", typeof(decimal));//宽度
                    dt.Columns.Add("productHeight", typeof(decimal));//厚度
                    dt.Columns.Add("needDate", typeof(DateTime));//需求日期
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //如果是定制薄垫,则产品形状为矩形
                        if (dt.Rows[i]["classId"].ToString() == "11")
                        {
                            dt.Rows[i]["productShape"] = "矩形";
                        }
                        else 
                        {
                            dt.Rows[i]["productShape"] = "";
                        }
                        //如果是索菲亚仓并来自于蜘蛛接口的数据，可取出需求日期和产品尺寸信息
                        if (dt.Rows[i]["warehouseId"].ToString() == "1039" & dt.Rows[i]["tradeFrom"].ToString() == "蜘蛛")
                        {
                            string CustomerRemark = dt.Rows[i]["CustomerRemark"].ToString();
                            dt.Rows[i]["needDate"] = CustomerRemark.Substring(CustomerRemark.LastIndexOf("：") + 1);//需求日期
                            string Remark = dt.Rows[i]["Remark"].ToString();
                            //取出尺寸信息
                            string sizeInfo = Remark.Substring(Remark.IndexOf("：") + 1, Remark.LastIndexOf(" ") - Remark.IndexOf("：") - 1);
                            //
                            string productLength = "", productWidth = "", productHeight = "";
                            productLength = sizeInfo.Substring(0, sizeInfo.IndexOf("*"));//长
                            productWidth = sizeInfo.Substring(sizeInfo.IndexOf("*") + 1, sizeInfo.LastIndexOf("*") - sizeInfo.IndexOf("*") - 1);//宽
                            productHeight = sizeInfo.Substring(sizeInfo.LastIndexOf("*") + 1);//厚
                            dt.Rows[i]["productLength"] = Convert.ToDecimal(productLength) / 10;//以厘米为单位
                            dt.Rows[i]["productWidth"] = Convert.ToDecimal(productWidth) / 10;
                            dt.Rows[i]["productHeight"] = Convert.ToDecimal(productHeight) / 10;
                        }
                        else 
                        {
                            dt.Rows[i]["needDate"] = "2000-01-01";//需求日期
                            dt.Rows[i]["productLength"] = 0;
                            dt.Rows[i]["productWidth"] = 0;
                            dt.Rows[i]["productHeight"] = 0;
                        }
                    }
                    dt.TableName = "CustOrderTable";
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception) 
            {
                return null;
            }
        }

        /// <summary>
        /// 获取网店管家省市区字典信息
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public DataSet wdgj_AddressMap() 
        {
            DataSet ds = new DataSet();
            //省
            string sqlCmd = "select * from G_Cfg_SndMap_Province";
            DataTable ProvDt = new DataTable();
            SqlSel.GetSqlSel(ref ProvDt, sqlCmd);
            ProvDt.TableName = "Province";
            //市
            sqlCmd = "select * from G_Cfg_SndMap_City";
            DataTable CityDt = new DataTable();
            SqlSel.GetSqlSel(ref CityDt, sqlCmd);
            CityDt.TableName = "City";
            //区
            sqlCmd = "select * from G_Cfg_SndMap_Area";
            DataTable AreaDt = new DataTable();
            SqlSel.GetSqlSel(ref AreaDt, sqlCmd);
            AreaDt.TableName = "Area";

            ds.Tables.Add(ProvDt);
            ds.Tables.Add(CityDt);
            ds.Tables.Add(AreaDt);

            ds.DataSetName = "AddressMap";
            return ds;
        }

        /// <summary>
        /// 返回网店管家店铺信息
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string wdgj_shopList() 
        {
            string sqlCmd = "select shopName,shopId from g_cfg_shopList";
            DataTable dt = new DataTable();
            if (SqlSel.GetSqlSel(ref dt, sqlCmd))
            {
                return JsonConvert.SerializeObject(dt);
            }
            else 
            {
                return null;
            }
        }

        /// <summary>
        /// 获取店铺日期内订单信息
        /// </summary>
        /// <param name="ShopId">店铺id</param>
        /// <param name="tradeDate">订单交易日期</param>
        /// <returns></returns>
        [WebMethod]
        public string wdgj_TradeList(string ShopId, string tradeDate) 
        {
            //获取订单信息
            string shortDate = Convert.ToDateTime(tradeDate).ToShortDateString();
            string sqlCmd = "select TradeId,TradeTime,TradeNo,SndTo,NickName,t1.Tel,t1.Adr,t1.GoodsTotal,CustomerRemark,t1.PostId,";
            sqlCmd += "TradeFrom,TradeStatus,t3.Name AS LogisticName from g_trade_tradeList t1 left join g_customer_customerlist t2 ";
            sqlCmd += " on t1.customerId=t2.customerId left join G_Cfg_LogisticList t3 on t1.LogisticID=t3.LogisticID";
            sqlCmd += " where t1.shopId='" + ShopId + "' and Convert(nvarchar(50),tradeTime,23)='" + tradeDate + "'";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            dt.Columns.Add("NeedDate");
            dt.Columns.Add("Items");
            //索菲亚订单提取需求日期（交期）
            if (ShopId == "1029")
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string CustomerRemark = dt.Rows[i]["CustomerRemark"].ToString();
                    string sndTo = dt.Rows[i]["SndTo"].ToString().Replace(" ", "");//去除空格
                    if (dt.Rows[i]["TradeFrom"].ToString() == "蜘蛛")
                    {
                        dt.Rows[i]["NeedDate"] = CustomerRemark.Substring(CustomerRemark.LastIndexOf("：") + 1);//需求日期
                    }
                    if (sndTo.Contains("规格"))
                    {
                        dt.Rows[i]["SndTo"] = sndTo.Substring(0, sndTo.IndexOf("规"));//联系人去除部分定制规格信息
                    }
                    else
                    {
                        dt.Rows[i]["SndTo"] = sndTo;
                    }
                }
            }
            //添加商品明细
            string curTradeId = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                curTradeId = dt.Rows[i]["TradeId"].ToString();
                dt.Rows[i]["Items"] = wdgj_TradeItems(curTradeId);
            }

            string tradeInfo = string.Empty;
            tradeInfo = JsonConvert.SerializeObject(dt);
            return tradeInfo;
            //dt.TableName = "tradeInfo";
            //return dt;
            
        }

        /// <summary>
        /// 根据订单id获取商品明细
        /// </summary>
        /// <param name="tradeId"></param>
        /// <returns></returns>
        [WebMethod]
        public string wdgj_TradeItems(string tradeId) 
        {
            string sqlCmd = "select GoodsNO,SellCount from g_trade_goodslist t1 left join G_Goods_GoodsList t2";
            sqlCmd += " on t1.goodsId=t2.GoodsId where t1.tradeId='" + tradeId + "'";
            DataTable dt = new DataTable();
            System.Text.StringBuilder itemStr = new System.Text.StringBuilder();
            if (SqlSel.GetSqlSel(ref dt, sqlCmd)) 
            {
                for (int i = 0; i < dt.Rows.Count; i++) 
                {
                    itemStr.AppendFormat("{0}:{1}|", dt.Rows[i]["GoodsNO"].ToString(), dt.Rows[i]["SellCount"].ToString());
                }
            }

            return itemStr.ToString().Substring(0, itemStr.ToString().Length - 1);
        }
    }
}
