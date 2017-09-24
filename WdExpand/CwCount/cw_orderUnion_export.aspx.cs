using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using FineUI;
using System.Data;
using IETCsoft.sql;
using System.Text;
using AspNet = System.Web.UI.WebControls;

namespace WdExpand.CwCount
{
    public partial class cw_orderUnion_export : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                loadShopList();
            }
        }


        //加载店铺信息
        protected void loadShopList()
        {
            DataTable dt = new DataTable();
            string sqlCmd = "select * from g_cfg_shoplist";
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            ddl_shopList.DataTextField = "shopName";
            ddl_shopList.DataValueField = "shopID";
            ddl_shopList.DataSource = dt;
            ddl_shopList.DataBind();
        }

        protected void btn_union_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                string dt1 = DatePicker1.Text;
                string dt2 = DatePicker2.Text;
                string sqlStr = "select * from ";
                sqlStr += "(select a.tradeno_orig,a.nickname_orig,a.moneyfrom_orig,a.paySum_orig,a.orderStatus_orig,a.customerDesc_orig,a.saleDesc_orig, ";
                sqlStr += "a.orderStartTime_orig,a.goodsName_orig,a.goodsCount_orig,a.tradeDesc_orig,b.custNickName, ";
                sqlStr += "b.tradeNo,b.cashGo,b.goodsCount,b.goodsName,b.payCount,b.payDate,(select tradename from t_trade_tradetype where paytype=t_trade_tradetype.id)as payType, ";
                sqlStr += "b.addDesc,b.customerName from t_trade_specreg b ";
                sqlStr += "left join t_trade_orderreg_orig a on tradeno=tradeno_orig ";
                sqlStr += "where shopid_orig='" + ddl_shopList.SelectedValue + "' and paydate>='" + dt1 + "' and paydate<='" + dt2 + "') aa ";
                sqlStr += "left join ";
                sqlStr += "(select tradeno2,tradestatusext,remark,tradeid,goodslist,(convert(float,rcvtotal)) as rcvtotal from v_TradeList t ";
                sqlStr += "where shopid='" + ddl_shopList.SelectedValue + "' and tradestatus>0 and tradeno2 <> '' ";
                sqlStr += "and t.tradeid=(select min(tradeid) from v_tradelist where tradeno2=t.tradeno2) ";
                sqlStr += ") bb on bb.tradeno2=aa.tradeno_orig order by paydate";

                SqlSel.GetSqlSel(ref dt, sqlStr);
                Grid1.DataSource = dt;
                Grid1.DataBind();
            }
            catch (Exception ex)
            {
                Alert.Show(ex.ToString());
                return;
            }
        }

        protected void btn_saveAsExcel_Click(object sender, EventArgs e)
        {
            try
            {
                Response.ClearContent();
                string fileName = string.Format("{0}-{1}", DateTime.Now.Ticks.ToString(), ddl_shopList.SelectedText);
                Response.AddHeader("content-disposition", "attachment; filename=" + fileName + ".xls");
                Response.ContentType = "application/excel";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(GetGridTableHtml(Grid1));
                Response.End();
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(ex.ToString());
            }
        }

        private string GetGridTableHtml(Grid grid)
        {
            StringBuilder sb = new StringBuilder();

            MultiHeaderTable mht = new MultiHeaderTable();
            mht.ResolveMultiHeaderTable(Grid1.Columns);


            sb.Append("<meta http-equiv=\"content-type\" content=\"application/excel; charset=UTF-8\"/>");


            sb.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");

            foreach (List<object[]> rows in mht.MultiTable)
            {
                sb.Append("<tr>");
                foreach (object[] cell in rows)
                {
                    int rowspan = Convert.ToInt32(cell[0]);
                    int colspan = Convert.ToInt32(cell[1]);
                    GridColumn column = cell[2] as GridColumn;

                    sb.AppendFormat("<th{0}{1}{2}>{3}</th>",
                        rowspan != 1 ? " rowspan=\"" + rowspan + "\"" : "",
                        colspan != 1 ? " colspan=\"" + colspan + "\"" : "",
                        colspan != 1 ? " style=\"text-align:center;\"" : "",
                        column.HeaderText);
                }
                sb.Append("</tr>");
            }


            foreach (GridRow row in grid.Rows)
            {
                sb.Append("<tr>");

                foreach (GridColumn column in mht.Columns)
                {
                    string html = row.Values[column.ColumnIndex].ToString();

                    if (column.ColumnID == "tfNumber")
                    {
                        html = (row.FindControl("spanNumber") as System.Web.UI.HtmlControls.HtmlGenericControl).InnerText;
                    }
                    else if (column.ColumnID == "tfGender")
                    {
                        html = (row.FindControl("labGender") as AspNet.Label).Text;
                    }


                    sb.AppendFormat("<td>{0}</td>", html);
                }

                sb.Append("</tr>");
            }

            sb.Append("</table>");

            return sb.ToString();
        }


        #region 多表头处理

        /// <summary>
        /// 处理多表头的类
        /// </summary>
        public class MultiHeaderTable
        {
            // 包含 rowspan，colspan 的多表头，方便生成 HTML 的 table 标签
            public List<List<object[]>> MultiTable = new List<List<object[]>>();
            // 最终渲染的列数组
            public List<GridColumn> Columns = new List<GridColumn>();


            public void ResolveMultiHeaderTable(GridColumnCollection columns)
            {
                List<object[]> row = new List<object[]>();
                foreach (GridColumn column in columns)
                {
                    object[] cell = new object[4];
                    cell[0] = 1;    // rowspan
                    cell[1] = 1;    // colspan
                    cell[2] = column;
                    cell[3] = null;

                    row.Add(cell);
                }

                ResolveMultiTable(row, 0);

                ResolveColumns(row);
            }

            private void ResolveColumns(List<object[]> row)
            {
                foreach (object[] cell in row)
                {
                    GroupField groupField = cell[2] as GroupField;
                    if (groupField != null && groupField.Columns.Count > 0)
                    {
                        List<object[]> subrow = new List<object[]>();
                        foreach (GridColumn column in groupField.Columns)
                        {
                            subrow.Add(new object[]
                            {
                                1,
                                1,
                                column,
                                groupField
                            });
                        }

                        ResolveColumns(subrow);
                    }
                    else
                    {
                        Columns.Add(cell[2] as GridColumn);
                    }
                }

            }

            private void ResolveMultiTable(List<object[]> row, int level)
            {
                List<object[]> nextrow = new List<object[]>();

                foreach (object[] cell in row)
                {
                    GroupField groupField = cell[2] as GroupField;
                    if (groupField != null && groupField.Columns.Count > 0)
                    {
                        // 如果当前列包含子列，则更改当前列的 colspan，以及增加父列（向上递归）的colspan
                        cell[1] = Convert.ToInt32(groupField.Columns.Count);
                        PlusColspan(level - 1, cell[3] as GridColumn,groupField.Columns.Count - 1);

                        foreach (GridColumn column in groupField.Columns)
                        {
                            nextrow.Add(new object[]
                            {
                                1,
                                1,
                                column,
                                groupField
                            });
                        }
                    }
                }

                MultiTable.Add(row);

                // 如果当前下一行，则增加上一行（向上递归）中没有子列的列的 rowspan
                if (nextrow.Count > 0)
                {
                    PlusRowspan(level);

                    ResolveMultiTable(nextrow, level + 1);
                }
            }

            private void PlusRowspan(int level)
            {
                if (level < 0)
                {
                    return;
                }

                foreach (object[] cells in MultiTable[level])
                {
                    GroupField groupField = cells[2] as GroupField;
                    if (groupField != null && groupField.Columns.Count > 0)
                    {
                        // ...
                    }
                    else
                    {
                        cells[0] = Convert.ToInt32(cells[0]) + 1;
                    }
                }

                PlusRowspan(level - 1);
            }

            private void PlusColspan(int level, GridColumn parent, int plusCount)
            {
                if (level < 0)
                {
                    return;
                }

                foreach (object[] cells in MultiTable[level])
                {
                    GridColumn column = cells[2] as GridColumn;
                    if (column == parent)
                    {
                        cells[1] = Convert.ToInt32(cells[1]) + plusCount;

                        PlusColspan(level - 1, cells[3] as GridColumn, plusCount);
                    }
                }
            }

        }
        #endregion



    }
}