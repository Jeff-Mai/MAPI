using MAPI.Lib;
using System;
using MAPI.Test.Handlers;

namespace MAPI.Test
{
    /// <summary>
    /// 每个网站程序必备的路由器，命名必须为Router。
    /// 该路由器将为每个URL找到对应的处理者。
    /// </summary>
    public class Router : IZRouter
    {
        #region IZRouter 成员

        public IZHandler GetHandler(string url)
        {
            //根据URL格式  找到对应的处理者
            //此处简单匹配，完全为了举例
            if (url.EndsWith("/news"))
            {
                return new MyNewsHandler();
            }
            else if (url.EndsWith("/users"))
            {
                return new MyUsersHandler();
            }
            else if (url.EndsWith("/"))
            {
                return new MyIndexHandler();
            }
            //...
            return null;
        }

        #endregion
    }
}
