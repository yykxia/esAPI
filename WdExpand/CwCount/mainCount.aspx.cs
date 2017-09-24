using System;
using System.Collections.Generic;
using FineUI;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IETCsoft.sql;
using System.Data;
using System.Text;
using System.IO;
using AspNet = System.Web.UI.WebControls;

namespace WdExpand
{
    public partial class mainCount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_confirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Str_Dtep.SelectedDate.ToString()) || string.IsNullOrEmpty(End_Dtep.SelectedDate.ToString()))
            {
                Alert.ShowInTop("日期不可为空！");
                return;
            }
            else
            {
                String dt1 = Str_Dtep.Text;
                String dt2 = End_Dtep.Text;
                DataTable dt = new DataTable();
                String sqlCmd = "select a.TradeNO,a.TradeTime,b.NickName,c.ShopName,";
                sqlCmd += "a.GoodsTotal,a.FavourableTotal,a.AllTotal,a.RcvTotal,a.SndTime,";
                sqlCmd += "case a.TradeStatus ";
                sqlCmd += "when 0 then '被取消' ";
                sqlCmd += "when 1 then '等待单' ";
                sqlCmd += "when 2 then '待审核' ";
                sqlCmd += "when 3 then '预订单' ";
                sqlCmd += "when 4 then '待结算' ";
                sqlCmd += "when 5 then '打单出库' ";
                sqlCmd += "when 6 then '待出库' ";
                sqlCmd += "when 7 then '待发货' ";
                sqlCmd += "when 8 then '发货在途' ";
                sqlCmd += "when 9 then '代销发货' ";
                sqlCmd += "when 10 then '委外发货' ";
                sqlCmd += "when 11 then '已完成' ";
                sqlCmd += "end as TradeStatus ";
                sqlCmd += "from G_Trade_TradeList a left join G_Customer_CustomerList b on ";
                sqlCmd += "a.CustomerID=b.CustomerID ";
                sqlCmd += "left join G_Cfg_ShopList c on c.ShopID=a.ShopID ";
                sqlCmd += "where CONVERT(varchar(100), SndTime, 23)<='" + dt2 + "' ";
                sqlCmd += "and CONVERT(varchar(100), SndTime, 23)>='" + dt1 + "' order by TradeTime";
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                Grid1.DataSource = dt;
                Grid1.DataBind();
            }
        }

        protected void btn_export_Click(object sender, EventArgs e)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=myexcel.xls");
            Response.ContentType = "application/excel";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Write(GetGridTableHtml(Grid1));
            Response.End();
        }

        private string GetGridTableHtml(Grid grid)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<meta http-equiv=\"content-type\" content=\"application/excel; charset=UTF-8\"/>");

            sb.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");

            sb.Append("<tr>");
            foreach (GridColumn column in grid.Columns)
            {
                sb.AppendFormat("<td>{0}</td>", column.HeaderText);
            }
            sb.Append("</tr>");


            foreach (GridRow row in grid.Rows)
            {
                sb.Append("<tr>");
                foreach (object value in row.Values)
                {
                    string html = value.ToString();
                    if (html.StartsWith(Grid.TEMPLATE_PLACEHOLDER_PREFIX))
                    {
                        // 模板列
                        string templateID = html.Substring(Grid.TEMPLATE_PLACEHOLDER_PREFIX.Length);
                        Control templateCtrl = row.FindControl(templateID);
                        html = GetRenderedHtmlSource(templateCtrl);
                    }
                    else
                    {
                        // 处理CheckBox
                        if (html.Contains("f-grid-static-checkbox"))
                        {
                            if (html.Contains("uncheck"))
                            {
                                html = "×";
                            }
                            else
                            {
                                html = "√";
                            }
                        }

                        // 处理图片
                        if (html.Contains("<img"))
                        {
                            string prefix = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "");
                            html = html.Replace("src=\"", "src=\"" + prefix);
                        }
                    }

                    sb.AppendFormat("<td>{0}</td>", html);
                }
                sb.Append("</tr>");
            }

            sb.Append("</table>");

            return sb.ToString();
        }

        /// <summary>
        /// 获取控件渲染后的HTML源代码
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        private string GetRenderedHtmlSource(Control ctrl)
        {
            if (ctrl != null)
            {
                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                    {
                        ctrl.RenderControl(htw);

                        return sw.ToString();
                    }
                }
            }
            return String.Empty;
        } 
    }
}