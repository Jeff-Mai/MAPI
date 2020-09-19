using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MAPI.Lib
{
    /// <summary>
    /// 运行日志
    /// </summary>
    public class RuningLog
    {
        /// <summary>
        /// 检查日志目录是否存在
        /// </summary>
        /// <returns></returns>
        private static string GetRunningLogDirectory()
        {
            string logDirectoryPath = AppDomain.CurrentDomain.BaseDirectory + @"RunningLog\";
            //日志目录是否存在 不存在创建
            if (!Directory.Exists(logDirectoryPath))
            { Directory.CreateDirectory(logDirectoryPath); }
            return logDirectoryPath;
        }
        /// <summary>
        /// 运行日志
        /// </summary>
        private static FileStream _runningLog = null;
        /// <summary>
        /// 运行日志
        /// </summary>
        /// <param name="msgType">消息类别</param>
        /// <param name="content">消息内容</param>
        private static void WriteRunningLog(string msgType, string content)
        {
            //await Task.Run(() =>
            //{
            if (_runningLog == null || !_runningLog.Name.Contains(DateTime.Now.ToString("yyyy-MM-dd")))
            { _runningLog = File.Open(GetRunningLogDirectory() + DateTime.Now.ToString("yyyy-MM-dd") + "Running.log", FileMode.Append, FileAccess.Write, FileShare.Read); }

            StringBuilder logInfo = new StringBuilder("");
            string currentTime = System.DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]");
            if (content != null)
            {
                logInfo.AppendLine("DateTime：" + currentTime.ToString());
                logInfo.AppendLine("Type：" + msgType);
                logInfo.AppendLine("Content：" + content);
                logInfo.AppendLine("-----------------------------------------------------------");
                logInfo.AppendLine();
                //获得字节数组
                byte[] data = System.Text.Encoding.Default.GetBytes(logInfo.ToString());
                //开始写入
                _runningLog.Write(data, 0, data.Length);
                //清空缓冲区、关闭流
                _runningLog.Flush();
            }
            //System.IO.File.AppendAllText(GetRunningLogDirectory() + DateTime.Now.ToString("yyyy-MM-dd") + "Running.log", logInfo.ToString());
            //});
        }
        /// <summary>
        /// 控制台看板,补录行
        /// </summary>
        private static void WriteLine(string content)
        {
            Console.WriteLine(content);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// 系统消息
        /// </summary>
        /// <param name="content"></param>
        public static void WriteLineMsg(string content)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("系统消息(SystemMsg):");
            WriteRunningLog("系统消息(SystemMsg)", content);
        }
        /// <summary>
        /// 请求消息
        /// </summary>
        /// <param name="content"></param>
        public static void RequestMsg(string content)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("请求消息(RequestMsg):");
            WriteLine(content);
            WriteRunningLog("请求消息(RequestMsg)", content);
        }
        /// <summary>
        /// 收到消息
        /// </summary>
        /// <param name="content"></param>
        public static void ReceivedMsg(string content)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("收到消息(ReceivedMsg):");
            WriteLine(content);
            WriteRunningLog("收到消息(ReceivedMsg)", content);
        }
        /// <summary>
        /// 响应消息
        /// </summary>
        /// <param name="content"></param>
        public static void ResponseMsg(string content)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("响应消息(ResponseMsg):");
            WriteLine(content);
            WriteRunningLog("响应消息(ResponseMsg)", content);
        }
        /// <summary>
        /// 广播消息
        /// </summary>
        /// <param name="content"></param>
        public static void BroadcastMsg(string content)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("广播消息(BroadcastMsg):");
            WriteLine(content);
            WriteRunningLog("广播消息(BroadcastMsg)", content);
        }
        /// <summary>
        /// 错误消息
        /// </summary>
        /// <param name="content"></param>
        public static void ErrorMsg(string content)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("错误消息(ErrorMsg):");
            WriteLine(content);
            WriteRunningLog("错误消息(ErrorMsg)", content);
        }
        /// <summary>
        /// 异常消息
        /// </summary>
        /// <param name="content"></param>
        public static void ExceptionMsg(string content)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("异常消息(ExceptionMsg):");
            WriteLine(content);
            WriteRunningLog("异常消息(ExceptionMsg)", content);
        }
    }
}
