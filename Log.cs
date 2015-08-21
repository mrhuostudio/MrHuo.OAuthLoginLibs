using System;
using System.IO;
using System.Text;
using System.Web;
using MrHuo.OAuthLoginLibs.Resources;

namespace MrHuo.OAuthLoginLibs.Core
{
    /// <summary>
    /// OAuthLogin日志类。记录文件于：~/OAuthLog.log
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// 普通消息
        /// </summary>
        /// <param name="msg"></param>
        public static void Debug(Type type, string msg)
        {
            RecordLog("Info", msg, type == null ? "UnknowPage" : type.ToString());
        }

        /// <summary>
        /// 异常消息
        /// </summary>
        /// <param name="ex"></param>
        public static void Debug(string page, Exception ex)
        {
            RecordLog("Error", ex.ToString(), page);
        }

        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ex"></param>
        public static void Debug(Type type, Exception ex)
        {
            Debug(type == null ? "UnknowPage" : type.ToString(), ex);
        }

        /// <summary>
        /// 记录到文件
        /// </summary>
        /// <param name="message"></param>
        private static void RecordLog(string type, string message, string page = null)
        {
            var isNeedRecordLog = System.Configuration.ConfigurationManager.AppSettings[RS.get(ResourceKey.SETTINGS_RecordOAuthLog)];
            var need = true;
            if (isNeedRecordLog != null)
            {
                bool.TryParse(isNeedRecordLog, out need);
            }
            if (need)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("====== Log Start ======");
                sb.AppendLine("Time: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                sb.AppendLine("Type: " + type);
                if (page != null)
                {
                    sb.AppendLine("Page: " + page);
                }
                sb.AppendLine("Message: " + message);
                sb.AppendLine("====== Log End ======");
                sb.AppendLine();

                File.AppendAllText(HttpContext.Current.Server.MapPath("~/OAuthLogin.log"), sb.ToString());
            }
        }
    }
}