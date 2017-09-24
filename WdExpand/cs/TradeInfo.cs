using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WdExpand.cs
{


    public class TradeInfo
    {
        public TradeDtl[] TradeDtl { get; set; }
    }

    public class TradeDtl
    {
        public string TradeId { get; set; }//订单Id
        public string TradeNo { get; set; }//订单号
        public string TradeTime { get; set; }//交易时间
        public string Contact { get; set; }//联系人
        public string NickName { get; set; }//客户昵称(PO号)
        public string Tel { get; set; }//联系电话
        public string Adr { get; set; }//地址信息
        public string GoodsTotal { get; set; }//合计金额
        public string CustomerRemark { get; set; }//备注
        public string PostId { get; set; }//物流单号
        public string TradeFrom { get; set; }//订单来源
        public string TradeStatus { get; set; }//订单状态
        public string NeedDate { get; set; }//需求日期(交期)
        public string Items { get; set; }//商品明细
        public string LogisticName { get; set; }//物流名称
    }

}