using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

namespace MAPI.Server
{
    class Program
    {
        /// <summary>
        /// 路由器
        /// </summary>
        static MAPI.Lib.IZRouter _router;
        /// <summary>
        /// 请求处理者
        /// </summary>
        static MAPI.Lib.IZHandler _handler;
        static void Main(string[] args)
        {
            HttpListener httpListener = new HttpListener();
            httpListener.Prefixes.Add("http://+:8081/");
            httpListener.Start();
            //开始异步接收request请求
            httpListener.BeginGetContext(new AsyncCallback(OnGetContext), httpListener);
            Console.WriteLine("监听端口:8081...");
            Console.Read();
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="ar"></param>
        static void OnGetContext(IAsyncResult ar)
        {
            HttpListener httpListener = ar.AsyncState as HttpListener;
            //接收到的请求context（一个环境封装体）
            HttpListenerContext context = httpListener.EndGetContext(ar);
            //开始 第二次 异步接收request请求
            httpListener.BeginGetContext(new AsyncCallback(OnGetContext), httpListener);
            /*-------------------------开始处理请求-----------------------------*/
            //接收的request数据
            HttpListenerRequest request = context.Request;
            //用来向客户端发送回复
            HttpListenerResponse response = context.Response;

            StreamReader reader = new StreamReader(request.InputStream);
            string text = reader.ReadToEnd();

            //或者JObject jo = JObject.Parse(jsonText);
            JObject jo = (JObject)JsonConvert.DeserializeObject(text);

            Console.WriteLine(text);
            string Sign = jo["Sign"].ToString();//输出 "深圳"
            Console.WriteLine(Sign);
            int A1 = Convert.ToInt32(jo["A1"]);
            Console.WriteLine(A1.ToString());
            JObject jo1 = (JObject)JsonConvert.DeserializeObject(jo["Data"].ToString());
            string DBox = jo1["DBox"].ToString();
            Console.WriteLine(DBox);
            string DD =  jo1["DD"][0].ToString();
            Console.WriteLine(DD);
            //string Sign = jo1["Sign"].ToString();


            Console.WriteLine(request.RawUrl);

            Console.WriteLine(request.Url.OriginalString);
            Console.WriteLine(request.Url.AbsoluteUri);
            Console.WriteLine(request.Url.PathAndQuery);
            Console.WriteLine(request.Url.Query);

            Console.WriteLine(request.HttpMethod);
            Console.WriteLine(request.ContentType);
            //以下开始与 开发者编写的Web网站程序交互
            //找网站中的路由器
            //if (_router == null)
            //{
            //    Assembly assemble = Assembly.LoadFile(Environment.CurrentDirectory + "\\web\\" + website_name + ".dll"); //加载web目录下的网站程序
            //    Type type = assemble.GetType(website_name + ".Router");  //注意网站程序中的路由器必须命名为 ns.Router
            //    _router = Activator.CreateInstance(type) as MAPI.Lib.IZRouter;  //创建路由器的实例
            //}

            //_handler = _router.GetHandler(request.Url.AbsolutePath);  //根据路由器，找web网站中对应的处理者
            //if (_handler != null)
            //{
            //    _handler.HandleRequest(request, response);   //开始处理请求（代码运行流程进入web网站程序）
            //}


            //返回
            response.ContentType = "html";
            response.ContentEncoding = Encoding.UTF8;
            using (Stream output = response.OutputStream)  //发送回复
            {
                byte[] buffer = Encoding.UTF8.GetBytes("<html><head><title>Web Server--News</title></head><body>first news</br>second news</body></html>");
                output.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
