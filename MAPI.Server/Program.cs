using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using MAPI.Lib;

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
            // 获取URL WebServer处理库名称
            var urlLibName = (from x in request.Url.Segments where x.Trim('/').Length > 0 select x).FirstOrDefault();
            urlLibName = urlLibName == null ? "" : urlLibName.Trim('/');
            //var libPath = string.Format(@"{0}\{1}\{1}.dll", Environment.CurrentDirectory, urlLibName);
            // WebServer处理库文件夹
            var directoryPath = string.Format(@"{0}\MAPI.Libs\", Environment.CurrentDirectory);
            // WebServer处理库文件
            var libPath = (from x in Directory.GetFiles(directoryPath)
                           where x.Contains(string.Format("{0}.dll", urlLibName))
                           select x).FirstOrDefault();
            if (libPath == null)
            {
                foreach (string path in Directory.GetDirectories(directoryPath))
                {
                    libPath = (from x in Directory.GetFiles(path)
                               where x.Contains(string.Format("{0}.dll", urlLibName))
                               select x).FirstOrDefault();
                    if (libPath != null) { break; }
                }
            }

            // 检查库是否存在运行目录
            if (urlLibName != null && File.Exists(libPath))
            {
                Console.WriteLine(libPath);
                // 返回：未指定URL WebServer处理库名称
                ResponseData rpData = new ResponseData()
                { Code = 100, Msg = "客户端应当继续发送请求。这个临时响应是用来通知客户端它的部分请求已经被服务器接收，且仍未被拒绝。", Data = urlLibName };
                response.ContentType = "application/json";
                response.ContentEncoding = Encoding.UTF8;
                //发送回复
                using (Stream output = response.OutputStream)
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(rpData.ToJson());
                    output.Write(buffer, 0, buffer.Length);
                }


                //找路由器
                if (_router == null)
                {
                    //加载当前目录下的处理程序
                    Assembly assemble = Assembly.LoadFile(libPath);
                    //注意网站程序中的路由器必须命名为 ns.Router
                    Type type = assemble.GetType(urlLibName + ".Router");
                    //创建路由器的实例
                    _router = Activator.CreateInstance(type) as MAPI.Lib.IZRouter;
                }
                //根据路由器，找web处理程序中对应的处理者
                _handler = _router.GetHandler(request.Url.AbsolutePath);
                if (_handler != null)
                {
                    //开始处理请求（代码运行流程进入web网站程序）
                    _handler.HandleRequest(request, response);
                }
            }
            else
            {
                // 返回：未指定URL WebServer处理库名称
                ResponseData rpData = new ResponseData()
                { Code = 404, Msg = "请求失败，请求所希望得到的资源未被在服务器上发现。", Data = "" };
                response.ContentType = "application/json";
                response.ContentEncoding = Encoding.UTF8;
                //发送回复
                using (Stream output = response.OutputStream)
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(rpData.ToJson());
                    output.Write(buffer, 0, buffer.Length);
                }
            }
        }
    }
}
