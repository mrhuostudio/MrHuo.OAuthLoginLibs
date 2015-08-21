using System;

namespace MrHuo.OAuthLoginLibs.Core
{
    /// <summary>
    /// OAuth验证登录完成之后返回的结果
    /// </summary>
    public class OAuthLoginResult
    {
        /// <summary>
        /// 当前OAuth配置
        /// </summary>
        public OAuthLoginConfig Config { get; set; }

        /// <summary>
        /// 服务器返回数据
        /// </summary>
        public String ServerResponse { get; set; }

        /// <summary>
        /// OAuth验证过程中触发的异常
        /// </summary>
        public Exception Error { get; set; }
    }
}