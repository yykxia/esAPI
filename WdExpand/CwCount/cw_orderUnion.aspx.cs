using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using FineUI;
using System.Data;
using IETCsoft.sql;
using System.Data.OleDb;

namespace WdExpand.CwCount
{
    public partial class cw_orderUnion : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                loadShopList();
                loadtradeType();
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
        //加载交易类型
        protected void loadtradeType() 
        {
            DataTable dt = new DataTable();
            string sqlCmd = "select * from t_trade_tradeType";
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            RadioButtonList1.DataTextField = "tradeName";
            RadioButtonList1.DataValueField = "id";
            RadioButtonList1.DataSource = dt;
            RadioButtonList1.DataBind();
        }

        protected void filePhoto_FileSelected(object sender, EventArgs e)
        {
            string fileName = fileData.ShortFileName;
            //获取文件格式
            string filetyp = fileName.Substring(fileName.LastIndexOf(".") + 1);
            if (!isValidType(filetyp)) 
            {
                fileData.Reset();
                Alert.ShowInTop("文件格式无效！");
                return;
            }
        }


        //验证文件格式合法性
         protected static bool isValidType(string fileType) 
         {
             string[] typeName = new string[] {"xls", "xlsx"};
             int id = Array.IndexOf(typeName, fileType);
             if (id != -1)
             {
                 return true;
             }
             else 
             {
                 return false;
             }
         }

         protected void btn_loadData_Click(object sender, EventArgs e)
         {
             try
             {
                 if (fileData.HasFile)
                 {
                     string fileName = fileData.ShortFileName;
                     //fileName = fileName.Replace(":", "_").Replace(" ", "_").Replace("\\", "_").Replace("/", "_");
                     string filetyp = fileName.Substring(fileName.LastIndexOf(".") + 1);
                     fileName = DateTime.Now.Ticks.ToString() + "." + filetyp;
                     string filePath = "~/wdexpand/upload/" + fileName;
                     fileData.SaveAs(Server.MapPath(filePath));//上传文件
                     DataTable uploadData = new DataTable();
                     uploadData = InputExcel(filePath);//得到excel数据

                     //判断列数是否符合
                     if (uploadData.Columns.Count != 9) 
                     {
                         Alert.ShowInTop("导入文件格式不符，请校验！");
                         return;
                     }
                     //
                     int maxId = 0;
                     string sqlCmd = "select max(id) from t_trade_specreg";
                     if (!string.IsNullOrEmpty(SqlSel.GetSqlScale(sqlCmd).ToString()))
                     {
                         maxId = Convert.ToInt32(SqlSel.GetSqlScale(sqlCmd));
                     }
                     string curUser = GetUser();
                     //插入数据库
                     for (int i = 0; i < uploadData.Rows.Count; i++) 
                     {
                         sqlCmd = "insert into t_trade_specreg (payDate,custNickName,tradeNo,cashGo,goodsCount,goodsName,payCount,customerName,addDesc,payType,operUser,shopID) values ";
                         sqlCmd += "('" + uploadData.Rows[i][0].ToString().Trim() + "',";
                         sqlCmd += "'" + uploadData.Rows[i][1].ToString().Trim() + "',";
                         sqlCmd += "'" + uploadData.Rows[i][2].ToString().Trim() + "',";
                         sqlCmd += "'" + uploadData.Rows[i][3].ToString().Trim() + "',";
                         sqlCmd += "'" + uploadData.Rows[i][4].ToString().Trim() + "',";
                         sqlCmd += "'" + uploadData.Rows[i][5].ToString().Trim() + "',";
                         sqlCmd += "'" + uploadData.Rows[i][6].ToString().Trim() + "',";
                         sqlCmd += "'" + uploadData.Rows[i][7].ToString().Trim() + "',";
                         sqlCmd += "'" + uploadData.Rows[i][8].ToString().Trim() + "',";
                         sqlCmd += "'" + RadioButtonList1.SelectedValue + "',";
                         sqlCmd += "'" + curUser + "',";
                         sqlCmd += "'" + ddl_shopList.SelectedValue + "')";

                         SqlSel.ExeSql(sqlCmd);
                     }

                     //显示导入的数据
                     bindGrid(maxId);
                     
                 }
                 else
                 {
                     Alert.ShowInTop("先选择相应文件！");
                     return;
                 }
             }
             catch (Exception ex) 
             {
                 Alert.ShowInTop(ex.ToString());
                 return;
             }
         }

        //excel转datatable
         protected DataTable InputExcel(string Path)
         {
             try
             {
                 string strConn = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + Server.MapPath(Path) + "; Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1'"; //此连接可以操作.xls与.xlsx文件 (支持Excel2003 和 Excel2007 的连接字符串)
                //备注： "HDR=yes;"是说Excel文件的第一行是列名而不是数据，"HDR=No;"正好与前面的相反。
                //      "IMEX=1 "如果列中的数据类型不一致，使用"IMEX=1"可必免数据类型冲突。
                 OleDbConnection conn = new OleDbConnection(strConn);
                 conn.Open();
                 OleDbDataAdapter myCommand = null;

                 string strExcel = "select * from [Sheet1$]";
                 myCommand = new OleDbDataAdapter(strExcel, strConn);
                 DataTable dt = new DataTable();
                 myCommand.Fill(dt);
                 conn.Close();
                 return dt;
             }
             catch (Exception ex)
             {
                 throw new Exception(ex.Message);
             }
         }

        //
         private void bindGrid(int startIndex) 
         {
             string sqlCmd = "select * from t_trade_specreg left join ";
             sqlCmd += "g_cfg_shoplist on t_trade_specreg.shopID=g_cfg_shoplist.shopID ";
             sqlCmd += "left join t_trade_tradeType on t_trade_tradeType.id=t_trade_specreg.payType ";
             sqlCmd += "where t_trade_specreg.id>" + startIndex + " order by tradeNo";
             DataTable dt = new DataTable();
             SqlSel.GetSqlSel(ref dt, sqlCmd);
             Grid1.DataSource = dt;
             Grid1.DataBind();
         }
    }
}