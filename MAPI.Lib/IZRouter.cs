using System;

namespace MAPI.Lib
{
    /// <summary>
    /// 路由接口，根据URL找到对应的请求处理者
    /// </summary>
    public interface IZRouter
    {
        /// <summary>
        /// 获取路由接口
        /// </summary>
        /// <param name="url">URL接口</param>
        /// <returns></returns>
        IZHandler GetHandler(string url);
    }
}
