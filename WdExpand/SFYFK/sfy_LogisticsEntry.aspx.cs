using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using FineUI;
using IETCsoft.sql;

namespace WdExpand.SFYFK
{
    public partial class sfy_LogisticsEntry : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                string UserId = GetUser();
            }
        }

        private void bindGrid() 
        {
            if (string.IsNullOrEmpty(trgb_logNo.Text) || trgb_logNo.Text.Length < 8)
            {
                Alert.ShowInTop("查询单号不合法！");
                return;
            }
            else
            {
                string sqlCmd = "SELECT t2.NickName,t1.sndto,t1.tel,t1.adr,t1.postId FROM G_trade_tradeList t1 ";
                sqlCmd += " left join g_customer_customerlist t2 on t2.CustomerID=t1.CustomerID";
                sqlCmd += " where t1.shopid='1029' and postId like '%" + trgb_logNo.Text + "%'";
                DataTable dt = new DataTable();
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                Grid1.DataSource = dt;
                Grid1.DataBind();
            }
        }

        protected void trgb_logNo_TriggerClick(object sender, EventArgs e)
        {
            bindGrid();
        }
    }
}