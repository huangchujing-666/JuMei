using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace jumei.Controllers
{
    public class IndexController : Controller
    {
        //
        // GET: /Index/

        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string connectionString = System.Configuration.ConfigurationManager.AppSettings["connectionString"];

        public ActionResult Index()
        {
            return View();
        }

        #region 读取sku所属信息
        /// <summary>
        /// 读取sku所属信息
        /// </summary>
        /// <param name="sku">SKU货号</param>
        /// <returns></returns>
        public JsonResult get_sku_msg(string sku_id)
        {
            string s = string.Empty;
            sku_id = sku_id.Trim().Replace('+','/'); //货号
            sku_msg my_sku_msg = new sku_msg();

            try
            {
                #region 读取SKU信息
                MySqlParameter[] sku_params = new MySqlParameter[] { new MySqlParameter("@sku_id", sku_id) };
                DataTable dt = Query("select * from bms_item where item_sku=@sku_id", sku_params);
                #endregion

                #region 读取SKU图片
                DataTable dt_img = Query("select * from bms_jumei_sku_image where sku=@sku_id order by sku_img_index asc", sku_params);
                #endregion
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        my_sku_msg.Category_1 = "485";//类目1
                        my_sku_msg.Category_2 = "498";//类目2
                        my_sku_msg.Category_3 = "499";//类目3
                        my_sku_msg.Category_4 = "501";//类目4
                        my_sku_msg.Brand = "6626";//品牌
                        my_sku_msg.Sku_name = item["title"].ToString();//中文名称
                        my_sku_msg.Sku_foreign_name = item["title"].ToString();//外文名称
                        //my_sku_msg.Sku_foreign_name = item["title_en"].ToString();//外文名称

                        string str_jumei_img = string.Empty;
                        string str_jumei_img2 = string.Empty;
                        str_jumei_img = "{\"normal\":[";
                        str_jumei_img2 = "[";
                        if (dt_img != null && dt_img.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt_img.Rows.Count; j++)
                            {
                                string str_jumei_img_url = dt_img.Rows[j]["sku_img_url"].ToString();
                                str_jumei_img += "{\"img\":\"" + str_jumei_img_url + "\",\"is_changed\":1},";
                                str_jumei_img2 += "{\"img\":\"" + str_jumei_img_url + "\",\"is_changed\":1},";
                            }
                            for (int k = 0; k < 10 - dt_img.Rows.Count; k++)
                            {
                                str_jumei_img += "{\"img\":\"\",\"is_changed\":0},";
                                str_jumei_img2 += "{\"img\":\"\",\"is_changed\":0},";
                            }
                            //foreach (DataRow item_img in dt_img.Rows)
                            //{
                            //    my_sku_msg.Sku_image_1 = "{\"normal\":[{\"img\":\"" + item_img[""].ToString() + "\",\"is_changed\":1},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0}],\"vertical\":[{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0}],\"diaoxing\":{\"img\":\"\",\"is_changed\":0}}";//图片
                            //}

                            str_jumei_img = str_jumei_img.Trim(',');
                            str_jumei_img2 = str_jumei_img2.Trim(',');
                            str_jumei_img += "],\"vertical\":[{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0}],\"diaoxing\":{\"img\":\"\",\"is_changed\":0}}";
                            str_jumei_img2 += "]";
                        }
                        else
                        {
                            str_jumei_img = "";
                            return Json("1");
                        }
                        my_sku_msg.Sku_image_1 = str_jumei_img;
                        //my_sku_msg.Sku_image_1 = "{\"normal\":[{\"img\":\"http://p0.jmstatic.com/global/image/201601/14/1452762625.562.jpg\",\"is_changed\":1},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0}],\"vertical\":[{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0}],\"diaoxing\":{\"img\":\"\",\"is_changed\":0}}";//图片
                        my_sku_msg.Sku_id = item["item_sku"].ToString();//sku
                        my_sku_msg.Sku_property = "OTHER";//规格
                        my_sku_msg.Sku_size = string.IsNullOrWhiteSpace(item["size"].ToString()) ? "12" : item["size"].ToString();//尺码
                        my_sku_msg.Sku_color = item["color"].ToString();//颜色

                        string sku_sale_price = string.IsNullOrWhiteSpace(item["tag_price"].ToString()) || decimal.Parse(item["tag_price"].ToString()) == 0 ? "999999" : item["tag_price"].ToString();
                        my_sku_msg.Sku_price = decimal.Parse(sku_sale_price).ToString("F0");//价格
                        my_sku_msg.Sku_area_code = "19";//货币
                        //my_sku_msg.Sku_image_2 = "[{\"img\":\"http://p0.jmstatic.com/global/image/201601/13/1452664107.3108.jpg\",\"is_changed\":1},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0}]";//图片2
                        my_sku_msg.Sku_image_2 = str_jumei_img2;
                        my_sku_msg.Sku_image_3 = "{ \"vertical_images\":[{ \"img\":\"\",\"is_changed\":0},{ \"img\":\"\",\"is_changed\":0},{ \"img\":\"\",\"is_changed\":0},{ \"img\":\"\",\"is_changed\":0},{ \"img\":\"\",\"is_changed\":0},{ \"img\":\"\",\"is_changed\":0},{ \"img\":\"\",\"is_changed\":0},{ \"img\":\"\",\"is_changed\":0},{ \"img\":\"\",\"is_changed\":0},{ \"img\":\"\",\"is_changed\":0}]}";//其他图片
                        my_sku_msg.Sku_auditing = "送审";//送审
                        my_sku_msg.Sku_trial_reason = "新";//描述
                    }

                }
                else
                {
                    return Json("");
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            return Json(my_sku_msg);
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

        #endregion

        #region 生成cookie
        /// <summary>
        /// 生成cookie
        /// </summary>
        /// <param name="str_cookie">cookie字符串形式</param>
        /// <returns></returns>
        public CookieContainer getCookie(string str_cookie)
        {
            CookieContainer my_cookie_container = new CookieContainer();
            string[] cookies = str_cookie.Split(';');
            foreach (string str in cookies)
            {
                string[] cookieNameValue = str.Split('=');
                Cookie ck = new Cookie(cookieNameValue[0].Trim().ToString(), cookieNameValue[1].Trim().ToString());
                ck.Domain = "a.jumeiglobal.com";//必须写对
                my_cookie_container.Add(ck);
            }

            return my_cookie_container;
        }

        #endregion

        #region 上传图片
        public string upload_img_one(string sku_id)
        {

            string sql_delete = "delete from bms_jumei_sku_image where sku = '" + sku_id.Replace('+','/') + "';";
            ExecuteSql(sql_delete);

            string s_log = string.Empty;
            try
            {
                string jumei_cookie = System.IO.File.ReadAllText("c:\\jumei_cookie.txt", Encoding.UTF8);
                //s_log += "1";
                if (string.IsNullOrWhiteSpace(jumei_cookie))
                {
                    return "请先打开桌面客户端登录";
                }
                NameValueCollection data = new NameValueCollection();
                CookieContainer my_cookie_container = getCookie(jumei_cookie);
                string sql_insert = string.Empty;
                string local_img_url = "c:\\img";
                string[] str_imgs = Directory.GetFiles(local_img_url);
                
                for (int i = 0; i < str_imgs.Length; i++)
                {
                    FileInfo fi = new FileInfo(str_imgs[i]);
                    if (fi.Length<1048576)
                    {
                        string str_img_url = str_imgs[i].ToString();                                                                                //图片
                        string img_value = HttpUploadFile("http://a.jumeiglobal.com/GlobalProduct/ProductImageUpload", my_cookie_container, new string[] { str_img_url }, data, Encoding.UTF8);
                        img_value = img_value.Replace(@"\", "").Trim();
                        img_value = img_value.Substring(img_value.IndexOf("http")).Replace("\"}", "");

                        sql_insert += "insert into bms_jumei_sku_image values(null,'" + sku_id.Trim().Replace('+','/') + "','" + img_value + "'," + (i + 1) + ");";
                    }
                    
                }

                int count = ExecuteSql(sql_insert);
                if (count > 0)
                    return "上传成功" + s_log;
                else
                    return "上传失败" + s_log;
            }
            catch { return s_log; }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="cookie">cookie</param>
        /// <param name="files">文件</param>
        /// <param name="data">参数</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string HttpUploadFile(string url, CookieContainer cookie, string[] files, NameValueCollection data, Encoding encoding)
        {
            //string boundary = "--------------" + DateTime.Now.Ticks.ToString("x");
            string boundary = "--------------WebKitFormBoundaryWJNBo8Ys3b8adNXG";
            //WebKitFormBoundaryWJNBo8Ys3b8adNXG
            byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endbytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");

            //1.HttpWebRequest
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";
            request.Method = "POST";
            request.CookieContainer = new CookieContainer();
            if (cookie != null && cookie.Count > 0)
            {
                request.CookieContainer = cookie;
            }
            request.KeepAlive = true;
            request.Credentials = CredentialCache.DefaultCredentials;

            using (Stream stream = request.GetRequestStream())
            {
                //1.1 key/value
                string formdataTemplate = "Content-Disposition: form-data; name=\"image\"\r\n\r\n{1}";
                if (data != null)
                {
                    foreach (string key in data.Keys)
                    {
                        stream.Write(boundarybytes, 0, boundarybytes.Length);
                        string formitem = string.Format(formdataTemplate, key, data[key]);
                        byte[] formitembytes = encoding.GetBytes(formitem);
                        stream.Write(formitembytes, 0, formitembytes.Length);
                    }
                }

                //1.2 file
                string headerTemplate = "Content-Disposition: form-data; name=\"image\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
                byte[] buffer = new byte[4096];
                int bytesRead = 0;
                for (int i = 0; i < files.Length; i++)
                {
                    stream.Write(boundarybytes, 0, boundarybytes.Length);
                    string header = string.Format(headerTemplate, "file" + i, Path.GetFileName(files[i]));
                    byte[] headerbytes = encoding.GetBytes(header);
                    stream.Write(headerbytes, 0, headerbytes.Length);

                    WebClient w = new WebClient();
                    byte[] bytes = w.DownloadData(files[i]);
                    stream.Write(bytes, 0, bytes.Length);
                    //using (FileStream fileStream = new FileStream(files[i], FileMode.Open, FileAccess.Read))
                    //{
                    //    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    //    {
                    //        stream.Write(buffer, 0, bytesRead);
                    //    }
                    //}
                }

                //1.3 form end
                stream.Write(endbytes, 0, endbytes.Length);
            }
            //2.WebResponse
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                return stream.ReadToEnd();
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
        #endregion

    }
}
