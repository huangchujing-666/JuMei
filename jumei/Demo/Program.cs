using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Demo
{
    class Program
    {
        private static String dir = @"C:\work\";
        static void Main(string[] args)
        {


            //if (webBrowser1.ReadyState == WebBrowserReadyState.Complete)
            //{
            //    webBrowser1.Navigate("http://a.jumeiglobal.com/GlobalProduct/ApplyNew");
            //}
            //webBrowser1.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
            #region MyRegion
            // HttpClient httpClient = new HttpClient();
            // httpClient.MaxResponseContentBufferSize = 256000;
            // httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");
            // String url = "http://a.jumeiglobal.com";
            // HttpResponseMessage response = httpClient.GetAsync(new Uri(url)).Result;
            // String result = response.Content.ReadAsStringAsync().Result;

            // String username = "apennie";
            // String password = "yapnjm32#@";
            // CookieContainer my_cookie_container = new CookieContainer();
            // Cookie[] cookies = httpClient.GetStringAsync.getState().getCookies();	
            // //string str_cookie = webBrowser1.Document.Cookie;
            //// string[] cookies = str_cookie.Split(';');
            // foreach (string str in cookies)
            // {
            //     string[] cookieNameValue = str.Split('=');
            //     Cookie ck = new Cookie(cookieNameValue[0].Trim().ToString(), cookieNameValue[1].Trim().ToString());
            //     ck.Domain = "a.jumeiglobal.com";//必须写对
            //     my_cookie_container.Add(ck);
            // }
            // System.IO.File.WriteAllText("c:\\jumei_cookie.txt", str_cookie, Encoding.UTF8); 
            #endregion
            //string uri = "http://www.windows.com/";
            //HttpClient client = new HttpClient();
            //string body = await client.GetStringAsync(uri);

            //HttpClient httpClient = new HttpClient();
            //httpClient.MaxResponseContentBufferSize = 256000;
            //httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");
            //String url = "http://a.jumeiglobal.com";
            //HttpResponseMessage response = httpClient.GetAsync(new Uri(url)).Result;
            //String result = response.Content.ReadAsStringAsync().Result;

            //String username = "apennie";
            //String password = "yapnjm32#@";

            //do
            //{
            //    String __EVENTVALIDATION = new Regex("id=\"__EVENTVALIDATION\" value=\"(.*?)\"").Match(result).Groups[1].Value;
            //    String __VIEWSTATE = new Regex("id=\"__VIEWSTATE\" value=\"(.*?)\"").Match(result).Groups[1].Value;
            //    String LBD_VCID_c_login_logincaptcha = new Regex("id=\"LBD_VCID_c_login_logincaptcha\" value=\"(.*?)\"").Match(result).Groups[1].Value;

            //    //图片验证码
            //    url = "http://passport.cnblogs.com" + new Regex("id=\"c_login_logincaptcha_CaptchaImage\" src=\"(.*?)\"").Match(result).Groups[1].Value;
            //    response = httpClient.GetAsync(new Uri(url)).Result;
            //    Write("amosli.png", response.Content.ReadAsByteArrayAsync().Result);

            //    Console.WriteLine("输入图片验证码：");
            //    String imgCode = "wupve";//验证码写到本地了，需要手动填写
            //    imgCode = Console.ReadLine();

            //    //开始登录
            //    url = "http://passport.cnblogs.com/login.aspx";
            //    List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            //    paramList.Add(new KeyValuePair<string, string>("__EVENTTARGET", ""));
            //    paramList.Add(new KeyValuePair<string, string>("__EVENTARGUMENT", ""));
            //    paramList.Add(new KeyValuePair<string, string>("__VIEWSTATE", __VIEWSTATE));
            //    paramList.Add(new KeyValuePair<string, string>("__EVENTVALIDATION", __EVENTVALIDATION));
            //    paramList.Add(new KeyValuePair<string, string>("tbUserName", username));
            //    paramList.Add(new KeyValuePair<string, string>("tbPassword", password));
            //    paramList.Add(new KeyValuePair<string, string>("LBD_VCID_c_login_logincaptcha", LBD_VCID_c_login_logincaptcha));
            //    paramList.Add(new KeyValuePair<string, string>("LBD_BackWorkaround_c_login_logincaptcha", "1"));
            //    paramList.Add(new KeyValuePair<string, string>("CaptchaCodeTextBox", imgCode));
            //    paramList.Add(new KeyValuePair<string, string>("btnLogin", "登  录"));
            //    paramList.Add(new KeyValuePair<string, string>("txtReturnUrl", "http://home.cnblogs.com/"));
            //    response = httpClient.PostAsync(new Uri(url), new FormUrlEncodedContent(paramList)).Result;
            //    result = response.Content.ReadAsStringAsync().Result;
            //    Write("myCnblogs.html", result);
            //} while (result.Contains("验证码错误，麻烦您重新输入"));

            //Console.WriteLine("登录成功！");

            ////用完要记得释放
            //httpClient.Dispose();

        }

        /// <summary>
        /// 写文件到本地
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="html"></param>
        public static void Write(string fileName, byte[] html)
        {
            try
            {
                File.WriteAllBytes(dir + fileName, html);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

        }
    }
}
