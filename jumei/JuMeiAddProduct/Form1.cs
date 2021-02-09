using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Threading;

namespace JuMeiAddProduct
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static string connectionString = System.Configuration.ConfigurationManager.AppSettings["connectionString"];
        //存放文件路径
        public static string filepath = string.Empty;

        /// <summary>
        /// 存放尺码数组
        /// </summary>
        public static string[] sizeli;
        private void Form1_Load(object sender, EventArgs e)
        {
            //选择文件    上传文件按钮不可见
            button2.Visible = false;
            textBox1.Visible = false;
            button3.Visible = false;
            btn_cancle.Visible = false;
            label3.Visible = false;
            progressBar1.Visible = false;
            webBrowser1.ScriptErrorsSuppressed = true;
            //窗体加载登录界面
            webBrowser1.Navigate("http://a.jumeiglobal.com");
        }

        public delegate void MyInvoke();
        /// <summary>
        /// 登录按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.textBox3.Text.Trim())||string.IsNullOrWhiteSpace(this.textBox4.Text.Trim()))
            {
                MessageBox.Show("用户名或密码不能为空！");
                return;
            }

            //一分钟刷新一次页面
            timer1.Interval = 1000 * 60;
            timer1.Enabled = true;
            timer1.Tick += timer1_Tick;
            timer1.Start();
            //加载首页  调用登录按钮click方法
            webBrowser1.Navigate("http://a.jumeiglobal.com");
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Document.GetElementById("userName").InnerText = this.textBox3.Text.Trim();// "apennie"
            webBrowser1.Document.GetElementById("password").InnerText = this.textBox4.Text.Trim();//"58ypnjm%@"
            webBrowser1.Document.GetElementById("submit-btn").InvokeMember("click");

            webBrowser1.DocumentCompleted += webBrowser1_DocumentCompleted;
            //  webBrowser1.Navigate("http://a.jumeiglobal.com/GlobalProduct/ApplyNew");
        }

        /// <summary>
        /// 定时请求页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                webBrowser1.Document.GetElementById("userName").InnerText = this.textBox3.Text.Trim();
                webBrowser1.Document.GetElementById("password").InnerText = this.textBox4.Text.Trim();
                webBrowser1.Document.GetElementById("submit-btn").InvokeMember("click");
                webBrowser1.DocumentCompleted += webBrowser1_DocumentCompleted;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //OpenFileDialog openFile = new OpenFileDialog();
            // openFile.Filter = "Excel(*.xlsx)|*.xlsx|Excel(*.xls)|*.xls";
            // openFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            // openFile.Multiselect = false;
            this.openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);//默认浏览路径
         //   this.openFileDialog1.Filter = "Excel(*.xlsx)|*.xlsx|Excel(*.xls)|*.xls";

            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
                filepath = this.openFileDialog1.FileName;

            }
            //else if (this.openFileDialog1.ShowDialog() == DialogResult.Cancel)
            //{
            //    this.textBox1.Text ="";
            //    return;
            //}
        }

        /// <summary>
        /// 请求上架页面    请求成功则将后续操作按钮改为可见
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.ReadyState == WebBrowserReadyState.Complete)
            {
                webBrowser1.Navigate("http://a.jumeiglobal.com/GlobalProduct/ApplyNew");//申请新品页面
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
            if (str_cookie.Contains("_JMCOOKIE"))
            {
                button2.Visible = true;
                textBox1.Visible = true;
                button3.Visible = true;
                btn_cancle.Visible = true;
            }
            // webBrowser1.Navigate("http://a.jumeiglobal.com/GlobalProduct/ApplyNew");
            //MessageBox.Show(str_cookie);
        }

        private void webBrowser1_LocationChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        List<ProductInfo> li = new List<ProductInfo>();

        List<ProductInfo> listImgSuc = new List<ProductInfo>();//图片上传成功



        List<string> successli = new List<string>();//存放上架成功的商品
        /// <summary>
        /// 上传按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = "文件正在上传，请不要关闭窗口或者做其他操作";
            //得到本地文件路径     判断文件路径是否为空 文件是否存在
            if (!string.IsNullOrWhiteSpace(filepath))
            {
                FileInfo f = new FileInfo(filepath);
                if (!f.Exists)
                {
                    MessageBox.Show("文件不存在！请检查文件路径");
                    return;
                }
                //获取集合对象过程  获取商品信息   上传下载图片  操作结果
                string msg = string.Empty;
                label3.Visible = true;
                progressBar1.Visible = true;
                backgroundWorker1.RunWorkerAsync();
                // this.listBox1.Text = sb.ToString();
                // MessageBox.Show("恭喜恭喜，文件上传完毕！");
                //4.0 将结果显示在文本框中         
            }
            else
            {
                MessageBox.Show("请选择Excel文件！");
            }
        }
        /// <summary>
        /// 取消上传按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_cancle_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = string.Empty;
            //提前终止后台程序操作
            backgroundWorker1.CancelAsync();
            MessageBox.Show("已经取消文件上传");
        }

        /// <summary>
        /// 调用BackgroundWorker的RunWorkerAsync()方法，当调用此方时，BackgroundWorker 通过触发DoWork 事件，开始执行后台操作，DoWork 事件的代码是在另一个线程里执行的。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            StringBuilder sb = new StringBuilder(200);
            string msg = string.Empty;
            //List<ProductInfo> li = new List<ProductInfo>();
            ReadExcGetList(filepath, out msg);
            if (!string.IsNullOrWhiteSpace(msg))
            {
                if (li.Count<=0&&msg.Contains("失败"))
                {
                    MessageBox.Show(msg);
                    return;
                }
                MessageBox.Show(msg);
            }
            //3.0 遍历对象集合post提交表单
            //StringBuilder sb = new StringBuilder(200);//存放参数
            for (int i = 0; i < li.Count; i++)
            {
                //类目
                sb.Append("&product[category_v3_1]=" + li[i].Category1 + "&product[category_v3_2]=" + li[i].Category2 + "&product[category_v3_3]=" + li[i].Category3 + "&product[category_v3_4]=" + li[i].Category4);
                //品牌
                sb.Append("&product[brand_id]=" + li[i].Brand);
                //产品名称                    
                sb.Append("&product[name]=" + li[i].Title);
                //外文名
                sb.Append("&product[foreign_language_name]=" + li[i].Titleen);  
                string imgs = "{\"normal\":[{\"img\":\"" + li[i].Imgfilepath1 + "\",\"is_changed\":1},{\"img\":\"" + li[i].Imgfilepath2 + "\",\"is_changed\":1},{\"img\":\"" + li[i].Imgfilepath3 + "\",\"is_changed\":1},{\"img\":\"" + li[i].Imgfilepath4 + "\",\"is_changed\":1},{\"img\":\"" + li[i].Imgfilepath5 + "\",\"is_changed\":1},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0}],\"vertical\":[{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0}],\"diaoxing\":{\"img\":\"\",\"is_changed\":0}}";
                //图片
                sb.Append("&product[images]=" + imgs);
                //hidden
                if (sizeli==null)
                {
                    sb.Append("&spu_1[id]=" + "-1");
                    //sku
                    sb.Append("&spu_1[upc_code]=" + (li[i].Itemsku.Contains('+') ? li[i].Itemsku.Replace('+', ' ') : li[i].Itemsku));
                    //规格     固定
                    sb.Append("&spu_1[property]=OTHER");
                    //尺码
                    sb.Append("&spu_1[size]=" + li[i].Size);
                    //颜色
                    sb.Append("&spu_1[attributes]=" + li[i].Color);
                    //价格
                    sb.Append("&spu_1[abroad_price]=" + li[i].Price);
                    //价格单位    固定$
                    sb.Append("&spu_1[area_code]=19");

                    sb.Append("&spu_1[abroad_url]=" + "");
                    string imgss = "[{\"img\":\"" + li[i].Imgfilepath1 + "\",\"is_changed\":1},{\"img\":\"" + li[i].Imgfilepath2 + "\",\"is_changed\":1},{\"img\":\"" + li[i].Imgfilepath3 + "\",\"is_changed\":1},{\"img\":\"" + li[i].Imgfilepath4 + "\",\"is_changed\":1},{\"img\":\"" + li[i].Imgfilepath5 + "\",\"is_changed\":1},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0}]";
                    //白底方图   固定格式
                    sb.Append("&spu_1[images]=" + imgss);
                    //竖图    固定格式
                    string shutu = "{\"vertical_images\":[{ \"img\":\"\",\"is_changed\":0},{ \"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{ \"img\":\"\",\"is_changed\":0},{ \"img\":\"\",\"is_changed\":0},{ \"img\":\"\",\"is_changed\":0},{ \"img\":\"\",\"is_changed\":0},{ \"img\":\"\",\"is_changed\":0},{ \"img\":\"\",\"is_changed\":0},{ \"img\":\"\",\"is_changed\":0}]}";
                    sb.Append("&spu_1[other_images]=" + shutu); 
                }
                else
                {
                    for (int j = 0; j < sizeli.Length; j++)
                    {
                        int ccc = j+1;
                    sb.Append("&spu_"+ccc+"[id]=" + "-1");
                    //sku
                    sb.Append("&spu_"+ccc+"[upc_code]=" + (li[i].Itemsku.Contains('+') ? li[i].Itemsku.Replace('+', ' ') : li[i].Itemsku)+sizeli[j].ToString());
                    //规格     固定
                    sb.Append("&spu_"+ccc+"[property]=OTHER");
                    //尺码
                    sb.Append("&spu_"+ccc+"[size]=" + sizeli[j].ToString());
                    //颜色
                    sb.Append("&spu_"+ccc+"[attributes]=" + li[i].Color);
                    //价格
                    sb.Append("&spu_"+ccc+"[abroad_price]=" + li[i].Price);
                    //价格单位    固定$
                    sb.Append("&spu_"+ccc+"[area_code]=19");

                    sb.Append("&spu_"+ccc+"[abroad_url]=" + "");
                    string imgss = "[{\"img\":\"" + li[i].Imgfilepath1 + "\",\"is_changed\":1},{\"img\":\"" + li[i].Imgfilepath2 + "\",\"is_changed\":1},{\"img\":\"" + li[i].Imgfilepath3 + "\",\"is_changed\":1},{\"img\":\"" + li[i].Imgfilepath4 + "\",\"is_changed\":1},{\"img\":\"" + li[i].Imgfilepath5 + "\",\"is_changed\":1},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0}]";
                    //白底方图   固定格式
                    sb.Append("&spu_"+ccc+"[images]=" + imgss);
                    //竖图    固定格式
                    string shutu = "{\"vertical_images\":[{ \"img\":\"\",\"is_changed\":0},{ \"img\":\"\",\"is_changed\":0},{\"img\":\"\",\"is_changed\":0},{ \"img\":\"\",\"is_changed\":0},{ \"img\":\"\",\"is_changed\":0},{ \"img\":\"\",\"is_changed\":0},{ \"img\":\"\",\"is_changed\":0},{ \"img\":\"\",\"is_changed\":0},{ \"img\":\"\",\"is_changed\":0},{ \"img\":\"\",\"is_changed\":0}]}";
                    sb.Append("&spu_"+ccc+"[other_images]=" + shutu); 
                    }
                }
                //送审    固定
                sb.Append("&auditing=送审");
                //描述    固定
                sb.Append("&product[trial_reason]=新");

                li[i].Postpara = sb.ToString();
                sb.Clear();
            }
            for (int i = 0; i < li.Count; i++)
            {
                string result = postProduct(li[i].Postpara);
                // <div class=\"alert alert-error\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\">×</button><strong>提示!</strong><br/> 一级类目有误<br/>海外官网价格式错误</div>                                         </div>
               // backgroundWorker1.ReportProgress(i * 10);
                // sb.Append(result);

                if (string.IsNullOrWhiteSpace(result))
                {
                    successli.Add(li[i].Itemsku);
                }
                else
                {
                    int error = result.IndexOf("<strong>提示!</strong>");
                    int error2 = result.IndexOf("</div", error + 1);
                    result = result.Substring(error + 20, error2 - error - 20).Replace("\n", "").Replace("<br/>", "****");
                    if (result.Contains("<a"))
                    {
                        int error3 = result.IndexOf("<a", 0);
                        result=result.Substring(0, error3);
                    }
                    li[i].Postresult += result.Replace(" ","");;
                }
            }
            MyInvoke mi = new MyInvoke(ReturnResult);
            this.BeginInvoke(mi);
            //this.listBox1.Text = sb.ToString();
            //int input = (int)e.Argument;
            //Thread.Sleep(input);
        }

        public void ReturnResult()
        {
            StringBuilder resultsb = new StringBuilder(200);
            if (successli.Count > 0)
            {
                resultsb.Append("上传成功货号：");
                foreach (string item in successli)
                {
                    resultsb.Append(item + "---");
                }
                resultsb.Append(Environment.NewLine);
            }

            foreach (ProductInfo item in li)
            {
                if (!string.IsNullOrWhiteSpace(item.Postresult))
                {
                    resultsb.Append("货号：" + item.Itemsku + "---" + item.Postresult + Environment.NewLine);
                }                
            }
            this.textBox2.Text = string.Empty;
            //this.textBox2.Text = resultsb.ToString();
            this.textBox2.Text = resultsb.ToString();
            //MessageBox.Show("恭喜恭喜，文件上传完毕");
        }



        /// <summary>
        /// 显示后台操作进度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        #region post提交   得到返回结果
        /// <summary>
        /// post提交   
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string postProduct(string param)
        {
            //读取cookie
            string jumei_cookie = System.IO.File.ReadAllText("c:\\jumei_cookie.txt", Encoding.UTF8);
            if (string.IsNullOrWhiteSpace(jumei_cookie))
            {
                return "请先打开桌面客户端登录";
            }
            NameValueCollection datas = new NameValueCollection();
            CookieContainer my_cookie_container = getCookie(jumei_cookie);
            ASCIIEncoding encoding = new ASCIIEncoding();
            //string postData = "userName=apennie&password=yapnjm32#@&encryptParam=07f0d772f1f90f2c452ef7ba933d9bf6";//{'userName':'apennie','password':'ypnjm21@#'}

            byte[] data = encoding.GetBytes(param);
            HttpWebRequest myrequest = (HttpWebRequest)WebRequest.Create("http://a.jumeiglobal.com/GlobalProduct/ApplyNew");
            //http://a.jumeiglobal.com/?request_uri=http%3A%2F%2Fa.jumeiglobal.com%2F&session=%2BVW8jXPKaUWSzORY76cKuzL2GRjiz2zWBiHmUf%2F5Iqs%3D&language=zh&expires=1460517817231
            myrequest.Method = "POST";
            myrequest.Accept = "text/html, application/xhtml+xml,application/xml;q=0.9,image/webp, */*;q=0.8";
            myrequest.ContentType = "application/x-www-form-urlencoded";
            myrequest.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko";
            myrequest.AllowAutoRedirect = false;
            myrequest.KeepAlive = true;
            myrequest.CookieContainer = my_cookie_container;
            Stream newStream = myrequest.GetRequestStream();
            StreamWriter sw = new StreamWriter(newStream, Encoding.UTF8);
            sw.Write(param);
            sw.Close();
            HttpWebResponse myResponse = (HttpWebResponse)myrequest.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string content = reader.ReadToEnd();
            return content;
        }
        #endregion

        #region 读取excel  上传下载图片  返回完整实体对象集合
        /// <summary>
        /// 读取excel对实体对象集合赋值
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public void ReadExcGetList(string filePaths, out string msg)
        {
            StringBuilder sb = new StringBuilder(200);//存放所有货号上传情况
            msg = "";
            #region 1.0将excel表格中的数据转换成datatable
            DataSet OleDsExcle = new DataSet();
            try
            {
                string strConn;
                //strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePaths + ";Extended Properties='Excel 8.0;HDR=False;IMEX=1'";
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePaths + ";Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1'";
                OleDbConnection OleConn = new OleDbConnection(strConn);
                OleConn.Open();
                String sql = "SELECT * FROM  [Sheet1$]";//可是更改Sheet名称，比如sheet2，等等   
                OleDbDataAdapter OleDaExcel = new OleDbDataAdapter(sql, OleConn);
                OleDaExcel.Fill(OleDsExcle, "Sheet1");
                OleConn.Close();
            }
            catch (Exception ex)
            {
                msg = "读取Excel失败，原因：" + ex.Message;
                return;
            }
            #endregion

            int count = OleDsExcle.Tables[0].Rows.Count;
            if (count <= 0)
            {
                msg = "Excel表格中无数据，请检查";
                return;
            }
            #region 2.0 遍历表格数据 初始化实体对象基本数据
            try
            {
                foreach (DataRow row in OleDsExcle.Tables[0].Rows)
                {
                    li.Add(new ProductInfo()
                    {
                        Itemsku = row["商家编码"].ToString(),
                        Title = row["产品名"].ToString(),
                        Brand = row["品牌"].ToString().Split('-')[1],
                        Titleen = row["外文名"].ToString(),
                        Price = row["价格"].ToString(),
                        Size = row["尺码"].ToString(),
                        Color = row["颜色"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                msg = "遍历表格数据失败，请检查表格，出错原因："+ex.Message;
            }
          
            #endregion
            //string str = "第一行" + Environment.NewLine + "第二行";
            string down_image_str = string.Empty;//下载图片结果字符串
            string upload_image_str = string.Empty;//上传图片结果字符串
            #region 3.0 根据货号 下载图片  上传图片得到图片路径 继续为试题对象赋值
            for (int i = 0; i < li.Count; i++)
            {
                string itemsku = li[i].Itemsku;
                if (li[i].Size.Contains('-'))
                {
                    sizeli = li[i].Size.Split('-');
                    itemsku += sizeli[0];
                }
                //下载图片 返回下载结果字符串   已经拼接好
                down_image_str = down_image(itemsku);
                if (down_image_str.Contains("成功"))//图片下载成功  开始上传
                {
                    //sb.Append(down_image_str + ",");//货号“”图片下载成功
                    upload_image_str = upload_image(itemsku);//上传图片  得到五个主图返回路径
                    if (upload_image_str.Contains("请重新登录"))
                    {
                        return;//提示打开客户端，重新登录
                    }
                    if (upload_image_str.Contains("上传失败"))
                    {
                        sb.Append(upload_image_str); //拼接字符串提示该货号失败原因     跳过该货号，继续下一个货号
                        continue;
                    }
                    if (upload_image_str.Contains("上传成功"))
                    {
                        string[] url = upload_image_str.Split('$');
                        li[i].Imgfilepath1 = url[0];
                        li[i].Imgfilepath2 = url[1];
                        li[i].Imgfilepath3 = url[2];
                        li[i].Imgfilepath4 = url[3];
                        li[i].Imgfilepath5 = url[4];
                        listImgSuc.Add(li[i]);
                    }
                }
                else//下载失败，跳过继续下一个货号
                {
                    sb.Append(down_image_str + Environment.NewLine);
                    continue;
                }
                li[i].Postresult += sb.ToString();
            }
            #endregion
            msg = sb.ToString();
            //return listImgSuc;
        }
        #endregion

        #region 上传图片
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="item_sku"></param>
        /// <returns></returns>
        public string upload_image(string sku_id)
        {
            string imgurl = string.Empty;//图片url
            int j = 0;//上传成功图片数量
            try
            {
                string sql_delete = "delete from bms_jumei_sku_image where sku = '" + sku_id.Replace('+', '/') + "';";
                ExecuteSql(sql_delete);
                string s_log = string.Empty;

                string jumei_cookie = System.IO.File.ReadAllText("c:\\jumei_cookie.txt", Encoding.UTF8);
                //s_log += "1";
                if (string.IsNullOrWhiteSpace(jumei_cookie))
                {
                    return "请重新登录！";
                }
                NameValueCollection data = new NameValueCollection();
                CookieContainer my_cookie_container = getCookie(jumei_cookie);
                string sql_insert = string.Empty;
                string local_img_url = "c:\\img";
                string[] str_imgs = Directory.GetFiles(local_img_url);
                string img_value = string.Empty;
                for (int i = 0; i < str_imgs.Length; i++)
                {
                    FileInfo fi = new FileInfo(str_imgs[i]);
                    if (fi.Length < 1048576)
                    {
                        string str_img_url = str_imgs[i].ToString();                                                                                //图片
                        img_value = HttpUploadFile("http://a.jumeiglobal.com/GlobalProduct/ProductImageUpload", my_cookie_container, new string[] { str_img_url }, data, Encoding.UTF8);
                        img_value = img_value.Replace(@"\", "").Trim();
                        img_value = img_value.Substring(img_value.IndexOf("http")).Replace("\"}", "");
                        if (img_value.Contains("http"))
                        {
                            j++;
                            imgurl += img_value + "$";
                        }
                        else
                        {
                            return "货号:" + sku_id + "图片上传失败";
                        }
                        sql_insert += "insert into bms_jumei_sku_image values(null,'" + ((sku_id.Contains('+'))? sku_id.Trim().Replace('+', '/') : sku_id.Trim()) + "','" + img_value + "'," + (i + 1) + ");";
                    }

                }
                int count = ExecuteSql(sql_insert);
                if (count > 0 && j >= 5)
                    return imgurl + "图片上传成功";
                else
                    return "货号:" + sku_id + "图片上传失败";
            }
            catch (Exception ex)
            {
                return "货号:" + sku_id + "图片上传失败，出错了：" + ex.Message;
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

        #region 上传文件
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

        #endregion

        #region  执行SQL语句，返回影响的记录数
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

        #region 下载图片
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public string down_image(string sku_id)
        {
            //接口密钥
            string key = "9763134a645adbb9ca1abafa7ec7db34";
            string message = string.Empty;//存放该货号下载图片情况
            //1.0调用接口获取图片路径
            try
            {
                WebClient wc = new WebClient();
                wc.Encoding = System.Text.Encoding.UTF8;
                //if (sku_id.Contains("+"))
                //{
                //    sku_id = sku_id.Replace('+', '/');
                //}
                if (sku_id=="33547SV+ETF")
                {
                    

                }
                string sku2 = sku_id.Contains("+") ? sku_id.Replace("+", "%2F") : sku_id;
                //replace("+","%2B")
                string data = wc.DownloadString(string.Format(@"http://112.74.75.82:8023/api/GetImageFilePath/GetFilePathBySku?skus={0}&key={1}", sku2, key));
                if (data.Contains("成功"))//获取路片路径成功    http://112.74.75.82:8023/
                {
                    data = data.Replace("\"", "");
                    string[] filepath = data.Split('#');
                    //2.0根据图片路径到图片服务器下载图片到本地
                    message = downLoadServerImg(filepath, sku_id);
                    if (message.Contains("成功"))
                    {
                        return "货号：" + sku_id + "下载图片成功";
                    }
                }
                else
                {
                    //获取图片路径出错，“货号+出错原因”
                    return "货号：" + sku_id + "下载图片失败---获取图片路径失败，原因:" + data;
                }
            }
            catch (Exception ex)
            {
                return "货号：" + sku_id + "下载图片出错---获取图片路径失败，原因:" + ex.Message;
                //  throw;
            }

            return "货号：" + sku_id + message;

        }

        #endregion

        #region 下载服务器图片
        /// <summary>
        /// 下载服务器图片
        /// </summary>
        /// <param name="filePath">图片路径数组集合</param>
        /// <param name="item_sku">货号</param>
        /// <returns></returns>
        public string downLoadServerImg(string[] filePath, string item_sku)
        {

            string s_log = string.Empty;
            string down_state = string.Empty; //下载状态
            int j = 0;
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
                // NetworkCredential cred = new NetworkCredential("SKY", "123456", "192.168.1.19:2222");
                client.Credentials = cred;
                string serverpath = string.Empty;
                for (int i = 0; i < 5; i++)//五个图片
                {
                    serverpath = filePath[i];
                    serverpath = serverpath.Replace("D://DATA//DATA//", "ftp:////192.168.1.19:2222//data//").Replace("//", "/");
                    string image_name = item_sku.Replace('/', '+') + "-主图-" + (i + 1).ToString() + ".jpg";
                    down_state = downLoadSJProductReport(serverpath, image_name);
                    if (down_state.Contains("失败"))
                    {
                        return down_state;
                    }
                    else if (down_state.Contains("成功"))
                    {
                        j++;
                        //return down_state;
                    }
                    down_state = string.Empty;
                    serverpath = string.Empty;
                }
                s_log += "4---";
            }

            catch (Exception ex) { return down_state = "下载失败！(" + ex.Message + ex.StackTrace + s_log + ")"; }
            return j == 5 ? down_state + "成功" : down_state + "失败";
        }

        #endregion

        #region 获取商检商检回执,并下载到本地(c:\img\)
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

        #endregion

        #region 下载文件
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

            //FtpWebRequest reqFTP;

            //try
            //{
            //    FileStream outputStream = new FileStream(filename, FileMode.Create);

            //    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(serverIP));

            //    reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;

            //    reqFTP.UseBinary = true;

            //    reqFTP.Credentials = new NetworkCredential(userName, password);

            //    FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();

            //    Stream ftpStream = response.GetResponseStream();

            //    long cl = response.ContentLength;

            //    int bufferSize = 2048;

            //    int readCount;

            //    byte[] buffer = new byte[bufferSize];

            //    readCount = ftpStream.Read(buffer, 0, bufferSize);

            //    while (readCount > 0)
            //    {
            //        outputStream.Write(buffer, 0, readCount);

            //        readCount = ftpStream.Read(buffer, 0, bufferSize);
            //    }

            //    ftpStream.Close();

            //    outputStream.Close();

            //    response.Close();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //return 1;
        }

        #endregion

        private void timer1_Tick_1(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("文件上传完毕！", "恭喜恭喜");
        }
    }
}
