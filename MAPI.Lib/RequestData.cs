using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MAPI.Lib
{
    /// <summary>
    /// HTTP请求消息
    /// </summary>
    [DataContract]
    public class RequestData
    {
        /// <summary>
        /// 数字签名
        /// </summary>
        [DataMember]
        public string Sign { get; set; }
        /// <summary>
        /// 格林威治时间戳
        /// </summary>
        [DataMember]
        public int Timestamp { get; set; }
        /// <summary>
        /// 操作指令
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
                    Sign = this.Sign,
                    Timestamp = this.Timestamp,
                    Data = je.Message
                };
                _json = Newtonsoft.Json.JsonConvert.SerializeObject(_retJson);
            }
            return _json;
        }
    }
}
