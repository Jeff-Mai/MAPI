using System;
using System.Collections.Generic;
using System.Text;

namespace MAPI.Server
{
    /// <summary>
    /// 运行服务配置
    /// </summary>
    public class Config : MAPI.Lib.JsonConfig
    {
        //protected List<string> _isDebug = null;
        ///// <summary>
        ///// APILib
        ///// </summary>
        //public string APILibs
        //{
        //    get
        //    {
        //        if (_isDebug == null)
        //        {
        //            if (!JObject.ContainsKey("APILibs"))
        //            { IsDebug = "0"; }
        //        }
        //        return _isDebug;
        //    }
        //    set
        //    {
        //        _isDebug = value;
        //        JObject["APILibs"] = _isDebug;
        //        Modify();
        //    }
        //}
    }
}