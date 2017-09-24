using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using IETCsoft.sql;
using System.Data;
using FineUI;

namespace WdExpand
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = tbxUserName.Text.Trim();
            string password = tbxPassword.Text.Trim();

            string psw = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "md5");

            //
            DataTable dt = new DataTable();
            string sqlCmd = "select * from T_UserList where userId='" + userName + "' and userPsw='" + psw + "'";
            SqlSel_Pro.GetSqlSel(ref dt, sqlCmd);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["enabled"].ToString() == "0")
                {
                    Alert.ShowInTop("用户未启用，请联系管理员！");
                    return;
                }
                Session["loginUser"] = userName;
                Response.Redirect("main.aspx");
            }
            else 
            {
                Alert.ShowInTop("用户名或密码错误！");
                return;
            }
        }
    }
}