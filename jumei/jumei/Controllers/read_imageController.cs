using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace jumei.Controllers
{

    

    public class read_imageController : Controller
    {
        public static string connectionString = System.Configuration.ConfigurationManager.AppSettings["connectionString"];

        //GET: /read_image/

        public ActionResult Index()
        {
            return View();
        }


        DataTable dt = new DataTable();
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public string down_image(string sku_id)
        {

            HttpClient client = new HttpClient();
            if (sku_id.Contains("+"))
            {
                sku_id = sku_id.Replace('+', '/');
            }
            string down_state = string.Empty; //下载状态
            string sku = sku_id.Trim(); //货号
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
                return "无此sku数据";
            }


            down_state = get_server_img(brand, sku);

            return down_state;

            #region 读取网络图片
            //string local_img_url = string.Empty;                                                                            //下载图片保存到本地
            //string sku = textBox1.Text.Trim();                                                                              //sku名称
            //MySqlParameter[] my_params = new MySqlParameter[] { 
            //    new MySqlParameter("@sku",sku)
            //};                                                                                                              //参数

            //dt = Query("select image_url from bms_item_image where item_sku=@sku order by sort", my_params);                //数据库中图片路径
            //local_img_url = "c:\\img";
            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    if (!Directory.Exists(local_img_url))
            //    {
            //        Directory.CreateDirectory(local_img_url);
            //    }
            //    else
            //    {
            //        Directory.Delete(local_img_url, true);
            //        if (!Directory.Exists(local_img_url))
            //            Directory.CreateDirectory(local_img_url);
            //    }
            //    try
            //    {
            //        WebClient wc = new WebClient();
            //        for (int i = 0; i < dt.Rows.Count; i++)
            //        {
            //            string img_url = dt.Rows[i]["image_url"].ToString();                                                    //数据库图片路径
            //            string url_img_name = System.IO.Path.GetFileName(img_url);                                              //图片名称

            //            wc.DownloadFile(img_url, local_img_url + "\\" + url_img_name);                                          //保存图片
            //        }
            //        wc.Dispose();
            //        MessageBox.Show("图片下载成功,共" + dt.Rows.Count + "涨");
            //    }
            //    catch
            //    {
            //        MessageBox.Show("网络异常，请重新下载");
            //    }
            //}
            #endregion

        }






        /// <summary>
        /// 获取共享服务器图片
        /// </summary>
        /// <param name="brand"></param>
        /// <param name="sku_code"></param>
        public string get_server_img(string brand, string sku_code)
        {

            string s_log = string.Empty;
            List<string> pathli = new List<string>();//存放共享服务器图片路径
            string down_state = string.Empty; //下载状态
            try
            {
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

                WebClient client = new WebClient();
                NetworkCredential cred = new NetworkCredential("yapingning", "123456", "192.168.1.19:2222");
                client.Credentials = cred;


                pathli = GetServerImagePath(sku_code);

                string serverpath = string.Empty;
                for (int i = 0; i < pathli.Count; i++)
                {
                    serverpath = pathli[i];
                    serverpath = serverpath.Replace("D://DATA//DATA//", "ftp:////192.168.1.19:2222//data//").Replace("//","/");
                    string image_name = sku_code.Replace('/','+') + "-主图-" + (i + 1).ToString() + ".jpg";
                    down_state = downLoadSJProductReport(serverpath, image_name);
                    serverpath = string.Empty;
                }
                //"ftp:\\\\192.168.1.19\\data\\Valentino\\商品图片\\JW2B0960NBDF30F\\聚美\\JW2B0960NBDF30F-主图-1.jpg"
                //"D://DATA//DATA//Valentino//商品图片//JW2B0960NBDF30F//聚美//JW2B0960NBDF30F-主图-1.jpg"
                //"ftp://192.168.1.19:2222/data/Kipling/商品图片/K09480900BLKF/聚美/K09480900BLKF-主图-1.jpg"           "K09480900BLKF-主图-1.jpg"
                //select location_path from mapi_location_images where location_path LIKE '%聚美%' and location_path LIKE '%主图%' and sku='JW2B0960NBDF30F'
                
                //string net_url_image = string.Empty;                                                                                                                    //网络共享图片
                //net_url_image = @"\\192.168.1.19\data\";
                //string[] brands = Directory.GetDirectories(net_url_image, brand);

                //if (brands.Length > 0)
                //{
                //    net_url_image = brands[0] + @"\商品图片\";
                //}
                //string[] sku_codes = Directory.GetDirectories(net_url_image, sku_code);
                //if (sku_codes.Length > 0)
                //{
                //    net_url_image = sku_codes[0] + @"\聚美\";
                //}
                //string[] net_image = Directory.GetFiles(net_url_image);
                //for (int i = 0; i < net_image.Length; i++)
                //{
                //    if (net_image[i].Contains("主图"))
                //    {

                //        //client.DownloadFile(net_image[i].ToString(), "c:\\img\\" + image_name);
                //        ////client.DownloadFile("file://192.168.1.19/data/Kipling/%E5%95%86%E5%93%81%E5%9B%BE%E7%89%87/K0856811ZGRNF/%E8%81%9A%E7%BE%8E/K0856811ZGRNF-%E4%B8%BB%E5%9B%BE-1.jpg", "c:\\img\\" + image_name);
                //        //s_log += "101---";

                //        #region
                //        //string path = @"E:\fabu\jumei\jumei_down_image\jumei_down_image.exe";
                //        //string fileName = path;
                //        //Process p = new Process();
                //        //p.StartInfo.UseShellExecute = false;
                //        //p.StartInfo.RedirectStandardOutput = true;
                //        //p.StartInfo.FileName = fileName;

                //        //p.StartInfo.CreateNoWindow = true;
                //        //p.StartInfo.Arguments = sku_code;//参数以空格分隔，如果某个参数为空，可以传入””
                //        //p.Start();
                //        //p.WaitForExit();
                //        ////此处可以返回一个字符串，此例是返回压缩成功之后的一个文件路径
                //        //string output = p.StandardOutput.ReadToEnd();
                //        //down_state = output;
                //        #endregion
                //        string serverIP = "ftp:";
                //        serverIP += net_image[i].ToString().Trim();
                //        serverIP = serverIP.Replace("\\", "/");
                //        serverIP = serverIP.Replace("192.168.1.19", "192.168.1.19:2222");
                //        string image_name = System.IO.Path.GetFileName(net_image[i].ToString());
                //        down_state = downLoadSJProductReport(serverIP, image_name);
                //    }
                //}
                s_log += "4---";
                return down_state;

            }
            catch (Exception ex) { return down_state = "下载失败！(" + ex.Message + ex.StackTrace + s_log + ")"; }
        }


        /// <summary>
        /// 根据sku得到共享服务器图片路径
        /// </summary>
        /// <param name="sku_code"></param>
        /// <returns></returns>
        public List<string> GetServerImagePath(string sku_code)
        {
            if (sku_code.Contains("+"))
            {
               sku_code= sku_code.Replace('+', '/');
            }
            List<string> pathli = new List<string>();
            MySqlParameter[] my_params = new MySqlParameter[] { 
              new MySqlParameter("@sku",sku_code)
            };
            string sqlstr = "select location_path from mapi_location_images where location_path LIKE '%聚美%' and location_path LIKE '%主图%' and sku=@sku";
            DataTable dt = new DataTable();
            dt = Query(sqlstr, my_params);

            if (dt.Rows.Count > 0 || dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    pathli.Add(row["location_path"].ToString());
                }
            }
            else
            {
                return null;
            }
            return pathli;
        }



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




        /// <summary>
        /// 获取商检商检回执,并下载到本地(c:\img\)
        /// </summary>
        /// <param name="serverIP">图片地址</param>
        /// <param name="img_name">图片名称</param>
        /// <returns></returns>
        public string downLoadSJProductReport(string serverIP, string img_name)
        {
            //ftp://192.168.1.124/
            string s = string.Empty;
            string userName = System.Configuration.ConfigurationManager.AppSettings["ftp_name"]; //ftp用户名
            string password = System.Configuration.ConfigurationManager.AppSettings["ftp_pwd"]; //ftp密码
            try
            {
                string localUrl = @"c:\img\" + img_name;
                int m = DownloadFtp(serverIP, userName, password, localUrl);
                if (m == 0)
                    s += "下载成功！";
                else
                    s += "下载失败！";
            }
            catch (Exception ex)
            {
                s = ex.Message;
            }
            //System.Threading.Thread.Sleep(1000); //延时2秒

            if (s.Contains("失败"))
            {
                s = "下载失败，请重新下载！";
            }

            return s;
        }


        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="serverIP">FTP地址</param>
        /// <param name="userName">FTP账号</param>
        /// <param name="password">FTP密码</param>
        /// <param name="filename">文件名</param>
        /// <returns></returns>
        public int DownloadFtp(string serverIP, string userName, string password, string filename)
        {
            FtpWebRequest reqFTP;
            string url;
            FileStream outputStream = new FileStream(filename, FileMode.Create);
            try
            {
                url = serverIP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.KeepAlive = false;
                reqFTP.Credentials = new NetworkCredential(userName, password);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();

                return 0;
            }
            catch (Exception ex)
            {
                outputStream.Close();
                try
                {
                    url = serverIP;
                    FileStream outputStream1 = new FileStream(filename, FileMode.Create);
                    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
                    reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                    reqFTP.UseBinary = true;
                    reqFTP.KeepAlive = false;
                    reqFTP.Credentials = new NetworkCredential(userName, password);
                    FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                    Stream ftpStream = response.GetResponseStream();
                    long cl = response.ContentLength;
                    int bufferSize = 2048;
                    int readCount;
                    byte[] buffer = new byte[bufferSize];

                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                    while (readCount > 0)
                    {
                        outputStream1.Write(buffer, 0, readCount);
                        readCount = ftpStream.Read(buffer, 0, bufferSize);
                    }
                    ftpStream.Close();
                    outputStream1.Close();
                    response.Close();

                    return 0;
                }
                catch
                {
                    return -2;
                }
            }
        }


    }

}
