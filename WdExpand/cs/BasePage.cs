using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FineUI;

namespace WdExpand
{
    public class BasePage : System.Web.UI.Page
    {
        public BasePage()
        {
        }

        protected string GetUser() 
        {
            if (Session["loginUser"] != null)
            {
                return Session["loginUser"].ToString();
            }
            else
            {
                PageContext.Redirect("~/wdexpand/default.aspx","_top");
                return null;
            }
        }
    }
}