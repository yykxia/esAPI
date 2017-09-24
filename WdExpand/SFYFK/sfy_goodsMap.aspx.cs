using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using FineUI;
using IETCsoft.sql;
using System.IO;
using Newtonsoft.Json.Linq;

namespace WdExpand.SFYFK
{
    public partial class sfy_goodsMap : BasePage
    {
        private bool AppendToEnd = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {


                string curUser = GetUser();

                JObject defaultObj = new JObject();
                defaultObj.Add("id", getMaxId());
                defaultObj.Add("goodsNo", "");
                defaultObj.Add("goodsSpec", "");
                defaultObj.Add("targetNo", "");
                defaultObj.Add("isMinUnitOrNo", false);
                defaultObj.Add("UnitTimes", 0);
                defaultObj.Add("isOrder", false);

                btn_addNew.OnClientClick = Grid1.GetAddNewRecordReference(defaultObj, AppendToEnd);

                bindGrid("ALL");

                
            }
        }
        //模拟数据库id增长列
        protected int getMaxId() 
        {
            int maxId;
            if (string.IsNullOrEmpty(label_hidden.Text))
            {
                maxId = 0;
            }
            else 
            {
                maxId = Convert.ToInt32(label_hidden.Text) + 1;
            }

            label_hidden.Text = maxId.ToString();
            return maxId;

        }

        protected void trgbx_NO_TriggerClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(trgbx_NO.Text))
            {
                Alert.ShowInTop("查询条件不可为空！");
            }
            else 
            {
                bindGrid(trgbx_NO.Text);
            }
        }

        protected void bindGrid(string goodNo)
        {
            try
            {
                if (goodNo == "ALL")
                {
                    goodNo = "%";
                }
                DataTable dt = new DataTable();
                string sqlCmd = "select * from t_MapGoodsList where targetNo like '" + goodNo + "' order by id";
                SqlSel_Pro.GetSqlSel(ref dt, sqlCmd);
                Grid1.DataSource = dt;
                Grid1.DataBind();

                sqlCmd = "select max(id) from t_MapGoodsList";
                string curMaxId = SqlSel_Pro.GetSqlScale(sqlCmd).ToString();
                if (string.IsNullOrEmpty(curMaxId))
                {
                    label_hidden.Text = "0";
                }
                else
                {
                    label_hidden.Text = curMaxId;
                }
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(ex.Message);
                return;
            }
        }

        //新增明细
        //protected void btn_addNew_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DataTable curDt = new DataTable();
        //        curDt = getDataTable();
        //        //新增一行默认数据
        //        DataRow newRow = curDt.NewRow();
        //        newRow[0] = Convert.ToInt32(label_hidden.Text) + 1;
        //        newRow[1] = "";
        //        newRow[2] = "";
        //        newRow[3] = "";
        //        newRow[4] = false;
        //        newRow[5] = 0;
        //        curDt.Rows.Add(newRow);
        //        Grid1.DataSource = curDt;
        //        Grid1.DataBind();
        //        //模拟数据库id列自增
        //        label_hidden.Text = (Convert.ToInt32(label_hidden.Text) + 1).ToString();
        //    }
        //    catch (Exception ex) 
        //    {
        //        Alert.ShowInTop(ex.Message);
        //        return;
        //    }

        //}

        protected DataTable getDataTable() 
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("id", typeof(int));
                dt.Columns.Add("goodsNo", typeof(string));
                dt.Columns.Add("goodsSpec", typeof(string));
                dt.Columns.Add("targetNo", typeof(string));
                dt.Columns.Add("isMinUnitOrNo", typeof(bool));
                dt.Columns.Add("UnitTimes", typeof(int));
                dt.Columns.Add("isOrder", typeof(bool));
                for (int i = 0; i < Grid1.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    GridRow grow = Grid1.Rows[i];
                    dr[0] = Grid1.DataKeys[i][0];
                    dr[1] = grow.Values[0].ToString();
                    dr[2] = grow.Values[1].ToString();
                    dr[3] = grow.Values[2].ToString();
                    dr[4] = grow.Values[3];
                    dr[5] = grow.Values[4];
                    dt.Rows.Add(dr);
                }
                return dt;
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
                return null;
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

                    if (string.IsNullOrEmpty(Grid1.Rows[rowIndex].Values[0].ToString()) || string.IsNullOrEmpty(Grid1.Rows[rowIndex].Values[2].ToString()))
                    {
                        Alert.ShowInTop("商品编码或索菲亚编码不可为空！");
                        return;
                    }
                    if (Convert.ToBoolean(Grid1.Rows[rowIndex].Values[3]) == true & Convert.ToInt32(Grid1.Rows[rowIndex].Values[4]) == 0)
                    {
                        Alert.ShowInTop("不为最小单位的商品请添加计量数量!");
                        return;
                    }
                    int rowID = Convert.ToInt32(Grid1.DataKeys[rowIndex][0]);
                    sqlCmd = "update t_MapGoodsList set goodsNo='" + Grid1.Rows[rowIndex].Values[0].ToString() + "',";
                    sqlCmd += "goodsSpec='" + Grid1.Rows[rowIndex].Values[1].ToString() + "',";
                    sqlCmd += "targetNo='" + Grid1.Rows[rowIndex].Values[2].ToString().Trim() + "',";
                    sqlCmd += "isMinUnitOrNo='" + Convert.ToBoolean(Grid1.Rows[rowIndex].Values[3]) + "',";
                    sqlCmd += "UnitTimes=" + Convert.ToInt32(Grid1.Rows[rowIndex].Values[4]) + ",isOrder='" + Convert.ToBoolean(Grid1.Rows[rowIndex].Values[5]) + "' ";
                    sqlCmd += "where id=" + rowID;
                    SqlSel_Pro.ExeSql(sqlCmd);
                }

                // 新增数据
                List<Dictionary<string, object>> newAddedList = Grid1.GetNewAddedList();
                DataTable table = createDataTable();
                for (int i = 0; i < newAddedList.Count; i++)
                {
                    //DataRow rowData = CreateNewData(table, newAddedList[i]);
                    //table.Rows.Add(rowData);
                    if (string.IsNullOrEmpty(newAddedList[i]["goodsNo"].ToString()) || string.IsNullOrEmpty(newAddedList[i]["targetNo"].ToString()))
                    {
                        Alert.ShowInTop("商品编码或索菲亚编码不可为空！");
                        return;
                    }
                    if (Convert.ToBoolean(newAddedList[i]["isMinUnitOrNo"]) == true & Convert.ToInt32(newAddedList[i]["UnitTimes"]) == 0)
                    {
                        Alert.ShowInTop("不为最小单位的商品请添加计量数量!");
                        return;
                    }
                    sqlCmd = "insert into t_MapGoodsList (goodsNo,goodsSpec,targetNo,isMinUnitOrNo,UnitTimes,isOrder) values ";
                    sqlCmd += "('" + newAddedList[i]["goodsNo"].ToString().Trim() + "','" + newAddedList[i]["goodsSpec"].ToString() + "','" + newAddedList[i]["targetNo"].ToString().Trim() + "',";
                    sqlCmd += "'" + Convert.ToBoolean(newAddedList[i]["isMinUnitOrNo"]) + "'," + Convert.ToInt32(newAddedList[i]["UnitTimes"]) + ",'" + Convert.ToBoolean(newAddedList[i]["isOrder"]) + "')";
                    SqlSel_Pro.ExeSql(sqlCmd);

                }
                //表格数据重新绑定
                bindGrid("ALL");

                Alert.Show("保存成功！");
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(ex.Message);
                return;
            }
        }

        private void UpdateDataRow(string columnName, Dictionary<string, object> rowDict, DataRow rowData)
        {
            if (rowDict.ContainsKey(columnName))
            {
                rowData[columnName] = rowDict[columnName];
            }
        }
        private DataRow CreateNewData(DataTable table, Dictionary<string, object> newAddedData)
        {
            DataRow rowData = table.NewRow();

            UpdateDataRow(newAddedData, rowData);

            return rowData;
        }

        private void UpdateDataRow(Dictionary<string, object> rowDict, DataRow rowData)
        {
            // 商品编码
            UpdateDataRow("goodsNo", rowDict, rowData);

            // 物料描述
            UpdateDataRow("goodsSpec", rowDict, rowData);

            // 索菲亚编码
            UpdateDataRow("targetNo", rowDict, rowData);

            // 是否最小计量单位
            UpdateDataRow("isMinUnitOrNo", rowDict, rowData);

            // 计量数量
            UpdateDataRow("UnitTimes", rowDict, rowData);


        }

        protected DataTable createDataTable()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("id", typeof(int));
                dt.Columns.Add("goodsNo", typeof(string));
                dt.Columns.Add("goodsSpec", typeof(string));
                dt.Columns.Add("targetNo", typeof(string));
                dt.Columns.Add("isMinUnitOrNo", typeof(bool));
                dt.Columns.Add("UnitTimes", typeof(int));
                dt.Columns.Add("isOrder", typeof(bool));
                return dt;
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
                return null;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            bindGrid("ALL");
        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int rowID = Convert.ToInt32(Grid1.DataKeys[e.RowIndex][0]);
                deleteFromDb(rowID);


                Alert.ShowInTop("删除数据成功!（表格数据已重新绑定）");
            }
        }

        // 删除选中行的脚本
        private string GetDeleteScript()
        {
            return Confirm.GetShowReference("删除选中行？", String.Empty, MessageBoxIcon.Question, Grid1.GetDeleteSelectedRowsReference(), String.Empty);
        }
        
        protected void DeleteRowByID(int rowid) 
        {
            List<Dictionary<string, object>> newAddedList = Grid1.GetNewAddedList();
            int find = -1;
            for (int i = 0; i < newAddedList.Count; i++) 
            {
                if (rowid == Convert.ToInt32(newAddedList[i]["id"])) 
                {
                    find = i;
                    GetDeleteScript();
                    break;
                }
            }

            if (find == -1) 
            {
                deleteFromDb(rowid);
            }
        }
        //服务器端删除
        protected void deleteFromDb(int DbId) 
        {
            string sqlCmd = "delete from t_MapGoodsList where id=" + DbId;
            SqlSel_Pro.ExeSql(sqlCmd);
            bindGrid("ALL");
        }

        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            int rowindex = Convert.ToInt32(Grid1.DataKeys[Grid1.SelectedRowIndex][0]);
            LinkButtonField deleteField = Grid1.FindColumn("Delete") as LinkButtonField;

            List<Dictionary<string, object>> newAddedList = Grid1.GetNewAddedList();
            int find = -1;
            for (int i = 0; i < newAddedList.Count; i++)
            {
                if (rowindex == Convert.ToInt32(newAddedList[i]["id"]))
                {
                    find = i;
                    //GetDeleteScript();
                    deleteField.OnClientClick = GetDeleteScript();
                    break;
                }
            }

            if (find == -1)
            {
                deleteFromDb(rowindex);
            }

        }

    }
}