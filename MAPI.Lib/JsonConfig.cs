using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MAPI.Lib
{
    /// <summary>
    /// Json Config
    /// 配置文件
    /// </summary>
    public class JsonConfig
    {
        /// <summary>
        /// 配置文件FileJson地址
        /// </summary>
        public virtual string AbsoluteJsonFilePath
        { get { return AppDomain.CurrentDomain.BaseDirectory + @"Config.Json"; } }
        protected JObject _jObject = null;

        /// <summary>
        /// Json配置对象
        /// </summary>
        public virtual JObject JObject
        {
            get
            {
                if (_jObject == null)
                {
                    //判断磁盘未找到文件,创建Json文件
                    if(!Directory.Exists(Path.GetDirectoryName(AbsoluteJsonFilePath)))
                    { Directory.CreateDirectory(Path.GetDirectoryName(AbsoluteJsonFilePath)); }

                    if (!System.IO.File.Exists(AbsoluteJsonFilePath))
                    {
                        FileStream _createFile = File.Open(AbsoluteJsonFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
                        //获得字节数组
                        byte[] data = System.Text.Encoding.Default.GetBytes("{\"IsDebug\":\"1\"}");
                        //开始写入
                        _createFile.Write(data, 0, data.Length);
                        //清空缓冲区、关闭流
                        _createFile.Flush();
                        _createFile.Close();
                    }
                    StreamReader file = File.OpenText(AbsoluteJsonFilePath);
                    JsonTextReader reader = new JsonTextReader(file);
                    _jObject = (JObject)JToken.ReadFrom(reader);
                    file.Close();
                }
                return _jObject;
            }
            set
            {
                _jObject = value;
                Modify();
            }
        }
        /// <summary>
        /// 修改对象
        /// </summary>
        /// <returns></returns>
        public virtual int Modify()
        {
            int modifyRowCount = 0;
            try
            {
                File.WriteAllText(AbsoluteJsonFilePath, JObject.ToString());
                modifyRowCount = 1;
            }
            catch (Newtonsoft.Json.JsonException se)
            { MAPI.Lib.SystemLog.WriteLogFromException(se, "修改Config.Json配置文件异常!"); }
            return modifyRowCount;
        }
        protected string _isDebug = null;
        /// <summary>
        /// 是否调试
        /// </summary>
        public string IsDebug
        {
            get
            {
                if (_isDebug == null)
                {
                    if (!JObject.ContainsKey("IsDebug"))
                    { IsDebug = "0"; }
                }
                return _isDebug;
            }
            set
            {
                _isDebug = value;
                JObject["IsDebug"] = _isDebug;
                Modify();
            }
        }
    }
}
