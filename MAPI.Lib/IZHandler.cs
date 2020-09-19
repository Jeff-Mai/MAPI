using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MAPI.Lib
{
    /// <summary>
    /// 请求处理接口，根据传进来的Request、Response处理请求并返回处理结果
    /// </summary>
    public interface IZHandler
    {
        /// <summary>
        /// 处理请求
        /// </summary>
        /// <param name="reqeust">请求Http内容</param>
        /// <param name="response">响应Http内容</param>
        void HandleRequest(HttpListenerRequest reqeust, HttpListenerResponse response);
    }
}
