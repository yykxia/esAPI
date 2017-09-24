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
    public partial class sfy_orderMgmt : System.Web.UI.Page
    {
        static private int lastQueryAction = 0;//条件查询动作
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                DatePicker1.SelectedDate = DateTime.Now;

                bindGrid(DatePicker1.Text, "%", Grid1.PageIndex, Grid1.PageSize);
            }
        }

        protected void tgb_PO_TriggerClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(tgb_PO.Text))
                {
                    //string sqlCmd = "select * from sfyordertab where cust_po_num='" + tgb_PO.Text + "'";
                    DataTable dt = bindGrid("%", tgb_PO.Text, 0, Grid1.PageSize);
                    Grid1.DataSource = dt;
                    Grid1.DataBind();

                    lastQueryAction = 2;//按PO号查询
                }
                else
                {

                }
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(ex.Message);
            }
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                string sqlCmd = "";
                // 修改的现有数据
                Dictionary<int, Dictionary<string, object>> modifiedDict = Grid1.GetModifiedDict();
                foreach (int rowIndex in modifiedDict.Keys)
                {
                    int rowID = Convert.ToInt32(Grid1.DataKeys[rowIndex][0]);
                    sqlCmd = "update [SFYOrderTab] set CONTACT_NAME='" + Grid1.Rows[rowIndex].Values[4].ToString() + "',";
                    sqlCmd += "PHONE_NUMBER='" + Grid1.Rows[rowIndex].Values[5].ToString() + "',ADDRESS='" + Grid1.Rows[rowIndex].Values[6].ToString() + "'";
                    sqlCmd += " where [SOId]=" + rowID;
                    SqlSel_Pro.ExeSql(sqlCmd);
                }
                Alert.Show("已保存");
                //刷新结果集
                //tgb_PO_TriggerClick(sender, e);
                reBound();

            }
            catch (Exception ex) 
            {
                Alert.Show(ex.ToString());
                return;
            }
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            string queryDate = DatePicker1.Text;//查询日期
            DataTable dt = bindGrid(queryDate, "%", 0, Grid1.PageSize);
            Grid1.DataSource = dt;
            Grid1.DataBind();

            lastQueryAction = 1;//按日期查询

        }

        private DataTable bindGrid(string dateStr, string poNumb, int pageIndex, int pageSize)
        {
            //数据集行数
            string sqlCmd = "select isnull(count(*),0) as rowsCount from sfyordertab where cust_po_num like '" + poNumb + "'";
            sqlCmd += " and convert(nvarchar,createdate,23) like '" + dateStr + "'";
            int rowCount = Convert.ToInt32(SqlSel_Pro.GetSqlScale(sqlCmd));
            Grid1.RecordCount = rowCount;

            DataTable dt = new DataTable();
            if (pageIndex == 0) 
            {
                sqlCmd = " select top "+ pageSize + " * from sfyordertab where cust_po_num like '" + poNumb + "'";
                sqlCmd += " and convert(nvarchar,createdate,23) like '" + dateStr + "' order by soid";
            }
            else
            {
                sqlCmd = "select top " + pageSize + " * from sfyordertab where soid>(";
                sqlCmd += " select max(soid) from ";
                sqlCmd += " (select top (" + pageIndex + "*" + pageSize + ") soid from sfyordertab where cust_po_num like '" + poNumb + "'";
                sqlCmd += " and convert(nvarchar,createdate,23) like '" + dateStr + "' order by soid) t)";
                sqlCmd += " and cust_po_num like '" + poNumb + "'";
                sqlCmd += " and convert(nvarchar,createdate,23) like '" + dateStr + "' order by soid";
            }
            if (SqlSel_Pro.GetSqlSel(ref dt, sqlCmd))
            {
                string tradeNo = string.Empty;
                DataTable apiDt = new DataTable();
                dt.Columns.Add("isAPI", typeof(string));
                dt.Columns.Add("APIStatus", typeof(bool));
                string str = "";
                foreach (DataRow dr in dt.Rows)
                {
                    tradeNo = dr["OrderId"].ToString();
                    str += " tradeNo='" + tradeNo + "' or";
                }

                sqlCmd = "select tradeNo from g_api_tradelist where " + str.Substring(0, str.Length - 2);

                if (!SqlSel.GetSqlSel(ref apiDt, sqlCmd))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["isNew"].ToString() == "1")
                        {
                            dr["isAPI"] = "异常";
                            dr["APIStatus"] = true;
                        }
                        else
                        {
                            dr["isAPI"] = "待同步";
                            dr["APIStatus"] = false;
                            continue;
                        }
                    }
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        tradeNo = dr["OrderId"].ToString();
                        if (dr["isNew"].ToString() == "1")
                        {
                            DataRow[] drArr = apiDt.Select("tradeNo='" + tradeNo + "'");
                            if (drArr.Length > 0)
                            {
                                dr["isAPI"] = "";
                                dr["APIStatus"] = false;
                            }
                            else
                            {
                                dr["isAPI"] = "异常";
                                dr["APIStatus"] = true;
                            }
                        }
                        else
                        {
                            dr["isAPI"] = "待同步";
                            dr["APIStatus"] = false;
                            continue;
                        }
                    }
                }
                return dt;
            }
            else
            {
                return null;
            }
        }

        protected void btn_rePush_Click(object sender, EventArgs e)
        {
            try
            {
                //是否存在非异常单
                CheckBoxField field1 = (CheckBoxField)Grid1.FindColumn("CheckBoxField1");
                string sqlCmd = string.Empty;
                foreach (GridRow row in Grid1.Rows) 
                {
                    int rowIndex = row.RowIndex;
                    bool isSel = field1.GetCheckedState(rowIndex);
                    if (isSel) 
                    {
                        string apiState = row.Values[7].ToString();//API状态
                        int rowID = Convert.ToInt32(Grid1.DataKeys[rowIndex][0]);
                        if (apiState != "异常")
                        {
                            continue;
                        }
                        else
                        {
                            sqlCmd = "update sfyordertab set isnew='0' where SOId='" + rowID + "'";
                            SqlSel_Pro.ExeSql(sqlCmd);
                        }
                    }
                }
                Alert.Show("操作成功！");
                reBound();


            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(ex.Message);
            }
        }

        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            reBound();
        }

        private void reBound()
        {
            DataTable dt = new DataTable();
            if (lastQueryAction == 1 || lastQueryAction == 0)
            {
                dt = bindGrid(DatePicker1.Text, "%", Grid1.PageIndex, Grid1.PageSize);
            }
            else if (lastQueryAction == 2)
            {
                dt = bindGrid("%", tgb_PO.Text, Grid1.PageIndex, Grid1.PageSize);
            }
            Grid1.DataSource = dt;
            Grid1.DataBind();
        }

        

    }
}