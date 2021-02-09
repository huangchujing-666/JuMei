using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace jumei_down_image
{
    class Program
    {
        static void Main(string[] args)
        {
            get_image img = new get_image();
            img.get_img(args);
            Console.WriteLine();
        }
    }

    public class get_image
    {
        public void get_img(string[] args)
        {
            string sku = args[0].Trim(); //货号
            string brand = string.Empty; //品牌            
            //get_server_img("Guess", "HWAMY1L5265ANRF");
            MySqlParameter[] my_params = new MySqlParameter[] { 
                new MySqlParameter("@sku",sku)
            };
            string sql_select = "select brand_name from bms_item where item_sku=@sku";

            DataTable dt = Query(sql_select, my_params);

            if (dt != null && dt.Rows.Count > 0)
            {
                brand = dt.Rows[0]["brand_name"].ToString();
            }
            else
            {
                return;
            }


            get_server_img(brand, sku);
        }


        /// <summary>
        /// 获取共享服务器图片
        /// </summary>
        /// <param name="brand"></param>
        /// <param name="sku_code"></param>
        public void get_server_img(string brand, string sku_code)
        {
            try
            {
                System.IO.File.WriteAllText("c:\\img_log.txt", "1---");
                string local_img_url = "c:\\img";
                if (!Directory.Exists(local_img_url))
                {
                    Directory.CreateDirectory(local_img_url);
                }
                else
                {
                    Directory.Delete(local_img_url, true);
                    if (!Directory.Exists(local_img_url))
                        Directory.CreateDirectory(local_img_url);
                }
                System.IO.File.WriteAllText("c:\\img_log.txt", "2---");

                WebClient client = new WebClient();
                NetworkCredential cred = new NetworkCredential("administrator", "123456", "192.168.1.19");
                client.Credentials = cred;

                string net_url_image = string.Empty;                                                                                                                    //网络共享图片
                net_url_image = @"\\192.168.1.19\data\";
                string[] brands = Directory.GetDirectories(net_url_image, brand);
                if (brands.Length > 0)
                {
                    net_url_image = brands[0] + @"\商品图片\";
                }
                System.IO.File.WriteAllText("c:\\img_log.txt", "3---");
                string[] sku_codes = Directory.GetDirectories(net_url_image, sku_code);
                if (sku_codes.Length > 0)
                {
                    net_url_image = sku_codes[0] + @"\聚美\";
                }
                System.IO.File.WriteAllText("c:\\img_log.txt", "4---");
                string[] net_image = Directory.GetFiles(net_url_image);
                for (int i = 0; i < net_image.Length; i++)
                {
                    if (net_image[i].Contains("主图"))
                    {
                        System.IO.File.AppendAllText("c:\\img_log.txt",net_image[i].ToString());
                        string image_name = System.IO.Path.GetFileName(net_image[i].ToString());
                        client.DownloadFile(net_image[i].ToString(), "c:\\img\\" + image_name);
                    }
                }

                //MessageBox.Show("下载成功！");
                Console.WriteLine(sku_code + "下载成功！");
            }
            catch { }
        }




        public static string connectionString = System.Configuration.ConfigurationManager.AppSettings["connectionString"];

        /// <summary>
        /// 执行查询语句，返回DataTable
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataTable</returns>
        public DataTable Query(string SQLString, params MySqlParameter[] cmdParms)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    try
                    {
                        da.Fill(dt);
                        cmd.Parameters.Clear();
                    }
                    catch (MySql.Data.MySqlClient.MySqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return dt;
                }
            }
        }

        public void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, string cmdText, MySqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (MySqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }


        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (MySql.Data.MySqlClient.MySqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }
    }
}
