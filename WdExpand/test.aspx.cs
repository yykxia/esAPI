using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IETCsoft.sql;

namespace WdExpand
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn1_Click(object sender, EventArgs e)
        {
            string sqlCmdStr = "select parentid from SFYOrderTab where OrderId='" + txb1.Text + "'";
            string orgId = SqlSel_Pro.GetSqlScale(sqlCmdStr).ToString();
            TextBox1.Text = orgId;
        }
    }
}