using mshtml;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("http://a.jumeiglobal.com");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Interval = 1000 * 60;
            timer1.Enabled = true;
            timer1.Tick += timer1_Tick;
            timer1.Start();
            webBrowser1.ScriptErrorsSuppressed = true;
            string content = "userName=apennie&password=yapnjm32#@&encryptParam=07f0d772f1f90f2c452ef7ba933d9bf6";
            webBrowser1.Document.GetElementById("userName").InnerText = "apennie";
            webBrowser1.Document.GetElementById("password").InnerText = "yapnjm32#@";//ypnjm21@#
            webBrowser1.Document.GetElementById("submit-btn").InvokeMember("click");

            webBrowser1.DocumentCompleted += webBrowser1_DocumentCompleted;

            //MessageBox.Show(webBrowser1.Version.ToString());
            //webBrowser1.Navigated += webBrowser1_Navigated;
            
        }

        void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            
        }

        void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.ReadyState== WebBrowserReadyState.Complete)
            {
                webBrowser1.Navigate("http://a.jumeiglobal.com/GlobalProduct/ApplyNew");
            }
            webBrowser1.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);

            CookieContainer my_cookie_container = new CookieContainer();
            string str_cookie = webBrowser1.Document.Cookie;
            string[] cookies = str_cookie.Split(';');
            foreach (string str in cookies)
            {
                string[] cookieNameValue = str.Split('=');
                Cookie ck = new Cookie(cookieNameValue[0].Trim().ToString(), cookieNameValue[1].Trim().ToString());
                ck.Domain = "a.jumeiglobal.com";//必须写对
                my_cookie_container.Add(ck);
            }
            System.IO.File.WriteAllText("c:\\jumei_cookie.txt", str_cookie, Encoding.UTF8);


            //http://a.jumeiglobal.com/GlobalProduct/ProductImageUpload
            //http://a.jumeiglobal.com/GlobalProduct/ProductImageUpload   //上传图片地址
            //WebKitFormBoundaryMK
            //WebKitFormBoundaryMK9rPB9aDyBc3Va0
            //mk9rPB9aDyBc3Va0
            //WebKitFormBoundaryWJNBo8Ys3b8adNXG
            //8d31b5c6a023d01
            //GoRIeBLzQH4rSqrt

            NameValueCollection data = new NameValueCollection();
            //data.Add("product[category_v3_1]", "485");
            //data.Add("product[category_v3_2]", "526");
            //data.Add("product[category_v3_3]", "529");
            //data.Add("product[category_v3_4]", "530");
            //data.Add("product[brand_id]", "5532");
            //data.Add("product[name]", "GUESS");
            //data.Add("product[foreign_language_name]", "GUESS");
            //data.Add("product[images]", "");
            //data.Add("spu_1[id]", "-1");
            //data.Add("spu_1[upc_code]", "SWVY4535570LIPF");
            //data.Add("spu_1[property]", "MS");
            //data.Add("spu_1[size]", "18");
            //data.Add("spu_1[attributes]", "");
            //data.Add("spu_1[abroad_price]", "700");
            //data.Add("spu_1[area_code]", "-1");
            //data.Add("spu_1[abroad_url]", "");
            //data.Add("spu_1[images]", "");
            //data.Add("spu_1[other_images]", "");
            //data.Add("auditing", "送审");
            //data.Add("product[trial_reason]", "123456");

            ///GlobalProduct/ProductImageUpload
            /////Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36

            //string s = UploadHelper.HttpUploadFile("http://a.jumeiglobal.com/GlobalProduct/ProductImageUpload", my_cookie_container, new string[] { @"C:\Users\Administrator\Desktop\SWVY4535570LIPF\SWVY4535570LIPF-主图-1.jpg" }, data);
            //UploadHelper.HttpUploadFile("http://a.jumeiglobal.com/GlobalProduct/ApplyNew", my_cookie_container, "", data);
            

            //MessageBox.Show(s);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int count = webBrowser1.Document.All.Count;
            for (int i = 0; i < count; i++)
            {
                HtmlElement GetElement = webBrowser1.Document.All[i];
                //取到包含input标签的元素（根据input的Name属性，找到该元素并赋值）
                if (GetElement.TagName.ToUpper().ToString() == "INPUT")
                {
                    // 产品名称
                    if (GetElement.Name.ToString() == "product[name]")
                    {
                        webBrowser1.Document.All[i].SetAttribute("value", "产品名称");
                    }
                    // 外文名
                    if (GetElement.Name.ToString() == "product[foreign_language_name]")
                    {
                        webBrowser1.Document.All[i].SetAttribute("value", "外文名");
                    }
                    // 尺码
                    if (GetElement.Name.ToString() == "spu_1[size]")
                    {
                        webBrowser1.Document.All[i].SetAttribute("value", "48");
                    }
                    // 海外官网价
                    if (GetElement.Name.ToString() == "spu_1[abroad_price]")
                    {
                        webBrowser1.Document.All[i].SetAttribute("value", "2000000");
                    }
                    //fileFormTmp    图片          <input type="file" name="image" class="pic-input" form="fileFormTmp"> html5
                }

                //下拉框
                // 类目1
                if (GetElement.Name.ToString() == "product[category_v3_1]")
                {
                    webBrowser1.Document.All[i].SetAttribute("value", "485");
                    object[] o = new object[] { "#leavel_1" };
                    webBrowser1.Document.InvokeScript("getCategoryNext", o);
                }
                // 类目2
                if (GetElement.Name.ToString() == "product[category_v3_2]")
                {
                    webBrowser1.Document.All[i].SetAttribute("value", "533");
                    object[] o = new object[] { ".category3" };
                    webBrowser1.Document.InvokeScript("getCategoryformPrev", o);    //getCategoryformPrev
                }
                // 类目3
                if (GetElement.Name.ToString() == "product[category_v3_3]")
                {
                    webBrowser1.Document.All[i].SetAttribute("value", "536");
                    object[] o = new object[] { ".category4" };
                    webBrowser1.Document.InvokeScript("getCategoryformPrev", o);    //getCategoryformPrev
                }
                // 类目4
                if (GetElement.Name.ToString() == "product[category_v3_4]")
                {
                    //object[] o = new object[] { ".category4" };
                    //object o1 = webBrowser1.Document.InvokeScript("getCategorySelf", o);    //getCategorySelf
                    webBrowser1.Document.All[i].SetAttribute("value", "537");
                }
                // 规格
                if (GetElement.Name.ToString() == "spu_1[property]")
                {
                    webBrowser1.Document.All[i].SetAttribute("value", "FORMAL");
                }

                // 海外官网价
                if (GetElement.Name.ToString() == "spu_1[area_code]")
                {
                    webBrowser1.Document.All[i].SetAttribute("value", "19");
                }

                // 品牌
                if (GetElement.Name.ToString() == "product[brand_id]")
                {
                    webBrowser1.Document.All[i].SetAttribute("value", "3610");
                }
            }
        }


        /// <summary>
        /// 提交产品库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            //checkButton
            webBrowser1.Document.GetElementById("checkButton").InvokeMember("click");

        }


        //确认送审
        private void button4_Click(object sender, EventArgs e)
        {
            webBrowser1.Document.GetElementById("dialog-reason").InnerText = "123456";
            //auditing
            //btn btn-success
            //var top = (from n in this.webBrowser1.Document.GetElementsByTagName("input").Cast<HtmlElement>()
            //           where n.InnerText == "确认送审"
            //           select n).First();
            //top.InvokeMember("click");

            int count = webBrowser1.Document.All.Count;
            for (int i = 0; i < count; i++)
            {
                HtmlElement GetElement = webBrowser1.Document.All[i];
                if (GetElement.GetAttribute("value") == "确认送审")
                {
                    GetElement.InvokeMember("click");                    
                }
            }

        }


        
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            
            NameValueCollection data = new NameValueCollection();
            CookieContainer my_cookie_container = new CookieContainer();
            string str_cookie = webBrowser1.Document.Cookie;
            MessageBox.Show(str_cookie);
            return;
            string[] cookies = str_cookie.Split(';');
            foreach (string str in cookies)
            {
                string[] cookieNameValue = str.Split('=');
                Cookie ck = new Cookie(cookieNameValue[0].Trim().ToString(), cookieNameValue[1].Trim().ToString());
                ck.Domain = "a.jumeiglobal.com";//必须写对
                my_cookie_container.Add(ck);
            }
            //SWVY4535570LIPF
            string sql_insert = string.Empty;
            string local_img_url = "c:\\img";
            string[] str_imgs = Directory.GetFiles(local_img_url);
            for (int i = 0; i < str_imgs.Length; i++)
            {
                string str_img_url = str_imgs[i].ToString();                                                                                //图片
                string s = UploadHelper.HttpUploadFile("http://a.jumeiglobal.com/GlobalProduct/ProductImageUpload", my_cookie_container, new string[] { str_img_url }, data);
                //string s = UploadHelper.HttpUploadFile("http://a.jumeiglobal.com/GlobalProduct/ProductImageUpload", my_cookie_container, new string[] { @"C:\Users\Administrator\Desktop\SWVY4535570LIPF\SWVY4535570LIPF-主图-1.jpg" }, data);
                s = s.Replace(@"\", "").Trim();
                s = s.Substring(s.IndexOf("http")).Replace("\"}", "");
                //MessageBox.Show(s);
                sql_insert += "insert into bms_jumei_sku_image values(null,'" + textBox1.Text.Trim() + "','" + s + "');";
            }

            int count = ExecuteSql(sql_insert);
            if (count > 0)
                MessageBox.Show("上传成功");
            else
                MessageBox.Show("上传失败");
        }



        /// <summary>
        /// 上传产品库
        /// </summary>
        /// <returns></returns>
        public string upload_product(CookieContainer my_cookie_container,string image_url)
        {
            string request_url_1 = string.Format("http://a.jumeiglobal.com");
            string request_url_2 = string.Format("http://a.jumeiglobal.com/&uid=5485&access_token=0c0f5e39cbf68d00f32bd8446c4e0ab1&language=zh");
            string request_url_3 = string.Format("http://a.jumeiglobal.com/?request_uri=http://a.jumeiglobal.com/&uid=5485&access_token=0c0f5e39cbf68d00f32bd8446c4e0ab1&language=zh");
            string request_url_4 = string.Format("https://v.jumei.com/v1/api/login");
            string request_url_5 = string.Format("https://v.jumei.com/v1/api/login.do?app_id=e27e3ecab118&request_uri=http://a.jumeiglobal.com/&tag=9");
            string request_url_6 = string.Format("http://click.srv.jumei.com:8080/");
            string request_url_7 = string.Format("http://a.jumeiglobal.com/GlobalProduct/ApplyNew");


            string s = string.Empty;
            string content = string.Empty;                            //参数
            NameValueCollection data = new NameValueCollection();

            content = string.Format(@"product[category_v3_1]={0}&product[category_v3_2]={1}&product[category_v3_3]={2}&product[category_v3_4]={3}
&product[brand_id]={4}&product[name]={5}&product[foreign_language_name]={6}&product[images]={7}
&spu_1[id]={8}&spu_1[upc_code]={9}&spu_1[property]={10}&spu_1[size]={11}&spu_1[attributes]={12}&spu_1[abroad_price]={13}&spu_1[area_code]={14}
&spu_1[abroad_url]={15}&spu_1[images]={16}&spu_1[other_images]={17}
&auditing={18}&product[trial_reason]={19}",
                                          "485","526","529","530","5532",
                                          "GUESS", "GUESS", image_url, "-1", "SWVY4535570LIPF", "MS", "18", "", "700", "19",
                                          "","","","送审","123456");
            //data.Add("product[category_v3_1]", "485");
            //data.Add("product[category_v3_2]", "526");
            //data.Add("product[category_v3_3]", "529");
            //data.Add("product[category_v3_4]", "530");
            //data.Add("product[brand_id]", "5532");
            //data.Add("product[name]", "GUESS");
            //data.Add("product[foreign_language_name]", "GUESS");
            //data.Add("product[images]", "");
            //data.Add("spu_1[id]", "-1");
            //data.Add("spu_1[upc_code]", "SWVY4535570LIPF");
            //data.Add("spu_1[property]", "MS");
            //data.Add("spu_1[size]", "18");
            //data.Add("spu_1[attributes]", "");
            //data.Add("spu_1[abroad_price]", "700");
            //data.Add("spu_1[area_code]", "19");
            //data.Add("spu_1[abroad_url]", "");
            //data.Add("spu_1[images]", "");
            //data.Add("spu_1[other_images]", "");
            //data.Add("auditing", "送审");
            //data.Add("product[trial_reason]", "123456");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(request_url_7);
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = new CookieContainer();
            request.CookieContainer = my_cookie_container;
            request.KeepAlive = false;
            request.Method = "post";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";
            request.AllowAutoRedirect = false;

            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.UTF8);
            myStreamWriter.Write(content);
            myStreamWriter.Close();


            HttpWebResponse response_body = (HttpWebResponse)request.GetResponse();
            StreamReader read_body = new StreamReader(response_body.GetResponseStream(), Encoding.UTF8);
            s = read_body.ReadToEnd();


            return s;
        }


        DataTable dt = new DataTable();
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            string sku = textBox1.Text.Trim(); //货号
            string brand = string.Empty; //品牌            
            //get_server_img("Guess", "HWAMY1L5265ANRF");
            MySqlParameter[] my_params = new MySqlParameter[] { 
                new MySqlParameter("@sku",sku)
            };
            string sql_select = "select brand_name from bms_item where item_sku=@sku";
            
            DataTable dt = Query(sql_select,my_params);
            if (dt != null && dt.Rows.Count > 0)
            {
                brand = dt.Rows[0]["brand_name"].ToString();
            }
            else
            {
                return;
            }
            

            get_server_img(brand, sku);

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
        public void get_server_img(string brand, string sku_code)
        {
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
                NetworkCredential cred = new NetworkCredential("administrator", "123456", "192.168.1.19");
                client.Credentials = cred;

                string net_url_image = string.Empty;                                                                                                                    //网络共享图片
                net_url_image = @"\\192.168.1.19\data\";
                string[] brands = Directory.GetDirectories(net_url_image, brand);
                if (brands.Length > 0)
                {
                    net_url_image = brands[0] + @"\商品图片\";
                }
                string[] sku_codes = Directory.GetDirectories(net_url_image, sku_code);
                if (sku_codes.Length > 0)
                {
                    net_url_image = sku_codes[0] + @"\聚美\";
                }

                string[] net_image = Directory.GetFiles(net_url_image);
                for (int i = 0; i < net_image.Length; i++)
                {
                    if (net_image[i].Contains("主图"))
                    {
                        string image_name = System.IO.Path.GetFileName(net_image[i].ToString());
                        client.DownloadFile(net_image[i].ToString(), "c:\\img\\" + image_name);
                    }
                }

                MessageBox.Show("下载成功！");
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



        /// <summary>
        /// 打开浏览器提交信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            string str_cookie = webBrowser1.Document.Cookie;
            //Process.Start("chrome.exe", "http://192.168.1.124:8055/index");
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"chrome.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.Arguments = "http://192.168.1.124:8055/index";//要传的参数
            
            Process.Start(startInfo);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (webBrowser1.Url.AbsoluteUri == "http://a.jumeiglobal.com/GlobalProduct/ApplyNew")
            {
                webBrowser1.Navigate("http://a.jumeiglobal.com/GlobalProduct/List"); //产品列表
            }
            else if (webBrowser1.Url.AbsoluteUri == "http://a.jumeiglobal.com/GlobalProduct/List")
            {
                webBrowser1.Navigate("http://a.jumeiglobal.com/GlobalProduct/ApplyNew"); //申请新产品
            }
            else
            {
                webBrowser1.Document.GetElementById("userName").InnerText = "apennie";
                webBrowser1.Document.GetElementById("password").InnerText = "yapnjm32#@";
                webBrowser1.Document.GetElementById("submit-btn").InvokeMember("click");

                webBrowser1.DocumentCompleted += webBrowser1_DocumentCompleted;
            }
        }

        private void webBrowser1_DocumentCompleted_1(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }



    }
}
