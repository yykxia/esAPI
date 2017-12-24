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
            //获取已发货送货单信息
            string shortDate = Convert.ToDateTime(tradeDate).ToShortDateString();
            string sqlCmd = "select TradeId,TradeTime,TradeNo,SndTo,NickName,t1.Tel,t1.Adr,t1.GoodsTotal,CustomerRemark,t1.warehouseId,";
            sqlCmd += "t1.PostId,t1.SndTime,t1.AllTotal,t1.Postage,TradeFrom,TradeStatus,t3.Name AS LogisticName,tradeNo2,t1.Remark";
            sqlCmd += " from g_trade_tradeList t1 left join g_customer_customerlist t2 ";
            sqlCmd += " on t1.customerId=t2.customerId left join G_Cfg_LogisticList t3 on t1.LogisticID=t3.LogisticID";
            sqlCmd += " where t1.shopId='" + ShopId + "' and Convert(nvarchar(50),sndTime,23)='" + tradeDate + "' and tradeStatus='11'";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dt.Rows[i];
                //精简联系人信息
                if (dr["SndTo"].ToString().Contains(" ") & dr["SndTo"].ToString().Contains("*"))
                {
                    string sndTo = dr["SndTo"].ToString();
                    dr["SndTo"] = sndTo.Substring(0, sndTo.IndexOf(" "));
                }
                if (ShopId == "1029")
                {
                    //排除非新业务店铺的这两个仓库的订单
                    if (dr["warehouseId"].ToString() != "1027" && dr["warehouseId"].ToString() != "1039")
                    {
                        dt.Rows.Remove(dr);
                    }
                }
            }
            //foreach (DataRow dr in dt.Rows)
            //{
            //    if (ShopId == "1029")
            //    {
            //        //排除非新业务店铺的这两个仓库的订单
            //        if (dr["warehouseId"].ToString() != "1027" && dr["warehouseId"].ToString() != "1039")
            //        {
            //            dt.Rows.Remove(dr);
            //        }
            //    }
            //}
            WdgjOrders WdgjOrder = new WdgjOrders
            {
                ShopId = ShopId,
                GetDate = Convert.ToDateTime(tradeDate)
            };
            List<OrderInfo> orders = new List<OrderInfo>();
            foreach (DataRow dr in dt.Rows)
            {
                string tradeId = dr["TradeId"].ToString();
                string origOrderNo = dr["tradeNo2"].ToString();
                OrderInfo orderInfo = new OrderInfo
                {
                    OrderId = tradeId,
                    TradeTime = Convert.ToDateTime(dr["TradeTime"]),
                    OrderNo = dr["TradeNo"].ToString(),
                    CustomerNo = dr["NickName"].ToString(),
                    Adr = dr["Adr"].ToString(),
                    ContactName = dr["SndTo"].ToString(),
                    ContactPhone = dr["Tel"].ToString(),
                    SendTime = Convert.ToDateTime(dr["SndTime"]),
                    LogisticName = dr["LogisticName"].ToString(),
                    PostId = dr["PostId"].ToString(),
                    NeedDate = Convert.ToDateTime(dr["SndTime"]),
                    WarehouseId = dr["warehouseId"].ToString()
                };
                //新业务部订单
                if (ShopId == "1029")
                {
                    //非手工单获取大客户编码
                    if (dr["TradeFrom"].ToString() == "蜘蛛")
                    {
                        DataTable tempDt = new DataTable();
                        sqlCmd = "select * from view_sfyorders where cust_po_num='" + dr["NickName"].ToString() + "'";
                        if (SqlSel_Pro.GetSqlSel(ref tempDt, sqlCmd))
                        {
                            orderInfo.CustomerNo = tempDt.Rows[0]["ouNO"].ToString();
                            orderInfo.ContactName = dr["SndTo"].ToString() + "/" + dr["NickName"].ToString();//客户姓名+PO号
                        }
                        else
                        {
                            orderInfo.CustomerNo = null;
                        }
                    }
                }
                
                //添加商品明细
                List<Goods> goodsList = new List<Goods>();
                DataTable goodsTable = new DataTable();
                sqlCmd = "select a.*,b.goodsNo from g_trade_goodsList a left join G_Goods_GoodsList b"
                        + " on a.goodsid = b.goodsid where tradeId = '" + tradeId + "'";
                SqlSel.GetSqlSel(ref goodsTable, sqlCmd);
                foreach (DataRow item in goodsTable.Rows)
                {
                    string goodsNo = item["goodsNo"].ToString();
                    Goods goods = new Goods
                    {
                        GoodsNo = goodsNo,
                        Descriptions = item["TradeSpec"].ToString(),//定制规格
                        SellCount = Convert.ToInt32(item["SellCount"]),
                        SellPrice = Convert.ToDouble(item["SellPrice"]),
                        IsOrder = string.IsNullOrEmpty(item["TradeSpec"].ToString()) ? false : true,
                        IsGift = Convert.ToDouble(item["SellPrice"]) > 0 ? false : true
                    };
                    if (ShopId == "1029" & dr["TradeFrom"].ToString() == "蜘蛛")
                    {
                        if (goods.Descriptions.Contains("*"))
                        {
                            string sizeInfo = goods.Descriptions;
                            string productLength = "", productWidth = "", productHeight = "";
                            productLength = sizeInfo.Substring(0, sizeInfo.IndexOf("*"));//长
                            productWidth = sizeInfo.Substring(sizeInfo.IndexOf("*") + 1, sizeInfo.LastIndexOf("*") - sizeInfo.IndexOf("*") - 1);//宽
                            productHeight = sizeInfo.Substring(sizeInfo.LastIndexOf("*") + 1);//厚
                            decimal lastLenth = Convert.ToDecimal(productLength) / 10;//以厘米为单位
                            decimal lastWidth = Convert.ToDecimal(productWidth) / 10;
                            decimal lastHeight = Convert.ToDecimal(productHeight) / 10;

                            goods.Descriptions = lastLenth + "*" + lastWidth + "*" + lastHeight;
                        }
                        //string[] orderArr = origOrderNo.Split(',');
                        //System.Text.StringBuilder builder = new System.Text.StringBuilder();
                        //foreach (string order in orderArr)
                        //{
                        //    builder.AppendFormat("'{0},'", order);
                        //}
                        //string orderListStr = builder.ToString().TrimEnd(',');
                        //sqlCmd = "select * from sfyordertab where orderId in (" + orderListStr + ")";
                        //DataTable sfyOrigOrders = new DataTable();
                        //SqlSel_Pro.GetSqlSel(ref sfyOrigOrders, sqlCmd);
                        //if (dr["Remark"].ToString().Contains("索菲亚定制"))
                        //{
                        //    goods.IsOrder = true;
                        //    goods.IsGift = false;
                        //}
                        //else
                        //{
                        //    goods.IsOrder = false;
                        //    goods.IsGift = false;
                        //}
                        //判断是否赠品
                        goods.IsGift = Convert.ToDouble(item["SellPrice"]) > 0 ? true : false;//接口赠品价格大于0为标识
                    }

                    goodsList.Add(goods);
                }
                orderInfo.GoodsList = goodsList;

                orders.Add(orderInfo);
            }

            WdgjOrder.Orders = orders;

            //dt.Columns.Add("NeedDate");
            //dt.Columns.Add("Items");
            ////索菲亚订单提取需求日期（交期）
            //if (ShopId == "1029")
            //{
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        string CustomerRemark = dt.Rows[i]["CustomerRemark"].ToString();
            //        string sndTo = dt.Rows[i]["SndTo"].ToString().Replace(" ", "");//去除空格
            //        if (dt.Rows[i]["TradeFrom"].ToString() == "蜘蛛")
            //        {
            //            dt.Rows[i]["NeedDate"] = CustomerRemark.Substring(CustomerRemark.LastIndexOf("：") + 1);//需求日期
            //        }
            //        if (sndTo.Contains("规格"))
            //        {
            //            dt.Rows[i]["SndTo"] = sndTo.Substring(0, sndTo.IndexOf("规"));//联系人去除部分定制规格信息
            //        }
            //        else
            //        {
            //            dt.Rows[i]["SndTo"] = sndTo;
            //        }
            //    }
            //}
            ////添加商品明细
            //string curTradeId = "";
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    curTradeId = dt.Rows[i]["TradeId"].ToString();
            //    dt.Rows[i]["Items"] = wdgj_TradeItems(curTradeId);
            //}

            //string tradeInfo = string.Empty;
            //tradeInfo = JsonConvert.SerializeObject(dt);
            //return tradeInfo;
            //dt.TableName = "tradeInfo";
            //return dt;
            return JsonConvert.SerializeObject(WdgjOrder);
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

        /// <summary>
        /// 网店管家退换货信息
        /// </summary>
        /// <param name="PostId"></param>
        /// <returns></returns>
        [WebMethod]
        public string Wdgj_SellBack(string PostId)
        {
            //查询所有退换货为待收货状态的单据
            string sqlCmd = "select t1.TradeNo,t1.tradeId,t2.shopName,t1.sndTo,t1.Adr,t1.tradeId2,t1.tradeId3 from G_SellBack_List t1 " +
                "left join g_cfg_shoplist t2 on t1.shopId=t2.shopId where tradeStatus='0' and RcvPostID like '%" + PostId + "%'";
            DataTable BillDt = new DataTable();
            SellBack_List _List = new SellBack_List();
            if (SqlSel.GetSqlSel(ref BillDt, sqlCmd))
            {
                _List.Counts = BillDt.Rows.Count;

                List<SellBack> BillList = new List<SellBack>();

                foreach (DataRow dr in BillDt.Rows)
                {
                    int BillId = Convert.ToInt32(dr["tradeId"]);
                    SellBack sellBack = new SellBack
                    {
                        TradeId = BillId,
                        TradeNo = dr["TradeNo"].ToString(),
                        CustomerName = dr["sndTo"].ToString(),
                        ShopName = dr["shopName"].ToString(),
                        Address = dr["Adr"].ToString()
                    };
                    //遍历获取退货商品
                    sqlCmd = "select t1.recId,t1.sellCount,t2.GoodsNo,t2.GoodsName,isnull(t3.RegSum,0) as RegSum," +
                        "(cast(t1.sellCount as int)-isnull(t3.RegSum,0)) as remainSum from G_SellBack_GoodsList t1 " +
                        "left join g_goods_goodsList t2 on cast(t1.goodsid as int)=t2.goodsid left join " +
                        "(select sum(RegCount) as RegSum,recId from ES_WH_SellBackRegister where goodsType='1' group by recId) t3" +
                            " on t3.recId=t1.recId where tradeId='" + BillId + "'";
                    DataTable GoodsDt = new DataTable();
                    SqlSel.GetSqlSel(ref GoodsDt, sqlCmd);
                    List<SellBack_Goods> BackGoods = new List<SellBack_Goods>();
                    foreach(DataRow goodsDr in GoodsDt.Rows)
                    {
                        SellBack_Goods _Goods = new SellBack_Goods
                        {
                            TradeId = BillId,
                            DealWith = "入库",
                            RegSum = Convert.ToInt32(goodsDr["RegSum"]),
                            RemainSum = Convert.ToInt32(goodsDr["remainSum"]),
                            RecId = Convert.ToInt32(goodsDr["recId"]),
                            GoodsNo = goodsDr["GoodsNo"].ToString(),
                            GoodsName = goodsDr["GoodsName"].ToString(),
                            Count = Convert.ToInt32(goodsDr["sellCount"])
                        };

                        BackGoods.Add(_Goods);
                    }
                    //有换货明细则增加换货商品信息
                    List<SellBack_Goods> SendGoods = new List<SellBack_Goods>();
                    if (dr["tradeId3"] != null)
                    {
                        //GoodsType=1  退货商品  GoodsType=2  换货商品
                        sqlCmd = "select t1.recId,t1.sellCount,t2.GoodsNo,t2.GoodsName" +
                            " from G_SellBack_GoodsList2 t1 " +
                            "left join g_goods_goodsList t2 on t1.goodsid=t2.goodsid  where tradeId='" + BillId + "'";

                        DataTable SendGoodsDt = new DataTable();
                        SqlSel.GetSqlSel(ref SendGoodsDt, sqlCmd);
                        foreach (DataRow goodsDr in SendGoodsDt.Rows)
                        {
                            SellBack_Goods _Goods = new SellBack_Goods
                            {
                                TradeId = BillId,
                                DealWith = "待发",
                                RecId = Convert.ToInt32(goodsDr["recId"]),
                                GoodsNo = goodsDr["GoodsNo"].ToString(),
                                GoodsName = goodsDr["GoodsName"].ToString(),
                                Count = Convert.ToInt32(goodsDr["sellCount"])
                            };

                            SendGoods.Add(_Goods);
                        }
                    }
                    sellBack.BackGoods = BackGoods;
                    sellBack.SendGoods = SendGoods;
                    BillList.Add(sellBack);
                }
                _List.List = BillList;
            }
            else
            {
                _List.Counts = 0;
            }

            return JsonConvert.SerializeObject(_List);
        }

        /// <summary>
        /// 处理退货收货登记
        /// </summary>
        /// <param name="JsonStr"></param>
        /// <returns></returns>
        [WebMethod]
        public bool Wdgj_SellBack_Confirm(string JsonStr)
        {
            bool result = true;
            List<SellBack_GoodsReg> list = JsonConvert.DeserializeObject<List<SellBack_GoodsReg>>(JsonStr);
            string sqlCmd = string.Empty;
            foreach(SellBack_GoodsReg goods in list)
            {
                if (goods.RemainSum > 0)
                {
                    sqlCmd = "insert into ES_WH_SellBackRegister (Operator,RegTime,BillId,recId,DealWith,GoodsType,RegCount) values" +
                        " ('刘雯雯',getdate(),'" + goods.TradeId + "','" + goods.RecId + "'," +
                        "'" + goods.DealWith + "','1','" + goods.RemainSum + "')";
                    if (SqlSel.ExeSql(sqlCmd) > 0)
                    {
                        continue;
                    }
                    else
                    {
                        result = false;
                        break;
                    }
                }
                else
                {
                    continue;
                }
            }

            return result;
        }
    }
}
