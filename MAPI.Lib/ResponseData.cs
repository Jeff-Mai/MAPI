using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MAPI.Lib
{
    /// <summary>
    /// HTTP响应结果
    /// </summary>
    [DataContract]
    public class ResponseData
    {
        /// <summary>
        /// 消息
        /// 100：客户端应当继续发送请求。
        /// 成功
        /// 200：请求已成功。
        /// 重定向
        /// 300：被请求的资源有一系列可供选择的回馈信息，每个都有自己特定的地址和浏览器驱动的商议信息。
        /// 请求错误
        /// 400：语义有误，当前请求无法被服务器理解。除非进行修改，否则客户端不应该重复提交这个请求。
        /// Code:200,404
        /// </summary>
        [DataMember]
        public int Code { get; set; }
        /// <summary>
        /// 成功/失败消息
        /// </summary>
        [DataMember]
        public string Msg { get; set; }
        /// <summary>
        /// 响应数据
        /// </summary>
        [DataMember]
        public string Data { get; set; }
        /// <summary>
        /// Object 转 Json
        /// </summary>
        /// <returns></returns>
        public virtual string ToJson()
        {
            string _json = "";
            try
            { _json = Newtonsoft.Json.JsonConvert.SerializeObject(this); }
            catch (Newtonsoft.Json.JsonException je)
            {
                var _retJson = new
                {
                    Code = 500,
                    Msg = string.Format("服务器遇到了一个未曾预料的状况，导致了它无法完成对请求的处理。一般来说，这个问题都会在服务器端的源代码出现错误时出现。\r\n{0}", je.Message),
                    Data = Newtonsoft.Json.JsonConvert.SerializeObject(je)
                };
                _json = Newtonsoft.Json.JsonConvert.SerializeObject(_retJson);
            }
            return _json;
        }
    }
}
