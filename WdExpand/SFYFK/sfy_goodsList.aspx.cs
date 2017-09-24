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
    public partial class sfy_goodsList : BasePage
    {
        private bool AppendToEnd = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {



                JObject defaultObj = new JObject();
                defaultObj.Add("id", getMaxId());
                defaultObj.Add("goodsNo", "");
                defaultObj.Add("goodsName", "");
                defaultObj.Add("specification", "");
                defaultObj.Add("enabled", false);

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
                string sqlCmd = "select * from [goodsList] where [goodsNo] like '" + goodNo + "' order by goodsno";
                SqlSel_Pro.GetSqlSel(ref dt, sqlCmd);
                Grid1.DataSource = dt;
                Grid1.DataBind();

                sqlCmd = "select max(id) from goodsList";
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

        protected void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                string curUser = GetUser();
                string sqlCmd = "";
                // 修改的现有数据
                Dictionary<int, Dictionary<string, object>> modifiedDict = Grid1.GetModifiedDict();
                foreach (int rowIndex in modifiedDict.Keys)
                {

                    if (string.IsNullOrEmpty(Grid1.Rows[rowIndex].Values[0].ToString()))
                    {
                        Alert.ShowInTop("商品编码不可为空！");
                        return;
                    }
                    int rowID = Convert.ToInt32(Grid1.DataKeys[rowIndex][0]);
                    sqlCmd = "update goodsList set goodsNo='" + Grid1.Rows[rowIndex].Values[0].ToString() + "',";
                    sqlCmd += "[goodsName]='" + Grid1.Rows[rowIndex].Values[1].ToString() + "',";
                    sqlCmd += "[specification]='" + Grid1.Rows[rowIndex].Values[2].ToString().Trim() + "',";
                    sqlCmd += "enabled='" + Convert.ToBoolean(Grid1.Rows[rowIndex].Values[3]) + "',";
                    sqlCmd += "[createDte]='" + DateTime.Now + "',[createUser]='"+curUser+"' ";
                    sqlCmd += "where id=" + rowID;
                    SqlSel_Pro.ExeSql(sqlCmd);
                }

                // 新增数据
                List<Dictionary<string, object>> newAddedList = Grid1.GetNewAddedList();
                for (int i = 0; i < newAddedList.Count; i++)
                {
                    //DataRow rowData = CreateNewData(table, newAddedList[i]);
                    //table.Rows.Add(rowData);
                    if (string.IsNullOrEmpty(newAddedList[i]["goodsNo"].ToString()))
                    {
                        Alert.ShowInTop("商品编码不可为空！");
                        return;
                    }
                    sqlCmd = "insert into goodsList (goodsNo,goodsName,specification,enabled,createDte,createUser) values ";
                    sqlCmd += "('" + newAddedList[i]["goodsNo"].ToString().Trim() + "','" + newAddedList[i]["goodsName"].ToString() + "','" + newAddedList[i]["specification"].ToString().Trim() + "',";
                    sqlCmd += "'" + Convert.ToBoolean(newAddedList[i]["enabled"]) + "','" + DateTime.Now + "','" + curUser + "')";
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
                    GetDeleteScript();//前端数据直接脚本删除
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
            string sqlCmd = "delete from goodsList where id=" + DbId;
            SqlSel_Pro.ExeSql(sqlCmd);
            bindGrid("ALL");
        }

    }
}