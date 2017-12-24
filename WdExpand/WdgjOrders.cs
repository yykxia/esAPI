using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WdExpand
{
    public class WdgjOrders
    {
        public string ShopId { get; set; }//店铺id,(索菲亚:1029;酒店:1038)
        public DateTime GetDate { get; set; }//抓取日期
        public List<OrderInfo> Orders { get; set; }//派送单（订单）列表
    }

    public class OrderInfo
    {
        public string OrderId { get; set; }//订单流水id
        public DateTime CreateTime { get; set; }//生成时间
        public string OrderNo { get; set; }//订单号
        public string CustomerNo { get; set; }//客户编号(索菲亚是经销商编号)
        public DateTime TradeTime { get; set; }//下单时间
        public string Adr { get; set; }//送货地址
        public string ContactName { get; set; }//联系人姓名
        public string ContactPhone { get; set; }//联系电话
        public DateTime NeedDate { get; set; }//交期
        public DateTime SendTime { get; set; }//送货时间
        public string LogisticName { get; set; }//物流名称
        public string PostId { get; set; }//物流单号
        public string WarehouseId { get; set; }//送货仓库
        public List<Goods> GoodsList { get; set; }//货品明细
    }


    public class Goods
    {
        public string GoodsNo { get; set; }//产品编码
        public double SellPrice { get; set; }//销售单价
        public int SellCount { get; set; }//销售数量
        public string Descriptions { get; set; }//PO号、规格等信息
        public bool IsOrder { get; set; }//是否定制
        public bool IsGift { get; set; }//是否赠品
    }

    public class SellBack_List
    {
        public int Counts { get; set; }
        public List<SellBack> List { get; set; }
    }

    public class SellBack
    {
        public int TradeId { get; set; }//退货单id
        public string TradeNo { get; set; }//退货单号
        public string ShopName { get; set; }//退货店铺
        public string CustomerName { get; set; }//客户
        public string Address { get; set; }//地址
        public int RegType { get; set; }//登记类型（退、换货）
        public List<SellBack_Goods> BackGoods { get; set; }//退货明细
        public List<SellBack_Goods> SendGoods { get; set; }//换货明细
    }

    public class SellBack_Goods
    {
        public string DealWith { get; set; }
        public string GoodsName { get; set; }
        public int Count { get; set; }
        public int RegSum { get; set; }//已登记数量
        public int RemainSum { get; set; }//收货数量
        public int TradeId { get; set; }//退货单号
        public int RecId { get; set; }//退货明细id
        public string GoodsNo { get; set; }
    }
    /// <summary>
    /// 入库登记信息
    /// </summary>
    public class SellBack_GoodsReg
    {
        public string GoodsName { get; set; }
        public int RemainSum { get; set; }
        public string DealWith { get; set; }
        public int TradeId { get; set; }//退货单号
        public int RecId { get; set; }//退货明细id
        public string GoodsNo { get; set; }
    }
}