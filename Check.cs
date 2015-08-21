using System.Web;

namespace MrHuo.OAuthLoginLibs.Core
{
    /// <summary>
    /// 内部类
    /// </summary>
    internal static class Check
    {
        /// <summary>
        /// 检查是否为Web环境。如果不是，抛出NotSupportedException异常
        /// </summary>
        public static void IsWebEnvironment()
        {
            if (HttpContext.Current == null)
            {
                throw Errors.OnlySupportedWeb();
            }
        }
    }
}