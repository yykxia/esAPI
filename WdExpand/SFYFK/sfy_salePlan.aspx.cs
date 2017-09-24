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
    public partial class sfy_salePlan : BasePage
    {
        private bool AppendToEnd = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string curUser = GetUser();

                //新增一行数据
                JObject defaultObj = new JObject();
                defaultObj.Add("id", getMaxId());
                defaultObj.Add("goodsNo", "");
                defaultObj.Add("giftNo", "");
                defaultObj.Add("enabled", true);
                defaultObj.Add("startDte", DateTime.Now.ToShortDateString());
                defaultObj.Add("endDte", "2020-01-01");

                btn_addNew.OnClientClick = Grid1.GetAddNewRecordReference(defaultObj, AppendToEnd);

                bindGrid("ALL");
            }
        }

        //加载数据
        protected void bindGrid(string loadField) 
        {
            try
            {
                DataTable dt = new DataTable();
                string sqlCmd = "";
                if (loadField == "ALL")//加载全部
                {
                    sqlCmd = "select (select goodsNo from goodsList where goodsList.id=SaleAct.origGoodsId) as goodsNo,";
                    sqlCmd += "(select goodsNo from goodsList where goodsList.id=SaleAct.giftGoodsId) as giftNo,";
                    sqlCmd += "CONVERT(varchar(100), startDte, 23) as dstartDte,";
                    sqlCmd += "CONVERT(varchar(100), endDte, 23) as dendDte,";
                    sqlCmd += " * from SaleAct order by giftGoodsId";
                }
                else //根据编码查询
                {
                    sqlCmd = "select * from  (select (select goodsNo from goodsList where goodsList.id=SaleAct.origGoodsId) as goodsNo,";
                    sqlCmd += "(select goodsNo from goodsList where goodsList.id=SaleAct.giftGoodsId) as giftNo,";
                    sqlCmd += "CONVERT(varchar(100), startDte, 23) as dstartDte,";
                    sqlCmd += "CONVERT(varchar(100), endDte, 23) as dendDte,";
                    sqlCmd += "  * from SaleAct) A where A.goodsNo='" + loadField + "'";
                }
                SqlSel_Pro.GetSqlSel(ref dt, sqlCmd);
                Grid1.DataSource = dt;
                Grid1.DataBind();
                //取当前最大数据id
                sqlCmd = "select max(id) from SaleAct";
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
                bindGrid(trgbx_NO.Text.Trim());
            }
        }

        //数据重新绑定
        protected void btn_reLoad_Click(object sender, EventArgs e)
        {
            bindGrid("ALL");
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                string sqlCmd = "";
                int origGoodsId = 0, giftGoodsId = 0;
                // 修改的现有数据
                Dictionary<int, Dictionary<string, object>> modifiedDict = Grid1.GetModifiedDict();
                foreach (int rowIndex in modifiedDict.Keys)
                {

                    if (Convert.ToBoolean(Grid1.Rows[rowIndex].Values[2]) == true & (string.IsNullOrEmpty(Grid1.Rows[rowIndex].Values[0].ToString()) || string.IsNullOrEmpty(Grid1.Rows[rowIndex].Values[2].ToString())))
                    {
                        Alert.ShowInTop("商品编码或赠品编码不可为空！");
                        return;
                    }
                    int rowID = Convert.ToInt32(Grid1.DataKeys[rowIndex][0]);
                    origGoodsId = getGoodIdByNo(Grid1.Rows[rowIndex].Values[0].ToString().Trim());
                    giftGoodsId = getGoodIdByNo(Grid1.Rows[rowIndex].Values[1].ToString().Trim());
                    if (origGoodsId == 0 || giftGoodsId == 0) 
                    {
                        Alert.ShowInTop("商品编码不存在，请校验！");
                        return;
                    }
                    sqlCmd = "update SaleAct set origGoodsId=" + origGoodsId + ",";
                    sqlCmd += "giftGoodsId=" + giftGoodsId + ",";
                    sqlCmd += "enabled='" + Convert.ToBoolean(Grid1.Rows[rowIndex].Values[2]) + "',";
                    sqlCmd += "startDte='" + Convert.ToDateTime(Grid1.Rows[rowIndex].Values[3]) + "',";
                    sqlCmd += "endDte='" + Convert.ToDateTime(Grid1.Rows[rowIndex].Values[4]) + "' ";
                    sqlCmd += "where id=" + rowID;
                    SqlSel_Pro.ExeSql(sqlCmd);
                }

                // 新增数据
                List<Dictionary<string, object>> newAddedList = Grid1.GetNewAddedList();
                for (int i = 0; i < newAddedList.Count; i++)
                {
                    //DataRow rowData = CreateNewData(table, newAddedList[i]);
                    //table.Rows.Add(rowData);
                    if (Convert.ToBoolean(newAddedList[i]["enabled"]) == true & (string.IsNullOrEmpty(newAddedList[i]["goodsNo"].ToString()) || string.IsNullOrEmpty(newAddedList[i]["giftNo"].ToString())))
                    {
                        Alert.ShowInTop("商品编码或赠品编码不可为空！");
                        return;
                    }

                    origGoodsId = getGoodIdByNo(newAddedList[i]["goodsNo"].ToString().Trim());
                    giftGoodsId = getGoodIdByNo(newAddedList[i]["giftNo"].ToString().Trim());
                    if (origGoodsId == 0 || giftGoodsId == 0)
                    {
                        Alert.ShowInTop("商品编码不存在，请校验！");
                        return;
                    }

                    sqlCmd = "insert into SaleAct (origGoodsId,giftGoodsId,enabled,startDte,endDte) values ";
                    sqlCmd += "(" + origGoodsId + "," + giftGoodsId + ",'" + Convert.ToBoolean(newAddedList[i]["enabled"]) + "',";
                    sqlCmd += "'" + Convert.ToDateTime(newAddedList[i]["startDte"]) + "','" + Convert.ToDateTime(newAddedList[i]["endDte"]) + "')";
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

        //获取商品Id
        protected int getGoodIdByNo(string goodsNo) 
        {
            try
            {
                int goodId = 0;
                string sqlCmd = "select id from goodsList where goodsNo='" + goodsNo + "'";
                goodId = Convert.ToInt32(SqlSel_Pro.GetSqlScale(sqlCmd));
                return goodId;
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(ex.Message);
                return 0;
            }
        }

    }
}