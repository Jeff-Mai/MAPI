using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace MAPI.Test.Handlers
{
    /// <summary>
    /// 请求News时的处理者  http://localhost:8081/news
    /// </summary>
    public class MyNewsHandler : MAPI.Lib.IZHandler
    {

        #region IZHandler 成员

        public void HandleRequest(HttpListenerRequest reqeust, HttpListenerResponse response)
        {
            Console.WriteLine("接收一个News请求：");
            Console.WriteLine("  HTTP头信息：");
            foreach (string key in reqeust.Headers.AllKeys)  //控制台输出请求信息
            {
                string[] values = reqeust.Headers.GetValues(key);
                if (values.Length > 0)
                {
                    string s = "";
                    foreach (string value in values)
                    {
                        s += value + ";";
                    }
                    Console.WriteLine("  " + key + "：" + s);
                }
            }
            //处理请求，从request对象中获取GET/POST提交的数据。
            //访问数据库
            //...

            response.ContentType = "html";
            response.ContentEncoding = Encoding.UTF8;
            using (Stream output = response.OutputStream)  //发送回复
            {
                byte[] buffer = Encoding.UTF8.GetBytes("<html><head><title>Web Server--News</title></head><body>first news</br>second news</body></html>");
                output.Write(buffer, 0, buffer.Length);
            }
        }

        #endregion
    }
}
