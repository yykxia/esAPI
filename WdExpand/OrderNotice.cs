using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WdExpand
{
    public class OrderNotice
    {
        public string action { get; set; }
        public string method { get; set; }
        public Param param { get; set; }
    }

    public class Param
    {
        public string uCode { get; set; }
        public string operateUserId { get; set; }
        public string informSn { get; set; }
        public string deliveryTime { get; set; }
        public string operateUser { get; set; }
        public string waybillSn { get; set; }
        public string serviceName { get; set; }
    }
}
