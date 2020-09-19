using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MAPI.Lib
{
    /// <summary>
    /// 系统日志
    /// </summary>
    public static class SystemLog
    {
        #region 公有变量,属性,方法(可重写)
        /// <summary>
        /// 检查日志目录是否存在
        /// </summary>
        /// <returns></returns>
        private static string GetSystemLogDirectory()
        {
            string logDirectoryPath = AppDomain.CurrentDomain.BaseDirectory + @"SysLog\";
            //日志目录是否存在 不存在创建
            if (!Directory.Exists(logDirectoryPath))
            { Directory.CreateDirectory(logDirectoryPath); }
            return logDirectoryPath;
        }
        /// <summary>
        /// 系统日志
        /// </summary>
        private static FileStream _systemLog = null;
        /// <summary>
        /// 系统错误日志
        /// </summary>
        private static FileStream _systemErrorLog = null;
        #endregion

        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="stackTrace">异常源</param>
        public static void WriteLogFromException(StackTrace stackTrace, string exceptionUserMsg)
        {
            if (_systemErrorLog == null || !_systemErrorLog.Name.Contains(DateTime.Now.ToString("yyyy-MM-dd")))
            { _systemErrorLog = File.Open(GetSystemLogDirectory() + DateTime.Now.ToString("yyyy-MM-dd") + "ErrorSource.log", FileMode.Append, FileAccess.Write, FileShare.Read); }

            StackFrame stackFrame = stackTrace.GetFrame(0);
            StringBuilder logInfo = new StringBuilder("");
            string currentTime = System.DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]");
            if (stackTrace != null)
            {
                logInfo.AppendLine("DateTime：" + currentTime.ToString());
                logInfo.AppendLine("LogType：ExceptionSource");
                logInfo.AppendLine("ExceptionUserMessage：" + exceptionUserMsg);
                //获取文件
                logInfo.AppendLine("File：" + stackFrame.GetFileName());
                //获取方法
                logInfo.AppendLine("Function：" + stackFrame.GetMethod().Name);
                //获取行
                logInfo.AppendLine("Line：" + stackFrame.GetFileLineNumber().ToString());
                //获取列
                logInfo.AppendLine("Column：" + stackFrame.GetFileColumnNumber().ToString());
                logInfo.AppendLine("-----------------------------------------------------------");
                //获得字节数组
                byte[] data = System.Text.Encoding.Default.GetBytes(logInfo.ToString());
                //开始写入
                _systemErrorLog.Write(data, 0, data.Length);
                //清空缓冲区、关闭流
                _systemErrorLog.Flush();
            }
        }
        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="exception">异常对象</param>
        public static void WriteLogFromException(Exception exception, string exceptionUserMsg)
        {
            if (_systemErrorLog == null || !_systemErrorLog.Name.Contains(DateTime.Now.ToString("yyyy-MM-dd")))
            { _systemErrorLog = File.Open(GetSystemLogDirectory() + DateTime.Now.ToString("yyyy-MM-dd") + "ErrorSource.log", FileMode.Append, FileAccess.Write, FileShare.Read); }

            StringBuilder logInfo = new StringBuilder("");
            string currentTime = System.DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]");
            if (exception != null)
            {
                logInfo.AppendLine("DateTime：" + currentTime.ToString());
                logInfo.AppendLine("LogType：Exception");
                logInfo.AppendLine("ExceptionUserMessage：" + exceptionUserMsg);
                //获取描述当前的异常的信息
                logInfo.AppendLine("ExceptionMessage：" + exception.Message);
                //获取当前实例的运行时类型
                logInfo.AppendLine("ExceptionType：" + exception.GetType());
                //获取或设置导致错误的应用程序或对象的名称
                logInfo.AppendLine("ExceptionSource：" + exception.Source);
                //获取引发当前异常的方法
                logInfo.AppendLine("ExceptionFunction：" + exception.TargetSite);
                //获取调用堆栈上直接桢的字符串表示形式
                logInfo.AppendLine("ExceptionStackTrace：" + exception.StackTrace);
                logInfo.AppendLine("-----------------------------------------------------------");
                //获得字节数组
                byte[] data = System.Text.Encoding.Default.GetBytes(logInfo.ToString());
                //开始写入
                _systemErrorLog.Write(data, 0, data.Length);
                //清空缓冲区、关闭流
                _systemErrorLog.Flush();
            }
        }
        /// <summary>
        /// 日常日志
        /// </summary>
        /// <param name="source">例如:ClassName OR FileName</param>
        /// <param name="function">例如:FunctionName</param>
        /// <param name="message">日志信息</param>
        public static void WriteLog(string source, string function, string message)
        {
            //await Task.Run(() =>
            //{
            if (_systemLog == null || !_systemLog.Name.Contains(DateTime.Now.ToString("yyyy-MM-dd")))
            { _systemLog = File.Open(GetSystemLogDirectory() + DateTime.Now.ToString("yyyy-MM-dd") + ".log", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite); }

            StringBuilder logInfo = new StringBuilder("");
            string currentTime = System.DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]");
#if IsDebug
            if (message != null)
            {
                logInfo.AppendLine("DateTime：" + currentTime.ToString() + "\r\n");
                logInfo.AppendLine("LogType：Info");
                //记录源:Class
                logInfo.AppendLine("Source：" + source );
                //记录源:Function
                logInfo.AppendLine("Function：" + function);
                //记录日志信息
                logInfo.AppendLine("Message：" + message);
                logInfo.AppendLine("-----------------------------------------------------------");
                 //获得字节数组
                    byte[] data = System.Text.Encoding.Default.GetBytes(logInfo.ToString());
                    //开始写入
                    _systemLog.Write(data, 0, data.Length);
                    //清空缓冲区、关闭流
                    _systemLog.Flush();
            }
#else
            if (message != null && message.Contains("Debug-") == false)
            {
                logInfo.AppendLine("DateTime：" + currentTime.ToString());
                logInfo.AppendLine("LogType：Info");
                //记录源:Class
                logInfo.AppendLine("Source：" + source);
                //记录源:Function
                logInfo.AppendLine("Function：" + function);
                //记录日志信息
                logInfo.AppendLine("Message：" + message);
                logInfo.AppendLine("-----------------------------------------------------------");
                //获得字节数组
                byte[] data = System.Text.Encoding.Default.GetBytes(logInfo.ToString());
                //开始写入
                _systemLog.Write(data, 0, data.Length);
                //清空缓冲区、关闭流
                _systemLog.Flush();
            }
#endif
            //System.IO.File.AppendAllText(GetSystemLogDirectory() + DateTime.Now.ToString("yyyy-MM-dd") + ".log", logInfo.ToString());
            //});
        }
    }
}
