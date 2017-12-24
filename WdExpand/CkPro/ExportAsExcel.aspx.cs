using System;
using IETCsoft.sql;
using System.Data;
using System.Text;
using System.IO;
using FineUI;
using System.Web.UI;
using Newtonsoft.Json.Linq;

namespace WdExpand.CkPro
{
    public partial class ExportAsExcel : System.Web.UI.Page
    {
        private static string StartDate = string.Empty;
        private static string EndDate = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                StartDate = Request["startDate"];
                EndDate = Request["endDate"];
                string sqlCmd = "select t1.recId,t3.GoodsNo,t3.GoodsName,t1.regCount,t1.dealwith,t1.regTime,t5.shopName," +
                                "t4.sndto,t4.adr,t4.rcvpostId from ES_WH_SellBackRegister t1 " +
                                "left join G_SellBack_GoodsList t2 on t1.recId=t2.recId " +
                                "left join g_goods_goodsList t3 on cast(t2.goodsid as int)= t3.goodsid " +
                                "left join G_SellBack_List t4 on t2.tradeId=t4.tradeId " +
                                "left join g_cfg_shoplist t5 on t4.shopId = t5.shopId " +
                                "where t1.regTime between '" + StartDate + "' and '" + EndDate + "' order by t1.regTime";
                DataTable dt = new DataTable();
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                Grid1.DataSource = dt;
                Grid1.DataBind();
                
            }
        }

        protected void btn_confirm_Click(object sender, EventArgs e)
        {
            if (Grid1.Rows.Count > 0)
            {
                string fileName = DateTime.Now.Ticks.ToString();
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=" + fileName + ".xls");
                Response.ContentType = "application/excel";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(GetGridTableHtml(Grid1, "电商仓库" + StartDate + "至" + EndDate + "退换货收货明细"));
                Response.End();
            }
            else
            {
                Alert.ShowInTop("无数据可操作！");
            }

        }

        #region Events
        private string GetGridTableHtml(Grid grid,string Title)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<meta http-equiv=\"content-type\" content=\"application/excel; charset=UTF-8\"/>");

            sb.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");
            //加表头
            sb.Append("<caption align=\"top\">");
            sb.AppendFormat(Title);
            sb.Append("</caption>");
            //sb.Append("<thead><tr>");
            //sb.AppendFormat("<td>{0}</td>", ddl_line.SelectedText);
            //sb.Append("</tr></thead>");
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
            //合计行导出
            sb.Append("<tr>");
            JObject summarty = grid.SummaryData;//获取合计行数据
            if (summarty != null && summarty.ToString() != "")//判断合计行数据是否为空
            {
                foreach (GridColumn column in grid.Columns)//遍历出列的id
                {
                    if (summarty.Property(column.ColumnID.ToString()) == null || summarty.Property(column.ColumnID.ToString()).ToString() == "")//判断合计行Json是否存在该节点
                    {
                        sb.AppendFormat("<td>{0}</td>", "");//如果没有就为空
                    }
                    else
                    {
                        sb.AppendFormat("<td>{0}</td>", summarty[column.ColumnID.ToString()].ToString());//如果有就写入数据
                    }
                }
            }
            sb.Append("</tr>");

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

        #endregion

    }
}