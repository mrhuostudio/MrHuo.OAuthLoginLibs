using System;
using MrHuo.OAuthLoginLibs.Core;

namespace MrHuo.OAuthLoginLibs.Interfaces
{
    /// <summary>
    /// 一个整合了OAuth登录的初始入口
    /// </summary>
    public interface IOAuthLogin : IDisposable
    {
        /// <summary>
        /// 开始发起验证登录请求
        /// </summary>
        /// <param name="platform">平台</param>
        /// <param name="scope">获取的权限（默认值根据不同平台不一致）</param>
        void BeginAuthoration(string platform, string scope = "");

        /// <summary>
        /// 验证请求是否完成
        /// </summary>
        /// <param name="code">获取的code</param>
        /// <param name="pageState">state</param>
        /// <returns></returns>
        OAuthLoginResult Login(string code, string pageState);
    }
}