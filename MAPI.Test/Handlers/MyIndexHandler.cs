using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace MAPI.Test.Handlers
{
    /// <summary>
    /// 默认请求处理者   http://locahost:8081/
    /// </summary>
    public class MyIndexHandler : MAPI.Lib.IZHandler
    {
        #region IZHandler 成员

        public void HandleRequest(HttpListenerRequest reqeust, HttpListenerResponse response)
        {
            Console.WriteLine("接收主页请求：");
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
                byte[] buffer = Encoding.UTF8.GetBytes("<html><head><title>Web Server--Home</title></head><body>Welcome to HomePage</body></html>");
                output.Write(buffer, 0, buffer.Length);
            }
        }

        #endregion
    }
}
